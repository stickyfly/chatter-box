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
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.KickBtn = New System.Windows.Forms.Button()
        Me.BanBtn = New System.Windows.Forms.Button()
        Me.LeaveBtn = New System.Windows.Forms.Button()
        Me.HostLabel = New System.Windows.Forms.Label()
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
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 12)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(534, 270)
        Me.RichTextBox1.TabIndex = 4
        Me.RichTextBox1.Text = ""
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(58, 288)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(488, 20)
        Me.TextBox1.TabIndex = 5
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 288)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(45, 19)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Send"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(560, 28)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(160, 108)
        Me.ListBox1.TabIndex = 7
        '
        'KickBtn
        '
        Me.KickBtn.Location = New System.Drawing.Point(560, 138)
        Me.KickBtn.Name = "KickBtn"
        Me.KickBtn.Size = New System.Drawing.Size(75, 23)
        Me.KickBtn.TabIndex = 8
        Me.KickBtn.Text = "Kick"
        Me.KickBtn.UseVisualStyleBackColor = True
        '
        'BanBtn
        '
        Me.BanBtn.Location = New System.Drawing.Point(645, 138)
        Me.BanBtn.Name = "BanBtn"
        Me.BanBtn.Size = New System.Drawing.Size(75, 23)
        Me.BanBtn.TabIndex = 8
        Me.BanBtn.Text = "Ban"
        Me.BanBtn.UseVisualStyleBackColor = True
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
        'ChatForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(732, 320)
        Me.Controls.Add(Me.HostLabel)
        Me.Controls.Add(Me.LeaveBtn)
        Me.Controls.Add(Me.BanBtn)
        Me.Controls.Add(Me.KickBtn)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ChatForm"
        Me.Text = "ChatForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents KickBtn As System.Windows.Forms.Button
    Friend WithEvents BanBtn As System.Windows.Forms.Button
    Friend WithEvents LeaveBtn As System.Windows.Forms.Button
    Friend WithEvents HostLabel As System.Windows.Forms.Label
End Class
