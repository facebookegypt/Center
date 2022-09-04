Imports System.Data.OleDb
Public Class Form2
    Private IC As New Class1, ConStr As String = IC.GetConStr
    Private StNm, StMob1, StMob2 As String, StID1 As Integer
    Private DGStdnts As DataGridView = New DataGridView With
        {.Name = "DGV1", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill, .ColumnCount = 4,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, .RowHeadersVisible = False,
        .BackgroundColor = Color.WhiteSmoke},
        Dt1 As DataTable
    Private Function GetGrpName(ByVal StudentID As Integer) As String
        Dim Rslt As String = String.Empty
        Dim SqlStr As String =
            <sql>SELECT Stdnts.StID, Grps.GrNm FROM Grps INNER JOIN Stdnts ON Grps.GrID = Stdnts.GrID WHERE (((Stdnts.StID)=?));</sql>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", StudentID)
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        Rslt = Rdr.GetString(1)
                    End While
                Else
                    Rslt = "لا يوجد"
                End If
            End Using
        End Using
        Return Rslt
    End Function
    Private Function GetData(ByVal SqlStr As String) As DataTable
        Dt1 = New DataTable With {.Locale = Globalization.CultureInfo.CurrentCulture}
        Using CN As New OleDbConnection With {.ConnectionString = ConStr},
            CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            SDA As OleDbDataAdapter = New OleDbDataAdapter(CMD)
            SDA.Fill(Dt1)
        End Using
        Return Dt1
    End Function
    Private Sub ConDGV(ByVal SqlStr As String)
        With DGStdnts
            .ReadOnly = True
            .AutoGenerateColumns = False
            .Columns(0).Name = "StID"
            .Columns(0).HeaderText = "كود الطالب"
            .Columns(0).DataPropertyName = "StID"

            .Columns(1).Name = "StNM"
            .Columns(1).HeaderText = "اسم الطالب"
            .Columns(1).DataPropertyName = "StNM"

            .Columns(2).Name = "Mob1"
            .Columns(2).HeaderText = "موبيل الطالب"
            .Columns(2).DataPropertyName = "Mob1"

            .Columns(3).Name = "Mob2"
            .Columns(3).HeaderText = "موبيل ولي الأمر"
            .Columns(3).DataPropertyName = "Mob2"
            .DataSource = New BindingSource(GetData(SqlStr), Nothing)
        End With
        GroupBox2.Controls.Add(DGStdnts)
        AddHandler DGStdnts.CellClick, AddressOf DGStdnts_CellClick
        DGStdnts.Invalidate(True)
        DGStdnts.Update()
    End Sub
    Private Sub DGStdnts_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        Dim SelectedRow As DataGridViewRow = DGStdnts.Rows(e.RowIndex)
        StID1 = Convert.ToInt32(SelectedRow.Cells.Item("STID").Value.ToString)
        TxtNm.Text = SelectedRow.Cells.Item("StNM").Value.ToString
        TxtMob1.Text = SelectedRow.Cells.Item("Mob1").Value.ToString
        TxtMob2.Text = SelectedRow.Cells.Item("Mob2").Value.ToString
        'Fetch Group Name
        ToolStripLabel1.Text = "المجموعه : " & GetGrpName(StID1)
        BtnEdit.Enabled = True
        BtnDel.Enabled = True
        BtnSave.Enabled = False
    End Sub
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
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        ToolStripStatusLabel1.Text = "لديك " & GetCount("Stdnts", "StID") & " طالب."
        ConDGV(<sql>SELECT * FROM Stdnts;</sql>.Value)
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

    Private Sub BtnDel_Click(sender As Object, e As EventArgs) Handles BtnDel.Click
        'Attend and Results Tables are linked with Students
        If StID1 <= 0 Then
            MsgBox("يجب اختيار طالب أولا.", MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim Ig As String = GetGrpName(StID1)
        If String.IsNullOrEmpty(Ig) Or
            Ig <> "لايوجد" Then
            MsgBox("يجب حذف الطالب أولا من المجوعة المنتمي اليها.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim N As Integer
        Dim SqlStr As String =
            <SQL>DELETE * Stdnts WHERE Stdnts.StID=?;</SQL>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With CMD.Parameters
                .AddWithValue("?", StID1)
            End With
            CN.Open()
            N = CMD.ExecuteNonQuery
        End Using
        MsgBox("تم حذف بيانات " & N.ToString & " طالب.")
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        'EDIT
        StNm = TxtNm.Text
        StMob1 = TxtMob1.Text
        StMob2 = TxtMob2.Text
        If StID1 <= 0 Then
            MsgBox("يجب اختيار طالب أولا.", MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim N As Integer
        Dim SqlStr As String =
            <SQL>UPDATE Stdnts SET StNm=?,Mob1=?,Mob2=?,DtMdfd=? WHERE Stdnts.StID=?;</SQL>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With CMD.Parameters
                .AddWithValue("?", StNm)
                .AddWithValue("?", StMob1)
                .AddWithValue("?", StMob2)
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", StID1)
            End With
            CN.Open()
            N = CMD.ExecuteNonQuery
        End Using
        MsgBox("تم تعديل بيانات " & N.ToString & " طالب.")
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
    End Sub

    Private Sub TxtNm_TextChanged(sender As Object, e As EventArgs) Handles TxtNm.TextChanged
        If TxtNm.Text.Contains("'") Or
        TxtNm.Text.Contains(",") Or
        TxtNm.Text.Contains(".") Then
            MsgBox("الاسم الذي أدخلته غير صحيح")
            SendKeys.Send("{BackSpace}")
            Exit Sub
        End If
    End Sub

    Private Sub TxtMob1_TextChanged(sender As Object, e As EventArgs) Handles TxtMob1.TextChanged
        If TxtMob1.Text.Contains("'") Or
            TxtMob1.Text.Contains(",") Or
            TxtMob1.Text.Contains(".") Then
            MsgBox("الرقم الذي أدخلته غير صحيح")
            SendKeys.Send("{BackSpace}")
            Exit Sub
        End If
    End Sub

    Private Sub TxtMob2_TextChanged(sender As Object, e As EventArgs) Handles TxtMob2.TextChanged
        If TxtMob2.Text.Contains("'") Or
            TxtMob2.Text.Contains(",") Or
            TxtMob2.Text.Contains(".") Then
            MsgBox("الرقم الذي أدخلته غير صحيح")
            SendKeys.Send("{BackSpace}")
            Exit Sub
        End If
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Save
        StNm = TxtNm.Text
        StMob1 = TxtMob1.Text
        StMob2 = TxtMob2.Text
        If StNm.Length <= 0 Then
            MsgBox("يجب ادخال اسم الطالب", MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        'In case no Groups were added, you need to create a pretend one (i.e. : GrID=1) to save a new student.
        'Then reassign the students to the new Groups accordingly.
        Dim N As Integer, GrNum As Integer = IC.GetIDFrmTbl("Grps", "GrID", "GrNm", "لايوجد").Find(Function(s As Integer)
                                                                                                       Return s
                                                                                                   End Function)
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
        BtnClear_Click(sender, e)
        ToolStripStatusLabel1.Text = "لديك " & GetCount("Stdnts", "StID") & " طالب."
        ConDGV(<SQL>SELECT * FROM Stdnts;</SQL>.Value)
    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "اعدادات الطلاب"
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        BackgroundImageLayout = ImageLayout.Stretch
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main2.jpg"), True)
        ComboBox1.SelectedIndex = 0
    End Sub
    Private Sub Form2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If TextBox1.Text.Contains("'") Or
                    TextBox1.Text.Contains(",") Or
                    TextBox1.Text.Contains(".") Then
                MsgBox("الاسم الذي أدخلته غير صحيح")
                Exit Sub
            End If
            If TextBox1.Text.Length <= 0 Then Exit Sub
            Dim SqlStr1 As String = String.Empty
            If ComboBox1.SelectedIndex = 0 Then
                SqlStr1 = <sql>SELECT * FROM Stdnts WHERE StNM LIKE '%<%= TextBox1.Text %>%';</sql>.Value
            ElseIf ComboBox1.SelectedIndex = 1 Then
                SqlStr1 = <sql>SELECT * FROM Stdnts WHERE Mob1 LIKE '%<%= TextBox1.Text %>%';</sql>.Value
            End If
            ConDGV(SqlStr1)
        End If
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        TextBox1.Text = String.Empty
        BtnClear_Click(sender, e)
    End Sub
End Class