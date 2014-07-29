Public Class ChatForm
    Public ChatConnection As Object = Nothing

    Private Sub ChatForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ChatConnection.leave()
    End Sub
    Private Sub ChatForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ChatConnection.GetType = GetType(ChatClientConnection) Then
            KickBtn.Enabled = False
            BanBtn.Enabled = False
            HostLabel.ForeColor = Color.Orange
            HostLabel.Text = "Host: " & ChatRoomsJoined(ChatConnection.ChatRoomIndex).HostUser.NickName.name
        Else
            HostLabel.ForeColor = Color.Blue
            HostLabel.Text = "Host: Me"    
        End If
    End Sub

    Private Sub SendBtn_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For i = 0 To ListBox1.SelectedItems.Count

        Next
    End Sub

    Private Sub KickBtn_Click(sender As Object, e As EventArgs) Handles KickBtn.Click

    End Sub

    Private Sub BanBtn_Click(sender As Object, e As EventArgs) Handles BanBtn.Click

    End Sub
End Class