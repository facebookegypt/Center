Imports System.Data.OleDb
Public Class Form2
    Private Property IC As New Class1
    Private Property Constr1 As String
    Private StNm, StMob1, StMob2 As String
    Public Property StID1 As Integer
    Public Property GrID1 As Integer
    Private DGStdnts As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill,
        .RowHeadersVisible = True, .BackgroundColor = Color.WhiteSmoke, .ColumnHeadersHeight = 50,
         .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing},
        Dt1 As DataTable, dt2 As DataTable
    Private Mnu1 As ToolStripMenuItem = New ToolStripMenuItem With
        {
        .Name = "Mnu1", .Enabled = True, .Text = "نقل الي مجموعة اخري", .Visible = True
    }
    Private Mnu2 As ToolStripMenuItem = New ToolStripMenuItem With
        {
        .Name = "Mnu2", .Enabled = True, .Text = "حذف من المجموعة الحالية", .Visible = True
    }
    Private Function GetGrpName(ByVal SqlStr As String) As String()
        Dim Rslt As String() = Nothing
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        Rslt = {Rdr.GetString(1)}
                    End While
                Else
                    Rslt = {"لايوجد"}
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
                .EnableHeadersVisualStyles = False  'Will display the custom formats of mine.
                .EditMode = DataGridViewEditMode.EditOnEnter
                .GridColor = SystemColors.Control
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
            .ReadOnly = True
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .AutoGenerateColumns = False
            .DataSource = Nothing
            .DataSource = New BindingSource(Dt2.DefaultView, Nothing)
        End With
        GroupBox2.Controls.Add(DGStdnts)
        With DGStdnts.ColumnHeadersDefaultCellStyle
            .BackColor = Color.DarkCyan
            .ForeColor = Color.White
            .Font = New Font("Arial", 13, FontStyle.Bold)
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        With DGStdnts.RowHeadersDefaultCellStyle
            .BackColor = Color.DarkCyan
            .ForeColor = Color.White
            .Font = New Font("Arial", 11, FontStyle.Regular)
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        With DGStdnts.DefaultCellStyle
            .SelectionBackColor = Color.LightCyan
            .SelectionForeColor = Color.Navy
        End With
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
        'SqlStr = <sql>SELECT * From Stdnts;</sql>.Value
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
        DIg2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DIg0.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DIg_1.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DIg1.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
    End Sub
    Private Sub DGStdnts_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        ToolStripStatusLabel2.Text = String.Empty
        StID1 = 0
        Dim SelectedRow As DataGridViewRow = DGStdnts.Rows(e.RowIndex)
        StID1 = Convert.ToInt32(SelectedRow.Cells.Item("STID").Value.ToString)
        TxtNm.Text = SelectedRow.Cells.Item("StNM").Value.ToString
        TxtMob1.Text = SelectedRow.Cells.Item("Mob1").Value.ToString
        TxtMob2.Text = SelectedRow.Cells.Item("Mob2").Value.ToString
        'Fetch Group Name
        Dim SqlStr As String =
            "SELECT Stdnts.StID, Grps.GrNm FROM (Stdnts INNER JOIN GrSt ON Stdnts.StID = GrSt.StID) INNER JOIN Grps " &
            "ON GrSt.GrID = Grps.GrID WHERE (((Stdnts.StID)=" & StID1 & "));"
        ToolStripStatusLabel2.Text = "المجموعه : " & GetGrpName(SqlStr).First
        Dim SqlStr1 As String = <sql>SELECT COUNT(GrID) FROM GrSt Where StID=<%= StID1 %>;</sql>.Value
        If Convert.ToInt32(IC.GetCount1(SqlStr1)) >= 1 Then
            Dim dropDownItems As ToolStripItemCollection = Form1.SToolStripMenuItem.DropDownItems
            dropDownItems.AddRange({Mnu1, Mnu2})
            AddHandler Mnu1.Click, AddressOf Mnu1_click
            AddHandler Mnu2.Click, AddressOf Mnu2_click
            ToolStripButton3.Enabled = True
        Else
            ToolStripButton3.Enabled = False
            Dim dropDownItems As ToolStripItemCollection = Form1.SToolStripMenuItem.DropDownItems
            If dropDownItems.Count > 0 Then
                dropDownItems.Clear()
            End If
        End If
        BtnEdit.Enabled = True
        BtnDel.Enabled = True
        BtnSave.Enabled = False
    End Sub
    Private Sub Mnu1_click(sender As Object, e As EventArgs)
        Class3.ShowDialog()
        RemoveHandler Mnu1.Click, AddressOf Mnu1_click
    End Sub
    Private Sub Mnu2_click(sender As Object, e As EventArgs)
        If StID1 <= 0 Then
            MsgBox("من فضلك اختر طالب أولا.")
            Exit Sub
        End If
        Dim SqlStr As String = <sql>SELECT COUNT(GrID) FROM GrSt Where StID=<%= StID1 %>;</sql>.Value
        If Convert.ToInt32(IC.GetCount1(SqlStr)) >= 1 Then
            Dim RUSre As MsgBoxResult = MsgBox("تأكيد حذف الطالب من المجموعة فقط.",
                                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                               MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
            If RUSre = MsgBoxResult.Yes Then
                Dim N As Integer
                Dim SqlStr1 As String =
                        <SQL>DELETE * FROM GrSt WHERE GrSt.StID=?;</SQL>.Value
                Using CN As New OleDbConnection With {.ConnectionString = Constr1},
                            CMD As New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", StID1)
                    End With
                    CN.Open()
                    N = CMD.ExecuteNonQuery
                    CMD.Parameters.Clear()
                End Using
                Dim SqlStr2 As String =
                        <SQL>DELETE * FROM Attnd WHERE Attnd.StID=?;</SQL>.Value
                Using CN As New OleDbConnection With {.ConnectionString = Constr1},
                            CMD As New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", StID1)
                    End With
                    CN.Open()
                    N = CMD.ExecuteNonQuery
                    CMD.Parameters.Clear()
                End Using
                MsgBox("تم حذف الطالب من المجموعة , وكشوف الغياب", _
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            End If
        End If
        RemoveHandler Mnu2.Click, AddressOf Mnu2_click
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
            DGStdnts.AllowUserToDeleteRows = False
            MsgBox("يجب اختيار طالب أولا.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim SqlStr1 As String =
            <sql>SELECT COUNT(StID) FROM GrSt Where StID=<%= StID1 %>;</sql>.Value
        Dim Ig As Integer = IC.GetCount1(SqlStr1)
        If Ig >= 1 Then
            DGStdnts.AllowUserToDeleteRows = False
            MsgBox("يجب حذف الطالب أولا من المجوعة المنتمي اليها.",
             MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        DGStdnts.AllowUserToDeleteRows = True
        Dim RUSre As MsgBoxResult = MsgBox("تأكيد حذف الطالب من البرنامج.",
                                               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                               MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
        If RUSre = MsgBoxResult.Yes Then
            Dim N As Integer
            Dim SqlStr As String =
                <SQL>DELETE * From Stdnts WHERE Stdnts.StID=?;</SQL>.Value
            Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                    CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                With CMD.Parameters
                    .AddWithValue("?", StID1)
                End With
                CN.Open()
                N = CMD.ExecuteNonQuery
            End Using
            MsgBox("تم حذف بيانات " & N.ToString & " طالب.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        Else
            MsgBox("تم الغاء العملية",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        End If
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
            MsgBox("يجب اختيار طالب أولا.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
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
        MsgBox("تم تعديل بيانات " & N.ToString & " طالب.",
               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        BtnSave.Enabled = True
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
    End Sub
    Private Sub TxtNm_TextChanged(sender As Object, e As EventArgs) Handles TxtNm.TextChanged
        If TxtNm.Text.Contains("'") Or
        TxtNm.Text.Contains(",") Or
        TxtNm.Text.Contains(".") Then
            MsgBox("الاسم الذي أدخلته غير صحيح",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            SendKeys.Send("{BackSpace}")
            Exit Sub
        End If
    End Sub
    Private Sub TxtMob1_TextChanged(sender As Object, e As EventArgs) Handles TxtMob1.TextChanged
        If TxtMob1.Text.Contains("'") Or
            TxtMob1.Text.Contains(",") Or
            TxtMob1.Text.Contains(".") Then
            MsgBox("الرقم الذي أدخلته غير صحيح",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            SendKeys.Send("{BackSpace}")
            Exit Sub
        End If
    End Sub
    Private Sub TxtMob2_TextChanged(sender As Object, e As EventArgs) Handles TxtMob2.TextChanged
        If TxtMob2.Text.Contains("'") Or
            TxtMob2.Text.Contains(",") Or
            TxtMob2.Text.Contains(".") Then
            MsgBox("الرقم الذي أدخلته غير صحيح",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
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
            MsgBox("يجب ادخال اسم الطالب",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        'In case no Groups were added, you need to create a pretend one (i.e. : GrID=1) to save a new student.
        'Then reassign the students to the new Groups accordingly.
        Dim N As Integer ', GrNum As Integer = IC.GetIDFrmTbl("Grps", "GrID", "GrNm", "لايوجد").Find(Function(s As Integer)
        'Return s
        'End Function)
        Dim SqlStr As String =
            <SQL>INSERT INTO Stdnts(StNm,Mob1,Mob2,DtCrtd,DtMdfd)VALUES(?,?,?,?,?);</SQL>.Value
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
        MsgBox("تم حفظ " & N.ToString & " طالب.",
               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        BtnClear_Click(sender, e)
        ToolStripStatusLabel1.Text = "لديك " & GetCount("Stdnts", "StID") & " طالب."
        ConDGV(<SQL>SELECT * FROM Stdnts;</SQL>.Value)
    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "اعدادات الطلاب"
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        Constr1 = IC.ConStr
        'WindowState = FormWindowState.Maximized
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main2.jpg"), True)
        ComboBox1.SelectedIndex = 0
        Dim sqlstr As String = <sql>SELECT GrDt.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID 
            GROUP BY GrDt.GrID, Grps.GrNm;</sql>.Value
        IC.GetGrps(Dt1, Constr1, sqlstr, ComboBox2, "GrNm", "GrID")
    End Sub
    Private Sub Form2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If TextBox1.Text.Contains("'") Or
                    TextBox1.Text.Contains(",") Or
                    TextBox1.Text.Contains(".") Then
                MsgBox("الاسم الذي أدخلته غير صحيح",
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
                Exit Sub
            End If
            If TextBox1.Text.Length <= 0 Then Exit Sub
            Dim SqlStr1 As String = String.Empty
            If ComboBox1.SelectedIndex = 0 Then
                SqlStr1 = <sql>SELECT * FROM Stdnts WHERE StNm LIKE '%<%= TextBox1.Text %>%';</sql>.Value
            ElseIf ComboBox1.SelectedIndex = 1 Then
                SqlStr1 = <sql>SELECT * FROM Stdnts WHERE Mob1 LIKE '%<%= TextBox1.Text %>%';</sql>.Value
            ElseIf ComboBox1.SelectedIndex = 2 Then
                If Not IsNumeric(TextBox1.Text) Then
                    TextBox1.SelectAll()
                    SendKeys.Send("{BackSpace}")
                    Exit Sub
                End If
                SqlStr1 = <sql>SELECT * FROM Stdnts WHERE StID=<%= Convert.ToInt32(TextBox1.Text) %>;</sql>.Value
            End If
            ConDGV(SqlStr1)
        End If
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs)
        TextBox1.Text = String.Empty
        BtnClear_Click(sender, e)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ComboBox2.Items.Count <= 0 Then
            MsgBox("من فضلك قم بانشاء مجموعة أولا.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If IsNothing(ComboBox2.SelectedItem) Then
            MsgBox("من فضلك اختر المجموعة أولا.",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        GrID1 = Convert.ToInt32(ComboBox2.SelectedValue)
        Form5.ShowDialog()
    End Sub
    Private Sub Form2_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        BackgroundImageLayout = ImageLayout.Stretch
        Invalidate(True)
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        'Students Days
        Dim SqlDel As String =
                "DROP VIEW StdntsDays;"
        Dim SqlCreate As String =
            "CREATE VIEW StdntsDays AS SELECT Stdnts.StID, Stdnts.StNm, Stdnts.Mob1, Stdnts.Mob2, Grps.GrID, Grps.GrNm, Grps.Lnm, " &
            "Grps.SubNm, GrDt.GrDtID, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2 FROM (Stdnts INNER JOIN (Grps INNER JOIN GrSt ON " &
            "Grps.GrID = GrSt.GrID) ON Stdnts.StID = GrSt.StID) INNER JOIN GrDt ON Grps.GrID = GrDt.GrID " &
            "WHERE (((Stdnts.StID)=" & StID1 & "));"
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
        Form10.SrcFrm = "StdntsDays"
        Form10.ShowDialog()
    End Sub

    Private Sub ComboBox2_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox2.SelectionChangeCommitted
        Dim SqlStr As String = <sql>SELECT COUNT(StID) FROM GrSt Where GrID=<%= Convert.ToInt32(ComboBox2.SelectedValue) %>;</sql>.Value
        Dim Rslt As String = "عدد الطلاب بالمجموعة : "
        GroupBox3.Text = Rslt & Convert.ToInt32(IC.GetCount1(SqlStr)) & " طالب."
    End Sub
    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim dropDownItems As ToolStripItemCollection = Form1.SToolStripMenuItem.DropDownItems
        If dropDownItems.Count > 0 Then
            dropDownItems.Clear()
        End If
    End Sub

    Private Sub Form2_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
        If e.KeyCode = Keys.A AndAlso e.Modifiers = Keys.Control Then
            If ToolStripButton1.Enabled = True Then
                ToolStripButton1_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control Then
            If BtnEdit.Enabled = True Then
                BtnEdit_Click(sender, e)
            End If
        End If
        If e.KeyData = Keys.Delete Then
            If BtnDel.Enabled = True Then
                BtnDel_Click(sender, e)
            End If
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim sqlstr As String = <sql>SELECT GrDt.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID 
            GROUP BY GrDt.GrID, Grps.GrNm;</sql>.Value
        IC.GetGrps(Dt1, Constr1, sqlstr, ComboBox2, "GrNm", "GrID")
    End Sub
End Class