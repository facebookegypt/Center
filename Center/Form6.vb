Imports System.Data.OleDb

Public Class Form6
    Private Dt1 As DataTable
    Public Property GroupsDic1 As Dictionary(Of Integer, String)
    Public Property IU As Class1 = New Class1
    Public Property Constr1 As String = IU.ConStr
    Private DG1 As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill, .EnableHeadersVisualStyles = True,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, .RowHeadersVisible = False,
        .BackgroundColor = Color.WhiteSmoke}
    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If String.IsNullOrEmpty(TxtNm.Text) Then
            MsgBox("من فضلك أدخل المجموعة أولا.")
            Exit Sub
        End If
        If String.IsNullOrEmpty(TxtSub.Text) Or String.IsNullOrEmpty(TextBox1.Text) Then
            MsgBox("من فضلك ادخل الصف الدراسي و المادة الدراسية.")
            Exit Sub
        End If
        Dim SqlStr As String =
            "INSERT INTO Grps (GrNm,Lnm,SubNm,DtCrtd,DtMdfd) VALUES (?,?,?,?,?);"
        Dim N As Integer
        Using CN = New OleDbConnection(Constr1),
                cmd = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With cmd.Parameters
                .AddWithValue("?", TxtNm.Text)
                .AddWithValue("?", TextBox1.Text)
                .AddWithValue("?", TxtSub.Text)
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", Now.Date)
            End With
            Try
                CN.Open()
                N = cmd.ExecuteNonQuery()
                MsgBox("تم الحفظ بنجاح.")
            Catch ex As Exception
                MsgBox("خطأ مجموعات : " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form6_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub
End Class