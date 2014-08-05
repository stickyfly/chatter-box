Imports System.Net
Imports System.Net.Sockets
Public Module Users
    Public KnownUsers As New List(Of User)

    Public ChatRoomsHosting As New List(Of HostChatRoom)
    Public ChatRoomsJoined As New List(Of ClientChatRoom)

    Public PersonalNickname As New MessageLanguage.nickname

    Public Class User
        Public IpAddress As String
        Public NickName As MessageLanguage.nickname
        Public Status As MessageLanguage.chatstate
        Public ComputerName As String
        Public key() As Byte

        Public UserID As UInteger
        Public Sub New(IpAddress As String, Nickname As MessageLanguage.nickname, Computername As String, key() As Byte)
            Me.IpAddress = IpAddress
            Me.NickName = Nickname
            Me.ComputerName = Computername
            Me.key = key
        End Sub
        Public Sub New()
        End Sub
        ''' <summary>
        ''' Returns the personal user
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly Self As New User(getipv4address.ToString, PersonalNickname, Dns.GetHostName, Nothing)
    End Class
    Public MustInherit Class ChatRoom
        Public ConnecteduserIndex As New List(Of UInteger) 'The list of Connected Users to the ChatRoom 
        Public PendingUserIndex As New List(Of UInteger) 'The List of Users that haven't responded to the Request yet
        Public PendingMessages As New List(Of Object) 'All messages that haven't been processed yet
        Public ID As UInteger
    End Class
    Public Class ClientChatRoom
        Inherits ChatRoom
        Public HostUser As User 'The user who Hosts the Chat
    End Class
    Public Class HostChatRoom
        Inherits ChatRoom
        Public UserListUpdated As Boolean 'Allows the NewConnectionHelper to flag that the userlist was updated
    End Class
End Module

Namespace MessageLanguage
    <Serializable()>
    Public Class publicmessage
        Public ChatRoomID As UInteger
        Public message As String
        Public colour As Color
        Public Font As Font
        Public SenderIp As String
    End Class
    <Serializable()>
    Public Class privatemessage
        Public ChatRoomID As UInteger
        Public text As String
        Public colour As Color
        Public Font As Font
        Public SenderIp As String
        Public ReceiverIp() As String
    End Class
    <Serializable()>
    Public Class kickmessage
        Public ChatRoomID As UInteger
        Public Reason As String
    End Class
    <Serializable()> Public Class banmessage
        Public ChatRoomID As UInteger
        Public Reason As String
        Public until As Date
    End Class
    <Serializable()> Public Class LeaveMessage
        Public ChatRoomID As UInteger
        Public SenderIp As String
    End Class
    <Serializable()> Public Class JoinMessage
        Public ChatRoomID As UInteger
        Public SenderIp As String
        Public NewUserIp As String
    End Class
    <Serializable()>
    Public Class nickname
        Public font As Font
        Public colour As Color
        Public name As String
    End Class
    <Serializable()>
    Public Class ConnectionRequest
        Public ChatRoomID As UInteger
        Public otherUserIps As New List(Of String)
        Public OtherPendingUserIps As New List(Of String)
    End Class
    <Serializable()>
    Class ConnectionRequestAnswer
        Public ChatRoomID As UInteger
        Public accept As Boolean
    End Class
    <Serializable()>
    Public Class KeyAndState
        Public key As System.Security.Cryptography.ECDiffieHellmanCngPublicKey
        Public Status As chatstate
        Public name As nickname
    End Class
    <Serializable()>
    Public Enum chatstate As Byte
        NotAvailable = 0
        Available = 1
    End Enum
End Namespace