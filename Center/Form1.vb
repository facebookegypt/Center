Imports System.Data.OleDb

Public Class Form1
    Private Property Ii As New Class1
    Private Property Constr As String = Ii.ConStr
    Private Function GrDtToday() As Integer
        Dim SqlStr As String = "SELECT COUNT(GrDtID) FROM GrDt WHERE Month(GrDt.Mnm)=? AND GrDt.GrDt1=?;"
        Using cn As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, cn) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", Month(Now.Date))
            CMD.Parameters.AddWithValue("?", Now.Date)
            cn.Open()
            Return Convert.ToInt32(CMD.ExecuteScalar)
        End Using
    End Function
    'Private Function GetDefGroup(ByVal TblNm As String, ByVal Fld As String) As Integer
    ' Dim SqlStr As String =
    ' <sql>SELECT COUNT( <%= Fld %> ) FROM <%= TblNm %>;</sql>.Value
    ' Using cn As OleDbConnection = New OleDbConnection With {.ConnectionString = IC.GetConStr},
    '             CMD As OleDbCommand = New OleDbCommand(SqlStr, cn) With {.CommandType = CommandType.Text}
    '         cn.Open()
    ' Return Convert.ToInt32(CMD.ExecuteScalar)
    'End Using
    'End Function
    'Private Sub CreateDef(ByVal SqlStr As String)
    ' Using cn As OleDbConnection = New OleDbConnection With {.ConnectionString = IC.GetConStr},
    '             CMD As OleDbCommand = New OleDbCommand(SqlStr, cn) With {.CommandType = CommandType.Text}
    '         cn.Open()
    '         CMD.ExecuteNonQuery()
    ' End Using
    ' End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Interval = 1000
        Timer1.Enabled = True
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        WindowState = FormWindowState.Maximized
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main1.jpg"), True)
        IsMdiContainer = True
        'Creates a Levels name = لا يوجد
        'Dim Lvlid As Integer
        'If GetDefGroup("Lvls", "LID") <= 0 Then
        ' CreateDef(<sql>INSERT INTO Lvls (LNm,SubNm) VALUES ("لايوجد", "لايوجد");</sql>.Value)
        ' Lvlid = IC.GetIDFrmTbl("Lvls", "LID", "Lnm", "لايوجد").Find(Function(s As Integer)
        ' Return s > 0
        ' End Function)
        ' End If
        ' 'Creates a group name = لا يوجد
        ' If GetDefGroup("Grps", "GrID") <= 0 Then
        ' CreateDef(<sql>INSERT INTO Grps (GrNm,LID) VALUES ("لايوجد", <%= Lvlid %>);</sql>.Value)
        ' End If
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
        Iu.CompRepair()
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
        If Form2.Visible = True Then Exit Sub
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
        If String.IsNullOrEmpty(BackUpFolder) Then Exit Sub
        Iu.CompRepair()
        'Create the file stream for the source file
        Dim BackUpPath As String = BackUpFolder
        Dim streamRead As New IO.FileStream(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm), IO.FileMode.Open)
        'Create the file stream for the destination file
        Dim streamWrite As IO.FileStream =
            New IO.FileStream(IO.Path.Combine(BackUpFolder, "BackUp--" & Now.Date.ToShortDateString.Replace("/", "_") & ".accdb.bak"),
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
        MsgBox("تم تصدير النسخة بنجاح.", MsgBoxStyle.Information)
        IsMdiContainer = True
    End Sub

    Private Sub QToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QToolStripMenuItem.Click
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
        IsMdiContainer = True
        With Form4
            .MdiParent = Me
            '.WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub

    Private Sub SToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SToolStripMenuItem1.Click
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
        IsMdiContainer = True
        Dim Frm7 As New Form7
        With Frm7
            .MdiParent = Me
            '.WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        With Ii.PopupNotifier1
            .ContentText = "السلام عليكم و رحمة الله و بركاته. لديك  " & GrDtToday() & " مجموعات اليوم."
            .Popup()
        End With
    End Sub
End Class
