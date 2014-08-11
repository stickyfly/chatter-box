Imports System.Net
Imports System.Net.Sockets

Public Module ChatConnections
    Public MustInherit Class ChatConnection
        Protected Const standartport As Integer = 41640
        Protected MyIpAddress As String = getipv4address.ToString

        Public ReadOnly ChatRoomIndex As Integer

        Public Event PublicMessageArrived(message As MessageLanguage.publicmessage)
        Public Event PrivateMessageArrived(message As MessageLanguage.privatemessage)
        Public Event UserLeft(user As User)
        Public Event GotKicked(KickMessage As MessageLanguage.kickmessage)

        Protected Sub RaisePublicMessageArrived(message As MessageLanguage.publicmessage)
            RaiseEvent PublicMessageArrived(message)
        End Sub 'to raise events from inheriting class
        Protected Sub RaisePrivateMessageArrived(message As MessageLanguage.privatemessage)
            RaisePrivateMessageArrived(message)
        End Sub
        Protected Sub RaiseUserLeft(user As User)
            RaiseEvent UserLeft(user)
        End Sub
        Protected Sub RaiseGotKicked(KickMessage As MessageLanguage.kickmessage)
            RaiseEvent GotKicked(KickMessage)
        End Sub

        Sub New(ByVal ChatRomIndex As Integer)
            Me.ChatRoomIndex = ChatRomIndex
            If Not ChatConnectionHelper.Running Then
                Dim CTH As New ChatConnectionHelper
            End If
            Dim timer As New Timer
            AddHandler timer.Tick, AddressOf timertick
            timer.Interval = 100
            timer.Start()
        End Sub
        Public Sub send(ByVal message As Object, user As User)
            Dim thread As New Threading.Thread(Sub() sendaway(message, user))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Public Sub send(ByVal message As Object, IpAddress As String)
            Dim thread As New Threading.Thread(Sub() sendaway(message, MatchIpToUser(IpAddress)))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Protected Sub sendaway(ByVal message As Object, user As User)

            Dim tcpclient = New TcpClient(user.IpAddress, standartport)
            Dim stream As NetworkStream = tcpclient.GetStream

            encrypt(stream, message, user.key)

            stream.Flush()
            tcpclient.Close()
        End Sub

        Protected MustOverride Sub timertick()
        Public MustOverride Sub Leave()
        Public MustOverride Sub SendMessage(text As String, colour As Color, font As Font)
        Public MustOverride Sub SendMessage(text As String, colour As Color, font As Font, Users As List(Of User))
        Public MustOverride Property ChatRoom
        Public Overridable Sub Kickuser(IpAddress As String, Reason As String)
        End Sub
    End Class


    Public Class ChatClientConnection
        Inherits ChatConnection
        Public Overrides Property ChatRoom()
            Get
                Return ChatRoomsJoined(ChatRoomIndex)
            End Get
            Set(value)
                ChatRoomsJoined(ChatRoomIndex) = value
            End Set
        End Property

        Sub New(ChatRoomIndex As Integer)
            MyBase.New(ChatRoomIndex)
        End Sub
        Protected Overrides Sub timertick()
            While ChatRoom.PendingMessages.Count > 0
                process_message(ChatRoom.PendingMessages(0))
                ChatRoom.PendingMessages.RemoveAt(0)
            End While
        End Sub

        ''' <summary>
        ''' Sends a message to everyone in the conversation
        ''' </summary>
        ''' <param name="text">The text in the message</param>
        ''' <param name="colour">The colour in the message</param>
        ''' <param name="font">The font used</param>
        ''' <remarks></remarks>
        ''' 
        Public Overrides Sub SendMessage(text As String, colour As Color, font As Font)
            Dim msg As New MessageLanguage.publicmessage
            msg.Text = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            send(msg, ChatRoom.hostUser)
        End Sub
        ''' <summary>
        ''' Sends a message to every Ip Address specified in the conversation
        ''' </summary>
        ''' <param name="text">The text in the message</param>
        ''' <param name="colour">The colour in the message</param>
        ''' <param name="font">The font used</param>
        ''' <param name="Users">The Users, the message is to be sent to</param>
        ''' <remarks></remarks>
        Public Overrides Sub SendMessage(text As String, colour As Color, font As Font, Users As List(Of User))
            Dim msg As New MessageLanguage.privatemessage
            msg.Text = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            Dim ReceiverIps(Users.Count - 1)
            For i = 0 To Users.Count - 1
                ReceiverIps(i) = Users(i).IpAddress
            Next
            msg.ReceiverIp = ReceiverIps
            send(msg, ChatRoom.HostUser)
        End Sub

        Public Overrides Sub Leave()
            Dim msg As New MessageLanguage.LeaveMessage
            msg.SenderIp = MyIpAddress
            send(msg, ChatRoom.HostUser)
        End Sub

        Sub process_message(message As Object)
            Select Case message.GetType
                Case GetType(MessageLanguage.publicmessage)
                    RaisePublicMessageArrived(message)
                Case GetType(MessageLanguage.privatemessage)
                    RaisePrivateMessageArrived(message)
                Case GetType(MessageLanguage.LeaveMessage)
                    RaiseUserLeft(MatchIpToUser(message.SenderIp))
                Case GetType(MessageLanguage.kickmessage)
                    RaiseGotKicked(message)
            End Select
        End Sub
    End Class

    Private Class ChatConnectionHelper
        Const standartport As Integer = 41640
        Dim tcplistener As TcpListener

        Public Shared Running As Boolean
        Sub New()
            If Not Running Then
                Running = True
                tcplistener = New System.Net.Sockets.TcpListener(Net.IPAddress.Any, standartport)
                tcplistener.Start()
                Dim timer As New Timer
                AddHandler timer.Tick, AddressOf timertick
                timer.Interval = 250
                timer.Start()
            End If
        End Sub
        Private Sub timertick()
            If tcplistener.Pending = True Then
                Dim tcpclient As TcpClient = tcplistener.AcceptTcpClient()
                ProcessClient(tcpclient)
            End If
        End Sub
        Private Sub ProcessClient(tcpclient As TcpClient)
            Dim message As Object
            Dim currentip As String = getipfromclient(tcpclient)

            Dim key() As Byte = MatchIpToUserKey(currentip)

            If Not key Is Nothing Then
                message = decrypt(tcpclient.GetStream(), key)

                For i = 0 To ChatRoomsJoined.Count - 1 'Go Through each Chatroom where joined...
                    If ChatRoomsJoined(i).HostUser.IpAddress = currentip And ChatRoomsJoined(i).ID = message.ChatRoomID Then 'If The HostIp and ID Equal chatroom ID and HostIP...
                        ChatRoomsJoined(i).PendingMessages.Add(message) 'Add a new message to chatroom's Pending Messages
                        Exit Sub
                    End If
                Next
                For i = 0 To ChatRoomsHosting.Count - 1 'same here again, but only check ID, because sender Ips vary and there is a possibility that a User is in 2+ Chatrooms
                    If ChatRoomsHosting(i).ID = message.ChatRoomID And message.SenderIp = currentip Then 'Still check if specified SenderIp in the message actually equals real SenderIp in case of hacks, etc.
                        ChatRoomsHosting(i).PendingMessages.Add(message)
                    End If
                Next
            Else
                MsgBox("A message couldn't be encrypted", MsgBoxStyle.Critical, "Somthing went wrong")
            End If

        End Sub
    End Class


    Public Class ChatHostConnection
        Inherits ChatConnection
        Public Overrides Property ChatRoom()
            Get
                Return ChatRoomsHosting(ChatRoomIndex)
            End Get
            Set(value)
                ChatRoomsHosting(ChatRoomIndex) = value
            End Set
        End Property

        Sub New(ByVal ChatRomIndex As Integer)
            MyBase.New(ChatRomIndex)
        End Sub
        Protected Overrides Sub timertick()
            While ChatRoom.PendingMessages.Count > 0
                process_message(ChatRoom.PendingMessages(0))
                ChatRoom.PendingMessages.RemoveAt(0)
            End While
            If ChatRoom.UserListUpdated Then 'syncronise userlist
                For i = 0 To ChatRoom.ConnectedUserIndex.Count - 1

                Next
            End If
        End Sub

        ''' <summary>
        ''' Sends a message to everyone in the conversation
        ''' </summary>
        ''' <param name="text">The text in the message</param>
        ''' <param name="colour">The colour in the message</param>
        ''' <param name="font">The font used</param>
        ''' <remarks></remarks>
        Public Overrides Sub SendMessage(text As String, colour As Color, font As Font)
            Dim msg As New MessageLanguage.publicmessage
            msg.Text = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            SendEveryone(msg)
        End Sub
        ''' <summary>
        ''' Sends a message to every specified Ip Address in the conversation
        ''' </summary>
        ''' <param name="text">The text in the message</param>
        ''' <param name="colour">The colour in the message</param>
        ''' <param name="font">The font used</param>
        ''' <param name="users">The Users, the message is to be sent to</param>
        ''' <remarks></remarks>
        Public Overrides Sub SendMessage(text As String, colour As Color, font As Font, users As List(Of User))
            Dim msg As New MessageLanguage.privatemessage
            msg.Text = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            Dim ReceiverIps(users.Count - 1)
            For i = 0 To users.Count - 1
                ReceiverIps(i) = users(i).IpAddress
            Next
            msg.ReceiverIp = ReceiverIps
            For i = 0 To users.Count - 1
                send(msg, users(i))
            Next
        End Sub

        Public Overrides Sub KickUser(IpAddress As String, Reason As String)
            Dim msg As New MessageLanguage.kickmessage
            msg.Reason = Reason
            send(msg, IpAddress)
        End Sub
        Public Overrides Sub Leave()
            Dim msg As New MessageLanguage.LeaveMessage
            msg.SenderIp = MyIpAddress
            SendEveryone(msg)
        End Sub

        Sub process_message(message As Object)
            Select Case message.GetType
                Case GetType(MessageLanguage.publicmessage)
                    PublicMessage(message, message.SenderIp)
                Case GetType(MessageLanguage.privatemessage)
                    PrivateMessage(message, message.SenderIp)
                Case GetType(MessageLanguage.LeaveMessage)
                    LeaveMessage(message, message.SenderIp)
            End Select
        End Sub
        Private Sub LeaveMessage(message As MessageLanguage.LeaveMessage, SenderIp As String)
            Dim userint As Integer = MatchIpToUserInt(message.SenderIp)
            SendEveryone(message, KnownUsers(userint))
            RaiseUserLeft(KnownUsers(userint))
            ChatRoom.ConnectedUserIndex.RemoveAt(userint)
        End Sub
        Private Sub PrivateMessage(message As MessageLanguage.privatemessage, SenderIp As String)
            RaisePrivateMessageArrived(message)
            For i = 0 To message.ReceiverIp.Count - 1
                If message.ReceiverIp(i) = MyIpAddress Then
                    RaisePrivateMessageArrived(message)
                Else
                    send(message, message.ReceiverIp(i))
                End If
            Next
        End Sub
        Private Sub PublicMessage(message As MessageLanguage.publicmessage, SenderIp As String)
            RaisePublicMessageArrived(message)
            SendEveryone(message, MatchIpToUser(SenderIp))
        End Sub

        Private Sub SendEveryone(ByVal message As Object)
            For i = 0 To ChatRoom.ConnectedUserIndex.Count - 1
                Dim k As Integer = i
                Dim thread As New Threading.Thread(Sub() sendaway(message, KnownUsers(ChatRoom.ConnectedUserIndex(k))))
                thread.Name = "TCP Client send"
                thread.Start()
            Next
        End Sub
        ''' <summary>
        ''' Sends A message to everyone in the chatroom but one User
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="But"></param>
        ''' <remarks></remarks>
        Private Sub SendEveryone(ByVal message As Object, ByVal But As User)
            For i = 0 To ChatRoom.ConnectedUserIndex.Count - 1
                If Not But.UserID = i Then
                    Dim k As Integer = i
                    Dim thread As New Threading.Thread(Sub() sendaway(message, KnownUsers(ChatRoom.ConnectedUserIndex(k))))
                    thread.Name = "TCP Client send"
                    thread.Start()
                End If
            Next
        End Sub
    End Class
End Module