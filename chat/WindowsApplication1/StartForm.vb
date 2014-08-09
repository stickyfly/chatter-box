Imports System.Net.Sockets

Public Class StartForm
    Public WithEvents NewConnectionEstablisher As New NewConnectionEstablisher(MessageLanguage.chatstate.Available)

    Private Sub StartForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PersonalNickname.name = "Laurin"
        PersonalNickname.colour = Color.Green
        'KnownUsers.add(New User("127.0.0.1", Nothing, "Laurin", {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32}))
    End Sub

    Private Sub chatlistener_ChatstateArrived(user As User) Handles NewConnectionEstablisher.ChatStateArrived
        If Not user.Status = MessageLanguage.chatstate.NotAvailable Then
            Dim LVI As New ListViewItem
            LVI.UseItemStyleForSubItems = False
            LVI.Text = user.NickName.name
            LVI.ForeColor = user.NickName.colour
            LVI.Font = user.NickName.font
            LVI.SubItems.Add(user.Status.ToString)
            LVI.SubItems.Add(user.ComputerName)
            LVI.SubItems.Add(user.IpAddress)
            LVI.Tag = user.UserID
            ListViewUsers.Items.Add(LVI)
        End If
    End Sub
    Private Sub incoming_request(ByVal ChatroomId As Integer, user As User,
                                 ByVal otherusers As List(Of String)) Handles NewConnectionEstablisher.connection_request
        If otherusers.Count = 0 Then
            Dim userint As Integer = KnownUsers.IndexOf(user)
            ListViewUsers.Items(userint).SubItems(1).BackColor = Color.Blue
            ListViewUsers.Items(userint).SubItems(1).Text = "New Message (" & KnownUsers(userint).Status.ToString & ")"

            FlashStatusOfSubItem(userint)

        Else
            Dim lvi As New ListViewItem
            lvi.SubItems.Add("Status")
            lvi.SubItems.Add(otherusers.Count)
            lvi.SubItems.Add(user.NickName.name, user.NickName.colour, Color.White, user.NickName.font)
            lvi.Tag = ChatRoomsJoined.Count - 1 'The ChatRooms ID
            ListViewGroups.Items.Add(lvi)

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
        ListViewUsers.Items.Clear()
        Button1.Enabled = False
        Scanner.ScanNetwork()
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
        BtnChat_Click(sender, Nothing)
    End Sub
    Private Sub BtnChat_Click(sender As Object, e As EventArgs) Handles BtnChat.Click
        If ListViewUsers.SelectedIndices.Count = 1 Then 'If only one user is selected...
            Dim found As Boolean
            For i = 0 To ChatRoomsJoined.Count - 1 'look, if there is a chat existing
                If ChatRoomsJoined(i).ConnecteduserIndex.Count + ChatRoomsJoined(i).PendingUserIndex.Count = 0 Then 'if only one person joined...
                    If ChatRoomsJoined(i).HostUser.UserID = ListViewUsers.Items(ListViewUsers.SelectedIndices(0)).Tag Then 'if the index of the selected item = the chatrooms only ip
                        NewConnectionEstablisher.OpenChatWindow(i)
                        found = True
                        Exit For
                    End If
                End If
            Next
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
End Class