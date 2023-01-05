Imports System.Data.OleDb
Imports System.Text.RegularExpressions

Public Class Form4
    Private Dt1 As DataTable
    Public Property IU As Class1 = New Class1
    Public Property Constr1 As String = IU.ConStr
    Private Property GrID As Integer
    Private Property GrDtID As Integer
    Public Property TaskID As Integer
    Private Property TaskNm As String
    Private WithEvents DG1 As DataGridView
    Private Sub GetGrps(ByVal SqlStr As String,
                        ByVal combobox As ComboBox,
                        Optional DisMem As String = "Value",
                        Optional ValMem As String = "Key")
        Dt1 = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt1)
        End Using
        With combobox
            .BeginUpdate()
            .DataSource = Dt1.DefaultView
            .DisplayMember = DisMem
            .ValueMember = ValMem
            .SelectedIndex = -1
            .EndUpdate()
            .Update()
        End With
    End Sub
    Private Function GetCount(ByVal SqlStr As String) As String
        Dim Rslt As String = String.Empty
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Rslt = "عدد الطلاب بالمجموعة : "
            Return Rslt & Convert.ToInt32(CMD.ExecuteScalar) & " طالب."
        End Using
    End Function
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        WindowState = FormWindowState.Normal
        DoubleBuffered = True
        DG1 = New DataGridView With
        {.Name = "DGV1", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill, .EnableHeadersVisualStyles = False, .RowHeadersVisible = True,
        .BackgroundColor = Color.WhiteSmoke, .ColumnHeadersHeight = 50, .RowHeadersWidth = 40,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing}
        Text = "اعدادات المجموعات"
        GetGrps("SELECT GrID,GrNm FROM Grps;", ComboBox2, "GrNm", "GrID")
        GetGrps("SELECT TaskID,TaskNm FROM Tsks;", ComboBox3, "TaskNm", "TaskID")
        With DateTimePicker1
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "MMMMyyyy"
            .ShowUpDown = True
        End With
        With DateTimePicker2
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "dddd dd/MMMM/yyyy hh:mm tt"
            .ShowUpDown = True
        End With
        'WindowState = FormWindowState.Maximized
        BackgroundImageLayout = ImageLayout.Stretch

        ComboBox1.ComboBox.SelectedIndex = 0
        AddHandler ComboBox1.ComboBox.SelectionChangeCommitted, AddressOf ComboBox1_OnSelectionChangedCommitted
    End Sub
    Private Sub ComboBox1_OnSelectionChangedCommitted(sender As Object, e As EventArgs)
        TextBox1.SelectAll()
    End Sub
    Private Sub Form4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Dispose(True) : Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Frm As New Form6
        Frm.ShowDialog()
        GetGrps("SELECT GrID,GrNm FROM Grps;", ComboBox2, "GrNm", "GrID")
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        TaskNm = TextBox2.Text
        If IsNothing(ComboBox2.SelectedItem) Then
            MsgBox("من فضلك اختر المجموعة أولا.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If IsNothing(ComboBox3.SelectedItem) Then
            MsgBox("من فضلك اختر الأعمال.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If TxtMrk.Text.Length <= 0 Then
            MsgBox("من فضلك ادخل الدرجة العظمي للأعمال.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim Mnthly As Integer = 0, NxtDy As Date = DateTimePicker2.Value.Date
        If CheckBox1.CheckState = CheckState.Checked Then
            Mnthly = 3
        End If
        Cursor = Cursors.WaitCursor
        Dim SqlStr1 As String =
            "INSERT INTO GrDT (GrID,Mnm,GrDt1,GrDt2,TaskID,TskFlMrk,TskNm,DtCrtd,DtMdfd) VALUES (?,?,?,?,?,?,?,?,?);"
        Dim N As Integer
        For I As Integer = 0 To Mnthly
            If Mnthly = 0 Then
                NxtDy = DateTimePicker2.Value.Date
                'Exit For
            End If
            NxtDy = DateAdd(DateInterval.WeekOfYear, I, DateTimePicker2.Value.Date)
            Using CN = New OleDbConnection(Constr1),
                cmd = New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text}
                With cmd.Parameters
                    .AddWithValue("?", GrID)
                    .AddWithValue("?", DateTimePicker1.Value.Date)
                    .AddWithValue("?", NxtDy)
                    .AddWithValue("?", DateTimePicker2.Value.TimeOfDay)
                    .AddWithValue("?", TaskID)
                    .AddWithValue("?", Convert.ToInt32(TxtMrk.Text))
                    .AddWithValue("?", TaskNm)
                    .AddWithValue("?", Now.Date)
                    .AddWithValue("?", Now.Date)
                End With
                Try
                    CN.Open()
                    N = cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                Catch ex As OleDbException
                    MsgBox("مشكلة فى الحفظ 1 : " & ex.Message,
                           MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
                    Cursor = Cursors.Default
                    CN.Close()
                    Exit Sub
                End Try
            End Using
        Next
        If N = 0 Then
            MsgBox("مشكلة فى عملية الحفظ, برجاء اعادة المحاولة.",
                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
        ElseIf N >= 1 Then
            MsgBox("تم الحفظ بنجاح.",
                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            ToolStripButton10_Click(sender, e)
        End If
        Cursor = Cursors.Default
    End Sub
    Private Sub ComboBox2_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox2.SelectionChangeCommitted
        GrID = Convert.ToInt32(ComboBox2.SelectedValue)
    End Sub
    Private Sub GetGrpsDts(ByVal SqlStr As String)
        Cursor = Cursors.WaitCursor
        Dim Dt2 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Try
            Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
                DataAdapter1.Fill(Dt2)
            End Using
            With DG1
                .EnableHeadersVisualStyles = False  'Will display the custom formats of mine.
                .AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow
                .EditMode = DataGridViewEditMode.EditOnEnter
                .GridColor = SystemColors.Control
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
                .ReadOnly = True
                .DefaultCellStyle.WrapMode = DataGridViewTriState.True
                .AutoGenerateColumns = False
                .DataSource = Nothing
                .DataSource = Dt2.DefaultView.Table
            End With
            GroupBox1.Controls.Add(DG1)
            With DG1.ColumnHeadersDefaultCellStyle
                .BackColor = Color.LightCyan
                .ForeColor = Color.DarkGreen
                .Font = New Font("Arial", 13, FontStyle.Bold)
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            With DG1.RowHeadersDefaultCellStyle
                .BackColor = Color.LightCyan
                .ForeColor = Color.DarkGreen
                .Font = New Font("Arial", 11, FontStyle.Regular)
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            With DG1.DefaultCellStyle
                .SelectionBackColor = Color.LightCyan
                .SelectionForeColor = Color.Navy
            End With
            AddHandler DG1.RowPostPaint, AddressOf DG1_RowPostPaint
            AddHandler DG1.CellClick, AddressOf DG1_CellClick
            Cursor = Cursors.Default
        Catch ex As Exception
            Cursor = Cursors.Default
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub DG1_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        Try
            GrID = Convert.ToInt32(DG1.CurrentRow.Cells("GrID").Value)
            GrDtID = Convert.ToInt32(DG1.CurrentRow.Cells("GrDtID").Value)
            TaskID = Convert.ToInt32(DG1.CurrentRow.Cells("TaskID").Value)
        Catch ex As Exception
            MsgBox("خطأ في التشغيل 1 : " & ex.Message)
            Exit Sub
        End Try
        ComboBox2.SelectedValue = GrID
        DateTimePicker1.Value = Convert.ToDateTime(DG1.CurrentRow.Cells("Mnm").Value)
        Dim DT0 As Date = Convert.ToDateTime(DG1.CurrentRow.Cells("GrDt1").Value).Date
        Dim DT As TimeSpan = Convert.ToDateTime(DG1.CurrentRow.Cells("GrDt2").Value).TimeOfDay
        DateTimePicker2.Value = DT0 + DT
        ComboBox3.SelectedValue = TaskID
        TextBox2.Text = Convert.ToString(DG1.CurrentRow.Cells("TskNm").Value)
        TxtMrk.Text = Convert.ToInt32(DG1.CurrentRow.Cells("TskFlMrk").Value)
        Dim SqlStr As String = <sql>SELECT Count([GrSt].[StID]) AS Expr1 FROM Stdnts INNER JOIN (GrSt INNER JOIN Grps ON 
            GrSt.GrID = Grps.GrID) ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID)=<%= GrID %>));</sql>.Value
        ToolStripLabel1.Text = GetCount(SqlStr)
        BtnEdit.Enabled = True
        BtnDel.Enabled = True
        BtnSave.Enabled = False
        ToolStripButton1.Enabled = True
    End Sub
    Private Sub DG1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim grid = TryCast(sender, DataGridView)
        Dim rowIdx As String = Convert.ToString(e.RowIndex + 1)
        Using centerFormat As StringFormat = New StringFormat() With
            {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim headerBounds =
                New Rectangle(e.RowBounds.Right - 42, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
            e.Graphics.DrawString(rowIdx, Font, SystemBrushes.Desktop, headerBounds, centerFormat)
        End Using
    End Sub
    Private Sub ComboBox3_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox3.SelectionChangeCommitted
        TaskID = Convert.ToInt32(ComboBox3.SelectedValue)
    End Sub
    Private Sub ConDGV(ByVal SqlStr As String)
        GetGrpsDts(SqlStr)
        Dim DIg1 As New DataGridViewTextBoxColumn With
            {.Name = "GrNm", .ValueType = GetType(String), .DataPropertyName = "GrNm", .HeaderText = "المجموعة",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader}
        Dim DIg2 As New DataGridViewTextBoxColumn With
            {.Name = "Lnm", .ValueType = GetType(String), .DataPropertyName = "Lnm", .HeaderText = "الصف الدراسي",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader}
        Dim DIg3 As New DataGridViewTextBoxColumn With
            {.Name = "SubNm", .ValueType = GetType(String), .DataPropertyName = "SubNm", .HeaderText = "المادة",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        Dim DIg4 As New DataGridViewTextBoxColumn With
            {.Name = "Mnm", .ValueType = GetType(Date), .DataPropertyName = "Mnm", .HeaderText = "الشهر",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        DIg4.DefaultCellStyle.Format = "MMMMyyyy"
        Dim DIg5 As New DataGridViewTextBoxColumn With
            {.Name = "GrDt1", .ValueType = GetType(Date), .DataPropertyName = "GrDt1", .HeaderText = "اليوم",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader}
        DIg5.DefaultCellStyle.Format = "dddd dd/MMMM"
        Dim DIg6 As New DataGridViewTextBoxColumn With
            {.Name = "GrDt2", .ValueType = GetType(Date), .DataPropertyName = "GrDt2", .HeaderText = "الساعة",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader}
        DIg6.DefaultCellStyle.Format = "hh:mm tt"
        Dim DIg7 As New DataGridViewTextBoxColumn With
            {.Name = "TaskNm", .ValueType = GetType(String), .DataPropertyName = "TaskNm", .HeaderText = "الأعمال",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader}
        Dim DIg8 As New DataGridViewTextBoxColumn With
            {.Name = "TskFlMrk", .ValueType = GetType(Integer), .DataPropertyName = "TskFlMrk", .HeaderText = "الدرجة",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader}
        Dim DIg9 As New DataGridViewTextBoxColumn With
            {.Name = "TskNm", .ValueType = GetType(String), .DataPropertyName = "TskNm", .HeaderText = "اسم الاختبار",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        Dim DIg As New DataGridViewTextBoxColumn With
            {.Name = "GrDtID", .ValueType = GetType(Integer), .DataPropertyName = "GrDtID", .Visible = False}
        Dim DIg0 As New DataGridViewTextBoxColumn With
            {.Name = "GrID", .ValueType = GetType(Integer), .DataPropertyName = "GrID", .Visible = False}
        Dim DIg_1 As New DataGridViewTextBoxColumn With
            {.Name = "TaskID", .ValueType = GetType(Integer), .DataPropertyName = "TaskID", .Visible = False}
        If DG1.Columns.Count <= 0 Then
            DG1.Columns.Insert(0, DIg1)
            DG1.Columns.Insert(1, DIg2)
            DG1.Columns.Insert(2, DIg3)
            DG1.Columns.Insert(3, DIg4)
            DG1.Columns.Insert(4, DIg5)
            DG1.Columns.Insert(5, DIg6)
            DG1.Columns.Insert(6, DIg7)
            DG1.Columns.Insert(7, DIg8)
            DG1.Columns.Insert(8, DIg9)
            DG1.Columns.Insert(9, DIg)
            DG1.Columns.Insert(10, DIg0)
            DG1.Columns.Insert(11, DIg_1)
            DG1.CellBorderStyle = DataGridViewCellBorderStyle.Single
            DG1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DG1.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True
        End If
        BtnClear.Enabled = True
        BtnDel.Enabled = False
        BtnEdit.Enabled = False
        BtnSave.Enabled = False
    End Sub
    Private Sub ToolStripButton10_Click(sender As Object, e As EventArgs) Handles ToolStripButton10.Click
        Dim SqlStr As String =
            "SELECT GrDt.GrDtID, GrDt.GrID, Grps.GrNm, Grps.Lnm, Grps.SubNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Tsks.TaskID, Tsks.TaskNm, " &
            "GrDt.TskFlMrk, GrDt.TskNm FROM Tsks INNER JOIN (GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) ON Tsks.TaskID = GrDt.TaskID " &
            "ORDER BY GrDt.GrID, GrDt.GrDt1;"
        ConDGV(SqlStr)
    End Sub
    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
        BtnSave.Enabled = True
        TextBox2.Clear()
        ComboBox2.SelectedItem = Nothing
        ComboBox3.SelectedItem = Nothing
        TxtMrk.Text = 0.ToString
        DateTimePicker1.Value = Now.Date
        DateTimePicker2.Value = Now.Date
        ToolStripButton1.Enabled = False
        RemoveHandler DG1.CellClick, AddressOf DG1_CellClick
        RemoveHandler ComboBox1.ComboBox.SelectionChangeCommitted, AddressOf ComboBox1_OnSelectionChangedCommitted
        RemoveHandler DG1.RowPostPaint, AddressOf DG1_RowPostPaint
    End Sub
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        'Edit
        Cursor = Cursors.WaitCursor
        TaskNm = TextBox2.Text.Trim
        Dim N As Integer, SqlStr As String =
            "UPDATE GrDt SET GrID=?, Mnm=?, GrDt1=?, GrDt2=?, TaskID=?, TskFlMrk=?, TskNm=?, DtMdfd=? WHERE GrDtID=?;"
        Using CN = New OleDbConnection(Constr1),
                cmd = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With cmd.Parameters
                .AddWithValue("?", GrID)
                .AddWithValue("?", DateTimePicker1.Value)
                .AddWithValue("?", DateTimePicker2.Value.Date)
                .AddWithValue("?", DateTimePicker2.Value.TimeOfDay)
                .AddWithValue("?", TaskID)
                .AddWithValue("?", Convert.ToInt32(TxtMrk.Text))
                .AddWithValue("?", TaskNm)
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", GrDtID)
            End With
            Try
                CN.Open()
                N = cmd.ExecuteNonQuery()
                MsgBox("تم التعديل بنجاح.",
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
                Cursor = Cursors.Default
            Catch ex As Exception
                MsgBox("خطأ في التعديل : " & ex.Message,
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
                Cursor = Cursors.Default
            End Try
        End Using
    End Sub
    Private Sub BtnDel_Click(sender As Object, e As EventArgs) Handles BtnDel.Click
        'Attend and Results Tables are linked with Students
        If GrDtID <= 0 Then
            DG1.AllowUserToDeleteRows = False
            MsgBox("يجب اختيار مجموعة أولا.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim SqlStr1 As String =
            <sql>SELECT COUNT(GrDtID) FROM Attnd Where GrDtID=<%= GrDtID %>;</sql>.Value
        Dim Ig As Integer = IU.GetCount1(SqlStr1)
        If Ig >= 1 Then
            DG1.AllowUserToDeleteRows = False
            MsgBox("يجب حذف الميعاد أولا من كشف الغياب.",
             MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        DG1.AllowUserToDeleteRows = True
        Dim RUSre As MsgBoxResult = MsgBox("تأكيد حذف هذا الميعاد من المجموعة.",
                                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                               MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel)
        If RUSre = MsgBoxResult.Yes Then
            Dim N As Integer
            Dim SqlStr As String =
                <SQL>DELETE * From GrDt WHERE GrDt.GrDtID=?;</SQL>.Value
            Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                    CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                With CMD.Parameters
                    .AddWithValue("?", GrDtID)
                End With
                CN.Open()
                N = CMD.ExecuteNonQuery
            End Using
            MsgBox("تم حذف الميعاد " & N.ToString & " المجموعة.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        Else
            MsgBox("تم الغاء العملية",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        End If
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
    End Sub
    Private Sub Form4_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'Use KeyCode when you don't care about the modifiers, KeyData when you do.
        If e.KeyCode = Keys.S AndAlso e.Modifiers = Keys.Control Then
            If BtnSave.Enabled = True Then
                BtnSave_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.N AndAlso e.Modifiers = Keys.Control Then
            If BtnClear.Enabled = True Then
                BtnClear_Click(sender, e)
            End If
        End If

        If e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control Then
            If BtnEdit.Enabled = True Then
                BtnEdit_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.A AndAlso e.Modifiers = Keys.Control Then
            If ToolStripButton10.Enabled = True Then
                ToolStripButton10_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.F AndAlso e.Modifiers = Keys.Control Then
            TextBox1.Focus()
        End If
        If e.KeyData = Keys.Delete Then
            If BtnDel.Enabled = True Then
                BtnDel_Click(sender, e)
            End If
        End If
    End Sub
    Private Sub TxtMrk_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtMrk.KeyPress
        'Numbers Only
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub TxtMrk_TextChanged(sender As Object, e As EventArgs) Handles TxtMrk.TextChanged
        'Copy & Paste
        Dim digitsOnly As Regex = New Regex("[^\d]")
        TxtMrk.Text = digitsOnly.Replace(TxtMrk.Text, "")
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If IsNothing(ComboBox1.ComboBox.SelectedItem) Or ComboBox1.ComboBox.SelectedIndex = -1 Then
                MsgBox("من فضلك أختر فلتر البحث أولا",
                       MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
                Exit Sub
            End If
            If TextBox1.Text.Contains("'") Or
                    TextBox1.Text.Contains(",") Or
                    TextBox1.Text.Contains(".") Then
                MsgBox("البحث الذي أدخلته غير صحيح",
                       MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
                Exit Sub
            End If
            If TextBox1.Text.Length <= 0 Then Exit Sub
            Dim SqlStr2 As String = String.Empty
            Dim SqlStr1 As String =
                "SELECT GrDt.GrDtID, GrDt.GrID, Grps.GrNm, Grps.Lnm, Grps.SubNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Tsks.TaskID, " &
                "Tsks.TaskNm, GrDt.TskFlMrk FROM Tsks INNER JOIN (GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) ON " &
                "Tsks.TaskID = GrDt.TaskID "
            Select Case ComboBox1.ComboBox.SelectedIndex
                Case Is = 0
                    Dim date1 As Date = "#" & TextBox1.Text & "#"
                    SqlStr2 = "WHERE (((GrDt.GrDt1)=#" & date1.Date.ToString("MM/dd/yyyy") & "#)) ORDER BY GrDt.GrID, GrDt.GrDt1;"
                Case Is = 1
                    Dim Lnm1 As String = TextBox1.Text
                    SqlStr2 = "WHERE (((Grps.Lnm) LIKE '%" & Lnm1 & "%')) ORDER BY GrDt.GrID, GrDt.GrDt1;"
                Case Is = 2
                    Dim SubNm1 As String = TextBox1.Text
                    SqlStr2 = "WHERE (((Grps.SubNm) LIKE '%" & SubNm1 & "%')) ORDER BY GrDt.GrID, GrDt.GrDt1;"
                Case Is = 3
                    SqlStr2 = "WHERE (((Grps.GrNm) LIKE '%" & TextBox1.Text & "%')) ORDER BY GrDt.GrID, GrDt.GrDt1;"
            End Select
            ConDGV(SqlStr1 & SqlStr2)
        End If
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        'بيان غياب كل يوم علي حدي
        If GrID = 0 OrElse GrDtID = 0 OrElse DG1.Rows.Count <= 0 Then
            Exit Sub
        End If
        Dim SqlDel As String =
                "DROP VIEW AttDailyRpt;"
        Dim SqlCreate As String =
                "CREATE VIEW AttDailyRpt AS SELECT GrDt.GrDtID, GrDt.GrID, Attnd.StID, Stdnts.StNm, Attnd.PStat, GrDt.Mnm, " &
                "GrDt.GrDt1, GrDt.GrDt2, Grps.GrNm, Grps.Lnm, Grps.SubNm FROM (GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) " &
                "INNER JOIN (Stdnts INNER JOIN Attnd ON Stdnts.StID = Attnd.StID) ON GrDt.GrDtID = Attnd.GrDtID WHERE " &
                "(((GrDt.GrDtID)=" & GrDtID & ") And ((GrDt.GrID)=" & GrID & ") And ((Attnd.PStat)=True));"
        Using CN As New OleDbConnection(Constr1),
                    CMDDel As New OleDbCommand(SqlDel, CN) With {.CommandType = CommandType.Text},
                    CMDCREATE As New OleDbCommand(SqlCreate, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Try
                CMDDel.ExecuteNonQuery()
                CMDCREATE.ExecuteNonQuery()
            Catch ex As OleDbException
                CMDCREATE.ExecuteNonQuery()
            End Try
        End Using
        Form10.SrcFrm = "DayGrp"
        Form10.ShowDialog()
    End Sub

    Private Sub Form4_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        DG1 = Nothing
    End Sub
End Class