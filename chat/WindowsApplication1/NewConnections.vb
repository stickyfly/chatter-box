Imports System.Net.Sockets
Imports System.Runtime.Serialization
Imports System.Net

Public Module NewConnections
    Public Class NewConnectionEstablisher
        Const standartport As Integer = 41639

        Dim MyIpaddress As String = getipv4address.ToString

        Public chatstate As MessageLanguage.chatstate

        Public Event ChatStateArrived(user As User, Exists As Boolean)
        ''' <summary>
        ''' Represents an Event that occurs, if a remote user establishes a connection
        ''' </summary>
        ''' <param name="ChatroomId">The Index that was allocated to the chatroom in the Chatroomsjoined Database</param>
        ''' <param name="user">The hos of the Chat</param>
        ''' <param name="otheruserIps">All invited and connected Users in the chatroom</param>
        ''' <remarks></remarks>
        Public Event connection_request(ByVal ChatroomId As Integer, user As User, ByVal otheruserIps As List(Of String))

        Dim edk As New System.Security.Cryptography.ECDiffieHellmanCng

        Dim tcplistener As System.Net.Sockets.TcpListener
        Sub New(ByVal chatstate As MessageLanguage.chatstate)
            Me.chatstate = chatstate
            tcplistener = New System.Net.Sockets.TcpListener(Net.IPAddress.Any, standartport)
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

                    Dim userint As Integer = MatchIpToUserInt(SenderIp)

                    Dim isNewUser As Boolean
                    If userint = -1 Then
                        isNewUser = True
                    ElseIf KnownUsers(userint).key Is Nothing Then
                        isNewUser = True
                    End If
                    If isNewUser Then
                        Dim newuser As New User
                        newuser.NickName = message.name
                        newuser.Status = message.Status
                        newuser.key = edk.DeriveKeyMaterial(message.key)
                        newuser.IpAddress = SenderIp
                        newuser.ComputerName = System.Net.Dns.GetHostEntry(newuser.IpAddress).HostName
                        KnownUsers.add(newuser)

                        message.key = edk.PublicKey
                        message.Status = chatstate
                        Try
                            send(message, SenderIp)
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try

                        RaiseEvent ChatStateArrived(newuser, False)
                    Else
                        KnownUsers(userint).Status = message.Status
                        KnownUsers(userint).NickName = message.name
                        RaiseEvent ChatStateArrived(KnownUsers(userint), True)
                    End If

                    disconnect()
                Case GetType(MessageLanguage.ConnectionRequest)
                    Dim message As MessageLanguage.ConnectionRequest = Data
                    Dim AllIps As List(Of String) = message.otherUserIps
                    AllIps.AddRange(message.OtherPendingUserIps)

                    If AllIps.Count = 1 Then 'If convo is one on one...
                        Dim CA As New MessageLanguage.ConnectionRequestAnswer
                        CA.accept = True
                        CA.ChatRoomID = message.ChatRoomID
                        Try
                            send(CA, SenderIp)
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                    End If

                    RaiseEvent connection_request(message.ChatRoomID, MatchIpToUser(SenderIp), AllIps)

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
                            ctr.ConnecteduserIndex.Add(KnownUsers.Count - 1) 'Add to Knownuserlist
                        End If
                    Next
                    For i = 0 To message.OtherPendingUserIps.Count - 1 'Same for users that havent joined yet...
                        Dim CurrentUserInt As Integer = MatchIpToUserInt(message.OtherPendingUserIps(i))
                        If Not CurrentUserInt = -1 Then
                            ctr.PendingUserIndex.Add(KnownUsers(CurrentUserInt).UserID)
                        Else
                            KnownUsers.add(New User(message.OtherPendingUserIps(i), Nothing, Nothing, Nothing)) 'Add UserIp and request full details 
                            AskforStatusAndEnrypt(message.OtherPendingUserIps(i))
                            ctr.PendingUserIndex.Add(KnownUsers.Count - 1)
                        End If
                    Next
                    ChatRoomsJoined.Add(ctr)

                Case GetType(MessageLanguage.ConnectionRequestAnswer)
                    Dim message As MessageLanguage.ConnectionRequestAnswer = Data

                    For i = 0 To ChatRoomsHosting.Count - 1 'Match the chatroom Id to chatroom
                        If ChatRoomsHosting(i).ID = message.ChatRoomID Then
                            Dim SenderID As UInteger = KnownUsers(MatchIpToUserInt(SenderIp)).UserID
                            If message.accept And ChatRoomsHosting(i).PendingUserIndex.Count + ChatRoomsHosting(i).ConnecteduserIndex.Count <> 1 Then 'If the connection was accepted and not one on one
                                ChatRoomsHosting(i).ConnecteduserIndex.Add(SenderID) 'Then add the user to connectedusers
                            End If
                            ChatRoomsHosting(i).PendingUserIndex.Remove(SenderID) 'remove from pending users
                            ChatRoomsHosting(i).UserListUpdated = True 'Flag that userlist was up
                            Exit For
                        End If
                    Next
            End Select
        End Sub
        Public Sub AskforStatusAndEnrypt(ipaddress As String)

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
                RaiseEvent ChatStateArrived(NoUser, False)
            End Try
        End Sub
        ''' <summary>
        ''' Sends a Chat Request to a known User or a Group
        ''' </summary>
        ''' <param name="Users"></param>
        ''' <remarks></remarks>
        Public Sub CreateNewChat(ByVal Users As List(Of User))
            Dim ctr As New HostChatRoom
            If Not Users.Count = 1 Then 'If it is not a one on one convo
                For i = 0 To Users.Count - 1
                    ctr.PendingUserIndex.Add(Users(i).UserID) 'Add the all users as pending
                Next
            Else
                ctr.ConnecteduserIndex.Add(Users(0).UserID) 'else show as connected, becauset is  one on one convo
            End If

            ctr.ID = ChatRoomsHosting.Count
            ChatRoomsHosting.Add(ctr)


            Dim con As New MessageLanguage.ConnectionRequest
            For i = 0 To Users.Count - 1  'create a list of other userIps in the Chat
                con.OtherPendingUserIps.Add(Users(i).IpAddress)
            Next

            Dim ctf As New ChatForm
            ctf.ChatConnection = New ChatHostConnection(ChatRoomsHosting.Count - 1)
            ctf.Show()
            For i = 0 To Users.Count - 1
                Dim newcon As MessageLanguage.ConnectionRequest = con 'make a new variable, because threading is a shit otherwhise
                newcon.OtherPendingUserIps.RemoveAt(i)
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
        ''' <summary>
        ''' Opens a Client Chat the user was invited to
        ''' </summary>
        ''' <param name="ChatRoomIndex">The index of the Chat</param>
        ''' <remarks></remarks>
        Public Sub OpenChatWindow(ChatRoomIndex As Integer)
            Dim ca As New MessageLanguage.ConnectionRequestAnswer
            ca.accept = True
            Try
                send(ca, ChatRoomsJoined(ChatRoomIndex).HostUser.IpAddress)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            Dim chatwindow As New ChatForm
            chatwindow.Text = ChatRoomsJoined(ChatRoomIndex).Name.Text
            chatwindow.ChatConnection = New ChatClientConnection(ChatRoomIndex)
            chatwindow.Show()
        End Sub
        Private Sub send(ByVal message As Object, ByVal user As User)
            Dim thread As New Threading.Thread(Sub() sendaway(message, user))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Private Sub send(ByVal message As Object, ByVal IpAddress As String)
            Dim thread As New Threading.Thread(Sub() sendaway(message, MatchIpToUser(IpAddress)))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Private Sub sendaway(ByVal message As Object, user As User)

            Dim tcpclient = New TcpClient(user.IpAddress, standartport)
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

        Public Sub disconnect()
            edk = New System.Security.Cryptography.ECDiffieHellmanCng
        End Sub
    End Class
End Module