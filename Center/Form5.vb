Imports System.Data.OleDb

Public Class Form5
    Private Property IC As New Class1
    Private Property Constr1 As String = IC.ConStr
    Private Property GrID1 As Integer = Form2.GrID1
    Private StNm, StMob1, StMob2 As String, StID1 As Integer
    Private WithEvents DGStdnts As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill,
        .EnableHeadersVisualStyles = False, .RowHeadersVisible = True, .BackgroundColor = Color.WhiteSmoke,
        .ColumnHeadersHeight = 50, .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing},
        Dt1 As DataTable

    Private Sub Form5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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
    End Sub
    Private Sub Form5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        ComboBox1.SelectedIndex = 2
        Text = "اضافة طلاب الي " & Form2.ComboBox2.Text
        Dim sqlstr As String =
            <sql>SELECT GrDt.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID GROUP BY GrDt.GrID, Grps.GrNm;</sql>.Value
        IC.GetGrps(Dt1, Constr1, sqlstr, ComboBox2, "GrNm", "GrID")
        ConDGV("SELECT Stdnts.StID, Stdnts.StNm FROM Stdnts LEFT JOIN (Grps RIGHT JOIN (GrSt LEFT JOIN GrDt ON " & _
               "GrSt.GrID = GrDt.GrID) ON Grps.GrID = GrDt.GrID) ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID) Is Null));")
        SetCols()
        Dt1 = New DataTable
        Dt1.Columns.Add(New DataColumn("StID") With {.DataType = GetType(Integer), .Caption = "كود الطالب"})
        Dt1.Columns.Add(New DataColumn("StNm") With {.DataType = GetType(String), .Caption = "اسم الطالب"})
        BtnClear.Enabled = True
        BtnSave.Enabled = False
        ToolStripButton1.Enabled = False
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectionChangeCommitted
        TextBox1.SelectAll()
        TextBox1.Text = String.Empty
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            If IsNothing(ComboBox1.SelectedItem) Or ComboBox1.SelectedIndex = -1 Then
                MsgBox("من فضلك أختر فلتر البحث أولا", _
                       MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
                Exit Sub
            End If
            If TextBox1.Text.Contains("'") Or
                    TextBox1.Text.Contains(",") Or
                    TextBox1.Text.Contains(".") Then
                MsgBox("الاسم الذي أدخلته غير صحيح", MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
                Exit Sub
            End If
            If TextBox1.Text.Length <= 0 Then Exit Sub
            Dim SqlStr2 As String = String.Empty
            Dim SqlStr1 As String =
                "SELECT Stdnts.StID, Stdnts.StNm FROM Stdnts LEFT JOIN (Grps RIGHT JOIN (GrSt LEFT JOIN GrDt ON " & _
                "GrSt.GrID = GrDt.GrID) ON Grps.GrID = GrDt.GrID) ON Stdnts.StID = GrSt.StID "
            If ComboBox1.SelectedIndex = 0 Then
                SqlStr2 = <sql>WHERE (((Stdnts.StNm) LIKE '%<%= TextBox1.Text %>%') AND ((GrSt.GrID) Is Null));</sql>.Value
            ElseIf ComboBox1.SelectedIndex = 1 Then
                SqlStr2 = <sql>WHERE (((Stdnts.Mob1) LIKE '%<%= TextBox1.Text %>%') AND ((GrSt.GrID) Is Null)) OR 
(((Stdnts.Mob2) LIKE '%<%= TextBox1.Text %>%'));</sql>.Value
            ElseIf ComboBox1.SelectedIndex = 2 Then
                If Not IsNumeric(TextBox1.Text) Then
                    TextBox1.SelectAll()
                    SendKeys.Send("{BackSpace}")
                    Exit Sub
                End If
                SqlStr2 = <sql>WHERE (((Stdnts.StID) = <%= Convert.ToInt32(TextBox1.Text) %>) AND ((GrSt.GrID) Is Null));</sql>.Value
            End If
            ConDGV(SqlStr1 & SqlStr2)
            SetCols()
        End If
    End Sub
    Private Sub AddCol()
        If Not DGStdnts.Columns.Contains("Chk") Then
            Dim AddColumn As New DataGridViewCheckBoxColumn
            With AddColumn
                .Name = "Chk"
                .ReadOnly = False
                .HeaderText = "اختر الطالب"
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
            End With
            DGStdnts.Columns.Insert(0, AddColumn)
        End If
    End Sub
    Private Dt2 As DataTable
    Private Sub ConDGV(ByVal SqlStr As String)
        DGStdnts.DataBindings.Clear()
        DGStdnts.DataSource = Nothing
        Dt2 = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        'SqlStr = <sql>SELECT * From Stdnts;</sql>.Value
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
            .DataSource = New BindingSource(Dt2, Nothing)
            .MultiSelect = False
        End With
        DGStdnts.DataSource = New BindingSource(Dt2, Nothing)
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
        AddCol()
        AddHandler DGStdnts.RowPostPaint, AddressOf DGSTDNTS_RowPostPaint
        AddHandler DGStdnts.CurrentCellDirtyStateChanged, AddressOf Dgstdnts_CurrentCellDirtyStateChanged
        AddHandler DGStdnts.CellValueChanged, AddressOf Dgstdnts_CellValueChanged
        AddHandler DGStdnts.EditingControlShowing, AddressOf DGStdnts_EditingControlShowing
    End Sub
    Private WithEvents m_editingcontrol As Control
    Private Sub DGStdnts_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs)
        m_editingcontrol = e.Control ' ←　Register EventHandler Here
    End Sub
    Private Sub m_editingcontrol_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles m_editingcontrol.PreviewKeyDown
        If e.KeyCode = Keys.Return Then
            DGStdnts.EndEdit()
        End If
    End Sub
    Sub Dgstdnts_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs)
        If DGStdnts.Columns(0).Name = "Chk" Then
            If DGStdnts.IsCurrentCellDirty Then
                DGStdnts.CommitEdit(DataGridViewDataErrorContexts.Commit)
                Io2 = Convert.ToInt32(DGStdnts(1, DGStdnts.CurrentCell.RowIndex).Value)
                Dim dr As DataRow = Dt1.NewRow
                dr("StID") = Io2
                dr("StNm") = Convert.ToString(DGStdnts.CurrentRow.Cells("StNm").Value)
                Dt1.Rows.Add(dr)
            End If
            BtnClear.Enabled = True
            BtnSave.Enabled = True
            ToolStripButton1.Enabled = True
        End If
    End Sub
    ' If a check box cell is clicked, this event handler disables  
    ' or enables the button in the same row as the clicked cell.
    Public Sub Dgstdnts_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.ColumnIndex = -1 Or e.RowIndex = -1 Then Exit Sub
        If DGStdnts.Columns(e.ColumnIndex).Name = "Chk" Then
            Dim checkCell As DataGridViewCheckBoxCell = CType(DGStdnts.Rows(e.RowIndex).Cells("Chk"), DataGridViewCheckBoxCell)
            DGStdnts.EndEdit()
            DGStdnts.Invalidate()
        End If
    End Sub
    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClear.Click
        Dim sqlstr As String =
            <sql>SELECT GrDt.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID GROUP BY GrDt.GrID, Grps.GrNm;</sql>.Value
        IC.GetGrps(Dt1, Constr1, sqlstr, ComboBox2, "GrNm", "GrID")
        Dt1.Rows.Clear()
        ConDGV(<sql>SELECT Stdnts.StID, Stdnts.StNm FROM Stdnts LEFT JOIN (Grps RIGHT JOIN (GrSt LEFT JOIN GrDt ON 
            GrSt.GrID = GrDt.GrID) ON Grps.GrID = GrDt.GrID) ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID) Is Null));</sql>.Value)
        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").Visible = True
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DGStdnts.Columns("Chk").Visible = True
        DGStdnts.Columns("Chk").ReadOnly = False
        DGStdnts.Columns("Chk").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        ToolStripButton1.Enabled = False
        BtnSave.Enabled = False
        GroupBox2.Text = "طلبة غير مسجلين بأي مجموعة"
    End Sub
    Private Io2 As Integer
    Private Sub DGSTDNTS_RowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs)
        Dim grid = TryCast(sender, DataGridView)
        Dim rowIdx As String = Convert.ToString(e.RowIndex + 1)
        Using centerFormat As StringFormat = New StringFormat() With
            {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim headerBounds =
                New Rectangle(e.RowBounds.Right - 42, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
            e.Graphics.DrawString(rowIdx, Font, SystemBrushes.ActiveBorder, headerBounds, centerFormat)
        End Using
    End Sub
    Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSave.Click
        If Dt1.Rows.Count <= 0 Then
            MsgBox("يجب اختيار طالب واحد علي الأقل", MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim sqlstr As String =
            "INSERT INTO GrSt(GrID,StID) VALUES(?,?);"
        Using cn As New OleDbConnection(Constr1),
            cmD As New OleDbCommand(sqlstr, cn)
            cmD.CommandType = CommandType.Text
            cn.Open()
            For Each DTR As DataRow In Dt1.Rows
                cmD.Parameters.AddWithValue("?", Convert.ToInt32(Form2.ComboBox2.SelectedValue))
                cmD.Parameters.AddWithValue("?", DTR(0))
                cmD.ExecuteNonQuery()
                cmD.Parameters.Clear()
            Next
        End Using
        MsgBox("تم اضافة الطلاب للمجموعة بنجاح",
             MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Information)
        Dt1.Rows.Clear()
        ConDGV("SELECT Stdnts.StID, Stdnts.StNm FROM Stdnts LEFT JOIN (Grps RIGHT JOIN (GrSt LEFT JOIN GrDt ON GrSt.GrID = GrDt.GrID) " & _
               "ON Grps.GrID = GrDt.GrID) ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID) Is Null));")
        SetCols()
        ToolStripButton1.Enabled = False
    End Sub
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        DGStdnts.DataSource = New BindingSource(Dt1, Nothing)
        SetCols()
    End Sub
    Private Sub ComboBox2_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectionChangeCommitted
        Dim sqlstr As String =
            "SELECT GrSt.StID, Stdnts.StNm FROM Stdnts INNER JOIN (Grps INNER JOIN GrSt ON Grps.GrID = GrSt.GrID) " & _
            "ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID)=?));"
        Dim Dt2 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(sqlstr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.SelectCommand.Parameters.AddWithValue("?", Convert.ToInt32(ComboBox2.SelectedValue))
            DataAdapter1.Fill(Dt2)
        End Using
        AddCol()
        DGStdnts.DataSource = Nothing
        DGStdnts.DataSource = New BindingSource(Dt2, Nothing)
        SetCols()

        BtnSave.Enabled = False
        ToolStripButton1.Enabled = False
        GroupBox2.Text = "طلبة مسجلين بالمجموعة : " & ComboBox2.Text
    End Sub

    Private Sub SetCols()
        DGStdnts.Columns("StID").Visible = True
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader

        DGStdnts.Columns("StNm").Visible = True
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        DGStdnts.Columns("Chk").Visible = False
    End Sub
End Class