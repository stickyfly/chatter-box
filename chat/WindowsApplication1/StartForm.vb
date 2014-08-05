Imports System.Net.Sockets

Public Class StartForm
    Public WithEvents NewConnectionEstablisher As New NewConnectionEstablisher(MessageLanguage.chatstate.Available)

    Private Sub StartForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PersonalNickname.name = "Laurin"
        PersonalNickname.colour = Color.Green
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

            ListView1.Items.Add(LVI)
        End If
    End Sub
    Private Sub incoming_request(ByRef accept As Boolean, user As User,
                                 ByVal otherusers As List(Of String)) Handles NewConnectionEstablisher.connection_request
        If otherusers.Count = 1 Then
            Dim userint As Integer = MatchIpToUserInt(otherusers(0))
            ListView1.Items(userint).SubItems(1).BackColor = Color.Blue
            ListView1.Items(userint).SubItems(1).Text = "New Message (" & KnownUsers(userint).Status.ToString & ")"

            FlashStatusOfSubItem(userint)

        Else

        End If
    End Sub
    Sub FlashStatusOfSubItem(ItemIndex As Integer)
        Dim t As New Timer
        t.Interval = 60
        ListView1.Items(ItemIndex).SubItems(1).Tag = True 'indicate, that blinking is on
        AddHandler t.Tick, Sub(sender As Object, e As EventArgs) dimcolour(sender, ItemIndex)
        t.Start()
    End Sub
    Sub dimcolour(sender As Object, ItemIndex As Integer)
        If ListView1.Items(ItemIndex).SubItems(1).Tag = False Then 'if flashing is meant to stop...
            sender.stop()
            ListView1.Items(ItemIndex).SubItems(1).BackColor = Color.White
            Exit Sub
        End If
        Dim colour As Color = ListView1.Items(ItemIndex).SubItems(1).BackColor
        Dim N As Integer = colour.G
        N -= 15
        If N < 17 Then
            N = 255
        End If
        ListView1.Items(ItemIndex).SubItems(1).BackColor = Color.FromArgb(colour.A, N, N, 255)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView1.Items.Clear()
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
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedIndices.Count = 1 Then
            BtnChat.Text = "Open Chat"
            BtnChat.Enabled = True
            ListView1.SelectedItems(0).SubItems(1).Tag = False 'indicate to stop flashing
            ListView1.SelectedItems(0).SubItems(1).Text = KnownUsers(ListView1.SelectedIndices(0)).Status.ToString 'Change Status back to normal
        ElseIf ListView1.SelectedIndices.Count = 0 Then
            BtnChat.Text = "Open Chat"
            BtnChat.Enabled = False
        Else
            BtnChat.Text = "Invite to a Chat"
            BtnChat.Enabled = True
        End If
    End Sub
    Private Sub SubItem_DoubleKlick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        Dim Users As New List(Of User)
        Users.Add(KnownUsers(ListView1.GetItemAt(e.X, e.Y).Index))
        NewConnectionEstablisher.CreateNewChat(Nothing)
    End Sub
    Private Sub BtnChat_Click(sender As Object, e As EventArgs) Handles BtnChat.Click
        Dim otherusers As New List(Of User)
        For i = 0 To ListView1.SelectedIndices.Count - 1
            otherusers.Add(KnownUsers(ListView1.SelectedIndices(i)))
        Next
        NewConnectionEstablisher.CreateNewChat(otherusers)
    End Sub
    Private Sub StartForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScanProgress.Left = 107
        ScanProgress.Width = Me.Width - 135
        TabControl1.Left = 107
        TabControl1.Width = Me.Width - 135
        TabControl1.Height = Me.Height - 133
    End Sub

    Private Sub TabSizeChanges(sender As Object, e As EventArgs) Handles TabControl1.SizeChanged
        ListView1.Size = TabControl1.Size
    End Sub
End Class