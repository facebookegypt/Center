Public Class Form1
    Dim IC As New Class1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 1000
        Timer1.Enabled = True
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        BackgroundImageLayout = ImageLayout.Stretch
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main1.jpg"), True)
    End Sub
    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Text = Now.ToString
    End Sub
    Private Sub MnuFilExit_Click(sender As Object, e As EventArgs) Handles MnuFilExit.Click
        Close()
    End Sub
    Private Sub MnuSt1_Click(sender As Object, e As EventArgs) Handles MnuSt1.Click
        IsMdiContainer = True
        With Form2
            .MdiParent = Me
            .WindowState = FormWindowState.Maximized
            .ShowIcon = True
            .Show()
            .Invalidate(True)
        End With
    End Sub
End Class
