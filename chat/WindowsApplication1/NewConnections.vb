Imports System.Net.Sockets
Imports System.Runtime.Serialization
Imports System.Net

Public Module NewConnectionListerner
    Public Class NewConnectionEstablisher
        Const standartport As Integer = 41639

        Dim port_ As Integer

        Public chatstate As MessageLanguage.chatstate

        Public Event ChatStateArrived(user As User)
        Public Event connection_request(ByRef accept As Boolean, user As User, ByVal otheruserIps As List(Of String))

        Dim edk As New System.Security.Cryptography.ECDiffieHellmanCng

        Dim tcplistener As System.Net.Sockets.TcpListener
        Sub New(ByVal chatstate As MessageLanguage.chatstate, Optional ByVal port As Integer = standartport)
            Me.chatstate = chatstate
            port_ = port
            tcplistener = New System.Net.Sockets.TcpListener(Net.IPAddress.Any, port)
            tcplistener.Start()
            Dim timer As New Timer
            AddHandler timer.Tick, AddressOf timertick
            timer.Interval = 250
            timer.Start()
        End Sub
        Private Sub timertick()
            If tcplistener.Pending = True Then
                Dim tcpclient As TcpClient = tcplistener.AcceptTcpClient()
                ProcessClient(tcpclient)
            End If
        End Sub
        Private Sub ProcessClient(tcpclient As TcpClient)
            Dim message
            Dim currentip As String = getipfromclient(tcpclient)


            Dim Key() As Byte = MatchIpToUserKey(currentip)

            If Not Key Is Nothing Then
                message = decrypt(tcpclient.GetStream(), Key)
            Else
                Dim formatter As New Formatters.Binary.BinaryFormatter
                message = formatter.Deserialize(tcpclient.GetStream())
            End If
            process_message(message, currentip)
            tcpclient.Close()
        End Sub
        Private Sub process_message(Data As Object, SenderIp As String)
            Select Case Data.GetType
                Case GetType(MessageLanguage.KeyAndState)
                    Dim message As MessageLanguage.KeyAndState = Data

                    Dim key() As Byte = MatchIpToUserKey(SenderIp)

                    If key Is Nothing Then
                        Dim newuser As New User
                        newuser.NickName = message.name
                        newuser.Status = message.Status
                        newuser.key = edk.DeriveKeyMaterial(message.key)
                        newuser.IpAddress = SenderIp
                        newuser.ComputerName = System.Net.Dns.GetHostEntry(newuser.IpAddress).HostName
                        KnownUsers.Add(newuser)

                        message.key = edk.PublicKey
                        message.Status = chatstate
                        Try
                            send(message, SenderIp)
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try

                        RaiseEvent ChatStateArrived(newuser)
                    Else
                        Dim UserInt As Integer = MatchIpToUserInt(SenderIp)
                        KnownUsers(UserInt).Status = message.Status
                        KnownUsers(UserInt).NickName = message.name
                        RaiseEvent ChatStateArrived(KnownUsers(UserInt))
                    End If

                    disconnect(SenderIp)
                Case GetType(MessageLanguage.ConnectionRequest)
                    Dim message As MessageLanguage.ConnectionRequest = Data
                    Dim connect As Boolean = True
                    RaiseEvent connection_request(connect, MatchIpToUser(SenderIp), message.otherUserIps)

                    Dim ca As New MessageLanguage.ConnectionRequestAnswer
                    ca.accept = connect
                    Try
                        send(ca, SenderIp)
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                    If connect Then
                        Dim ctr As New ClientChatRoom
                        ctr.HostUser = MatchIpToUser(SenderIp) 'The hostuser
                        ctr.ID = message.ChatRoomID 'The Host's ChatRoomID

                        For i = 0 To message.otherUserIps.Count - 1 'For each joined user...
                            Dim CurrentUserInt As Integer = MatchIpToUserInt(message.otherUserIps(i))
                            If Not CurrentUserInt = -1 Then
                                ctr.ConnecteduserIndex.Add(KnownUsers(CurrentUserInt).UserID)
                            Else
                                KnownUsers.add(New User(message.otherUserIps(i), Nothing, Nothing, Nothing)) 'Add UserIp and request full details 
                                AskforStatusAndEnrypt(message.otherUserIps(i))
                            End If
                        Next

                        ChatRoomsJoined.Add(ctr)
                        Dim chatwindow As New ChatForm
                        chatwindow.ChatConnection = New ChatClientConnection(ChatRoomsJoined.Count - 1)
                        chatwindow.Show()
                    End If

                Case GetType(MessageLanguage.ConnectionRequestAnswer)
                    Dim cra As New MessageLanguage.ConnectionRequestAnswer

                    For i = 0 To ChatRoomsHosting.Count - 1 'Match the chatroom Id to chatroom
                        If ChatRoomsHosting(i).ID = Data.ChatRoomID Then
                            Dim SenderID As UInteger = KnownUsers(MatchIpToUserInt(SenderIp)).UserID
                            If Data.accept Then 'If the connection was accepted
                                ChatRoomsHosting(i).ConnectedUserIndex.Add(SenderID) 'Then add the user to connectedusers
                            End If
                            ChatRoomsHosting(i).PendingUserIndex.Remove(SenderID) 'remove from pending users
                            ChatRoomsHosting(i).UserListUpdated = True 'Flag that userlist was up
                            Exit For
                        End If
                    Next
            End Select
        End Sub
        Public Sub AskforStatusAndEnrypt(ipaddress As String)
            NewConnectionHelper.ClientIpConnections.Add(ipaddress)

            Dim msg As New MessageLanguage.KeyAndState
            msg.key = edk.PublicKey
            msg.Status = chatstate
            msg.name = PersonalNickname
            Dim thread As New Threading.Thread(Sub() DoWorkCheckStatusAndDecrypt(msg, ipaddress))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Private Sub DoWorkCheckStatusAndDecrypt(message As Object, ipAddress As String)
            Try
                sendaway(message, New User(ipAddress, Nothing, Nothing, Nothing))
            Catch ex As Exception
                Dim NoUser As New User
                NoUser.IpAddress = ipAddress
                RaiseEvent ChatStateArrived(NoUser)
            End Try
        End Sub

        ''' <summary>
        ''' Sends a Chat Request to a known User or a Group
        ''' </summary>
        ''' <param name="Users"></param>
        ''' <remarks></remarks>
        Public Sub CreateNewChat(ByVal Users As List(Of User))
            Dim ctr As New HostChatRoom
            For i = 0 To Users.Count - 1
                ctr.PendingUserIndex.Add(Users(i).UserID)
            Next

            ctr.ID = ChatRoomsHosting.Count
            ChatRoomsHosting.Add(ctr)

            'Get new ChatWindows...
            Dim con As New MessageLanguage.ConnectionRequest

            For i = 0 To Users.Count - 1  'create a list of other userIps in the Chat
                con.otherUserIps.Add(Users(i).IpAddress)
            Next

            For i = 0 To Users.Count - 1
                Try
                    send(con, Users(i))
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Next
        End Sub
        Public Sub InviteUserToChat(user As User, ChatRoomID As Integer)
            Dim Con As New MessageLanguage.ConnectionRequest

            For i = 0 To ChatRoomsHosting.Count - 1  'create a list of other users in the Chat
                Con.otherUserIps.Add(MatchIpToUser(ChatRoomsHosting(ChatRoomID).ConnectedUserIndex(i)).IpAddress)
            Next

            Try
                send(Con, user)
            Catch ex As Exception

            End Try
        End Sub
        Private Sub send(ByVal message As Object, user As User)
            Dim thread As New Threading.Thread(Sub() sendaway(message, user))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Private Sub send(ByVal message As Object, IpAddress As String)
            Dim thread As New Threading.Thread(Sub() sendaway(message, MatchIpToUser(IpAddress)))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Private Sub sendaway(ByVal message As Object, user As User)

            Dim tcpclient = New TcpClient(user.IpAddress, port_)
            Dim stream As NetworkStream = tcpclient.GetStream

            If Not user.key Is Nothing Then
                encrypt(stream, message, user.key)
            Else
                Dim serializer As New Formatters.Binary.BinaryFormatter
                serializer.Serialize(stream, message)
            End If
            stream.Flush()
            tcpclient.Close()
        End Sub

        Public Sub disconnect(IpAddress As String)
            NewConnectionHelper.ClientIpConnections.Remove(IpAddress)
            port_ = standartport

            edk = New System.Security.Cryptography.ECDiffieHellmanCng
        End Sub
    End Class
    Public Class NewConnectionHelper
        Public Shared ClientIpConnections As New List(Of String)
    End Class
End Module