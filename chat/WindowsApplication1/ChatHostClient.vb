Imports System.Net
Imports System.Net.Sockets

Module ChatConnections
    Public MustInherit Class ChatConnection
        Protected Const standartport As Integer = 41640
        Protected MyIpAddress As String = getipv4address.ToString

        Public ReadOnly ChatRoomIndex As Integer

        Public Event PublicMessageArrived(message As MessageLanguage.publicmessage)
        Public Event PrivateMessageArrived(message As MessageLanguage.privatemessage)
        Public Event NickNameUpdated(IpAddress As String, nickname As MessageLanguage.nickname)
        Public Event UserLeft(user As User)
        Public Event GotBanned(BanMessage As MessageLanguage.banmessage)
        Public Event GotKicked(KickMessage As MessageLanguage.kickmessage)

        Protected Sub RaisePublicMessageArrived(message As MessageLanguage.publicmessage)
            RaiseEvent PublicMessageArrived(message)
        End Sub 'to raise events from inheriting class
        Protected Sub RaisePrivateMessageArrived(message As MessageLanguage.privatemessage)
            RaisePrivateMessageArrived(message)
        End Sub
        Protected Sub RaiseNickNameUpdated(IpAddress As String, nickname As MessageLanguage.nickname)
            RaiseEvent NickNameUpdated(IpAddress, nickname)
        End Sub
        Protected Sub RaiseUserLeft(user As User)
            RaiseEvent UserLeft(user)
        End Sub
        Protected Sub RaiseGotKicked(KickMessage As MessageLanguage.kickmessage)
            RaiseEvent GotKicked(KickMessage)
        End Sub
        Protected Sub RaiseGotBanned(BanMessage As MessageLanguage.banmessage)
            RaiseEvent GotBanned(BanMessage)
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

        MustOverride Sub timertick()

        Protected Sub send(ByVal message As Object, user As User)
            Dim thread As New Threading.Thread(Sub() sendaway(message, user))
            thread.Name = "TCP Client send"
            thread.Start()
        End Sub
        Protected Sub send(ByVal message As Object, IpAddress As String)
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

    End Class


    Public Class ChatClientConnection

        Inherits ChatConnection

        Sub New(ChatRoomIndex As Integer)
            MyBase.New(ChatRoomIndex)
        End Sub
        Overrides Sub timertick()
            While ChatRoomsJoined(ChatRoomIndex).PendingMessages.Count > 0
                process_message(ChatRoomsJoined(ChatRoomIndex).PendingMessages(0))
                ChatRoomsJoined(ChatRoomIndex).PendingMessages.RemoveAt(0)
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
        Public Sub SendMessage(text As String, colour As Color, font As Font)
            Dim msg As New MessageLanguage.publicmessage
            msg.message = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            send(msg, ChatRoomsJoined(ChatRoomIndex).HostUser)
        End Sub
        ''' <summary>
        ''' Sends a message to every Ip Address specified in the conversation
        ''' </summary>
        ''' <param name="text">The text in the message</param>
        ''' <param name="colour">The colour in the message</param>
        ''' <param name="font">The font used</param>
        ''' <param name="IpAddresses">The Ip Addresses, the message is to be sent to</param>
        ''' <remarks></remarks>
        Public Sub SendMessage(text As String, colour As Color, font As Font, IpAddresses() As String)
            Dim msg As New MessageLanguage.privatemessage
            msg.text = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            msg.ReceiverIp = IpAddresses
            send(msg, ChatRoomsJoined(ChatRoomIndex).HostUser)
        End Sub
        ''' <summary>
        ''' Refreshes and sends the current Nickname
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateNickname()
            Dim msg As New MessageLanguage.NicknameChanged
            msg.Nickname = PersonalNickname
            msg.SenderIp = MyIpAddress
            send(msg, ChatRoomsJoined(ChatRoomIndex).HostUser)
        End Sub
        Public Sub Leave()
            Dim msg As New MessageLanguage.LeaveMessage
            msg.SenderIp = MyIpAddress
            send(msg, ChatRoomsJoined(ChatRoomIndex).HostUser)
        End Sub

        Sub process_message(message As Object)
            Select Case message.GetType
                Case GetType(MessageLanguage.publicmessage)
                    RaisePublicMessageArrived(message)
                Case GetType(MessageLanguage.privatemessage)
                    RaisePrivateMessageArrived(message)
                Case GetType(MessageLanguage.NicknameChanged)
                    RaiseNickNameUpdated(message.IpAddress, message.Nickname)
                Case GetType(MessageLanguage.LeaveMessage)
                    Dim userint As Integer = MatchIpToUserInt(message.SenderIp)
                    RaiseUserLeft(KnownUsers(userint))
                Case GetType(MessageLanguage.kickmessage)
                    RaiseGotKicked(message)
                Case GetType(MessageLanguage.banmessage)
                    RaiseGotBanned(message)
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

        Sub New(ByVal ChatRomIndex As Integer)
            MyBase.New(ChatRomIndex)
        End Sub
        Overrides Sub timertick()
            While ChatRoomsHosting(ChatRoomIndex).PendingMessages.Count > 0
                process_message(ChatRoomsHosting(ChatRoomIndex).PendingMessages(0))
                ChatRoomsHosting(ChatRoomIndex).PendingMessages.RemoveAt(0)
            End While
            If ChatRoomsHosting(ChatRoomIndex).UserListUpdated Then
                For i = 0 To ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs.Count - 1

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
        Public Sub SendMessage(text As String, colour As Color, font As Font)
            Dim msg As New MessageLanguage.publicmessage
            msg.message = text
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
        ''' <param name="IpAddresses">The Ip Addresses, the message is to be sent to</param>
        ''' <remarks></remarks>
        Public Sub SendMessage(text As String, colour As Color, font As Font, IpAddresses() As String)
            Dim msg As New MessageLanguage.privatemessage
            msg.text = text
            msg.colour = colour
            msg.Font = font
            msg.SenderIp = MyIpAddress
            msg.ReceiverIp = IpAddresses
            For i = 0 To IpAddresses.Count - 1
                send(msg, IpAddresses(i))
            Next
        End Sub
        ''' <summary>
        ''' Refreshes and sends the current Nickname
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateNickname()
            Dim msg As New MessageLanguage.NicknameChanged
            msg.Nickname = PersonalNickname
            msg.SenderIp = MyIpAddress
            SendEveryone(msg)
        End Sub
        Public Sub BanUser(IpAddress As String, Reason As String, until As Date)
            Dim msg As New MessageLanguage.banmessage
            msg.Reason = Reason
            msg.until = until
            send(msg, IpAddress)
        End Sub
        Public Sub KickUser(IpAddress As String, Reason As String)
            Dim msg As New MessageLanguage.kickmessage
            msg.Reason = Reason
            send(msg, IpAddress)
        End Sub
        Public Sub Leave()
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
                Case GetType(MessageLanguage.NicknameChanged)
                    nickname(message, message.SenderIp)
                Case GetType(MessageLanguage.LeaveMessage)
                    LeaveMessage(message, message.SenderIp)
            End Select
        End Sub
        Private Sub LeaveMessage(message As MessageLanguage.LeaveMessage, SenderIp As String)
            Dim userint As Integer = MatchIpToUserInt(message.SenderIp)
            RaiseUserLeft(KnownUsers(userint))
            ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs.RemoveAt(userint)
            For i = 0 To ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs.Count - 1

                If Not SenderIp = MatchIDToUser(ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs(i)).IpAddress Then
                    send(message, ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs(i))
                End If
            Next
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
            Dim SenderUserInt = MatchIpToUserInt(SenderIp)
            For i = 0 To ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs.Count - 1
                If Not SenderUserInt = ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs(i) Then
                    send(message, MatchIDToUser(ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs(i)).IpAddress)
                End If
            Next
        End Sub
        Private Sub nickname(message As MessageLanguage.NicknameChanged, SenderIp As String)
            RaiseNickNameUpdated(SenderIp, message.Nickname)
            Dim SenderInt As Integer = MatchIpToUserInt(SenderIp)
            For i = 0 To ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs.Count - 1
                If Not SenderInt = i Then
                    send(message, MatchIDToUser(ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs(i)))
                End If
            Next
        End Sub
        Private Sub SendEveryone(ByVal message As Object)
            For i = 0 To ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs.Count - 1
                Dim k As Integer = i
                Dim thread As New Threading.Thread(Sub() sendaway(message, MatchIDToUser(ChatRoomsHosting(ChatRoomIndex).ConnectedUserIDs(k))))
                thread.Name = "TCP Client send"
                thread.Start()
            Next
        End Sub

    End Class

End Module