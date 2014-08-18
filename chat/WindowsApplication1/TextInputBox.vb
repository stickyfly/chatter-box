Public Class TextInputBox
    Public ReadOnly Property ResultText As String
        Get
            Return TextBox1.Text
        End Get
    End Property
    Public Property Prompt As String
    Public Property Title As String

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

End Class