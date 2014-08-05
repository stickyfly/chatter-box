Public Class ChatForm
    Public ChatConnection As ChatConnection
    Public Property ChatRoom As ChatRoom
        Get
            If GetType(ChatConnection) = GetType(ChatHostConnection) Then
                Return ChatRoomsHosting(ChatConnection.ChatRoomIndex)
            Else
                Return ChatRoomsJoined(ChatConnection.ChatRoomIndex)
            End If
        End Get
        Set(value As ChatRoom)
            If GetType(ChatConnection) = GetType(ChatHostConnection) Then
                ChatRoomsHosting(ChatConnection.ChatRoomIndex) = value
            Else
                ChatRoomsJoined(ChatConnection.ChatRoomIndex) = value
            End If
        End Set
    End Property
    Private Sub ChatForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ChatConnection.leave()
    End Sub
    Private Sub ChatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ChatRoom.ConnecteduserIndex.Count + ChatRoom.PendingUserIndex.Count = 1 Then
            KickBtn.Enabled = False
            BanBtn.Enabled = False
            HostLabel.Text = ""
        Else
            If ChatConnection.GetType = GetType(ChatClientConnection) Then
                KickBtn.Enabled = False
                BanBtn.Enabled = False
                HostLabel.ForeColor = Color.Orange
                HostLabel.Text = "Host: " & ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.name
            Else
                HostLabel.ForeColor = Color.Blue
                HostLabel.Text = "Host: Me"
            End If
        End If
    End Sub

    Private Sub SendBtn_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckedListBox1.CheckedItems.Count = CheckedListBox1.Items.Count Then
            ChatConnection.SendMessage(TextBox1.Text, Color.Black, Nothing)
        Else
            Dim Users As New List(Of User)
            For i = 0 To CheckedListBox1.CheckedIndices.Count - 1
                Users.Add(KnownUsers(ChatRoom.ConnecteduserIndex(CheckedListBox1.CheckedIndices(i))))
            Next
            ChatConnection.SendMessage(TextBox1.Text, Color.Black, Nothing, Users)
        End If
        TextBox1.Text = ""
    End Sub

    Private Sub KickBtn_Click(sender As Object, e As EventArgs) Handles KickBtn.Click

    End Sub

    Private Sub BanBtn_Click(sender As Object, e As EventArgs) Handles BanBtn.Click

    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Enter Then
            SendBtn_Click(sender, e)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

    End Sub
End Class