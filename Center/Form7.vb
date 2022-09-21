Imports System.Data.OleDb

Public Class Form7
    Public Property IC As New Class1
    Private Property GrDtID1 As Integer
    Private Property StID1 As Integer
    Private Property StAttVal As Boolean
    Private Property GrID1 As Integer
    Public Property Constr1 As String = IC.ConStr
    Private TRV As TreeView = New TreeView With {
        .Dock = DockStyle.Fill,
        .RightToLeftLayout = True,
        .LineColor = Color.Red,
        .Font = New Font("Ariel", 11),
        .PathSeparator = "-",
        .HotTracking = True,
        .HideSelection = False, .FullRowSelect = True, .ShowLines = True, .ShowPlusMinus = True, .ShowRootLines = True,
        .BorderStyle = BorderStyle.None
    }
    Private DGStdnts As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill,
        .RowHeadersVisible = True, .BackgroundColor = Color.WhiteSmoke, .ColumnHeadersHeight = 50,
         .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing}
    Private Dtable As DataTable, Dtable1 As DataTable
    Private Function GetMainTree(ByVal DT As DataTable) As TreeView
        DT = IC.GetData(<SQL>SELECT Grps.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID GROUP BY Grps.GrID, 
            Grps.GrNm ORDER BY GrNm;</SQL>.Value)
        If DT.Rows.Count > 0 Then
            TRV.Nodes.Clear()
            TRV.BeginUpdate()
            For Each dr As DataRow In DT.Rows
                Dim DadNode As TreeNode = New TreeNode With {
                    .Name = dr("GrID").ToString(),
                    .Text = dr("GrNm").ToString(),
                    .Tag = dr("GrID").ToString(),
                    .ForeColor = Color.Navy
                }
                TRV.Nodes.Add(DadNode)
            Next
            TRV.EndUpdate()
        End If
        Return TRV
    End Function
    Private Sub TRV_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs)
        Dim DT As DataTable =
            IC.GetData(<SQL>SELECT GrDt.GrDtID, GrDt.GrID, Tsks.TaskID, Grps.GrNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Tsks.TaskNm,  
            Tsks.Fmark FROM Tsks INNER JOIN (Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID) ON  
            Tsks.TaskID = GrDt.TaskID WHERE (((GrDt.GrID)=<%= Integer.Parse(e.Node.Name) %>));</SQL>.Value)
        e.Node.Nodes.Clear()
        If DT.Rows.Count > 0 Then
            For Each dr As DataRow In DT.Rows
                Dim DadNode As TreeNode = New TreeNode With {
                    .Name = dr("GrDtID").ToString(),
                    .Text = Format(dr("Mnm"), "MMMMyyyy") & " -- " & Format(dr("GrDt1"), "dddd, dd/MMMM") &
                    " -- " & Format(dr("GrDt2"), "hh:mm tt") & " -- " & dr("TaskNm").ToString & "( " & dr("Fmark").ToString & " )",
                    .Tag = dr("TaskID").ToString()}
                If e.Node.Level = 0 Then e.Node.Nodes.Add(DadNode)
            Next
        End If
        If e.Node.Level = 0 Then
            GrID1 = Integer.Parse(e.Node.Name)
            Label4.Text = String.Empty
            DGStdnts.Visible = False
            BtnSave.Enabled = False
            BtnDel.Enabled = True
        Else
            GrDtID1 = Integer.Parse(e.Node.Name)
            GrID1 = Integer.Parse(e.Node.Parent.Name)
            'Attnd
            Dim SqlStr1 As String = <sql>SELECT COUNT(GrDtID) FROM Attnd Where GrDtID=<%= GrDtID1 %>;</sql>.Value
            If IC.GetCount1(SqlStr1) <= 0 Then
                MsgBox("برجاء تسجيل غياب المجموعة أولا")
                Exit Sub
            End If
            Label4.Text = e.Node.Text
            DGStdnts.Visible = True
            BtnSave.Enabled = False
            BtnDel.Enabled = True
        End If
        ConDGV("SELECT Stdnts.StID, Stdnts.StNm, Rslts.Mrk FROM (Stdnts INNER JOIN (Grps INNER JOIN GrSt ON Grps.GrID = GrSt.GrID) " &
               "ON Stdnts.StID = GrSt.StID) INNER JOIN Rslts ON Stdnts.StID = Rslts.StID " &
               "WHERE (((GrSt.GrID)=" & GrID1 & ") And ((Rslts.GrDtID)=" & GrDtID1 & ")) ORDER BY Stdnts.StID;")
    End Sub
    Private Sub AddCol()
        If Not DGStdnts.Columns.Contains("Pscore") Then
            Dim AddColumn As New DataGridViewCheckBoxColumn
            With AddColumn
                .HeaderText = "درجة الطالب / 30"
                .Name = "Pscore"
                .FlatStyle = FlatStyle.Standard
                .CellTemplate = New DataGridViewCheckBoxCell()
                .CellTemplate.Style.BackColor = Color.Beige
                .ValueType = GetType(Boolean)
                .ThreeState = False
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                .DataPropertyName = "Mrk"
            End With
            DGStdnts.Columns.Insert(2, AddColumn)
        End If
    End Sub
    Private Sub ConDGV(ByVal SqlStr As String)
        'SqlStr = <sql>SELECT * From Stdnts;</sql>.Value
        Dim Dt2 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt2)
        End Using
        With DGStdnts
            .EnableHeadersVisualStyles = False  'Will display the custom formats of mine.
            .EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
            .GridColor = SystemColors.Control
            .SelectionMode = DataGridViewSelectionMode.CellSelect
            .DataSource = New BindingSource(Dt2, Nothing)
            .MultiSelect = False
        End With
        GroupBox3.Controls.Add(DGStdnts)
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
            .Alignment = DataGridViewContentAlignment.MiddleCenter
            .Font = New Font("Arial", 11.25, FontStyle.Bold)
        End With
        If DGStdnts.Columns.Contains("Mrk") Then
            DGStdnts.Columns.Remove("Mrk")
        End If

        AddCol()
        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        AddHandler DGStdnts.RowPostPaint, AddressOf DGStdnts_RowPostPaint
        AddHandler DGStdnts.CellValidating, AddressOf DGStdnts_CellValidating
        AddHandler DGStdnts.CellValidated, AddressOf DGStdnts_CellValidated
        AddHandler DGStdnts.CellClick, AddressOf DGStdnts_CellClick
        AddHandler DGStdnts.CurrentCellDirtyStateChanged, AddressOf DGStdnts_CurrentCellDirtyStateChanged
        AddHandler DGStdnts.CellEndEdit, AddressOf DGStdnts_CallEndEdit
        '    AddHandler DGStdnts.CurrentCellDirtyStateChanged, AddressOf DGStdnts_CurrentCellDirtyStateChanged
    End Sub
    Private Sub DGStdnts_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        'Check to ensure that the row CheckBox is clicked.
        If e.ColumnIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        If e.RowIndex >= 0 AndAlso DGStdnts.Columns(e.ColumnIndex).Name = "Pscore" Then
            'Reference the GridView Row.
            Dim row As DataGridViewRow = DGStdnts.Rows(e.RowIndex)
            'Set the CheckBox selection.
            StID1 = Convert.ToInt32(row.Cells("StID").Value)
            DGStdnts.EndEdit()
        End If
    End Sub
    Sub DGStdnts_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs)
        If DGStdnts.Columns(2).Name = "Pscore" Then
            If DGStdnts.IsCurrentCellDirty Then
                DGStdnts.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End If
    End Sub
    Private Sub DGStdnts_CallEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        If BtnSave.Enabled = True Then RemoveHandler DGStdnts.CellEndEdit, AddressOf DGStdnts_CallEndEdit : Exit Sub
        If IsNothing(DGStdnts.CurrentCell) Then Exit Sub
        If e.ColumnIndex = 2 And TypeOf DGStdnts.CurrentCell Is DataGridViewTextBoxCell Then
            'do code to UPDATE database
            'EDIT
            Dim N As Integer
            Dim SqlStr As String =
                <SQL>UPDATE Rslts SET Mrk=?, Dtmdfd=? WHERE Rslts.StID=? AND Rslts.GrDtID=?;</SQL>.Value
            Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                    CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                With CMD.Parameters
                    .AddWithValue("?", Convert.ToDouble(DGStdnts.CurrentRow.Cells("Pscore").FormattedValue))
                    .AddWithValue("?", Now.Date)
                    .AddWithValue("?", StID1)
                    .AddWithValue("?", GrDtID1)
                End With
                CN.Open()
                N = CMD.ExecuteNonQuery
            End Using
            If N >= 1 Then
                DGStdnts.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen
            Else
                DGStdnts.CurrentRow.DefaultCellStyle.BackColor = Color.Red
            End If
        End If
    End Sub
    Private Sub DGStdnts_CellValidated(sender As Object, e As DataGridViewCellEventArgs)
        DGStdnts.Rows(e.RowIndex).ErrorText = Nothing
    End Sub
    Private Sub DGStdnts_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs)
        DGStdnts.Rows(e.RowIndex).ErrorText = ""
        ' Don't try to validate the 'new row' until finished 
        ' editing since there Is Not any point in validating its initial value.
        If DGStdnts.Columns(e.ColumnIndex).Name = "Pscore" Then
            If e.FormattedValue.ToString.Length >= 1 Then
                If Not IsNumeric(e.FormattedValue) OrElse Convert.ToInt32(e.FormattedValue) > 30 Then
                    e.Cancel = True
                    DGStdnts.Rows(e.RowIndex).ErrorText = "اقصي درجة 30"
                End If
            End If
        End If
    End Sub
    Private Sub DGStdnts_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim grid = TryCast(sender, DataGridView)
        Dim rowIdx As String = Convert.ToString(e.RowIndex + 1)
        Using centerFormat As StringFormat = New StringFormat() With
            {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim headerBounds =
                New Rectangle(e.RowBounds.Right - 42, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
            e.Graphics.DrawString(rowIdx, Font, SystemBrushes.ActiveCaptionText, headerBounds, centerFormat)
        End Using
    End Sub
    Private Sub Form7_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Dim SqlStr As String =
            "SELECT Attnd.StID, Stdnts.StNm FROM (Stdnts INNER JOIN GrSt ON Stdnts.StID = GrSt.StID) INNER JOIN Attnd ON " &
            "Stdnts.StID = Attnd.StID WHERE (((Attnd.GrDtID)=" & GrDtID1 & ") AND ((Attnd.PStat)=False));"
        Dim Dt2 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt2)
        End Using
        DGStdnts.DataSource = Dt2
        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        AddCol()
        BtnSave.Enabled = True
        BtnDel.Enabled = False
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Dim CNTra As OleDbTransaction = Nothing
        Dim N As Integer
        Dim SqlStr1 As String =
                "DELETE * FROM Rslts WHERE GrDtID=?;"
        'If CType(IRow.Cells("PStat").Value, Boolean) = True Then
        'اليوم الواحد ممكن يكون فيه أكتر من مجموعه
        Using cn As New OleDbConnection(Constr1)
            '           CNTra = cn.BeginTransaction(IsolationLevel.ReadCommitted)
            Dim cmd = New OleDbCommand(SqlStr1, cn) With {.CommandType = CommandType.Text}
            With cmd.Parameters
                .AddWithValue("?", GrDtID1)
            End With
            cn.Open()
            'Irow in the database "True" then do nothing
            N = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
        End Using
        Dim sqlstr As String = "INSERT INTO Rslts(GrDtID,StID,Mrk,DtCrtd,DtMdfd) VALUES(?,?,?,?,?);"
        For Each IRow As DataGridViewRow In DGStdnts.Rows
            'INSERT
            Using cn As New OleDbConnection(Constr1)
                Dim cmd = New OleDbCommand(sqlstr, cn) With {.CommandType = CommandType.Text}
                With cmd.Parameters
                    .AddWithValue("?", GrDtID1)
                    .AddWithValue("?", CType(IRow.Cells("StID").Value, Integer))
                    .AddWithValue("?", CType(IRow.Cells("Pscore").Value, Double))
                    .AddWithValue("?", Now.Date)
                    .AddWithValue("?", Now.Date)
                End With
                cn.Open()
                cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
            End Using
        Next
        MsgBox("تم تسجيل الدرجات بنجاح",
               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        ConDGV("SELECT Stdnts.StID, Stdnts.StNm, Rslts.Mrk FROM (Stdnts INNER JOIN (Grps INNER JOIN GrSt ON Grps.GrID = GrSt.GrID) " &
               "ON Stdnts.StID = GrSt.StID) INNER JOIN Rslts ON Stdnts.StID = Rslts.StID " &
               "WHERE (((GrSt.GrID)=" & GrID1 & ") And ((Rslts.GrDtID)=" & GrDtID1 & ")) ORDER BY Stdnts.StID;")
    End Sub
    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        DoubleBuffered = True
        'WindowState = FormWindowState.Maximized
        MdiParent = Form1
        'Create View of all Groups Dates
        With TRV
            .BackColor = SystemColors.Control
            .ImageList = ImageList1
            .ImageIndex = 0
            .SelectedImageIndex = 1
        End With
        Dim dt3 As DataTable = New DataTable
        GroupBox1.Controls.Add(TRV)
        AddHandler TRV.AfterSelect, AddressOf TRV_AfterSelect
        GetMainTree(dt3)
    End Sub

    Private Sub Form7_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
        If e.KeyData = Keys.Delete Then
            If BtnDel.Enabled = True Then
                BtnDel_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub BtnDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDel.Click
        'Delete
        'Attend and Results Tables are linked with Students
        If DGStdnts.Rows.Count <= 0 Or GrDtID1 <= 0 Then
            DGStdnts.AllowUserToDeleteRows = False
            MsgBox("يجب اختيار ميعاد أولا.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim SqlStr1 As String =
            <sql>SELECT COUNT(GrDtID) FROM Rslts Where GrDtID=<%= GrDtID1 %>;</sql>.Value


        Dim AreUsURE As MsgBoxResult =
            MsgBox("يجب حذف درجات طلاب المجوعه فى هذا اليوم أولا.",
         MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
        If AreUsURE = MsgBoxResult.Yes Then
            DGStdnts.AllowUserToDeleteRows = True
            Dim RUSre As MsgBoxResult = MsgBox("تأكيد حذف كشف الغياب من البرنامج.",
                                                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                                   MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
            If RUSre = MsgBoxResult.Yes Then
                Dim N As Integer
                Dim SqlStr As String =
                    <SQL>DELETE * From Attnd WHERE Attnd.GrDtID=?;</SQL>.Value
                Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                        CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", GrDtID1)
                    End With
                    CN.Open()
                    N = CMD.ExecuteNonQuery
                End Using
                MsgBox("تم حذف عدد  " & N.ToString & " كشف غياب.",
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            Else
                MsgBox("تم الغاء العملية",
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            End If
            BtnSave.Enabled = False
            BtnDel.Enabled = False
        Else
            DGStdnts.AllowUserToDeleteRows = False
        End If
    End Sub
End Class