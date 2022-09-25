﻿Imports System.Data.OleDb

Public Class Form1
    Private Property Ii As New Class1
    Private Property Constr As String = Ii.ConStr
    Private Function GrDtToday() As Integer
        Dim SqlStr As String = "SELECT COUNT(GrDtID) FROM GrDt WHERE Month(GrDt.Mnm)=? AND GrDt.GrDt1=?;"
        Using CN = New OleDbConnection With {.ConnectionString = Constr},
                CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", Month(Now.Date))
            CMD.Parameters.AddWithValue("?", Now.Date)
            Try
                CN.Open()
            Catch ex As OleDbException
                MsgBox("مشكلة فى التعرف علي قاعدة البيانات : " & vbCrLf & ex.Message,
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
                Class2.ShowDialog()
            End Try
            Return Convert.ToInt32(CMD.ExecuteScalar)
        End Using
    End Function
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        RightToLeft = RightToLeft.Yes
        Timer1.Interval = 1000
        Timer1.Enabled = True
        DoubleBuffered = True
        WindowState = FormWindowState.Maximized
        BackgroundImage = My.Resources.ResourceManager.GetObject("Main1")
        LayoutMdi(MdiLayout.ArrangeIcons)
        IsMdiContainer = True
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
            .ContentText = "السلام عليكم و رحمة الله و بركاته. لديك  " & GrDtToday() & " مجموعات اليوم."
            .Popup()
        End With
    End Sub
    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Text = Now.ToString
    End Sub
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Dim Iu As Class1 = New Class1
        Dim BackUpFolder As String = My.Settings.LocalBackUpFolder
        Iu.CompRepair(BackUpFolder)
        'Create the file stream for the source file
        Dim BackUpPath As String = BackUpFolder
        'Exception: if Database file is open then an error occures 'File is being used by another process" 01Sept2022
        Dim streamRead As New IO.FileStream(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm), IO.FileMode.Open)
        'Create the file stream for the destination file
        Dim streamWrite As IO.FileStream =
            New IO.FileStream(IO.Path.Combine(BackUpFolder, "BackUp--" &
                                              Now.Date.ToShortDateString.Replace("/", "_") & ".accdb.bak"),
                                          IO.FileMode.Create)
        'Determine the size in bytes of the source file (-1 as our position starts at 0)
        Dim lngLen As Long = streamRead.Length - 1
        Dim byteBuffer(1048576) As Byte   'our stream buffer
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
    End Sub
    Private Sub SToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SToolStripMenuItem.Click
        If Form2.Visible = True Then Form2.BringToFront()
        IsMdiContainer = True
        With Form2
            .MdiParent = Me
            '.WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
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
        If Form3.Visible = True Then Form3.BringToFront()
        IsMdiContainer = True
        With Form3
            .MdiParent = Me
            ' .WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub
    Private Sub MnuFilExi_Click(sender As Object, e As EventArgs) Handles MnuFilExi.Click
        Close()
    End Sub
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        BackgroundImageLayout = ImageLayout.Stretch
        Invalidate(True)
        Update()
        Refresh()
    End Sub

    Private Sub MnuGr_Click(sender As Object, e As EventArgs) Handles MnuGr.Click
        If Form4.Visible = True Then
            Form4.BringToFront()
            Exit Sub
        End If
        IsMdiContainer = True
        With Form4
            .MdiParent = Me
            '.WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub

    Private Sub SToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SToolStripMenuItem1.Click
        If Form8.Visible = True Then
            Form8.BringToFront()
            Exit Sub
        End If
        IsMdiContainer = True
        Dim Frm7 As New Form8
        With Frm7
            .MdiParent = Me
            '.WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub

    Private Sub MnuMarks_Click(sender As Object, e As EventArgs) Handles MnuMarks.Click
        If Form7.Visible Then
            Form7.BringToFront()
            Exit Sub
        End If
        IsMdiContainer = True
        Dim Frm7 As New Form7
        With Frm7
            .MdiParent = Me
            '.WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub
End Class
