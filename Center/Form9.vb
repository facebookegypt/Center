Imports System.Data.OleDb
Public Class Form9
    Private TRV As TreeView
    Private Property Ii As New Class1
    Private Property Constr As String = Ii.ConStr
    'Private ReadOnly Property CurAttMnth As Integer = Form1.CurrentAttndMonth
    'Private ReadOnly Property CurAttMnthNm As String = Form1.CurrentAttndMonthNm
    Private SelectedGrID As Integer
    Private CuRRdT As Date = Form1.CurrentAttndMonthNm 'CurAttMnthNm
    Private CrrDtNm As String = Format(CuRRdT, "MMMM/yyyy")

    Private Sub Form9_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Text = "كشف غياب عن شهر (  " & Form1.CurrentAttndMonthNm & "  )"
        KeyPreview = True
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        MdiParent = Form1
        Dim SqlStr2 As String =
            "SELECT GrDt.GrID FROM GrDt WHERE (((GrDt.GrDtID)=?));"
        Using CN As New OleDbConnection(Constr),
                CMD As New OleDbCommand(SqlStr2, CN) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", Form1.CurrentAttndMonth)
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                Dim Ord As Integer = Rdr.GetOrdinal("GrID") 'كود المجموعة فى هذا الميعاد
                If Rdr.HasRows Then
                    While Rdr.Read
                        SelectedGrID = Rdr.GetInt32(Ord)
                    End While
                End If
            End Using
        End Using
        'GetGroups 

        Dim SqlStr4 As String =
            "SELECT GrDt.GrID, Grps.GrNm FROM (Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID) INNER JOIN Attnd ON " &
            "GrDt.GrDtID = Attnd.GrDtID GROUP BY GrDt.GrID, Grps.GrNm, Format([Mnm],'mmmm/yyyy') HAVING " &
            "(((Format([Mnm],'mmmm/yyyy'))='" & CrrDtNm & "'));"
        Dim DT As New DataTable
        DT = Ii.GetData(SqlStr4)
        With ComboBox1
            '.Items.Clear()
            .BeginUpdate()
            .DataSource = DT.DefaultView.Table
            .DisplayMember = "GrNm"
            .ValueMember = "GrID"
            .SelectedIndex = 0
            .Refresh()
            .EndUpdate()
        End With
        Dim SqlStr1 As String =
            "DROP VIEW GrAttDt;"
        Using CN As New OleDbConnection(Constr),
                CMD As New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text}
            Try
                CN.Open()
                CMD.ExecuteNonQuery()
            Catch ex As OleDbException
                Dim SqlStr As String =
            "CREATE VIEW GrAttDt AS SELECT GrDt.GrDtID, GrDt.GrID, Attnd.StID, Stdnts.StNm, Attnd.PStat, " &
            "Format([Mnm],'mmmm/yyyy') AS Mnm1, GrDt.GrDt1, GrDt.GrDt2, Grps.GrNm, Grps.Lnm, Grps.SubNm FROM " &
            "(GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) INNER JOIN (Stdnts INNER JOIN Attnd ON Stdnts.StID = Attnd.StID) " &
            "ON GrDt.GrDtID = Attnd.GrDtID WHERE (((GrDt.GrID)=" & SelectedGrID & ") And " &
            "Format([Mnm],'mmmm/yyyy')='" & CrrDtNm & "');"
                Dim CMD1 As New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                Try
                    CMD1.ExecuteNonQuery()
                Catch ex1 As OleDbException
                    MsgBox(ex1.Message)
                End Try
            End Try
        End Using

    End Sub
    Private Sub Form9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectionChangeCommitted
        'الأيام بتاعت المجموعة دي اللي اتسجل فيها الغياب
        Dim DT As DataTable = New DataTable
        Dim SqlStr As String =
            "SELECT GrDt.GrDtID FROM(GrDt) WHERE (((Format([Mnm],'mmmm/yyyy'))='" & CrrDtNm & "') AND ((GrDt.GrID)=" & SelectedGrID & "));"
        DT = Ii.GetData(SqlStr)
        Dim N As Integer = DT.Rows.Count
        Label1.Text = "عدد الأيام ( " & N & " ) يوم خلال شهر " & CrrDtNm

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
        With TRV
            .BackColor = SystemColors.Control
            .ImageIndex = 0
            .SelectedImageIndex = 1
        End With
        Dim dt3 As DataTable = New DataTable
        'AddHandler TRV.AfterSelect, AddressOf TRV_AfterSelect
        GroupBox1.Controls.Add(TRV)
        'GetMainTree(dt3)
    End Sub
    Private Function GetMainTree(ByVal DT As DataTable) As TreeView
        DT = Ii.GetData(<SQL>SELECT Grps.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID GROUP BY Grps.GrID, 
            Grps.GrNm ORDER BY Grps.GrID ASC;</SQL>.Value)
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
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
End Class