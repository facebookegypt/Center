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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuFilSav = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuFilExi = New System.Windows.Forms.ToolStripMenuItem()
        Me.StudentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuGr = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripComboBox3 = New System.Windows.Forms.ToolStripComboBox()
        Me.MnuMrks = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMarks = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuAllRpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuRpt1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip2
        '
        Me.MenuStrip2.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip2.Dock = System.Windows.Forms.DockStyle.Right
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.StudentsToolStripMenuItem, Me.GroupsToolStripMenuItem, Me.TimesToolStripMenuItem, Me.ToolStripComboBox3, Me.MnuMrks, Me.MnuAllRpt})
        Me.MenuStrip2.Location = New System.Drawing.Point(691, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MenuStrip2.Size = New System.Drawing.Size(109, 450)
        Me.MenuStrip2.TabIndex = 1
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.FileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QToolStripMenuItem, Me.MnuFilSav, Me.ToolStripMenuItem5, Me.MnuFilExi})
        Me.FileToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.FileToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.FileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(96, 37)
        Me.FileToolStripMenuItem.Text = "ملف"
        '
        'QToolStripMenuItem
        '
        Me.QToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.QToolStripMenuItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.QToolStripMenuItem.ForeColor = System.Drawing.Color.SeaGreen
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
        Me.StudentsToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.StudentsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.StudentsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SToolStripMenuItem})
        Me.StudentsToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.StudentsToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.StudentsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.StudentsToolStripMenuItem.Name = "StudentsToolStripMenuItem"
        Me.StudentsToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.StudentsToolStripMenuItem.Size = New System.Drawing.Size(96, 37)
        Me.StudentsToolStripMenuItem.Text = "الطلاب"
        '
        'SToolStripMenuItem
        '
        Me.SToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.SToolStripMenuItem.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.SToolStripMenuItem.ForeColor = System.Drawing.Color.SeaGreen
        Me.SToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SToolStripMenuItem.Name = "SToolStripMenuItem"
        Me.SToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SToolStripMenuItem.Text = "اعدادات الطلاب"
        Me.SToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupsToolStripMenuItem
        '
        Me.GroupsToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.GroupsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.GroupsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuGr})
        Me.GroupsToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.GroupsToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.GroupsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.GroupsToolStripMenuItem.Name = "GroupsToolStripMenuItem"
        Me.GroupsToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupsToolStripMenuItem.Size = New System.Drawing.Size(96, 37)
        Me.GroupsToolStripMenuItem.Text = "المجموعات"
        '
        'MnuGr
        '
        Me.MnuGr.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuGr.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuGr.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuGr.Name = "MnuGr"
        Me.MnuGr.Size = New System.Drawing.Size(180, 22)
        Me.MnuGr.Text = "اعدادات المجموعات"
        Me.MnuGr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TimesToolStripMenuItem
        '
        Me.TimesToolStripMenuItem.BackColor = System.Drawing.Color.Transparent
        Me.TimesToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.TimesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SToolStripMenuItem1})
        Me.TimesToolStripMenuItem.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.TimesToolStripMenuItem.ForeColor = System.Drawing.Color.Green
        Me.TimesToolStripMenuItem.Name = "TimesToolStripMenuItem"
        Me.TimesToolStripMenuItem.Padding = New System.Windows.Forms.Padding(6)
        Me.TimesToolStripMenuItem.Size = New System.Drawing.Size(96, 37)
        Me.TimesToolStripMenuItem.Text = "الغياب"
        '
        'SToolStripMenuItem1
        '
        Me.SToolStripMenuItem1.BackColor = System.Drawing.Color.Transparent
        Me.SToolStripMenuItem1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.SToolStripMenuItem1.ForeColor = System.Drawing.Color.SeaGreen
        Me.SToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SToolStripMenuItem1.Name = "SToolStripMenuItem1"
        Me.SToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.SToolStripMenuItem1.Text = "تسجيل الغياب"
        Me.SToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Me.ToolStripComboBox3.Size = New System.Drawing.Size(94, 27)
        Me.ToolStripComboBox3.Sorted = True
        '
        'MnuMrks
        '
        Me.MnuMrks.BackColor = System.Drawing.Color.Transparent
        Me.MnuMrks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.MnuMrks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuMarks})
        Me.MnuMrks.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.MnuMrks.ForeColor = System.Drawing.Color.Green
        Me.MnuMrks.Name = "MnuMrks"
        Me.MnuMrks.Padding = New System.Windows.Forms.Padding(6)
        Me.MnuMrks.Size = New System.Drawing.Size(96, 37)
        Me.MnuMrks.Text = "الدرجات"
        '
        'MnuMarks
        '
        Me.MnuMarks.BackColor = System.Drawing.Color.Transparent
        Me.MnuMarks.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold)
        Me.MnuMarks.ForeColor = System.Drawing.Color.SeaGreen
        Me.MnuMarks.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuMarks.Name = "MnuMarks"
        Me.MnuMarks.Size = New System.Drawing.Size(180, 22)
        Me.MnuMarks.Text = "اعدادات الدرجات"
        '
        'MnuAllRpt
        '
        Me.MnuAllRpt.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.MnuAllRpt.BackColor = System.Drawing.Color.Transparent
        Me.MnuAllRpt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.MnuAllRpt.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuRpt1})
        Me.MnuAllRpt.Font = New System.Drawing.Font("Arial", 13.0!, System.Drawing.FontStyle.Bold)
        Me.MnuAllRpt.ForeColor = System.Drawing.Color.Green
        Me.MnuAllRpt.Name = "MnuAllRpt"
        Me.MnuAllRpt.Padding = New System.Windows.Forms.Padding(6)
        Me.MnuAllRpt.Size = New System.Drawing.Size(96, 37)
        Me.MnuAllRpt.Text = "تقارير مجمعة"
        '
        'MnuRpt1
        '
        Me.MnuRpt1.Name = "MnuRpt1"
        Me.MnuRpt1.Size = New System.Drawing.Size(226, 26)
        Me.MnuRpt1.Text = "جدول مواعيد المجموعات"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.WindowsApp1.My.Resources.Resources.Main1
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MenuStrip2)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
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
    Friend WithEvents MnuMrks As ToolStripMenuItem
    Friend WithEvents MnuMarks As ToolStripMenuItem
    Friend WithEvents MnuAllRpt As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripSeparator
    Friend WithEvents ToolStripComboBox3 As ToolStripComboBox
    Friend WithEvents MnuRpt1 As ToolStripMenuItem
End Class
