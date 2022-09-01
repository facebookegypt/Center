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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MnuFil = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuFilBak = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuFilExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuSt = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuSt1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuStRp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuFilBacSav = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Transparent
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.Right
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuFil, Me.MnuSt})
        Me.MenuStrip1.Location = New System.Drawing.Point(674, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(126, 450)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MnuFil
        '
        Me.MnuFil.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuFilBak, Me.ToolStripMenuItem1, Me.MnuFilBacSav, Me.ToolStripMenuItem2, Me.MnuFilExit})
        Me.MnuFil.Image = Global.WindowsApp1.My.Resources.Resources.Icon1
        Me.MnuFil.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuFil.Name = "MnuFil"
        Me.MnuFil.Size = New System.Drawing.Size(113, 36)
        Me.MnuFil.Text = "ملف"
        Me.MnuFil.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'MnuFilBak
        '
        Me.MnuFilBak.Name = "MnuFilBak"
        Me.MnuFilBak.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MnuFilBak.ShortcutKeyDisplayString = "F3"
        Me.MnuFilBak.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.MnuFilBak.Size = New System.Drawing.Size(220, 22)
        Me.MnuFilBak.Text = "إعدادات النسخة الاحتياطية"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(217, 6)
        '
        'MnuFilExit
        '
        Me.MnuFilExit.Name = "MnuFilExit"
        Me.MnuFilExit.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MnuFilExit.ShortcutKeyDisplayString = "Esc"
        Me.MnuFilExit.Size = New System.Drawing.Size(220, 22)
        Me.MnuFilExit.Text = "خروج"
        '
        'MnuSt
        '
        Me.MnuSt.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuSt1, Me.ToolStripMenuItem3, Me.MnuStRp})
        Me.MnuSt.Image = Global.WindowsApp1.My.Resources.Resources.Stdnts
        Me.MnuSt.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.MnuSt.Name = "MnuSt"
        Me.MnuSt.Size = New System.Drawing.Size(113, 36)
        Me.MnuSt.Text = "الطلاب"
        Me.MnuSt.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'MnuSt1
        '
        Me.MnuSt1.Name = "MnuSt1"
        Me.MnuSt1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MnuSt1.ShortcutKeyDisplayString = "F3"
        Me.MnuSt1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.MnuSt1.Size = New System.Drawing.Size(180, 22)
        Me.MnuSt1.Text = "الاعدادات"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(177, 6)
        '
        'MnuStRp
        '
        Me.MnuStRp.Name = "MnuStRp"
        Me.MnuStRp.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MnuStRp.ShortcutKeyDisplayString = "F4"
        Me.MnuStRp.Size = New System.Drawing.Size(180, 22)
        Me.MnuStRp.Text = "التقارير"
        '
        'MnuFilBacSav
        '
        Me.MnuFilBacSav.Name = "MnuFilBacSav"
        Me.MnuFilBacSav.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.MnuFilBacSav.Size = New System.Drawing.Size(220, 22)
        Me.MnuFilBacSav.Text = "حفظ فى مجلد"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(217, 6)
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Timer1 As Timer
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents MnuFil As ToolStripMenuItem
    Friend WithEvents MnuFilExit As ToolStripMenuItem
    Friend WithEvents MnuFilBak As ToolStripMenuItem
    Friend WithEvents MnuSt As ToolStripMenuItem
    Friend WithEvents MnuSt1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripSeparator
    Friend WithEvents MnuStRp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents MnuFilBacSav As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
End Class
