Imports System.Data.OleDb
Public Class Form9
    Private Property Ii As New Class1
    Private Property Constr As String = Ii.ConStr
    Private ReadOnly Property CurAttMnth As Integer = Form1.CurrentAttndMonth
    Private ReadOnly Property CurAttMnthNm As String = Form1.CurrentAttndMonthNm
    Private Property SelectedGrDtID As Integer
    Private CuRRdT As Date = CurAttMnthNm
    Private CrrDtNm As String = Format(CuRRdT, "MMMM/yyyy")
    Private Sub Form9_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "كشف غياب عن شهر (  " & CurAttMnthNm & "  )"
        KeyPreview = True
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Dim SqlStr2 As String =
            "SELECT GrDt.GrID FROM GrDt WHERE (((GrDt.GrDtID)=?));"
        Using CN As New OleDbConnection(Constr),
                CMD As New OleDbCommand(SqlStr2, CN) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", CurAttMnth)
            CN.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                Dim Ord As Integer = Rdr.GetOrdinal("GrID")
                If Rdr.HasRows Then
                    While Rdr.Read
                        SelectedGrDtID = Rdr.GetInt32(Ord)
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
            "ON GrDt.GrDtID = Attnd.GrDtID WHERE (((GrDt.GrID)=" & SelectedGrDtID & ") And " &
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
End Class