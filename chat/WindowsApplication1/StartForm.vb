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
            LVI.SubItems.Add(user.Status)
            LVI.SubItems.Add(user.ComputerName)
            LVI.SubItems.Add(user.IpAddress)

            ListView1.Items.Add(LVI)
        End If
    End Sub
    Private Sub incoming_request(ByRef accept As Boolean, user As User,
                                 ByVal otherusers As List(Of String)) Handles NewConnectionEstablisher.connection_request

        If MsgBoxResult.Yes = MsgBox(user.NickName.name &
                         " wants to invite you into a chat. Do you want to establish a connection?",
                         MsgBoxStyle.YesNo, "New connection") Then
            accept = True
        Else
            accept = False
        End If
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
            InviteChatBtn.Text = "Open Chat"
            InviteChatBtn.Enabled = True
        ElseIf ListView1.SelectedIndices.Count = 0 Then
            InviteChatBtn.Text = "Open Chat"
            InviteChatBtn.Enabled = False
        Else
            InviteChatBtn.Text = "Invite to a Chat"
            InviteChatBtn.Enabled = True
        End If
    End Sub
    Private Sub InviteChatBtn_Click(sender As Object, e As EventArgs) Handles InviteChatBtn.Click
        Dim otherusers As New List(Of User)
        For i = 0 To ListView1.SelectedIndices.Count - 1
            otherusers.Add(KnownUsers(ListView1.SelectedIndices(i)))
        Next

        NewConnectionEstablisher.CreateNewChat(otherusers)
    End Sub
    Private Sub StartForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        ScanProgress.Left = 107
        ScanProgress.Width = Me.Width - 135
        ListView1.Left = 107
        ListView1.Width = Me.Width - 135
        ListView1.Height = Me.Height - 133
    End Sub
End Class