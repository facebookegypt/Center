Imports System.Data.OleDb

Public Class Form7
    Public Property IC As New Class1
    Private Property GrDtID1 As Integer
    Private Property StID1 As Integer
    Private Property StAttVal As Boolean
    Private Property GrID1 As Integer
    Private Property FlMrk As Integer
    Public Property Constr1 As String = IC.ConStr
    Private WithEvents TRV As TreeView
    Private WithEvents DGStdnts As New DataGridView
    Private RptSrc As String
    Private Function GetMainTree(ByVal DT As DataTable) As TreeView
        DT = IC.GetData(<SQL>SELECT Grps.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID GROUP BY Grps.GrID, 
            Grps.GrNm ORDER BY Grps.GrID, Grps.GrNm ASC;</SQL>.Value)
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
        Dim SqlStr1 As String =
            "SELECT GrDt.GrDtID, GrDt.GrID, Tsks.TaskID, Grps.GrNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Tsks.TaskNm, GrDt.TskNm " &
            "FROM Tsks INNER JOIN (Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID) ON Tsks.TaskID = GrDt.TaskID " &
            "WHERE (((GrDt.GrID)=" & Integer.Parse(e.Node.Name) & ")) ORDER BY GrDt.GrDt1, GrDt.GrDt2 ASC;"
        Dim DT As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        DT = IC.GetData(SqlStr1)
        e.Node.Nodes.Clear()
        If DT.Rows.Count > 0 Then
            For Each dr As DataRow In DT.Rows
                Dim DadNode As TreeNode = New TreeNode With {
                    .Name = dr("GrDtID").ToString(),
                    .Text = Format(dr("Mnm"), "MMMMyyyy") & " -- " & Format(dr("GrDt1"), "dddd, dd/MMMM") &
                    " -- " & Format(dr("GrDt2"), "hh:mm tt") & " -- " & dr("TaskNm").ToString & "( " & dr("TskNm").ToString & " )",
                    .Tag = dr("TaskID").ToString()}
                If e.Node.Level = 0 Then e.Node.Nodes.Add(DadNode)
            Next
        End If
        DT.Dispose()
        If e.Node.Level = 0 Then
            GrID1 = Integer.Parse(e.Node.Name)
            Label4.Text = String.Empty
            DGStdnts.Visible = False
            BtnSave.Enabled = False
            BtnDel.Enabled = True
            ToolStripButton1.Enabled = False
        Else
            GrDtID1 = Integer.Parse(e.Node.Name)
            GrID1 = Integer.Parse(e.Node.Parent.Name)
            'Attnd
            Dim SqlStr2 As String = <sql>SELECT COUNT(GrDtID) FROM Attnd Where GrDtID=<%= GrDtID1 %>;</sql>.Value
            If IC.GetCount1(SqlStr2) <= 0 Then
                MsgBox("برجاء تسجيل غياب المجموعة أولا")
                DGStdnts.Columns.Clear()
                DGStdnts.DataSource = Nothing
                BtnSave.Enabled = False
                BtnDel.Enabled = False
                BtnClear.Enabled = False
                Exit Sub
            End If
            Label4.Text = e.Node.Text
            DGStdnts.Visible = True
            BtnClear.Enabled = True
            BtnSave.Enabled = False
            BtnDel.Enabled = True
            ToolStripButton1.Enabled = True
            Dim SqlDel As String =
                "DROP VIEW MarksRpt;"
            Dim SqlCreate As String =
                "CREATE VIEW MarksRpt AS SELECT GrDt.GrDtID, GrDt.GrID, Rslts.StID, Stdnts.StNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Grps.GrNm, " &
                "Grps.Lnm, Grps.SubNm, Tsks.TaskID, Tsks.TaskNm, Rslts.Mrk, GrDt.TskFlMrk, GrDt.TskNm FROM Tsks INNER JOIN (Stdnts INNER JOIN " &
                "((GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) INNER JOIN Rslts ON GrDt.GrDtID = Rslts.GrDtID) ON " &
                "Stdnts.StID = Rslts.StID) ON Tsks.TaskID = GrDt.TaskID WHERE (((GrDt.GrDtID)=" & GrDtID1 & ") And " &
                "((GrDt.GrID)=" & GrID1 & "));"
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
            RptSrc = "MarksRpt"
        End If

        ConDGV("SELECT Stdnts.StID, Stdnts.StNm, Rslts.Mrk FROM (Stdnts INNER JOIN (Grps INNER JOIN GrSt ON Grps.GrID = GrSt.GrID) " &
               "ON Stdnts.StID = GrSt.StID) INNER JOIN Rslts ON Stdnts.StID = Rslts.StID " &
               "WHERE (((GrSt.GrID)=" & GrID1 & ") And ((Rslts.GrDtID)=" & GrDtID1 & ")) ORDER BY Stdnts.StID ASC;")
        AddCol()
    End Sub
    Private Sub AddCol()
        If GrDtID1 <= 0 Then Exit Sub
        Using CN As New OleDbConnection(Constr1),
                CMD As New OleDbCommand("SELECT GrDt.TskFlMrk FROM GrDt WHERE GrDtID=?;", CN) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", GrDtID1)
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                ' Call GetOrdinal and assign value to variable.
                Dim TskFlMrkOrd As Integer = Rdr.GetOrdinal("TskFlMrk")
                If Rdr.HasRows Then
                    While Rdr.Read
                        FlMrk = Rdr!TskFlMrk
                    End While
                End If
            End Using
        End Using
        Dim AddColumn As New DataGridViewTextBoxColumn
        If Not DGStdnts.Columns.Contains("Pscore") Then
            With AddColumn
                .DisplayIndex = 2
                .HeaderText = "الدرجة/" & FlMrk.ToString
                .Name = "Pscore"
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                .CellTemplate = New DataGridViewTextBoxCell()
                .CellTemplate.Style.BackColor = Color.Beige
                .CellTemplate.Style.ForeColor = Color.Black
                .CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ValueType = GetType(Double)
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                .DataPropertyName = "Mrk"
            End With
            DGStdnts.Columns.Insert(2, AddColumn)
        End If
        DGStdnts.Refresh()
        DGStdnts.Update()
    End Sub
    Private Sub DGStdnts_DataError(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs)
        ' If the data source raises an exception when a cell value is 
        ' commited, display an error message.
        If DGStdnts.Columns(e.ColumnIndex).Name = "Pscore" Then
            If e.Exception IsNot Nothing And e.Exception.Message = "Input string was not in a correct format." _
                        AndAlso e.Context = DataGridViewDataErrorContexts.Commit Then
                e.Cancel = True
            End If
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
        GroupBox3.Controls.Add(DGStdnts)
        With DGStdnts
            .DataSource = Nothing
            .DataSource = New BindingSource(Dt2, Nothing)
            .EnableHeadersVisualStyles = False  'Will display the custom formats of mine.
            .EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
            .GridColor = SystemColors.Control
            .SelectionMode = DataGridViewSelectionMode.CellSelect
            .Name = "DGV2"
            .BorderStyle = BorderStyle.None
            .RightToLeft = RightToLeft.Yes
            .AllowUserToAddRows = False
            .Dock = DockStyle.Fill
            .RowHeadersVisible = True
            .BackgroundColor = Color.WhiteSmoke
            .ColumnHeadersHeight = 50
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .MultiSelect = False
        End With
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

        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        'DGStdnts.Columns("Mrk").Visible = False
        AddHandler DGStdnts.RowPostPaint, AddressOf DGStdnts_RowPostPaint
        AddHandler DGStdnts.CellValidating, AddressOf DGStdnts_CellValidating
        AddHandler DGStdnts.CellValidated, AddressOf DGStdnts_CellValidated
        AddHandler DGStdnts.CellClick, AddressOf DGStdnts_CellClick
        AddHandler DGStdnts.CurrentCellDirtyStateChanged, AddressOf DGStdnts_CurrentCellDirtyStateChanged
        AddHandler DGStdnts.CellEndEdit, AddressOf DGStdnts_CallEndEdit
        AddHandler DGStdnts.DataError, AddressOf DGStdnts_DataError
    End Sub
    Private Sub DGStdnts_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        'Check to ensure that the row CheckBox is clicked.
        If e.ColumnIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        If e.RowIndex >= 0 AndAlso DGStdnts.Columns(e.ColumnIndex).Name = "Pscore" Then
            'Reference the GridView Row.
            Dim row As DataGridViewRow = DGStdnts.Rows(e.RowIndex)
            'Set the CheckBox selection.
            StID1 = Convert.ToInt32(row.Cells("StID").Value)
            DGStdnts.EndEdit(DataGridViewDataErrorContexts.Commit)
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
        DGStdnts.Rows(e.RowIndex).ErrorText = String.Empty
        ' Don't try to validate the 'new row' until finished 
        ' editing since there Is Not any point in validating its initial value.
        If DGStdnts.Columns(e.ColumnIndex).Name = "Pscore" Then
            If e.FormattedValue.ToString.Length >= 1 Then
                If Not IsNumeric(e.FormattedValue) OrElse Convert.ToDouble(e.FormattedValue) > FlMrk Then
                    e.Cancel = True
                    DGStdnts.Rows(e.RowIndex).ErrorText = "اقصي درجة " & FlMrk.ToString
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
            e.Graphics.DrawString(rowIdx, Font, Brushes.White, headerBounds, centerFormat)
        End Using
    End Sub
    Private Sub Form7_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Dim SqlStr As String =
            "SELECT Attnd.StID, Stdnts.StNm FROM (Stdnts INNER JOIN GrSt ON Stdnts.StID = GrSt.StID) INNER JOIN Attnd ON " &
            "Stdnts.StID = Attnd.StID WHERE (((Attnd.GrDtID)=" & GrDtID1 & ") AND ((Attnd.PStat)=False));"
        Dim Dt2 As New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt2)
        End Using
        If DGStdnts.Columns.Contains("Mrk") Then
            DGStdnts.Columns.Remove("Mrk")
        End If
        DGStdnts.DataSource = New BindingSource(Dt2, Nothing)
        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        AddCol()
        If DGStdnts.Rows.Count >= 1 Then
            BtnSave.Enabled = True
            BtnDel.Enabled = False
        ElseIf DGStdnts.Rows.Count <= 0 Then
            BtnSave.Enabled = False
            BtnDel.Enabled = False
        End If
        Dt2.Dispose()
        Dt2 = Nothing
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Dim CNTra As OleDbTransaction = Nothing
        Cursor.Current = Cursors.WaitCursor
        Dim N As Integer
        Dim SqlStr1 As String =
                "DELETE * FROM Rslts WHERE GrDtID=?;"
        'اليوم الواحد ممكن يكون فيه أكتر من مجموعه
        Using CN As New OleDbConnection(Constr1)
            '           CNTra = cn.BeginTransaction(IsolationLevel.ReadCommitted)
            Dim cmd = New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text}
            With cmd.Parameters
                .AddWithValue("?", GrDtID1)
            End With
            Try
                CN.Open()
                'Irow in the database "True" then do nothing
                N = cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
                cmd.Dispose()
            Catch ex As OleDbException
                MsgBox("مشكلة فى حفظ الدرجات 1 : " & vbCrLf & ex.Message,
                       MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
                Cursor.Current = Cursors.Default
                CN.Close()
                Exit Sub
            End Try
        End Using
        Dim sqlstr As String = "INSERT INTO Rslts(GrDtID,StID,Mrk,DtCrtd,DtMdfd) VALUES(?,?,?,?,?);"
        For Each IRow As DataGridViewRow In DGStdnts.Rows
            'INSERT
            Using CN As New OleDbConnection(Constr1)
                Dim cmd = New OleDbCommand(sqlstr, CN) With {.CommandType = CommandType.Text}
                With cmd.Parameters
                    .AddWithValue("?", GrDtID1)
                    .AddWithValue("?", CType(IRow.Cells("StID").Value, Integer))
                    .AddWithValue("?", CType(IRow.Cells("Pscore").Value, Double))
                    .AddWithValue("?", Now.Date)
                    .AddWithValue("?", Now.Date)
                End With
                Try
                    CN.Open()
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                    cmd.Dispose()
                Catch ex As OleDbException
                    MsgBox("مشكلة فى حفظ الدرجات 2 : " & vbCrLf & ex.Message,
                           MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
                    Cursor.Current = Cursors.Default
                    CN.Close()
                    Exit Sub
                End Try
            End Using
        Next
        MsgBox("تم تسجيل الدرجات بنجاح",
               MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
        ConDGV("SELECT Stdnts.StID, Stdnts.StNm, Rslts.Mrk FROM (Stdnts INNER JOIN (Grps INNER JOIN GrSt ON Grps.GrID = GrSt.GrID) " &
               "ON Stdnts.StID = GrSt.StID) INNER JOIN Rslts ON Stdnts.StID = Rslts.StID " &
               "WHERE (((GrSt.GrID)=" & GrID1 & ") And ((Rslts.GrDtID)=" & GrDtID1 & ")) ORDER BY Stdnts.StID ASC;")
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        DoubleBuffered = True
        'WindowState = FormWindowState.Maximized
        'MdiParent = Form1
        TRV = New TreeView With {
        .Dock = DockStyle.Fill,
        .RightToLeftLayout = True,
        .LineColor = Color.Red,
        .Font = New Font("Ariel", 11),
        .PathSeparator = "-",
        .HotTracking = True,
        .HideSelection = False, .FullRowSelect = True, .ShowLines = True, .ShowPlusMinus = True, .ShowRootLines = True,
        .BorderStyle = BorderStyle.None
    }
        'Create View of all Groups Dates
        With TRV
            .BackColor = SystemColors.Control
            .ImageList = ImageList1
            .ImageIndex = 0
            .SelectedImageIndex = 1
        End With
        AddHandler TRV.AfterSelect, AddressOf TRV_AfterSelect
        GroupBox1.Controls.Add(TRV)
        Dim dt3 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        GetMainTree(dt3)
        dt3.Dispose()
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
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If DGStdnts.Rows.Count <= 0 AndAlso Not TRV.SelectedNode.Parent Is Nothing Then
            MsgBox("يجب تسجيل الدرجات أولا.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Form10.SrcFrm = RptSrc
        Form10.ShowDialog()
    End Sub
    Private Sub BtnDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDel.Click
        'Delete
        'Attend and Results Tables are linked with Students
        If DGStdnts.Rows.Count <= 0 OrElse GrDtID1 <= 0 Then
            DGStdnts.AllowUserToDeleteRows = False
            MsgBox("يجب اختيار ميعاد أولا.",
                   MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical)
            Exit Sub
        End If
        Dim SqlStr1 As String =
            <sql>SELECT COUNT(Rslts.GrDtID) FROM Rslts WHERE Rslts.GrDtID=<%= GrDtID1 %>;</sql>.Value
        Dim AreUsURE As MsgBoxResult =
            MsgBox("يجب حذف درجات طلاب المجوعه فى هذا اليوم أولا.",
         MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
        If AreUsURE = MsgBoxResult.Yes Then
            DGStdnts.AllowUserToDeleteRows = True
            Dim RUSre As MsgBoxResult = MsgBox("تأكيد حذف كشف الدرجات من البرنامج.",
                                                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight +
                                                   MsgBoxStyle.Critical + MsgBoxStyle.YesNoCancel)
            If RUSre = MsgBoxResult.Yes Then
                Dim N As Integer
                Dim SqlStr As String =
                    <SQL>DELETE * FROM Rslts WHERE Rslts.GrDtID=?;</SQL>.Value
                Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                        CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", GrDtID1)
                    End With
                    CN.Open()
                    N = CMD.ExecuteNonQuery
                End Using
                MsgBox("تم حذف درجات عدد  " & N.ToString & " طلاب.",
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
    Private Sub Form7_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TRV = Nothing
        DGStdnts = Nothing
    End Sub
    Private Sub Form7_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Dispose()
    End Sub
End Class