Imports System.Net
Imports System.Net.Sockets
Public Module Users
    Public KnownUsers As New UserCollection

    Public ChatRoomsHosting As New List(Of HostChatRoom)
    Public ChatRoomsJoined As New List(Of ClientChatRoom)

    Public PersonalNickname As New MessageLanguage.nickname
    Public Class UserCollection
        Inherits List(Of User)
        Public Shadows Sub add(user As User)
            user.UserID = Me.Count
            MyBase.Add(user)
        End Sub
    End Class
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
        Public Text As String
        Public colour As Color
        Public Font As Font
        Public SenderIp As String
    End Class
    <Serializable()>
    Public Class privatemessage
        Public ChatRoomID As UInteger
        Public Text As String
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
        Public name As String = ""
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

Public Class ColourListBox
    Inherits ListBox

    Sub New()
        MyBase.DrawMode = Windows.Forms.DrawMode.OwnerDrawVariable
    End Sub
    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If e.Index >= 0 Then
            Dim Gr As Graphics = e.Graphics
            Dim Rct As Rectangle = e.Bounds
            Dim Li As ListboxItem = MyBase.Items(e.Index)

            Dim FCB As New SolidBrush(Li.ForeColor) 'ForeColor brush
            Dim BCB As New SolidBrush(Li.ForeColor) 'Backcolor brush

            If CBool(e.State And DrawItemState.Selected) Then 'if item selected, invert colours
                'invert
                FCB.Color = Li.BackColor
                BCB.Color = Li.ForeColor
            Else          
                FCB.Color = Li.ForeColor
                BCB.Color = Li.BackColor
            End If
            Dim ind As Integer = Li.Text.IndexOf(" ")
            Dim time As String = Li.Text.Substring(0, ind)
            Dim Message As String = Li.Text.Substring(ind + 1)

            Dim NewX As Integer = Gr.MeasureString(time, e.Font).Width
            Dim sf As New StringFormat()
            sf.Trimming = StringTrimming.Word
            sf.Alignment = Li.Allignment

            Gr.FillRectangle(BCB, Rct)
            Gr.FillRectangle(Brushes.White, New Rectangle(Rct.X, Rct.Y, NewX, Rct.Height))

            Gr.DrawString(time, e.Font, Brushes.Black, Rct.X, Rct.Y)
            If sf.Alignment = StringAlignment.Near Then
                NewX = Rct.X + NewX
            Else
                NewX = Rct.Right
            End If
            Gr.DrawString(Message, e.Font, FCB, NewX, Rct.Y, sf)
        End If
    End Sub

    Public Class ListboxItem

        ''' <summary>
        ''' sets or returns the Text of the item
        ''' </summary>
        Public Property Text() As String
        ''' <summary>
        ''' Sets or Gets the ForeColor of the Item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BackColor() As Color = Color.Black

        Public Property ForeColor() As Color = Color.White

        Public Property Allignment As StringAlignment = StringAlignment.Near

        Public Property Tag As Object

        ''' <summary>
        ''' returns the text of the item
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Text
        End Function

        Public Sub New()
        End Sub

        Public Sub New(ByVal Text As String)
            Me.Text = Text
        End Sub

        Public Sub New(ByVal Text As String, ByVal ForeColour As Color)
            Me.ForeColor = ForeColour
        End Sub

        Public Sub New(ByVal Text As String, ByVal ForeColour As Color, ByVal BackColour As Color)
            Me.Text = Text
            Me.BackColor = BackColour
            Me.ForeColor = ForeColour
        End Sub

        Public Sub New(ByVal Text As String, ByVal ForeColour As Color, ByVal BackColour As Color, ByVal Allignment As StringAlignment)
            Me.Text = Text
            Me.ForeColor = ForeColour
            Me.BackColor = BackColour
            Me.Allignment = Allignment
        End Sub
    End Class
End Class
