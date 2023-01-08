Imports System.Data.OleDb

Public Class Form1
    Private Property Ii As New Class1
    Private Property Constr As String = Ii.ConStr
    Public Property CurrentAttndMonth As Integer
    Public Property CurrentAttndMonthNm As String
    Private Function GrDtToday() As Integer
        Dim SqlStr As String = "SELECT COUNT(GrDtID) FROM GrDt WHERE Month(GrDt.Mnm)=? AND GrDt.GrDt1=?;"
        Dim N As Integer = Nothing
        Try
            Using CN = New OleDbConnection With {.ConnectionString = Constr},
                CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                CN.Open()
                With CMD.Parameters
                    .AddWithValue("?", Month(Now.Date))
                    .AddWithValue("?", Now.Date)
                End With
                N = Convert.ToInt32(CMD.ExecuteScalar)
                CMD.Parameters.Clear()
                CMD.Dispose()
                Return N
            End Using
        Catch ex As OleDbException
            Return 0
            MsgBox("مشكلة فى التعرف علي قاعدة البيانات : " & vbCrLf & ex.Message,
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
            Class2.ShowDialog()
        End Try
    End Function
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        RightToLeft = RightToLeft.Yes
        'Timer1.Interval = 1000
        'Timer1.Enabled = True
        DoubleBuffered = True
        WindowState = FormWindowState.Maximized
        If Not IsNothing(My.Settings.LstBckPDt) And My.Settings.BackUpSet <> 0 And My.Settings.LocalBackUpFolder.Length >= 1 Then
            If My.Settings.BackUpSet = 1 Then  'Day
                If DateDiff(DateInterval.Day, Now.Date, My.Settings.LstBckPDt) > 2 Then
                    Dim I As Date = Ii.LstUpdt(My.Settings.LocalBackUpFolder)
                End If
            ElseIf My.Settings.BackUpSet = 2 Then  'Month
                If DateDiff(DateInterval.Day, Now.Date, My.Settings.LstBckPDt) > 2 Then
                    Dim I As Date = Ii.LstUpdt(My.Settings.LocalBackUpFolder)
                End If
            End If
        End If
        With Ii.PopupNotifier1
            .ShowCloseButton = True
            .ShowOptionsButton = True
            .TitleFont = New Font("Times New Roman", 12, FontStyle.Bold)
            .TitleColor = Color.Black
            .TitlePadding = New Padding(5)
            .ContentFont = New Font("Arial", 10.25, FontStyle.Bold)
            .ContentColor = Color.White
            .IsRightToLeft = True
            .TitleText = "مرحبا فى برنامج C E N T E R"
            .Delay = 5000
            .BorderColor = Color.Red
            .HeaderColor = Color.Red
            .BodyColor = Color.DeepSkyBlue
            .GradientPower = 10
            .ContentText = "السلام عليكم و رحمة الله و بركاته. لديك  " & GrDtToday() & " مجموعات اليوم."
            .Popup()
        End With
        Dim ComboItems As New Dictionary(Of Integer, String)
        Dim Dis As New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Dim SqlStr As String = "SELECT First(GrDt.GrDtID) AS FirstOfGrDtID, Format([Mnm],'mmmmyyyy') AS Expr1 FROM GrDt GROUP BY " &
            "Format([Mnm],'mmmmyyyy') ORDER BY Format([Mnm],'mmmmyyyy') DESC;"
        Dis = Ii.GetData(SqlStr)
        ComboItems.Clear()
        For Each DtR As DataRow In Dis.Rows
            ComboItems.Add(DtR("FirstOfGrDtID"), DtR("Expr1"))
        Next
        ComboItems.Add(0, "اختر الشهر")
        ToolStripComboBox3.ComboBox.BeginUpdate()
        With ToolStripComboBox3
            .ComboBox.DataSource = New BindingSource(ComboItems, Nothing)
            .ComboBox.DisplayMember = "Value"
            .ComboBox.ValueMember = "Key"
            .Sorted = True
            .SelectedItem = {0, "اختر الشهر"}
            .ComboBox.EndUpdate()
        End With
        ComboItems.Clear()
        ComboItems = Nothing
        Dis.Dispose()
        AddHandler ToolStripComboBox3.ComboBox.DropDown, AddressOf Tlstrp_DropDown
        AddHandler ToolStripComboBox3.ComboBox.DropDownClosed, AddressOf Tlstrp_DropDownClossed
    End Sub
    Private Sub Tlstrp_DropDownClossed(sender As Object, e As EventArgs)
        Dim Cbo As ComboBox = CType(sender, ComboBox)
        RemoveHandler ToolStripComboBox3.ComboBox.SelectionChangeCommitted, AddressOf Tlstrp_Slctioncom
    End Sub
    Private Sub Tlstrp_DropDown(sender As Object, e As EventArgs)
        Dim Cbo As ComboBox = CType(sender, ComboBox)
        AddHandler ToolStripComboBox3.ComboBox.SelectionChangeCommitted, AddressOf Tlstrp_Slctioncom
    End Sub
    Private Sub Tlstrp_Slctioncom(sender As Object, e As EventArgs)
        Dim Cbo As ComboBox = CType(sender, ComboBox)
        If DirectCast(Cbo.SelectedItem, KeyValuePair(Of Integer, String)).Key = 0 Then Exit Sub
        CurrentAttndMonthNm = DirectCast(Cbo.SelectedItem, KeyValuePair(Of Integer, String)).Value.ToString
        CurrentAttndMonth = DirectCast(Cbo.SelectedItem, KeyValuePair(Of Integer, String)).Key

        'Dim Frm9 As New Form
        'IsMdiContainer = True
        For Each Frm As Form In Application.OpenForms
            If Frm.Text.Contains(CurrentAttndMonthNm) Then
                Frm.Visible = True
                Frm.BringToFront()
                Exit Sub
            End If
        Next
        Dim Frm9 As New Form
        Frm9 = Form9
        Frm9.Show(Me)
    End Sub
    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Dim Iu As Class1 = New Class1
        Dim BackUpFolder, BackUpPath As String
        Dim streamRead As IO.FileStream, streamWrite As IO.FileStream
        Dim lngLen As Long, byteBuffer(1048576) As Byte   'our stream buffer
        Try
            BackUpFolder = My.Settings.LocalBackUpFolder
            Iu.CompRepair(BackUpFolder)
            'Create the file stream for the source file
            BackUpPath = BackUpFolder
            'Exception: if Database file is open then an error occures 'File is being used by another process" 01Sept2022
            streamRead = New IO.FileStream(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm), IO.FileMode.Open)
            'Create the file stream for the destination file
            streamWrite =
                New IO.FileStream(IO.Path.Combine(BackUpFolder, "BackUp--" &
                                                  Now.Date.ToShortDateString.Replace("/", "_") & ".accdb.bak"),
                                              IO.FileMode.Create)
            'Determine the size in bytes of the source file (-1 as our position starts at 0)
            lngLen = streamRead.Length - 1
            Dim intBytesRead As Integer    'number of bytes read
            While streamRead.Position < lngLen    'keep streaming until EOF
                'Read from the Source
                intBytesRead = (streamRead.Read(byteBuffer, 0, 1048576))
                'Write to the Target
                streamWrite.Write(byteBuffer, 0, intBytesRead)
                'Display the progress
                'ToolStripProgressBar1.Value = CInt(streamRead.Position / lngLen * 100)
                Application.DoEvents()    'do it
            End While
            'Clean up 
            streamWrite.Flush()
            streamWrite.Close()
            streamRead.Close()
            RemoveHandler ToolStripComboBox3.ComboBox.DropDown, AddressOf Tlstrp_DropDown
            RemoveHandler ToolStripComboBox3.ComboBox.DropDownClosed, AddressOf Tlstrp_DropDownClossed
        Catch ex As Exception
            MsgBox("Closing : " & ex.Message)
        End Try
    End Sub
    Private Sub SToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SToolStripMenuItem.Click
        If Form2.Visible = True Then
            Form2.BringToFront()
            Exit Sub
        End If
        Dim Frm2 As New Form
        Frm2 = Form2
        Frm2.Show(Me)
    End Sub
    Private Sub MnuFilSav_Click(sender As Object, e As EventArgs) Handles MnuFilSav.Click
        Dim Iu As Class1 = New Class1
        Dim BackUpFolder As String = Iu.OfdOpn("اختر مجلد حفظ نسخة احتياطية")
        If String.IsNullOrEmpty(BackUpFolder) Then
            Exit Sub
        End If
        Iu.CompRepair(BackUpFolder)
        'Create the file stream for the source file
        Using StrmRdr As New IO.FileStream(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm), IO.FileMode.Open),
            StrmWrtr As New IO.FileStream(IO.Path.Combine(BackUpFolder,
                                                            "BackUp--" & Now.Date.ToShortDateString.Replace("/", "_") & ".accdb.bak"),
                                            IO.FileMode.Create)
            Dim lngLen As Long = StrmRdr.Length - 1
            Dim byteBuffer(1048576) As Byte   'our stream buffer
            Dim intBytesRead As Integer    'number of bytes read
            While StrmRdr.Position < lngLen    'keep streaming until EOF
                'Read from the Source
                intBytesRead = (StrmRdr.Read(byteBuffer, 0, 1048576))
                StrmWrtr.Write(byteBuffer, 0, intBytesRead)
                Application.DoEvents()    'do it
            End While
        End Using
        MsgBox("تم تصدير النسخة بنجاح.", MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
    End Sub
    Private Sub QToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QToolStripMenuItem.Click
        If Form3.Visible = True Then
            Form3.BringToFront()
            Exit Sub
        End If
        'IsMdiContainer = True
        Dim Frm3 As New Form
        Frm3 = Form3
        Frm3.Show(Me)
    End Sub
    Private Sub MnuFilExi_Click(sender As Object, e As EventArgs) Handles MnuFilExi.Click
        Close()
    End Sub
    Private Sub MnuGr_Click(sender As Object, e As EventArgs) Handles MnuGr.Click
        If Form4.Visible = True Then
            Form4.BringToFront()
            Exit Sub
        End If
        'IsMdiContainer = True
        Dim Frm4 As New Form
        Frm4 = Form4
        Frm4.Show(Me)
    End Sub
    Private Sub SToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SToolStripMenuItem1.Click
        If Form8.Visible = True Then
            Form8.BringToFront()
            Exit Sub
        End If
        Dim Frm8 As New Form
        Frm8 = Form8
        Frm8.Show(Me)
    End Sub
    Private Sub MnuMarks_Click(sender As Object, e As EventArgs) Handles MnuMarks.Click
        If Form7.Visible Then
            Form7.BringToFront()
            Exit Sub
        End If
        Dim Frm7 As New Form
        Frm7 = Form7
        Frm7.Show(Me)
    End Sub
    Private Sub ToolStripComboBox3_DropDownClosed(sender As Object, e As EventArgs) Handles ToolStripComboBox3.DropDownClosed
        ActiveControl = Nothing
        Enabled = True
        Activate()
    End Sub
    Private Sub MnuRpt1_Click(sender As Object, e As EventArgs) Handles MnuRpt1.Click
        Dim SqlStrDel As String =
            "DROP VIEW GrpsDtsRpt;"
        Dim SqlStrCreate As String =
            "CREATE VIEW GrpsDtsRpt AS SELECT Grps.GrID, Grps.GrNm, Grps.Lnm, Grps.SubNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2 FROM " &
            "Grps INNER JOIN (GrSt INNER JOIN GrDt ON GrSt.GrID = GrDt.GrID) ON Grps.GrID = GrSt.GrID GROUP BY Grps.GrID, Grps.GrNm, " &
            "Grps.Lnm, Grps.SubNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2;"
        Using CN As New OleDbConnection(Constr),
                    CMDDel As New OleDbCommand(SqlStrDel, CN) With {.CommandType = CommandType.Text},
                    CMDCREATE As New OleDbCommand(SqlStrCreate, CN) With {.CommandType = CommandType.Text}
            Try
                CN.Open()
                CMDDel.ExecuteNonQuery()
                CMDCREATE.ExecuteNonQuery()
            Catch ex As OleDbException
                CMDCREATE.ExecuteNonQuery()
            End Try
        End Using

        Form10.SrcFrm = "GrpsDtsRpt"
        Form10.ShowDialog()
    End Sub
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Dispose(True)
    End Sub
End Class
