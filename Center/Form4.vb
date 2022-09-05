Imports System.Data.OleDb

Public Class Form4
    Private Iu As Class1 = New Class1, ComboItem As Dictionary(Of Integer, String)
    Private Gnm1, SubNm1 As String, Lid1 As Integer
    Private DG As DataGridView = New DataGridView With
        {.Name = "DGV1", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill, .ColumnCount = 5,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, .RowHeadersVisible = False,
        .BackgroundColor = Color.WhiteSmoke},
        Dt1 As DataTable
    Private Sub ConDGV(ByVal SqlStr As String)
        With DG
            .DataSource = Nothing
            .ReadOnly = True
            .AutoGenerateColumns = False
            .Columns(0).Name = "GrID"
            .Columns(0).HeaderText = "كود المجموعة"
            .Columns(0).DataPropertyName = "GrID"
            .Columns(0).Visible = False

            .Columns(1).Name = "GrNm"
            .Columns(1).HeaderText = "اسم المجموعة"
            .Columns(1).DataPropertyName = "GrNm"
            .Columns(1).ReadOnly = False

            .Columns(2).Name = "LID"
            .Columns(2).HeaderText = "الكود"
            .Columns(2).DataPropertyName = "LID"
            .Columns(2).Visible = False

            .Columns(3).Name = "Lnm"
            .Columns(3).HeaderText = "الصف الدراسي"
            .Columns(3).DataPropertyName = "Lnm"
            .Columns(3).ReadOnly = False

            .Columns(4).Name = "SubNm"
            .Columns(4).HeaderText = "المادة الدراسية"
            .Columns(4).DataPropertyName = "SubNm"
            .Columns(4).ReadOnly = False

            .DataSource = New BindingSource(Iu.GetData(SqlStr), Nothing)
        End With
        GroupBox2.Controls.Add(DG)
        AddHandler DG.CellClick, AddressOf DG_CellClick
        DG.Invalidate(True)
        DG.Update()
    End Sub
    Private Sub DG_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        Dim SelectedRow As DataGridViewRow = DG.Rows(e.RowIndex)
        Lid1 = Convert.ToInt32(SelectedRow.Cells.Item("LID").Value)
        ComboBox1.SelectedValue = Lid1
        TxtNm.Text = SelectedRow.Cells.Item("GrNm").Value.ToString
        TxtSub.Text = SelectedRow.Cells.Item("SubNm").Value.ToString
        'Fetch Group Name
        'ToolStripLabel1.Text = "المجموعه : " & GetGrpName(StID1)
        BtnEdit.Enabled = True
        BtnDel.Enabled = True
        BtnSave.Enabled = False
    End Sub
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "اعدادات المجموعات"
        KeyPreview = True
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        WindowState = FormWindowState.Maximized
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main3.jpg"), True)
        With DateTimePicker1
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "MMMMyyyy"
            .ShowUpDown = True
            .Invalidate(True)
            .Update()
        End With
        With DateTimePicker2
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "d dddd"
            .ShowCheckBox = True
            .Checked = False
            .Show()
            .Invalidate(True)
            .Update()
        End With
        With DateTimePicker3
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "hh:mm tt"
            .ShowCheckBox = True
            .Checked = False
            .ShowUpDown = True
            .Show()
            .Invalidate(True)
            .Update()
        End With
        PopulateCombo()
        With ComboBox1
            .BeginUpdate()
            .DataSource = New BindingSource(ComboItem, Nothing)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            .SelectedIndex = -1
            .EndUpdate()
        End With
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        ConDGV(<sql>SELECT Grps.GrID, Grps.GrNm, Lvls.LID, Lvls.Lnm, Lvls.SubNm FROM Lvls INNER JOIN Grps ON Lvls.LID = Grps.LID;</sql>.Value)
    End Sub
    Private Sub Form4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        ComboBox1.SelectedIndex = -1
        TxtSub.Text = String.Empty
        TxtNm.Text = String.Empty
        BtnSave.Enabled = True
        BtnDel.Enabled = False
        BtnEdit.Enabled = False
    End Sub
    Private Sub PopulateCombo()
        ComboItem = New Dictionary(Of Integer, String)
        ComboItem.Clear()
        Dim SqlStr As String =
             "SELECT LID,Lnm FROM Lvls;"
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Iu.GetConStr},
                 CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        ComboItem.Add(Rdr.GetInt32(0), Rdr.GetString(1))
                    End While
                End If
            End Using
        End Using
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Save
        Gnm1 = TxtNm.Text
        SubNm1 = TxtSub.Text
        If Gnm1.Length <= 0 Or SubNm1.Length <= 0 Then
            MsgBox("يجب ادخال اسم المادة الدراسية و الصنف الدراسي",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        'In case no Groups were added, you need to create a pretend one (i.e. : GrID=1) to save a new student.
        'Then reassign the students to the new Groups accordingly.
        Dim N As Integer ', GrNum As Integer = IC.GetIDFrmTbl("Grps", "GrID", "GrNm", "لايوجد").Find(Function(s As Integer)
        'Return s
        'End Function)
        Dim SqlStr As String =
            <SQL>INSERT INTO Grps(GrNm,LID,DtCrtd,DtMdfd)VALUES(?,?,?,?);</SQL>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Iu.GetConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With CMD.Parameters
                .AddWithValue("?", Gnm1)
                .AddWithValue("?", Convert.ToInt32(ComboBox1.SelectedValue))
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", Now.Date)
            End With
            CN.Open()
            N = CMD.ExecuteNonQuery
        End Using
        MsgBox("تم الحفظ " & N.ToString)
        BtnClear_Click(sender, e)
        ConDGV(<sql>SELECT Grps.GrID, Grps.GrNm, Lvls.LID, Lvls.Lnm, Lvls.SubNm FROM Lvls INNER JOIN Grps ON Lvls.LID = Grps.LID;</sql>.Value)
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Iu.GetConStr},
                CMD As OleDbCommand = New OleDbCommand("SELECT DISTINCT(SubNm) AS SUBN FROM Lvls WHERE LID=?;", CN) With
                {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", Convert.ToInt32(ComboBox1.SelectedValue))
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        TxtSub.Text = Rdr.GetString(0)
                    End While
                End If
            End Using
        End Using
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged

    End Sub

    Private Sub Combo2Pop(ByVal Cbo As ComboBox, ByVal SqlStr As String)
        ComboItem = New Dictionary(Of Integer, String)
        ComboItem.Clear()
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Iu.GetConStr},
        CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Using Rdr As OleDb.OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        ComboItem.Add(Rdr.GetInt32(0), Rdr.GetString(1))
                    End While
                End If
            End Using
        End Using
        With Cbo
            .BeginUpdate()
            .DataSource = New BindingSource(ComboItem, Nothing)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            .EndUpdate()
        End With
    End Sub
    Private Sub ComboBox2_DropDown(sender As Object, e As EventArgs) Handles ComboBox2.DropDown
        Dim SqlStr As String =
                  "SELECT GrID,GrNm FROM Grps;"
        Combo2Pop(ComboBox2, SqlStr)
    End Sub

    Private Sub ComboBox2_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox2.SelectionChangeCommitted
        ToolStripLabel1.Text = String.Empty
        Dim SqlStr As String =
                  "SELECT Lvls.Lnm, Lvls.SubNm FROM Lvls INNER JOIN Grps ON Lvls.LID = Grps.LID WHERE (((Grps.GrID)=?));"
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Iu.GetConStr},
        CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", Convert.ToInt32(ComboBox2.SelectedValue))
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        ToolStripLabel1.Text = "الصف " & Rdr.GetString(0) & " - " & Rdr.GetString(1)
                    End While
                End If
            End Using
        End Using
    End Sub

    Private Sub Form4_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        BackgroundImageLayout = ImageLayout.Stretch
        Invalidate(True)
        Update()
        Refresh()
    End Sub

    Private Sub ComboBox3_DropDown(sender As Object, e As EventArgs) Handles ComboBox3.DropDown
        Dim SqlStr As String =
                  "SELECT TaskID,TaskNm FROM Tsks;"
        Combo2Pop(ComboBox3, SqlStr)
    End Sub
End Class