Public Class Form3
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        BackgroundImageLayout = ImageLayout.Center
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main3.png"), True)
        TxtNm.Select()
        TxtNm.Text = My.Settings.LocalBackUpFolder
        TxtNm.ReadOnly = True
    End Sub
    Private Sub Form3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Dim Iu As Class1 = New Class1
        TxtNm.Text = Iu.OfdOpn("من فضلك أختر مجلد لحفظ النسخة الاحتياطية.")
    End Sub

    Private Sub Form3_Click(sender As Object, e As EventArgs) Handles Me.Click
        Class2.Show()
    End Sub
End Class