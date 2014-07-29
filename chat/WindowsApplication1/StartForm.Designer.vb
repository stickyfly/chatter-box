<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StartForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.NameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ComputerNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IpHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ScanProgress = New System.Windows.Forms.ProgressBar()
        Me.InviteChatBtn = New System.Windows.Forms.Button()
        Me.StatusHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(89, 50)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Search for other users"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.AllowColumnReorder = True
        Me.ListView1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameHeader, Me.StatusHeader, Me.ComputerNameHeader, Me.IpHeader})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(107, 41)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(329, 192)
        Me.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'NameHeader
        '
        Me.NameHeader.Text = "Name"
        Me.NameHeader.Width = 90
        '
        'ComputerNameHeader
        '
        Me.ComputerNameHeader.Text = "ComputerName"
        Me.ComputerNameHeader.Width = 95
        '
        'IpHeader
        '
        Me.IpHeader.Text = "IP"
        Me.IpHeader.Width = 80
        '
        'ScanProgress
        '
        Me.ScanProgress.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ScanProgress.Location = New System.Drawing.Point(107, 12)
        Me.ScanProgress.Name = "ScanProgress"
        Me.ScanProgress.Size = New System.Drawing.Size(329, 23)
        Me.ScanProgress.TabIndex = 2
        '
        'InviteChatBtn
        '
        Me.InviteChatBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InviteChatBtn.Enabled = False
        Me.InviteChatBtn.Location = New System.Drawing.Point(354, 239)
        Me.InviteChatBtn.Name = "InviteChatBtn"
        Me.InviteChatBtn.Size = New System.Drawing.Size(82, 35)
        Me.InviteChatBtn.TabIndex = 3
        Me.InviteChatBtn.Text = "Chat"
        Me.InviteChatBtn.UseVisualStyleBackColor = True
        '
        'StatusHeader
        '
        Me.StatusHeader.Text = "Status"
        '
        'StartForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(448, 285)
        Me.Controls.Add(Me.InviteChatBtn)
        Me.Controls.Add(Me.ScanProgress)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.Button1)
        Me.MinimumSize = New System.Drawing.Size(250, 250)
        Me.Name = "StartForm"
        Me.Text = "Chatter Box"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ScanProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents NameHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents IpHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents ComputerNameHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents InviteChatBtn As System.Windows.Forms.Button
    Friend WithEvents StatusHeader As System.Windows.Forms.ColumnHeader

End Class
