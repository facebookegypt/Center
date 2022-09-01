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
            .WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub
    Private Sub MnuFilBak_Click(sender As Object, e As EventArgs) Handles MnuFilBak.Click
        IsMdiContainer = True
        With Form3
            .MdiParent = Me
            .WindowState = FormWindowState.Normal
            .ShowIcon = True
            .Show()
        End With
    End Sub
    Private Sub MnuFilBacSav_Click(sender As Object, e As EventArgs) Handles MnuFilBacSav.Click
        Dim Iu As Class1 = New Class1
        Dim BackUpFolder As String = Iu.OfdOpn("اختر مجلد حفظ نسخة احتياطية")
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
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim Iu As Class1 = New Class1
        Dim BackUpFolder As String = My.Settings.LocalBackUpFolder
        Iu.CompRepair()
        'Create the file stream for the source file
        Dim BackUpPath As String = BackUpFolder
        'Exception: if Database file is open then an error occures 'File is being used by another process" 01Sept2022
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
    End Sub
End Class
