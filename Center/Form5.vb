Imports System.Data.OleDb

Public Class Form5
    Private DG, DG1 As DataGridView, IU As Class1 = New Class1
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        With DateTimePicker2
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "d dddd"
            .ShowCheckBox = True
            .Checked = False
        End With
        With DateTimePicker3
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "hh:mm tt"
            .ShowCheckBox = True
            .Checked = False
            .ShowUpDown = True
        End With
    End Sub
    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs)
        'save Group
        Dim N As Integer
        Dim SqlStr As String =
            "INSERT INTO GrDt(GrID,Mnm,GrDt1,GrDt2,TaskID,DtCrtd,DtMdfd)VALUES(?,?,?,?,?,?,?);"
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Iu.ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With CMD.Parameters
                .AddWithValue("?", Convert.ToInt32(ComboBox2.SelectedValue))
                .AddWithValue("?", DateTimePicker1.Value.Date)
                .AddWithValue("?", DateTimePicker2.Value.Date)
                .AddWithValue("?", DateTimePicker3.Value.TimeOfDay)
                .AddWithValue("?", Convert.ToInt32(ComboBox3.SelectedValue))
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", Now.Date)
            End With
            CN.Open()
            N = CMD.ExecuteNonQuery()
        End Using
        MsgBox("تم الحفظ " & N.ToString)
        ToolStripButton6_Click(sender, e)
    End Sub
    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs)
        DG1 = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, .RowHeadersVisible = False,
        .BackgroundColor = Color.WhiteSmoke}
        Dim SqlStr As String =
        <sql>SELECT GrDt.GrDtID, Grps.GrID, Tsks.TaskID, Lvls.LID, Grps.GrNm, Lvls.Lnm, Lvls.SubNm, Tsks.TaskNm, GrDt.Mnm, GrDt.GrDt1, 
        GrDt.GrDt2 FROM Lvls INNER JOIN ((Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID) INNER JOIN Tsks ON GrDt.TaskID = Tsks.TaskID) 
        ON Lvls.LID = Grps.LID;</sql>.Value
        With DG1
            .DataSource = Nothing
            .ReadOnly = True
            .ColumnCount = 10
            .AutoGenerateColumns = False
            .Columns(0).Name = "GrDtID"
            .Columns(0).DataPropertyName = "GrDtID"
            .Columns(0).Visible = False

            .Columns(1).Name = "GrID"
            .Columns(1).DataPropertyName = "GrID"
            .Columns(1).Visible = False

            .Columns(2).Name = "TaskID"
            .Columns(2).DataPropertyName = "TaskID"
            .Columns(2).Visible = False

            .Columns(3).Name = "GrNm"
            .Columns(3).HeaderText = "المجموعة"
            .Columns(3).DataPropertyName = "GrNm"
            .Columns(3).ReadOnly = True

            .Columns(4).Name = "Lnm"
            .Columns(4).HeaderText = "الصف"
            .Columns(4).DataPropertyName = "Lnm"
            .Columns(4).ReadOnly = True

            .Columns(5).Name = "SubNm"
            .Columns(5).HeaderText = "المادة الدراسية"
            .Columns(5).DataPropertyName = "SubNm"
            .Columns(5).ReadOnly = False

            .Columns(6).Name = "TaskNm"
            .Columns(6).HeaderText = "الأعمال"
            .Columns(6).DataPropertyName = "TaskNm"
            .Columns(6).ReadOnly = False

            .Columns(7).Name = "Mnm"
            .Columns(7).HeaderText = "الشهر"
            .Columns(7).DataPropertyName = "Mnm"
            .Columns(7).ReadOnly = False

            .Columns(8).Name = "GrDt1"
            .Columns(8).HeaderText = "اليوم"
            .Columns(8).DataPropertyName = "GrDt1"
            .Columns(8).ReadOnly = False

            .Columns(9).Name = "GrDt2"
            .Columns(9).HeaderText = "الساعة"
            .Columns(9).DataPropertyName = "GrDt2"
            .Columns(9).ReadOnly = False

            .DataSource = New BindingSource(Iu.GetData(SqlStr), Nothing)
        End With
        GroupBox4.Controls.Add(DG1)
        AddHandler DG1.CellFormatting, AddressOf DGV_CellFormatting
        AddHandler DG1.CellClick, AddressOf DGV_CellClick
        DG1.Invalidate(True)
        DG1.Update()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub DGV_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        Dim GrID1 As Integer
        'Dim SelectedRow As DataGridViewRow = DG1.Rows(e.RowIndex)
        'GrDt2 = Convert.ToInt32(SelectedRow.Cells.Item("GrDtID").Value)
        'GrID1 = Convert.ToInt32(SelectedRow.Cells.Item("GrID").Value)
        ComboBox2.SelectedValue = GrID1
        'TxtNm.Text = SelectedRow.Cells.Item("GrNm").Value.ToString
        'TxtSub.Text = SelectedRow.Cells.Item("SubNm").Value.ToString
        'Fetch Group Name
        'ToolStripLabel1.Text = "المجموعه : " & GetGrpName(StID1)
        'BtnEdit.Enabled = True
        'BtnDel.Enabled = True
        'BtnSave.Enabled = False
    End Sub
    Private Sub DGV_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        'If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        'If DG1.Columns(e.ColumnIndex).Name = "Mnm" AndAlso IsDate(e.Value) Then
        ' DG1.Columns(e.ColumnIndex).DefaultCellStyle.Format = "MMMMyyyy"
        ' End If
        ' If DG1.Columns(e.ColumnIndex).Name = "GrDt1" AndAlso IsDate(e.Value) Then
        ' DG1.Columns(e.ColumnIndex).DefaultCellStyle.Format = "dddd yyyy/MM/dd"
        ' End If
        ' If DG1.Columns(e.ColumnIndex).Name = "GrDt2" AndAlso IsDate(e.Value) Then
        ' DG1.Columns(e.ColumnIndex).DefaultCellStyle.Format = "hh:mm t"
        ' End If
    End Sub
    Private Sub Combo2Pop(ByVal Cbo As ComboBox, ByVal SqlStr As String)
        ' ComboItem = New Dictionary(Of Integer, String)
        ' ComboItem.Clear()
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = IU.ConStr},
        CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        '                ComboItem.Add(Rdr.GetInt32(0), Rdr.GetString(1))
                    End While
                End If
            End Using
        End Using
    End Sub
    Private Sub ComboBox2_DropDown(sender As Object, e As EventArgs)
        Dim SqlStr As String =
                  "SELECT GrID,GrNm FROM Grps;"
        Combo2Pop(ComboBox2, SqlStr)
        With ComboBox2
            .BeginUpdate()
            '   .DataSource = New BindingSource(ComboItem, Nothing)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            .EndUpdate()
        End With
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub ComboBox2_SelectionChangeCommitted(sender As Object, e As EventArgs)
        ToolStripLabel1.Text = String.Empty
        Dim SqlStr As String =
                  "SELECT Lvls.Lnm, Lvls.SubNm FROM Lvls INNER JOIN Grps ON Lvls.LID = Grps.LID WHERE (((Grps.GrID)=?));"
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = IU.ConStr},
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


    Private Sub ComboBox3_DropDown(sender As Object, e As EventArgs)
        Dim SqlStr As String =
                  "SELECT TaskID,TaskNm FROM Tsks;"
        Combo2Pop(ComboBox3, SqlStr)
        With ComboBox3
            .BeginUpdate()
            '   .DataSource = New BindingSource(ComboItem, Nothing)
            .DisplayMember = "Value"
            .ValueMember = "Key"
            .EndUpdate()
        End With
    End Sub
End Class