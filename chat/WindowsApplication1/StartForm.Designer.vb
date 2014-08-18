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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListViewUsers = New System.Windows.Forms.ListView()
        Me.NameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.StatusHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ComputerNameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.IpHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ScanProgress = New System.Windows.Forms.ProgressBar()
        Me.BtnChat = New System.Windows.Forms.Button()
        Me.BtnIgnore = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextRightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ColourToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FontToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColourPanel = New System.Windows.Forms.Panel()
        Me.BtnFont = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ListViewGroups = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextRightClick.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(89, 50)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "&Search for other users"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListViewUsers
        '
        Me.ListViewUsers.AllowColumnReorder = True
        Me.ListViewUsers.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListViewUsers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameHeader, Me.StatusHeader, Me.ComputerNameHeader, Me.IpHeader})
        Me.ListViewUsers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewUsers.FullRowSelect = True
        Me.ListViewUsers.Location = New System.Drawing.Point(3, 3)
        Me.ListViewUsers.Name = "ListViewUsers"
        Me.ListViewUsers.Size = New System.Drawing.Size(315, 160)
        Me.ListViewUsers.TabIndex = 1
        Me.ListViewUsers.UseCompatibleStateImageBehavior = False
        Me.ListViewUsers.View = System.Windows.Forms.View.Details
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
        Me.BtnChat.Text = "&Open Chat"
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
        Me.BtnIgnore.Text = "&Ignore"
        Me.ToolTip1.SetToolTip(Me.BtnIgnore, "Ignore and hide conversation")
        Me.BtnIgnore.UseVisualStyleBackColor = True
        Me.BtnIgnore.Visible = False
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.ContextMenuStrip = Me.TextRightClick
        Me.TextBox1.Location = New System.Drawing.Point(12, 209)
        Me.TextBox1.MaxLength = 100
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(89, 20)
        Me.TextBox1.TabIndex = 6
        Me.TextBox1.Text = "New User"
        Me.ToolTip1.SetToolTip(Me.TextBox1, "The personal Username shown to other Users")
        '
        'TextRightClick
        '
        Me.TextRightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ColourToolStripMenuItem, Me.FontToolStripMenuItem})
        Me.TextRightClick.Name = "TextRightClick"
        Me.TextRightClick.Size = New System.Drawing.Size(111, 48)
        '
        'ColourToolStripMenuItem
        '
        Me.ColourToolStripMenuItem.Name = "ColourToolStripMenuItem"
        Me.ColourToolStripMenuItem.Size = New System.Drawing.Size(110, 22)
        Me.ColourToolStripMenuItem.Text = "Colour"
        '
        'FontToolStripMenuItem
        '
        Me.FontToolStripMenuItem.Name = "FontToolStripMenuItem"
        Me.FontToolStripMenuItem.Size = New System.Drawing.Size(110, 22)
        Me.FontToolStripMenuItem.Text = "Font"
        '
        'ColourPanel
        '
        Me.ColourPanel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ColourPanel.BackColor = System.Drawing.Color.Black
        Me.ColourPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ColourPanel.Location = New System.Drawing.Point(15, 255)
        Me.ColourPanel.Name = "ColourPanel"
        Me.ColourPanel.Size = New System.Drawing.Size(20, 20)
        Me.ColourPanel.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.ColourPanel, "Click to change the colour of you nickname")
        '
        'BtnFont
        '
        Me.BtnFont.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnFont.Location = New System.Drawing.Point(62, 255)
        Me.BtnFont.Name = "BtnFont"
        Me.BtnFont.Size = New System.Drawing.Size(20, 20)
        Me.BtnFont.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.BtnFont, "Click to change the font of your nickname")
        Me.BtnFont.UseVisualStyleBackColor = True
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
        Me.TabPage1.Controls.Add(Me.ListViewUsers)
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
        Me.TabPage2.Controls.Add(Me.ListViewGroups)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(321, 166)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Group Chats"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ListViewGroups
        '
        Me.ListViewGroups.AllowColumnReorder = True
        Me.ListViewGroups.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListViewGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListViewGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewGroups.FullRowSelect = True
        Me.ListViewGroups.Location = New System.Drawing.Point(3, 3)
        Me.ListViewGroups.Name = "ListViewGroups"
        Me.ListViewGroups.Size = New System.Drawing.Size(315, 160)
        Me.ListViewGroups.TabIndex = 2
        Me.ListViewGroups.UseCompatibleStateImageBehavior = False
        Me.ListViewGroups.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 101
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Users"
        Me.ColumnHeader3.Width = 115
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Host"
        Me.ColumnHeader4.Width = 79
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 193)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Nickname:"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 239)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Colour:"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(59, 239)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Font:"
        '
        'StartForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(448, 285)
        Me.Controls.Add(Me.BtnFont)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ColourPanel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.BtnIgnore)
        Me.Controls.Add(Me.BtnChat)
        Me.Controls.Add(Me.ScanProgress)
        Me.Controls.Add(Me.Button1)
        Me.MinimumSize = New System.Drawing.Size(250, 250)
        Me.Name = "StartForm"
        Me.Text = "Chatter Box"
        Me.TextRightClick.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListViewUsers As System.Windows.Forms.ListView
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
    Friend WithEvents ListViewGroups As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ColourPanel As System.Windows.Forms.Panel
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents TextRightClick As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ColourToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FontToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents BtnFont As System.Windows.Forms.Button

End Class
