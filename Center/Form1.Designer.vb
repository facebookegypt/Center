<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuFilSav = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuFilExi = New System.Windows.Forms.ToolStripMenuItem()
        Me.StudentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.تقاريرالطلابToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuGr = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.تقاريرالمجموعاتToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripComboBox3 = New System.Windows.Forms.ToolStripComboBox()
        Me.تسجيلالدرحاتToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMarks = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuMrkRpts = New System.Windows.Forms.ToolStripMenuItem()
        Me.تقاريرمجمعةToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'MenuStrip2
        '
        Me.MenuStrip2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.MenuStrip2.Dock = System.Windows.Forms.DockStyle.Right
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.StudentsToolStripMenuItem, Me.GroupsToolStripMenuItem, Me.TimesToolStripMenuItem, Me.ToolStripComboBox3, Me.تسجيلالدرحاتToolStripMenuItem, Me.تقاريرمجمعةToolStripMenuItem})
        Me.MenuStrip2.Location = New System.Drawing.Point(678, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MenuStrip2.Size = New System.Drawing.Size(122, 450)
        Me.MenuStrip2.TabIndex = 1
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QToolStripMenuItem, Me.MnuFilSav, Me.ToolStripMenuItem5, Me.MnuFilExi})
        Me.FileToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.FileToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.FileToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Icon1
        Me.FileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(109, 69)
        Me.FileToolStripMenuItem.Text = "ملف"
        Me.FileToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.FileToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'QToolStripMenuItem
        '
        Me.QToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.QToolStripMenuItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.QToolStripMenuItem.ForeColor = System.Drawing.Color.SeaGreen
        Me.QToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.account_menu
        Me.QToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.QToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.QToolStripMenuItem.Name = "QToolStripMenuItem"
        Me.QToolStripMenuItem.Padding = New System.Windows.Forms.Padding(0, 4, 0, 4)
        Me.QToolStripMenuItem.Size = New System.Drawing.Size(239, 28)
        Me.QToolStripMenuItem.Text = "إعدادات النسخة الإحتياطية"
        Me.QToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MnuFilSav
        '
        Me.MnuFilSav.BackColor = System.Drawing.Color.Transparent
        Me.MnuFilSav.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuFilSav.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuFilSav.Image = Global.WindowsApp1.My.Resources.Resources.folder_add
        Me.MnuFilSav.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.MnuFilSav.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuFilSav.Name = "MnuFilSav"
        Me.MnuFilSav.Padding = New System.Windows.Forms.Padding(0, 4, 0, 4)
        Me.MnuFilSav.ShortcutKeyDisplayString = "Ctrl+Shift+S"
        Me.MnuFilSav.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.MnuFilSav.Size = New System.Drawing.Size(239, 28)
        Me.MnuFilSav.Text = "حفظ في مجلد"
        Me.MnuFilSav.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(236, 6)
        '
        'MnuFilExi
        '
        Me.MnuFilExi.BackColor = System.Drawing.Color.Transparent
        Me.MnuFilExi.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuFilExi.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuFilExi.Image = Global.WindowsApp1.My.Resources.Resources.door_out
        Me.MnuFilExi.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.MnuFilExi.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuFilExi.Name = "MnuFilExi"
        Me.MnuFilExi.Padding = New System.Windows.Forms.Padding(0, 4, 0, 4)
        Me.MnuFilExi.ShortcutKeyDisplayString = "Esc"
        Me.MnuFilExi.Size = New System.Drawing.Size(239, 28)
        Me.MnuFilExi.Text = "خروج"
        Me.MnuFilExi.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'StudentsToolStripMenuItem
        '
        Me.StudentsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SToolStripMenuItem, Me.ToolStripMenuItem1, Me.تقاريرالطلابToolStripMenuItem})
        Me.StudentsToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.StudentsToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.StudentsToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Stdnts
        Me.StudentsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.StudentsToolStripMenuItem.Name = "StudentsToolStripMenuItem"
        Me.StudentsToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.StudentsToolStripMenuItem.Size = New System.Drawing.Size(109, 69)
        Me.StudentsToolStripMenuItem.Text = "الطلاب"
        Me.StudentsToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.StudentsToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'SToolStripMenuItem
        '
        Me.SToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.SToolStripMenuItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.SToolStripMenuItem.ForeColor = System.Drawing.Color.SeaGreen
        Me.SToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.group_gear
        Me.SToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SToolStripMenuItem.Name = "SToolStripMenuItem"
        Me.SToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.SToolStripMenuItem.Text = "اعدادات الطلاب"
        Me.SToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(153, 6)
        '
        'تقاريرالطلابToolStripMenuItem
        '
        Me.تقاريرالطلابToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.تقاريرالطلابToolStripMenuItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.تقاريرالطلابToolStripMenuItem.ForeColor = System.Drawing.Color.SeaGreen
        Me.تقاريرالطلابToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.تقاريرالطلابToolStripMenuItem.Name = "تقاريرالطلابToolStripMenuItem"
        Me.تقاريرالطلابToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.تقاريرالطلابToolStripMenuItem.Text = "تقارير الطلاب"
        '
        'GroupsToolStripMenuItem
        '
        Me.GroupsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuGr, Me.ToolStripMenuItem2, Me.تقاريرالمجموعاتToolStripMenuItem})
        Me.GroupsToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.GroupsToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.GroupsToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Icon3
        Me.GroupsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.GroupsToolStripMenuItem.Name = "GroupsToolStripMenuItem"
        Me.GroupsToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupsToolStripMenuItem.Size = New System.Drawing.Size(109, 69)
        Me.GroupsToolStripMenuItem.Text = "المجموعات"
        Me.GroupsToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.GroupsToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'MnuGr
        '
        Me.MnuGr.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuGr.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuGr.Image = Global.WindowsApp1.My.Resources.Resources.table_gear
        Me.MnuGr.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuGr.Name = "MnuGr"
        Me.MnuGr.Size = New System.Drawing.Size(179, 22)
        Me.MnuGr.Text = "اعدادات المجموعات"
        Me.MnuGr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(176, 6)
        '
        'تقاريرالمجموعاتToolStripMenuItem
        '
        Me.تقاريرالمجموعاتToolStripMenuItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.تقاريرالمجموعاتToolStripMenuItem.ForeColor = System.Drawing.Color.SeaGreen
        Me.تقاريرالمجموعاتToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.تقاريرالمجموعاتToolStripMenuItem.Name = "تقاريرالمجموعاتToolStripMenuItem"
        Me.تقاريرالمجموعاتToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.تقاريرالمجموعاتToolStripMenuItem.Text = "تقارير المجموعات"
        '
        'TimesToolStripMenuItem
        '
        Me.TimesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SToolStripMenuItem1, Me.ToolStripMenuItem3})
        Me.TimesToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.TimesToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.TimesToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Time
        Me.TimesToolStripMenuItem.Name = "TimesToolStripMenuItem"
        Me.TimesToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.TimesToolStripMenuItem.Size = New System.Drawing.Size(109, 69)
        Me.TimesToolStripMenuItem.Text = "الغياب"
        Me.TimesToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.TimesToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'SToolStripMenuItem1
        '
        Me.SToolStripMenuItem1.BackColor = System.Drawing.Color.Transparent
        Me.SToolStripMenuItem1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.SToolStripMenuItem1.ForeColor = System.Drawing.Color.SeaGreen
        Me.SToolStripMenuItem1.Image = Global.WindowsApp1.My.Resources.Resources.page_gear
        Me.SToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SToolStripMenuItem1.Name = "SToolStripMenuItem1"
        Me.SToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.SToolStripMenuItem1.Text = "تسجيل الغياب"
        Me.SToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(177, 6)
        '
        'ToolStripComboBox3
        '
        Me.ToolStripComboBox3.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ToolStripComboBox3.ForeColor = System.Drawing.Color.ForestGreen
        Me.ToolStripComboBox3.MaxDropDownItems = 3
        Me.ToolStripComboBox3.MergeAction = System.Windows.Forms.MergeAction.Insert
        Me.ToolStripComboBox3.Name = "ToolStripComboBox3"
        Me.ToolStripComboBox3.Padding = New System.Windows.Forms.Padding(2)
        Me.ToolStripComboBox3.Size = New System.Drawing.Size(107, 27)
        Me.ToolStripComboBox3.Sorted = True
        '
        'تسجيلالدرحاتToolStripMenuItem
        '
        Me.تسجيلالدرحاتToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuMarks, Me.ToolStripMenuItem4, Me.MnuMrkRpts})
        Me.تسجيلالدرحاتToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.تسجيلالدرحاتToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.تسجيلالدرحاتToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.accept_button
        Me.تسجيلالدرحاتToolStripMenuItem.Name = "تسجيلالدرحاتToolStripMenuItem"
        Me.تسجيلالدرحاتToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.تسجيلالدرحاتToolStripMenuItem.Size = New System.Drawing.Size(109, 69)
        Me.تسجيلالدرحاتToolStripMenuItem.Text = "الدرجات"
        Me.تسجيلالدرحاتToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.تسجيلالدرحاتToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'MnuMarks
        '
        Me.MnuMarks.BackColor = System.Drawing.Color.Transparent
        Me.MnuMarks.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuMarks.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuMarks.Image = Global.WindowsApp1.My.Resources.Resources.script_gear
        Me.MnuMarks.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuMarks.Name = "MnuMarks"
        Me.MnuMarks.Size = New System.Drawing.Size(163, 22)
        Me.MnuMarks.Text = "اعدادات الدرجات"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(160, 6)
        '
        'MnuMrkRpts
        '
        Me.MnuMrkRpts.BackColor = System.Drawing.Color.Transparent
        Me.MnuMrkRpts.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuMrkRpts.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuMrkRpts.Name = "MnuMrkRpts"
        Me.MnuMrkRpts.Size = New System.Drawing.Size(163, 22)
        Me.MnuMrkRpts.Text = "تقارير الدرجات"
        '
        'تقاريرمجمعةToolStripMenuItem
        '
        Me.تقاريرمجمعةToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.تقاريرمجمعةToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.report_user
        Me.تقاريرمجمعةToolStripMenuItem.Name = "تقاريرمجمعةToolStripMenuItem"
        Me.تقاريرمجمعةToolStripMenuItem.Size = New System.Drawing.Size(109, 36)
        Me.تقاريرمجمعةToolStripMenuItem.Text = "تقارير مجمعة"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MenuStrip2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Timer1 As Timer
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents QToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StudentsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TimesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents MnuFilSav As ToolStripMenuItem
    Friend WithEvents MnuFilExi As ToolStripMenuItem
    Friend WithEvents MnuGr As ToolStripMenuItem
    Friend WithEvents تسجيلالدرحاتToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MnuMarks As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents تقاريرالطلابToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents تقاريرالمجموعاتToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents MnuMrkRpts As ToolStripMenuItem
    Friend WithEvents تقاريرمجمعةToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents ToolStripComboBox3 As ToolStripComboBox
End Class
