Imports System.Data.OleDb

Public Class Form8
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
        {
        .Name = "DGV2",
        .BorderStyle = BorderStyle.None,
        .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False,
        .Dock = DockStyle.Fill,
        .EnableHeadersVisualStyles = False,
        .RowHeadersVisible = True,
        .BackgroundColor = Color.WhiteSmoke,
        .ColumnHeadersHeight = 50,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
        .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
    }
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
    Private Sub Form7_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        Text = "تسجيل حضور و غياب الطلبة"
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
        Dtable = New DataTable
        Dtable.Columns.Add(New DataColumn("GrDtID") With {.DataType = GetType(Integer), .Caption = "اليوم"})
        Dtable.Columns.Add(New DataColumn("StID") With {.DataType = GetType(Integer), .Caption = "كود الطالب"})
        Dtable.Columns.Add(New DataColumn("StNm") With {.DataType = GetType(String), .Caption = "الاسم"})
        Dtable.Columns.Add(New DataColumn("GrID") With {.DataType = GetType(Integer), .Caption = "كود المجموعه"})
    End Sub
    Private Sub TRV_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs)
        Dim DT As DataTable =
            IC.GetData(<SQL>SELECT GrDt.GrDtID, GrDt.GrID, Tsks.TaskID, Grps.GrNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Tsks.TaskNm 
            FROM Tsks INNER JOIN (Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID) ON  
            Tsks.TaskID = GrDt.TaskID WHERE (((GrDt.GrID)=<%= Integer.Parse(e.Node.Name) %>));</SQL>.Value)
        e.Node.Nodes.Clear()
        If DT.Rows.Count > 0 Then
            For Each dr As DataRow In DT.Rows
                Dim DadNode As TreeNode = New TreeNode With {
                    .Name = dr("GrDtID").ToString(),
                    .Text = Format(dr("Mnm"), "MMMMyyyy") & " -- " & Format(dr("GrDt1"), "dddd, dd/MMMM") &
                    " -- " & Format(dr("GrDt2"), "hh:mm tt") & " -- " & dr("TaskNm").ToString,
                    .Tag = dr("TaskID").ToString()}
                If e.Node.Level = 0 Then e.Node.Nodes.Add(DadNode)
            Next
        End If
        If e.Node.Level = 0 Then
            GrID1 = Integer.Parse(e.Node.Name)
            Label4.Text = String.Empty
            DGStdnts.Visible = False
            BtnSave.Enabled = False
        Else
            GrDtID1 = Integer.Parse(e.Node.Name)
            GrID1 = Integer.Parse(e.Node.Parent.Name)
            Label4.Text = e.Node.Text
            DGStdnts.Visible = True
        End If
        ConDGV("SELECT Stdnts.StID, Stdnts.StNm, Attnd.PStat FROM (Stdnts INNER JOIN Attnd ON Stdnts.StID = Attnd.StID) INNER JOIN " &
               "(GrSt INNER JOIN Grps ON GrSt.GrID = Grps.GrID) ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID)=" & GrID1 & ") AND " &
               "((Attnd.GrDtID)=" & GrDtID1 & ")) ORDER BY Stdnts.StID;")
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
            .EditMode = DataGridViewEditMode.EditOnEnter
            .GridColor = SystemColors.Control
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
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
        End With
        If DGStdnts.Columns.Contains("PStat") Then
            DGStdnts.Columns.Remove("PStat")
        End If
        AddCol()
        DGStdnts.Columns("StID").HeaderCell.Value = "كود الطالب"
        DGStdnts.Columns("StID").ReadOnly = True
        DGStdnts.Columns("StID").AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        DGStdnts.Columns("StNm").HeaderCell.Value = "اسم الطالب"
        DGStdnts.Columns("StNm").ReadOnly = True
        DGStdnts.Columns("StNm").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        AddHandler DGStdnts.RowPostPaint, AddressOf DGSTDNTS_RowPostPaint
        AddHandler DGStdnts.CellValueChanged, AddressOf DGStdnts_CellValueChanged
        AddHandler DGStdnts.CurrentCellDirtyStateChanged, AddressOf DGStdnts_CurrentCellDirtyStateChanged
        AddHandler DGStdnts.CellFormatting, AddressOf Dgstdnts_CellFormatting
    End Sub
    Private Sub Dgstdnts_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs)
        If DGStdnts.Columns(e.ColumnIndex).Name = "PStat" Then
            For Each Irow As DataGridViewRow In DGStdnts.Rows
                If CType(Irow.Cells("PStat").Value, Boolean) = True Then
                    Irow.DefaultCellStyle.BackColor = Color.Honeydew
                End If
            Next
        End If
    End Sub
    Sub DGStdnts_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs)
        If TypeOf DGStdnts.CurrentCell Is DataGridViewCheckBoxCell And DGStdnts.IsCurrentCellDirty Then
            StID1 = Convert.ToInt32(DGStdnts.CurrentRow.Cells("StID").Value)
            DGStdnts.CommitEdit(DataGridViewDataErrorContexts.Commit)
            DGStdnts.EndEdit()
        End If
    End Sub
    ' If a check box cell is clicked, this event handler disables  
    ' or enables the button in the same row as the clicked cell.
    Private Sub DGStdnts_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        If e.ColumnIndex = -1 Or e.RowIndex = -1 Then Exit Sub
        If DGStdnts.Columns(e.ColumnIndex).Name = "PStat" Then
            RemoveHandler DGStdnts.CellValueChanged, AddressOf DGStdnts_CellValueChanged
            Dim checkCell As DataGridViewCheckBoxCell = CType(DGStdnts.Rows(e.RowIndex).Cells("PStat"), DataGridViewCheckBoxCell)
            Dim N As Integer
            Dim SqlStr As String =
                <SQL>UPDATE Attnd SET PStat=?, Dtmdfd=? WHERE Attnd.StID=? AND Attnd.GrDtID=?;</SQL>.Value
            Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                    CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                With CMD.Parameters
                    .AddWithValue("?", Convert.ToBoolean(DGStdnts.CurrentRow.Cells("PStat").FormattedValue))
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
            AddHandler DGStdnts.CellValueChanged, AddressOf DGStdnts_CellValueChanged
            DGStdnts.Refresh()
        End If
    End Sub
    Private Sub AddCol()
        If Not DGStdnts.Columns.Contains("PStat") Then
            Dim AddColumn As New DataGridViewCheckBoxColumn
            With AddColumn
                .HeaderText = "الغياب"
                .Name = "PStat"
                .FlatStyle = FlatStyle.Standard
                .CellTemplate = New DataGridViewCheckBoxCell()
                .CellTemplate.Style.BackColor = Color.Beige
                .ValueType = GetType(Boolean)
                .ThreeState = False
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                .DataPropertyName = "PStat"
            End With
            DGStdnts.Columns.Insert(0, AddColumn)
        End If
    End Sub
    Private Sub DGSTDNTS_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim grid = TryCast(sender, DataGridView)
        Dim rowIdx As String = Convert.ToString(e.RowIndex + 1)
        Using centerFormat As StringFormat = New StringFormat() With
            {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            Dim headerBounds =
                New Rectangle(e.RowBounds.Right - 42, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
            e.Graphics.DrawString(rowIdx, Font, SystemBrushes.ActiveCaptionText, headerBounds, centerFormat)
        End Using
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Dim CNTra As OleDbTransaction = Nothing
        Dim N As Integer
        Dim SqlStr1 As String =
                "DELETE * FROM Attnd WHERE GrDtID=?;"
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

        Dim sqlstr As String = "INSERT INTO Attnd(GrDtID,StID,Pstat,DtCrtd,DtMdfd) VALUES(?,?,?,?,?);"
            For Each IRow As DataGridViewRow In DGStdnts.Rows
                'INSERT
                Using cn As New OleDbConnection(Constr1)
                    Dim cmd = New OleDbCommand(sqlstr, cn) With {.CommandType = CommandType.Text}
                    With cmd.Parameters
                        .AddWithValue("?", GrDtID1)
                        .AddWithValue("?", CType(IRow.Cells("StID").Value, Integer))
                        .AddWithValue("?", CType(IRow.Cells("PStat").Value, Boolean))
                        .AddWithValue("?", Now.Date)
                        .AddWithValue("?", Now.Date)
                    End With
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                End Using
            Next
            MsgBox("تم تسجيل غياب المجموعة اليوم بنجاح",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Information)
            ConDGV("SELECT Stdnts.StID, Stdnts.StNm, Attnd.PStat FROM (Stdnts INNER JOIN Attnd ON Stdnts.StID = Attnd.StID) INNER JOIN " &
                   "(GrSt INNER JOIN Grps ON GrSt.GrID = Grps.GrID) ON Stdnts.StID = GrSt.StID WHERE (((GrSt.GrID)=" & GrID1 & ") AND " &
                   "((Attnd.GrDtID)=" & GrDtID1 & ")) ORDER BY Stdnts.StID;")

    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        Dim SqlStr As String =
            "SELECT Stdnts.StID, Stdnts.StNm FROM Stdnts INNER JOIN GrSt ON Stdnts.StID = GrSt.StID WHERE " &
            "(((GrSt.GrID)=" & GrID1 & ")) ORDER BY Stdnts.StID;"
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
    End Sub

    Private Sub Form8_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
    End Sub
End Class