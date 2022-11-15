Imports System.Data.OleDb
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class Form9
    Private cryRpt As New ReportDocument
    Private crtableLogoninfos As New TableLogOnInfos
    Private crtableLogoninfo As New TableLogOnInfo
    Private crConnectionInfo As New ConnectionInfo
    Private CrTables As Tables
    Private CrTable As Table
    Private Property Ii As New Class1
    Private Property Constr As String = Ii.ConStr
    Private SelectedGrID As Integer
    Private CuRRdT As Date = Form1.CurrentAttndMonthNm 'CurAttMnthNm
    Private CrrDtNm As String = Format(CuRRdT, "MMMM/yyyy")
    Private Property DT As DataTable
    Private Sub Form9_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Text = "كشف غياب و درجات عن شهر (  " & Form1.CurrentAttndMonthNm & "  )"
        KeyPreview = True
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        MdiParent = Form1
        Button1.Enabled = False
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
    End Sub
    Private Sub Form9_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub ComboBox1_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectionChangeCommitted
        SelectedGrID = Convert.ToInt32(ComboBox1.SelectedValue)
        Dim SqlStr1 As String =
            "DROP VIEW GrAttMnth;"
        Dim SqlStr3 As String =
            "CREATE VIEW GrAttMnth AS SELECT Stdnts.StID, Attnd.GrDtID, Grps.GrID, Stdnts.StNm, Attnd.PStat, Rslts.Mrk, " &
            "Format([Mnm],'mmmm/yyyy') AS Mnm1, GrDt.GrDt1, GrDt.GrDt2, GrDt.TaskID, GrDt.TskFlMrk, Grps.GrNm, Grps.Lnm, " &
            "Grps.SubNm FROM (GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) INNER JOIN (Rslts RIGHT JOIN (Attnd INNER JOIN Stdnts " &
            "ON Attnd.StID = Stdnts.StID) ON (Rslts.StID = Attnd.StID) And (Rslts.GrDtID = Attnd.GrDtID)) ON " &
            "GrDt.GrDtID = Attnd.GrDtID WHERE (((Grps.GrID)=" & SelectedGrID & ") And ((Format([Mnm],'mmmm/yyyy'))='" & CrrDtNm & "'));"
        Using CN As New OleDbConnection(Constr),
                CMD As New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text},
                CMD1 As New OleDbCommand(SqlStr3, CN) With {.CommandType = CommandType.Text}
            Try
                CN.Open()
                CMD.ExecuteNonQuery()
                CMD1.ExecuteNonQuery()
            Catch ex As OleDbException
                CMD1.ExecuteNonQuery()
            End Try
        End Using
        'الأيام بتاعت المجموعة دي اللي اتسجل فيها الغياب
        Dim DT As DataTable = New DataTable
        Dim SqlStr As String =
            "SELECT GrDt.GrDtID FROM GrDt WHERE (((Format([Mnm],'mmmm/yyyy'))='" & CrrDtNm & "') AND ((GrDt.GrID)=" & SelectedGrID & "));"
        DT = Ii.GetData(SqlStr)
        Dim N As Integer = DT.Rows.Count
        Label1.Text = "عدد الأيام ( " & N & " ) يوم خلال شهر " & CrrDtNm
        Button1.Enabled = True
    End Sub
    Private Sub GetData(ByVal SqlStr As String)
        DT = New DataTable
        Using CN As New OleDbConnection(Constr)
            Using DataAdapter1 As New OleDbDataAdapter(SqlStr, CN)
                DataAdapter1.Fill(DT)
            End Using
        End Using
    End Sub
    Private Sub AssignConnection(rpt As ReportDocument)
        Dim connection As New ConnectionInfo With {
            .DatabaseName = "",    '****When using OleDB, you need to use Blank value here
            .ServerName = "",      'When using OleDB, you need to use Blank value here
            .UserID = "Admin",
            .Password = My.Settings.dbPass
        }
        ' First we assign the connection to all tables in the main report
        For Each table As Table In rpt.Database.Tables  'CrystalDecisions.CrystalReports.Engine
            AssignTableConnection(table, connection)
        Next
        ' Now loop through all the sections and its objects to do the same for the subreports
        For Each section As Section In rpt.ReportDefinition.Sections    'CrystalDecisions.CrystalReports.Engine
            ' In each section we need to loop through all the reporting objects
            For Each reportObject As ReportObject In section.ReportObjects  'CrystalDecisions.CrystalReports.Engine
                If reportObject.Kind = ReportObjectKind.SubreportObject Then
                    Dim subReport As SubreportObject = DirectCast(reportObject, SubreportObject)
                    Dim subDocument As ReportDocument = subReport.OpenSubreport(subReport.SubreportName)
                    For Each table As Table In subDocument.Database.Tables  'CrystalDecisions.CrystalReports.Engine
                        AssignTableConnection(table, connection)
                    Next
                    subDocument.SetDatabaseLogon(connection.UserID, connection.Password, connection.ServerName, connection.DatabaseName)
                End If
            Next
        Next
        rpt.SetDatabaseLogon(connection.UserID, connection.Password, connection.ServerName, connection.DatabaseName)
    End Sub
    Private Sub AssignTableConnection(ByVal table As CrystalDecisions.CrystalReports.Engine.Table, ByVal connection As ConnectionInfo)
        ' Cache the logon info block
        Dim logOnInfo As TableLogOnInfo = table.LogOnInfo
        connection.Type = logOnInfo.ConnectionInfo.Type
        ' Set the connection
        logOnInfo.ConnectionInfo = connection
        ' Apply the connection to the table!
        table.LogOnInfo.ConnectionInfo.DatabaseName = connection.DatabaseName
        table.LogOnInfo.ConnectionInfo.ServerName = connection.ServerName
        table.LogOnInfo.ConnectionInfo.UserID = connection.UserID
        table.LogOnInfo.ConnectionInfo.Password = connection.Password
        table.LogOnInfo.ConnectionInfo.Type = connection.Type
        table.ApplyLogOnInfo(logOnInfo)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SqlStr As String = String.Empty
        DT = New DataTable With {.Locale = Globalization.CultureInfo.CurrentCulture}
        Dim RptPath As String = IO.Path.Combine(Application.StartupPath, "Reports")
        SqlStr = "SELECT * FROM GrAttMnth;"
        GetData(SqlStr)
        If DT.Rows.Count <= 0 Then
            MsgBox("هذه المجموعة حضرت بالكامل",
           MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.Information)
            CrystalReportViewer1.ReportSource = Nothing
            Exit Sub
        End If
        cryRpt.Load(IO.Path.Combine(RptPath, "CrystalReport6.rpt"))
        AssignConnection(cryRpt)
        Try
            cryRpt.SetDataSource(DT)
            'YOU MUST ALLWAYS PROVIDE CONNECTION INFO TO YOUR REPORTDOCUMENT BEFORE OPEN IT
            CrystalReportViewer1.ReportSource = cryRpt
            CrystalReportViewer1.Refresh()
            Button1.Enabled = False
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class