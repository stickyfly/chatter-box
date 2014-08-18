Public Class ChatForm
    Public WithEvents ChatConnection As ChatConnection
    WithEvents listbox As New ColourListBox
    Public Property chatroom As ChatRoom
        Get
            Return ChatConnection.ChatRoom
        End Get
        Set(value As ChatRoom)
            ChatConnection.ChatRoom = value
        End Set
    End Property
    Private Sub ChatForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        chatroom.WindowOpened = False
        ChatConnection.Leave()
    End Sub
    Private Sub ChatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listbox.Dock = DockStyle.Fill
        Panel1.Controls.Add(listbox)
        chatroom.WindowOpened = True
        If chatroom.AllUserCount = 1 Then
            KickBtn.Visible = False
            HostLabel.Text = ""
            If ChatConnection.GetType = GetType(ChatClientConnection) Then
                Dim lvi As New ListViewItem(ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.Text)
                lvi.Tag = -2
                lvi.ForeColor = ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.colour
                lvi.Font = ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.font
                lvi.Text = ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.Text
                lvi.Checked = True
            End If
        Else
            If ChatConnection.GetType = GetType(ChatClientConnection) Then
                KickBtn.Visible = False
                HostLabel.ForeColor = Color.Orange
                HostLabel.Text = "Host: " & ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.Text
            Else
                HostLabel.ForeColor = Color.Blue
                HostLabel.Text = "Host: Me"
            End If
        End If
        UpdateUserList()
    End Sub
    Private Sub UpdateUserList()
        For i = 0 To chatroom.ConnecteduserIndex.Count - 1
            Dim lvi As New ListViewItem
            lvi.Checked = True
            lvi.Text = KnownUsers(chatroom.ConnecteduserIndex(i)).NickName.Text
            lvi.ForeColor = KnownUsers(chatroom.ConnecteduserIndex(i)).NickName.colour
            lvi.Font = KnownUsers(chatroom.ConnecteduserIndex(i)).NickName.font
            lvi.Tag = chatroom.ConnecteduserIndex.Count
            ListViewUsers.Items.Add(lvi)
        Next
        For i = 0 To chatroom.PendingUserIndex.Count - 1
            Dim lvi As New ListViewItem
            lvi.Checked = True
            lvi.Text = KnownUsers(chatroom.PendingUserIndex(i)).NickName.Text
            lvi.ForeColor = KnownUsers(chatroom.PendingUserIndex(i)).NickName.colour
            lvi.Font = KnownUsers(chatroom.PendingUserIndex(i)).NickName.font
            lvi.BackColor = Color.LightGray
            lvi.Tag = chatroom.PendingUserIndex.Count
            ListViewUsers.Items.Add(lvi)
        Next
    End Sub
    Private Sub SendBtn_Click(sender As Object, e As EventArgs) Handles BtnSend.Click
        listbox.Items.Add(New ColourListBox.ListboxItem(TextBox1.Text, Color.Black, Color.LightSkyBlue, StringAlignment.Far, Now))
        If ListViewUsers.CheckedItems.Count = ListViewUsers.Items.Count Then
            ChatConnection.SendMessage(TextBox1.Text, Color.Black, Nothing)
        Else
            Dim Users As New List(Of User)
            For i = 0 To ListViewUsers.CheckedIndices.Count - 1
                Users.Add(KnownUsers(chatroom.ConnecteduserIndex(ListViewUsers.CheckedIndices(i))))
            Next
            ChatConnection.SendMessage(TextBox1.Text, Color.Black, Nothing, Users)
        End If
        TextBox1.Text = ""
    End Sub

    Private Sub KickBtn_Click(sender As Object, e As EventArgs) Handles KickBtn.Click
        TextInputBox.Prompt = "Specify a message to kick " & ListViewUsers.SelectedItems(0).Text
        TextInputBox.Title = "Kick User"
        If TextInputBox.ShowDialog() = MsgBoxResult.Ok Then
            ChatConnection.Kickuser(KnownUsers(chatroom.ConnecteduserIndex(ListViewUsers.SelectedIndices(0))).IpAddress, TextInputBox.ResultText)
            chatroom.ConnecteduserIndex.RemoveAt(ListViewUsers.SelectedIndices(0))
            UpdateUserList()
        End If
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Enter And BtnSend.Enabled = True Then
            SendBtn_Click(sender, e)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If sender.text = "" Then
            BtnSend.Enabled = False
        Else
            BtnSend.Enabled = True
        End If
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ListViewUsers.SelectedIndices.Count = 1 Then
            KickBtn.Enabled = True
        Else
            KickBtn.Enabled = False
        End If
    End Sub

    Private Sub MessagePrivate_Arrived(message As MessageLanguage.privatemessage) Handles ChatConnection.PrivateMessageArrived
        listbox.Items.Add(New ColourListBox.ListboxItem(message.Text, message.colour, Now, message.Font))
    End Sub
    Private Sub MessagePublic_Arrived(message As MessageLanguage.publicmessage) Handles ChatConnection.PublicMessageArrived
        listbox.Items.Add(New ColourListBox.ListboxItem(message.Text, message.colour, Now, message.Font))
    End Sub
    Private Sub KickMessage_Arrived(message As MessageLanguage.kickmessage) Handles ChatConnection.GotKicked
        MsgBox("You were kicked out of the conversation by the Host")
        ChatRoomsJoined.RemoveAt(chatroom.ID)
        StartForm.UpdateListViewGroupItems()
        Me.ChatConnection = Nothing
        Me.Close()
    End Sub
    Private Sub UserLeft(user As User) Handles ChatConnection.UserLeft
        Dim lbi As New ColourListBox.ListboxItem
        lbi.BackColor = Color.LightGreen
        lbi.Time = Now
        lbi.Text = user.NickName.Text & " left the conversation"
        listbox.Items.Add(lbi)
    End Sub

    Private Sub LeaveBtn_Click(sender As Object, e As EventArgs) Handles LeaveBtn.Click
        If Not chatroom.AllUserCount = 1 Then
            ChatConnection.Leave()
        End If
        Me.Close()
    End Sub
End Class