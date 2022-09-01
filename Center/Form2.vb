Imports System.Data.OleDb

Public Class Form2
    Private IC As New Class1, ConStr As String = IC.GetConStr
    Private StNm, StMob1, StMob2 As String
    Private DGStdnts As DataGridView = New DataGridView With {.Name = "DGV1"}, Dt1 As DataTable

    Private Function GetData(ByVal SqlStr As String) As DataTable
        Dt1 = New DataTable With {.Locale = System.Globalization.CultureInfo.CurrentCulture}
        Using CN As New OleDbConnection With {.ConnectionString = ConStr},
            CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            SDA As OleDbDataAdapter = New OleDbDataAdapter(CMD)
            SDA.Fill(Dt1)
        End Using
        Return Dt1
    End Function

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        For Each Ctrl As Control In Me.GroupBox1.Controls
            If TypeOf Ctrl Is TextBox Then
                Ctrl.Text = String.Empty
            End If
        Next
        StNm = String.Empty
        StMob1 = String.Empty
        StMob2 = String.Empty
        TxtNm.Select()
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
    End Sub

    Private Function GetCount(ByVal TblNm As String, FldNm As String) As Integer
        Dim SqlStr As String =
            <sql>SELECT COUNT( <%= FldNm %> ) FROM <%= TblNm %>;</sql>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Return CMD.ExecuteScalar
        End Using
    End Function
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Save
        StNm = TxtNm.Text
        StMob1 = TxtMob1.Text
        StMob2 = TxtMob2.Text
        'In case no Groups were added, you need to create a pretend one (i.e. : GrID=1) to save a new student.
        'Then reassign the students to the new Groups accordingly.
        Dim N As Integer, GrNum As Integer = 1
        Dim SqlStr As String =
            <SQL>INSERT INTO Stdnts(StNm,Mob1,Mob2,GrID,DtCrtd,DtMdfd)VALUES(?,?,?,?,?,?);</SQL>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With CMD.Parameters
                .AddWithValue("?", StNm)
                .AddWithValue("?", StMob1)
                .AddWithValue("?", StMob2)
                .AddWithValue("?", GrNum)
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", Now.Date)
            End With
            CN.Open()
            N = CMD.ExecuteNonQuery
        End Using
        MsgBox("تم حفظ " & N.ToString & " طالب.")
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "اعدادات الطلاب. لديك " & GetCount("Stdnts", "StID") & " طالب."
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        BackgroundImageLayout = ImageLayout.Stretch
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main2.jpg"), True)

    End Sub
    Private Sub Form2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
End Class