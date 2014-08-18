<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChatForm
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.BtnSend = New System.Windows.Forms.Button()
        Me.KickBtn = New System.Windows.Forms.Button()
        Me.LeaveBtn = New System.Windows.Forms.Button()
        Me.HostLabel = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ListViewUsers = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(557, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Users:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(58, 288)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(493, 20)
        Me.TextBox1.TabIndex = 5
        '
        'BtnSend
        '
        Me.BtnSend.Enabled = False
        Me.BtnSend.Location = New System.Drawing.Point(12, 288)
        Me.BtnSend.Name = "BtnSend"
        Me.BtnSend.Size = New System.Drawing.Size(45, 19)
        Me.BtnSend.TabIndex = 6
        Me.BtnSend.Text = "Send"
        Me.BtnSend.UseVisualStyleBackColor = True
        '
        'KickBtn
        '
        Me.KickBtn.Location = New System.Drawing.Point(557, 155)
        Me.KickBtn.Name = "KickBtn"
        Me.KickBtn.Size = New System.Drawing.Size(160, 23)
        Me.KickBtn.TabIndex = 8
        Me.KickBtn.Text = "Kick"
        Me.KickBtn.UseVisualStyleBackColor = True
        '
        'LeaveBtn
        '
        Me.LeaveBtn.Location = New System.Drawing.Point(645, 288)
        Me.LeaveBtn.Name = "LeaveBtn"
        Me.LeaveBtn.Size = New System.Drawing.Size(75, 23)
        Me.LeaveBtn.TabIndex = 8
        Me.LeaveBtn.Text = "Leave"
        Me.LeaveBtn.UseVisualStyleBackColor = True
        '
        'HostLabel
        '
        Me.HostLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HostLabel.AutoSize = True
        Me.HostLabel.Location = New System.Drawing.Point(651, 9)
        Me.HostLabel.Name = "HostLabel"
        Me.HostLabel.Size = New System.Drawing.Size(32, 13)
        Me.HostLabel.TabIndex = 9
        Me.HostLabel.Text = "Host:"
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(539, 270)
        Me.Panel1.TabIndex = 11
        '
        'ListViewUsers
        '
        Me.ListViewUsers.CheckBoxes = True
        Me.ListViewUsers.Location = New System.Drawing.Point(557, 28)
        Me.ListViewUsers.Name = "ListViewUsers"
        Me.ListViewUsers.Size = New System.Drawing.Size(160, 121)
        Me.ListViewUsers.TabIndex = 12
        Me.ListViewUsers.UseCompatibleStateImageBehavior = False
        Me.ListViewUsers.View = System.Windows.Forms.View.List
        '
        'ChatForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(732, 320)
        Me.Controls.Add(Me.ListViewUsers)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.HostLabel)
        Me.Controls.Add(Me.LeaveBtn)
        Me.Controls.Add(Me.KickBtn)
        Me.Controls.Add(Me.BtnSend)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ChatForm"
        Me.Text = "ChatForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents BtnSend As System.Windows.Forms.Button
    Friend WithEvents KickBtn As System.Windows.Forms.Button
    Friend WithEvents LeaveBtn As System.Windows.Forms.Button
    Friend WithEvents HostLabel As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ListViewUsers As System.Windows.Forms.ListView
End Class
