Public Class Form3
    Private Property BckPSt As Integer
    Private Property BckPd As Integer
    Private Property BckPdt As Date
    Private Property AreUSure As MsgBoxResult
    Private Sub Form3_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        DoubleBuffered = True
        WindowState = FormWindowState.Normal
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        BackgroundImageLayout = ImageLayout.Center
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main3.png"), True)
        TxtNm.Select()
        TxtNm.Text = My.Settings.LocalBackUpFolder
        TxtNm.ReadOnly = True
        BckPSt = My.Settings.BackUpSet
        RadioButton1.AutoCheck = True
        RadioButton2.AutoCheck = True
        If BckPSt = 0 Then
            RadioButton1.Checked = False
            RadioButton2.Checked = False
        ElseIf BckPSt = 1 Then
            RadioButton1.Checked = True
            RadioButton2.Checked = False
            '1 Day
            BckPd = 1
            'Previous Date
            BckPdt = Now.Date.AddDays(1)
        ElseIf BckPSt = 2 Then
            RadioButton1.Checked = False
            RadioButton2.Checked = True
            '30 Days
            BckPd = 30
            'Previous Date
            BckPdt = Now.Date
            BckPdt = Now.Date.AddDays(30)
        End If
        Dim Iu As Class1 = New Class1
        Dim I As Date
        If Not IsNothing(My.Settings.LstBckPDt) And My.Settings.BackUpSet <> 0 And My.Settings.LocalBackUpFolder.Length >= 1 Then
            If My.Settings.BackUpSet = 1 Then  'Day
                If DateDiff(DateInterval.Day, Now.Date, My.Settings.LstBckPDt) = 1 Then
                    I = Iu.LstUpdt(My.Settings.LocalBackUpFolder)
                Else
                    I = My.Settings.LstBckPDt
                End If
            ElseIf My.Settings.BackUpSet = 2 Then  'Month
                If DateDiff(DateInterval.Day, Now.Date, My.Settings.LstBckPDt) >= 30 Then
                    I = Iu.LstUpdt(My.Settings.LocalBackUpFolder)
                Else
                    I = My.Settings.LstBckPDt
                End If
            End If
            ToolStripStatusLabel2.Text = Format(I, "dddd, dd/MMMM/yyyy")
        End If
    End Sub
    Private Sub Form3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Label1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Label1.Click
        Dim Iu As Class1 = New Class1
        TxtNm.Text = Iu.OfdOpn("من فضلك أختر مجلد لحفظ النسخة الاحتياطية.")
    End Sub
    Private Sub RadioButton1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RadioButton1.MouseClick
        If RadioButton1.Checked = True Then
            My.Settings.BackUpSet = 1
            My.Settings.BackUpDt = Now.Date
            My.Settings.Save()
            Dim Iu As Class1 = New Class1
            AreUSure = MsgBox("هل ترغب فى تحديث النسخة الاحتياطية الأن؟",
                                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                               MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
            If AreUSure = MsgBoxResult.Yes Then
                ToolStripStatusLabel2.Text = Format(Iu.LstUpdt(TxtNm.Text), "dddd, dd/MMMM/yyyy")
            End If
        End If
    End Sub

    Private Sub RadioButton2_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RadioButton2.MouseClick
        If RadioButton2.Checked = True Then
            My.Settings.BackUpSet = 2
            My.Settings.BackUpDt = Now.Date
            My.Settings.Save()
            Dim Iu As Class1 = New Class1
            AreUSure = MsgBox("هل ترغب فى تحديث النسخة الاحتياطية الأن؟", _
                                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                               MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
            If AreUSure = MsgBoxResult.Yes Then
                ToolStripStatusLabel2.Text = Format(Iu.LstUpdt(TxtNm.Text), "dddd, dd/MMMM/yyyy hh:mm tt")
            End If
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

    End Sub
End Class