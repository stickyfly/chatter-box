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
        Me.components = New System.ComponentModel.Container()
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"asdas", "Hello", "", ""}, -1)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.NameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.StatusHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ComputerNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IpHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ScanProgress = New System.Windows.Forms.ProgressBar()
        Me.BtnChat = New System.Windows.Forms.Button()
        Me.BtnIgnore = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
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
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameHeader, Me.StatusHeader, Me.ComputerNameHeader, Me.IpHeader})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        ListViewItem2.UseItemStyleForSubItems = False
        Me.ListView1.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem2})
        Me.ListView1.Location = New System.Drawing.Point(3, 3)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(315, 160)
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'NameHeader
        '
        Me.NameHeader.Text = "Name"
        Me.NameHeader.Width = 90
        '
        'StatusHeader
        '
        Me.StatusHeader.Text = "Status"
        '
        'ComputerNameHeader
        '
        Me.ComputerNameHeader.Text = "ComputerName"
        Me.ComputerNameHeader.Width = 97
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
        'BtnChat
        '
        Me.BtnChat.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnChat.Enabled = False
        Me.BtnChat.Location = New System.Drawing.Point(354, 239)
        Me.BtnChat.Name = "BtnChat"
        Me.BtnChat.Size = New System.Drawing.Size(82, 35)
        Me.BtnChat.TabIndex = 3
        Me.BtnChat.Text = "Open Chat"
        Me.ToolTip1.SetToolTip(Me.BtnChat, "Chat to the selected user(s)")
        Me.BtnChat.UseVisualStyleBackColor = True
        '
        'BtnIgnore
        '
        Me.BtnIgnore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnIgnore.Location = New System.Drawing.Point(266, 239)
        Me.BtnIgnore.Name = "BtnIgnore"
        Me.BtnIgnore.Size = New System.Drawing.Size(82, 35)
        Me.BtnIgnore.TabIndex = 3
        Me.BtnIgnore.Text = "Ignore"
        Me.ToolTip1.SetToolTip(Me.BtnIgnore, "Ignore and hide conversation")
        Me.BtnIgnore.UseVisualStyleBackColor = True
        Me.BtnIgnore.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(107, 41)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(329, 192)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ListView1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(321, 166)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Users"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(321, 166)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Group Chats"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'StartForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(448, 285)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.BtnIgnore)
        Me.Controls.Add(Me.BtnChat)
        Me.Controls.Add(Me.ScanProgress)
        Me.Controls.Add(Me.Button1)
        Me.MinimumSize = New System.Drawing.Size(250, 250)
        Me.Name = "StartForm"
        Me.Text = "Chatter Box"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ScanProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents NameHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents IpHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents ComputerNameHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents BtnChat As System.Windows.Forms.Button
    Friend WithEvents StatusHeader As System.Windows.Forms.ColumnHeader
    Friend WithEvents BtnIgnore As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage

End Class
