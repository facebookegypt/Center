Imports System.Data.OleDb
Public Class Form2
    Public Property IC As New Class1
    Public Property Constr1 As String = IC.ConStr
    Private StNm, StMob1, StMob2 As String, StID1 As Integer
    Private DGStdnts As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill,
        .EnableHeadersVisualStyles = False, .RowHeadersVisible = True, .BackgroundColor = Color.WhiteSmoke,
        .ColumnHeadersHeight = 50, .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing},
        Dt1 As DataTable
    Private Function GetGrpName(ByVal SqlStr As String, ByVal StudentID As Integer) As String
        Dim Rslt As String = String.Empty
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
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
    Private Sub GetStdnts(ByVal SqlStr As String)
        Dim Dt2 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt2)
        End Using
        With DGStdnts
            .AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders
            .RowHeadersWidth = 30
            With .ColumnHeadersDefaultCellStyle
                .WrapMode = DataGridViewTriState.True
                .Alignment = DataGridViewContentAlignment.MiddleCenter
                .BackColor = Color.FloralWhite
            End With
            With .RowsDefaultCellStyle
                .WrapMode = DataGridViewTriState.True
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            .ReadOnly = True
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .AutoGenerateColumns = False
            .DataSource = Nothing
            .DataSource = New BindingSource(Dt2.DefaultView, Nothing)
        End With
        GroupBox2.Controls.Add(DGStdnts)
        AddHandler DGStdnts.RowPostPaint, AddressOf DGSTDNTS_RowPostPaint
        AddHandler DGStdnts.CellClick, AddressOf DGStdnts_CellClick
    End Sub
    Private Sub DGSTDNTS_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim grid = TryCast(sender, DataGridView)
        Dim rowIdx As String = Convert.ToString(e.RowIndex + 1)
        Using centerFormat As StringFormat = New StringFormat() With
            {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim headerBounds =
                New Rectangle(e.RowBounds.Right - 42, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
            e.Graphics.DrawString(rowIdx, Font, SystemBrushes.ControlText, headerBounds, centerFormat)
        End Using
    End Sub
    Private Sub ConDGV(ByVal SqlStr As String)
        SqlStr = <sql>SELECT * From Stdnts;</sql>.Value
        GetStdnts(SqlStr)
        Dim DIg2 As New DataGridViewTextBoxColumn With
            {.Name = "StNm", .ValueType = GetType(String), .DataPropertyName = "StNm", .HeaderText = "اسم الطالب"}
        Dim DIg0 As New DataGridViewTextBoxColumn With
            {.Name = "Mob1", .ValueType = GetType(String), .DataPropertyName = "Mob1", .HeaderText = "رقم الموبيل"}
        Dim DIg_1 As New DataGridViewTextBoxColumn With
            {.Name = "Mob2", .ValueType = GetType(String), .DataPropertyName = "Mob2", .HeaderText = "موبيل ولي الأمر"}
        Dim DIg1 As New DataGridViewTextBoxColumn With
            {.Name = "StID", .ValueType = GetType(Integer), .DataPropertyName = "StID", .Visible = False}
        If DGStdnts.Columns.Count <= 0 Then
            DGStdnts.Columns.Insert(0, DIg2)
            DGStdnts.Columns.Insert(1, DIg0)
            DGStdnts.Columns.Insert(2, DIg_1)
            DGStdnts.Columns.Insert(3, DIg1)
        End If
    End Sub
    Private Sub DGStdnts_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        Dim SelectedRow As DataGridViewRow = DGStdnts.Rows(e.RowIndex)
        StID1 = Convert.ToInt32(SelectedRow.Cells.Item("STID").Value.ToString)
        TxtNm.Text = SelectedRow.Cells.Item("StNM").Value.ToString
        TxtMob1.Text = SelectedRow.Cells.Item("Mob1").Value.ToString
        TxtMob2.Text = SelectedRow.Cells.Item("Mob2").Value.ToString
        'Fetch Group Name
        'ToolStripLabel1.Text = "المجموعه : " & GetGrpName(StID1)
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
        StID1 = 0
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
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Return Convert.ToInt32(CMD.ExecuteScalar)
        End Using
    End Function
    Private Sub BtnDel_Click(sender As Object, e As EventArgs) Handles BtnDel.Click
        'Attend and Results Tables are linked with Students
        If StID1 <= 0 Then
            MsgBox("يجب اختيار طالب أولا.", MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        'Dim Ig As String = GetGrpName(StID1)
        'If String.IsNullOrEmpty(Ig) Or
        ' Ig <> "لايوجد" Then
        ' MsgBox("يجب حذف الطالب أولا من المجوعة المنتمي اليها.",
        ' MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
        ' Exit Sub
        ' End If
        Dim N As Integer
        Dim SqlStr As String =
            <SQL>DELETE * Stdnts WHERE Stdnts.StID=?;</SQL>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
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
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
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
        Dim N As Integer ', GrNum As Integer = IC.GetIDFrmTbl("Grps", "GrID", "GrNm", "لايوجد").Find(Function(s As Integer)
        'Return s
        'End Function)
        Dim SqlStr As String =
            <SQL>INSERT INTO Stdnts(StNm,Mob1,Mob2,DtCrtd,DtMdfd)VALUES(?,?,?,?,?,?);</SQL>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With CMD.Parameters
                .AddWithValue("?", StNm)
                .AddWithValue("?", StMob1)
                .AddWithValue("?", StMob2)
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
        WindowState = FormWindowState.Maximized
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
            ElseIf ComboBox1.SelectedIndex = 2 Then
                If Not IsNumeric(TextBox1.Text) Then
                    TextBox1.SelectAll()
                    SendKeys.Send("{BackSpace}")
                    Exit Sub
                End If
                SqlStr1 = <sql>SELECT * FROM Stdnts WHERE StID LIKE '%<%= Convert.ToInt32(TextBox1.Text) %>%';</sql>.Value
            End If
            ConDGV(SqlStr1)
        End If
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        TextBox1.Text = String.Empty
        BtnClear_Click(sender, e)
    End Sub

    Private Sub Form2_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        BackgroundImageLayout = ImageLayout.Stretch
        Invalidate(True)
        Update()
        Refresh()
    End Sub
End Class