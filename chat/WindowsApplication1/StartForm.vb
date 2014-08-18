Imports System.Net.Sockets

Public Class StartForm
    Public WithEvents NewConnectionEstablisher As New NewConnectionEstablisher(MessageLanguage.chatstate.Available)

    Private Sub StartForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub chatlistener_ChatstateArrived(user As User, UserExists As Boolean) Handles NewConnectionEstablisher.ChatStateArrived
        If Not user.Status = MessageLanguage.chatstate.NotAvailable Then
            UpdateListViewUserItems()
        End If
    End Sub
    Public Sub UpdateListViewUserItems()
        ListViewUsers.Items.Clear()
        For i = 0 To KnownUsers.Count - 1
            If Not KnownUsers(i).Status = MessageLanguage.chatstate.NotAvailable Then
                Dim LVI As New ListViewItem
                LVI.UseItemStyleForSubItems = False
                LVI.Text = KnownUsers(i).NickName.Text
                LVI.ForeColor = KnownUsers(i).NickName.colour
                LVI.Font = KnownUsers(i).NickName.font
                LVI.SubItems.Add(KnownUsers(i).Status.ToString)
                LVI.SubItems.Add(KnownUsers(i).ComputerName)
                LVI.SubItems.Add(KnownUsers(i).IpAddress)
                LVI.Tag = KnownUsers(i).UserID
                ListViewUsers.Items.Add(LVI)
            End If
        Next
    End Sub
    Public Sub UpdateListViewGroupItems()
        ListViewGroups.Items.Clear()
        For i = 0 To ChatRoomsJoined.Count - 1
            Dim lvi As New ListViewItem(ChatRoomsJoined(i).HostUser.NickName.Text)
            lvi.ForeColor = ChatRoomsJoined(i).HostUser.NickName.colour
            lvi.Font = ChatRoomsJoined(i).HostUser.NickName.font
            lvi.SubItems.Add(ChatRoomsJoined(i).ConnecteduserIndex.Count + ChatRoomsJoined(i).PendingUserIndex.Count)
            lvi.SubItems.Add(ChatRoomsJoined(i).HostUser.NickName.Text, ChatRoomsJoined(i).HostUser.NickName.colour, Color.White, ChatRoomsJoined(i).HostUser.NickName.font)
            lvi.Tag = ChatRoomsJoined.Count - 1 'The ChatRooms ID
            ListViewGroups.Items.Add(lvi)
        Next
    End Sub
    Private Sub incoming_request(ByVal ChatroomId As Integer, user As User,
                                 ByVal otherusers As List(Of String)) Handles NewConnectionEstablisher.connection_request
        If otherusers.Count = 0 Then
            Dim userint As Integer = KnownUsers.IndexOf(user)
            ListViewUsers.Items(userint).SubItems(1).BackColor = Color.Blue
            ListViewUsers.Items(userint).SubItems(1).Text = "New Message (" & KnownUsers(userint).Status.ToString & ")" 'its not realy a message, but indicates, that somebody opened his chat to talk

            FlashStatusOfSubItem(userint)
        Else
            UpdateListViewGroupItems()
        End If
    End Sub
    Sub FlashStatusOfSubItem(ItemIndex As Integer)
        Dim t As New Timer
        t.Interval = 60
        ListViewUsers.Items(ItemIndex).SubItems(1).Tag = True 'indicate, that blinking is on
        AddHandler t.Tick, Sub(sender As Object, e As EventArgs) dimcolour(sender, ItemIndex)
        t.Start()
    End Sub
    Sub dimcolour(sender As Object, ItemIndex As Integer)
        If ListViewUsers.Items(ItemIndex).SubItems(1).Tag = False Then 'if flashing is meant to stop...
            sender.stop()
            ListViewUsers.Items(ItemIndex).SubItems(1).BackColor = Color.White
            Exit Sub
        End If
        Dim colour As Color = ListViewUsers.Items(ItemIndex).SubItems(1).BackColor
        Dim N As Integer = colour.G
        N -= 15
        If N < 17 Then
            N = 255
        End If
        ListViewUsers.Items(ItemIndex).SubItems(1).BackColor = Color.FromArgb(colour.A, N, N, 255)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        Try
            Scanner.ScanNetwork()
        Catch
            MsgBox("Couldn't scan Network")
            Button1.Enabled = True
        End Try
        AddHandler Scanner.FinishedScan, AddressOf finishedscan
        AddHandler Scanner.ProgressUpdated, AddressOf newipsarrived
    End Sub
    Private Sub finishedscan()
        ScanProgress.Value = 100
        Button1.Enabled = True

        ScanProgress.Value = 0
    End Sub
    Private Sub newipsarrived(ips As List(Of String), progress As Single)
        For i = 0 To ips.Count - 1
            NewConnectionEstablisher.AskforStatusAndEnrypt(ips(i))
        Next
        ScanProgress.Value = progress
    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListViewUsers.SelectedIndexChanged
        If ListViewUsers.SelectedIndices.Count = 1 Then
            BtnChat.Text = "Open Chat"
            BtnChat.Enabled = True
            ListViewUsers.SelectedItems(0).SubItems(1).Tag = False 'indicate to stop flashing
            ListViewUsers.SelectedItems(0).SubItems(1).Text = KnownUsers(ListViewUsers.SelectedIndices(0)).Status.ToString 'Change Status back to normal
        ElseIf ListViewUsers.SelectedIndices.Count = 0 Then
            BtnChat.Text = "Open Chat"
            BtnChat.Enabled = False
        Else
            BtnChat.Text = "Invite to a Chat"
            BtnChat.Enabled = True
        End If
    End Sub
    Private Sub SubItem_DoubleKlick(sender As Object, e As MouseEventArgs) Handles ListViewUsers.MouseDoubleClick
        BtnChat.Text = "Open Chat"
        BtnChat.Enabled = True
        ListViewUsers.SelectedItems(0).SubItems(1).Tag = False 'indicate to stop flashing
        ListViewUsers.SelectedItems(0).SubItems(1).Text = KnownUsers(ListViewUsers.SelectedIndices(0)).Status.ToString 'Change Status back to normal
        BtnChat_Click(sender, Nothing)
    End Sub
    Private Sub BtnChat_Click(sender As Object, e As EventArgs) Handles BtnChat.Click
        If ListViewUsers.SelectedIndices.Count = 1 Then 'If only one user is selected...
            Dim found As Boolean
            For i = 0 To ChatRoomsJoined.Count - 1 'look, if there is a chat existing
                If ChatRoomsJoined(i).ConnecteduserIndex.Count + ChatRoomsJoined(i).PendingUserIndex.Count = 0 Then 'if only one person joined...
                    If ChatRoomsJoined(i).HostUser.UserID = ListViewUsers.Items(ListViewUsers.SelectedIndices(0)).Tag Then 'if the index of the selected item = the chatrooms only ip
                        If Not ChatRoomsJoined(i).WindowOpened Then
                            NewConnectionEstablisher.OpenChatWindow(i)
                        End If
                        found = True
                        Exit For
                    End If
                End If
            Next
            'same for host... later

            If Not found Then
                Dim userlist As New List(Of User)
                userlist.Add(KnownUsers(ListViewUsers.Items(ListViewUsers.SelectedIndices(0)).Tag))
                NewConnectionEstablisher.CreateNewChat(userlist)
            End If
        Else
            Dim otherusers As New List(Of User)
            For i = 0 To ListViewUsers.SelectedIndices.Count - 1
                otherusers.Add(KnownUsers(ListViewUsers.SelectedIndices(i)))
            Next
            NewConnectionEstablisher.CreateNewChat(otherusers)
        End If
    End Sub
    Private Sub StartForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScanProgress.Left = 107
        ScanProgress.Width = Me.Width - 135
        TabControl1.Left = 107
        TabControl1.Width = Me.Width - 135
        TabControl1.Height = Me.Height - 133
    End Sub

    Private Sub TabSizeChanges(sender As Object, e As EventArgs) Handles TabControl1.SizeChanged
        ListViewUsers.Size = TabControl1.Size
    End Sub

    Private Sub Panel1_Click(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles ColourPanel.Click
        ColorDialog1.Color = ColourPanel.BackColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ColourPanel.BackColor = ColorDialog1.Color
            TextBox1.ForeColor = ColorDialog1.Color
            PersonalNickname.colour = ColorDialog1.Color
        End If
    End Sub
    Private Sub BtnFont_Click(sender As System.Object, e As System.EventArgs) Handles BtnFont.Click
        If FontDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextBox1.Font = FontDialog1.Font
            PersonalNickname.font = FontDialog1.Font
        End If
    End Sub

    Private Sub ColourToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ColourToolStripMenuItem.Click
        Panel1_Click(sender, Nothing)
    End Sub

    Private Sub FontToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FontToolStripMenuItem.Click
        BtnFont_Click(sender, Nothing)
    End Sub
    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        PersonalNickname.Text = TextBox1.Text
    End Sub
End Class