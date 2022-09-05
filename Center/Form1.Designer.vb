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
        Me.MnuFilExi = New System.Windows.Forms.ToolStripMenuItem()
        Me.StudentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuGr = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'MenuStrip2
        '
        Me.MenuStrip2.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip2.Dock = System.Windows.Forms.DockStyle.Right
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.StudentsToolStripMenuItem, Me.GroupsToolStripMenuItem, Me.TimesToolStripMenuItem})
        Me.MenuStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuStrip2.Location = New System.Drawing.Point(674, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Size = New System.Drawing.Size(126, 450)
        Me.MenuStrip2.TabIndex = 1
        Me.MenuStrip2.Text = "MenuStrip2"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QToolStripMenuItem, Me.MnuFilSav, Me.MnuFilExi})
        Me.FileToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Icon1
        Me.FileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(113, 36)
        Me.FileToolStripMenuItem.Text = "ملف"
        Me.FileToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'QToolStripMenuItem
        '
        Me.QToolStripMenuItem.Name = "QToolStripMenuItem"
        Me.QToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.QToolStripMenuItem.Text = "إعدادات النسخة الإحتياطية"
        Me.QToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MnuFilSav
        '
        Me.MnuFilSav.Name = "MnuFilSav"
        Me.MnuFilSav.Size = New System.Drawing.Size(201, 22)
        Me.MnuFilSav.Text = "حفظ في مجلد"
        Me.MnuFilSav.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MnuFilExi
        '
        Me.MnuFilExi.Name = "MnuFilExi"
        Me.MnuFilExi.Size = New System.Drawing.Size(201, 22)
        Me.MnuFilExi.Text = "خروج"
        Me.MnuFilExi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StudentsToolStripMenuItem
        '
        Me.StudentsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SToolStripMenuItem})
        Me.StudentsToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Stdnts
        Me.StudentsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.StudentsToolStripMenuItem.Name = "StudentsToolStripMenuItem"
        Me.StudentsToolStripMenuItem.Size = New System.Drawing.Size(113, 36)
        Me.StudentsToolStripMenuItem.Text = "الطلاب"
        Me.StudentsToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SToolStripMenuItem
        '
        Me.SToolStripMenuItem.Name = "SToolStripMenuItem"
        Me.SToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.SToolStripMenuItem.Text = "اعدادات الطلاب"
        Me.SToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupsToolStripMenuItem
        '
        Me.GroupsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuGr})
        Me.GroupsToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Icon3
        Me.GroupsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.GroupsToolStripMenuItem.Name = "GroupsToolStripMenuItem"
        Me.GroupsToolStripMenuItem.Size = New System.Drawing.Size(113, 36)
        Me.GroupsToolStripMenuItem.Text = "المجموعات"
        Me.GroupsToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MnuGr
        '
        Me.MnuGr.Name = "MnuGr"
        Me.MnuGr.Size = New System.Drawing.Size(171, 22)
        Me.MnuGr.Text = "اعدادات المجموعات"
        Me.MnuGr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TimesToolStripMenuItem
        '
        Me.TimesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SToolStripMenuItem1})
        Me.TimesToolStripMenuItem.Image = Global.WindowsApp1.My.Resources.Resources.Time
        Me.TimesToolStripMenuItem.Name = "TimesToolStripMenuItem"
        Me.TimesToolStripMenuItem.Size = New System.Drawing.Size(113, 36)
        Me.TimesToolStripMenuItem.Text = "الغياب"
        Me.TimesToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SToolStripMenuItem1
        '
        Me.SToolStripMenuItem1.Name = "SToolStripMenuItem1"
        Me.SToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.SToolStripMenuItem1.Text = "اعدادات المواعيد"
        Me.SToolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
End Class
