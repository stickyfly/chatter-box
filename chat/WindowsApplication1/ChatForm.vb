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
        ChatConnection.leave()
    End Sub
    Private Sub ChatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listbox.Dock = DockStyle.Fill
        Panel1.Controls.Add(listbox)

        If chatroom.ConnecteduserIndex.Count + chatroom.PendingUserIndex.Count = 0 Then
            KickBtn.Enabled = False
            HostLabel.Text = ""
        Else
            If ChatConnection.GetType = GetType(ChatClientConnection) Then
                KickBtn.Enabled = False
                HostLabel.ForeColor = Color.Orange
                HostLabel.Text = "Host: " & ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.name
            Else
                HostLabel.ForeColor = Color.Blue
                HostLabel.Text = "Host: Me"
            End If
        End If
        For i = 0 To chatroom.ConnecteduserIndex.Count - 1
            CheckedListBox1.Items.Add(KnownUsers(chatroom.ConnecteduserIndex(i)).NickName.name, True)
        Next
        For i = 0 To chatroom.PendingUserIndex.Count - 1
            CheckedListBox1.Items.Add(KnownUsers(chatroom.PendingUserIndex(i)).NickName.name)
        Next
    End Sub

    Private Sub SendBtn_Click(sender As Object, e As EventArgs) Handles BtnSend.Click
        listbox.Items.Add(New ColourListBox.ListboxItem("[" & Now.ToShortTimeString & "] " & TextBox1.Text, Color.Black, Color.LightSkyBlue, StringAlignment.Far))
        If CheckedListBox1.CheckedItems.Count = CheckedListBox1.Items.Count Then
            ChatConnection.SendMessage(TextBox1.Text, Color.Black, Nothing)
        Else
            Dim Users As New List(Of User)
            For i = 0 To CheckedListBox1.CheckedIndices.Count - 1
                Users.Add(KnownUsers(chatroom.ConnecteduserIndex(CheckedListBox1.CheckedIndices(i))))
            Next
            ChatConnection.SendMessage(TextBox1.Text, Color.Black, Nothing, Users)
        End If
        TextBox1.Text = ""
    End Sub

    Private Sub KickBtn_Click(sender As Object, e As EventArgs) Handles KickBtn.Click
        If MsgBox("Are you sure, you want to kick " & CheckedListBox1.SelectedItems(0), MsgBoxStyle.YesNo, "Kick User") = MsgBoxResult.Yes Then

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

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        If CheckedListBox1.SelectedIndices.Count = 1 Then
            KickBtn.Enabled = True
        Else
            KickBtn.Enabled = False
        End If
    End Sub

    Private Sub MessagePrivate_Arrived(message As MessageLanguage.privatemessage) Handles ChatConnection.PrivateMessageArrived
        listbox.Items.Add(New ColourListBox.ListboxItem("[" & Now.ToShortTimeString & "] " & message.Text, message.colour, Color.White))
    End Sub
    Private Sub MessagePublic_Arrived(message As MessageLanguage.publicmessage) Handles ChatConnection.PublicMessageArrived
        listbox.Items.Add(New ColourListBox.ListboxItem("[" & Now.ToShortTimeString & "] " & message.Text, message.colour, Color.White))
    End Sub
    Private Sub KickMessage_Arrived(message As MessageLanguage.kickmessage) Handles ChatConnection.GotKicked
        MsgBox("You were kicked out of the conversation by the Host")
        ChatRoomsJoined.RemoveAt(chatroom.ID)
        Me.ChatConnection = Nothing
        Me.Close()
    End Sub
    Private Sub UserLeft(user As User) Handles ChatConnection.UserLeft

    End Sub
End Class