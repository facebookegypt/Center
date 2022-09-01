Imports System.Data.OleDb

Public Class Form2
    Private IC As New Class1, ConStr As String = IC.GetConStr
    Private DGStdnts As DataGridView, Dt1 As DataTable
    Private Function GetCount(ByVal TblNm As String, FldNm As String) As Integer
        Dim SqlStr As String =
            <sql>SELECT COUNT( <%= FldNm %> ) FROM <%= TblNm %>;</sql>.Value
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Return CMD.ExecuteScalar
        End Using
    End Function
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "اعدادات الطلاب. لديك " & GetCount("Stdnts", "StID") & " طالب."
        DoubleBuffered = True
        RightToLeft = RightToLeft.Yes
        KeyPreview = True
        BackgroundImageLayout = ImageLayout.Stretch
        BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Main2.jpg"), True)

    End Sub
End Class