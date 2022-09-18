Imports System.Data.OleDb

Public Class Form6
    Private Dt1 As DataTable
    Public Property IU As Class1 = New Class1
    Public Property Constr1 As String = IU.ConStr
    Private Property GrID As Integer
    Private DG1 As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill, .EnableHeadersVisualStyles = False,
        .RowHeadersVisible = True, .BackgroundColor = Color.WhiteSmoke,
        .ColumnHeadersHeight = 50,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing}
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If String.IsNullOrEmpty(TxtNm.Text) Or TxtNm.Text.Contains("لايوجد") Then
            MsgBox("من فضلك أدخل المجموعة أولا. مع العلم لا يمكن استخدام 'لايوجد' في اسم للمجموعة")
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
        KeyPreview = True
        MaximizeBox = False
    End Sub
    Private Sub Form6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub GetGrpsDts(ByVal SqlStr As String)
        Dim Dt2 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt2)
        End Using
        With DG1
            .AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders
            .RowHeadersWidth = 30
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            With .ColumnHeadersDefaultCellStyle
                .WrapMode = DataGridViewTriState.True
                .Alignment = DataGridViewContentAlignment.MiddleCenter
                .BackColor = Color.FloralWhite
            End With
            With .RowsDefaultCellStyle
                .WrapMode = DataGridViewTriState.True
                .Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            .ReadOnly = True
            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
            .AutoGenerateColumns = False
            .DataSource = Nothing
            .DataSource = New BindingSource(Dt2.DefaultView, Nothing)
        End With
        GroupBox4.Controls.Add(DG1)
        AddHandler DG1.RowPostPaint, AddressOf DG1_RowPostPaint
        AddHandler DG1.CellClick, AddressOf DG1_CellClick
    End Sub
    Private Sub DG1_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex = -1 Or e.ColumnIndex = -1 Then Exit Sub
        Try
            GrID = Convert.ToInt32(DG1.CurrentRow.Cells("GrID").Value)
        Catch ex As Exception
            MsgBox("خطأ في التشغيل 1 : " & ex.Message)
            Exit Sub
        End Try
        TxtNm.Text = Convert.ToString(DG1.CurrentRow.Cells("GrNm").Value)
        TextBox1.Text = Convert.ToString(DG1.CurrentRow.Cells("Lnm").Value)
        TxtSub.Text = Convert.ToString(DG1.CurrentRow.Cells("SubNm").Value)
        BtnEdit.Enabled = True
        BtnDel.Enabled = True
        BtnSave.Enabled = False
    End Sub
    Private Sub DG1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim dgv = DirectCast(sender, DataGridView)
        Dim rowHeader As String = Convert.ToString(e.RowIndex + 1)
        Using b As SolidBrush = New SolidBrush(Color.Black)
            e.Graphics.DrawString(rowHeader, dgv.DefaultCellStyle.Font, b, e.ClipBounds.Width - 30, e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        Dim SqlStr As String =
        <sql>SELECT * From Grps;</sql>.Value
        GetGrpsDts(SqlStr)
        Dim DIg2 As New DataGridViewTextBoxColumn With
            {.Name = "GrNm", .ValueType = GetType(String), .DataPropertyName = "GrNm", .HeaderText = "المجموعة"}
        Dim DIg0 As New DataGridViewTextBoxColumn With
            {.Name = "Lnm", .ValueType = GetType(String), .DataPropertyName = "Lnm", .HeaderText = "الصف الدراسي"}
        Dim DIg_1 As New DataGridViewTextBoxColumn With
            {.Name = "SubNm", .ValueType = GetType(String), .DataPropertyName = "SubNm", .HeaderText = "المادة الدراسية"}
        Dim DIg1 As New DataGridViewTextBoxColumn With
            {.Name = "GrId", .ValueType = GetType(Integer), .DataPropertyName = "GrID", .Visible = False}
        If DG1.Columns.Count <= 0 Then
            DG1.Columns.Insert(0, DIg2)
            DG1.Columns.Insert(1, DIg0)
            DG1.Columns.Insert(2, DIg_1)
            DG1.Columns.Insert(3, DIg1)
        End If
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        BtnEdit.Enabled = False
        BtnDel.Enabled = False
        BtnSave.Enabled = True
        TxtSub.Clear()
        TxtNm.Clear()
        TextBox1.Clear()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        'Edit
        Dim N As Integer, SqlStr As String =
            "UPDATE Grps SET GrNm=?, Lnm=?, SubNm=?, DtMdfd=? WHERE GrID=?;"
        Using CN = New OleDbConnection(Constr1),
                cmd = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            With cmd.Parameters
                .AddWithValue("?", TxtNm.Text)
                .AddWithValue("?", TextBox1.Text)
                .AddWithValue("?", TxtSub.Text)
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", GrID)
            End With
            Try
                CN.Open()
                N = cmd.ExecuteNonQuery()
                MsgBox("تم التعديل بنجاح.")
            Catch ex As Exception
                MsgBox("خطأ في التعديل : " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub BtnDel_Click(sender As Object, e As EventArgs) Handles BtnDel.Click
        'Del
        Dim SqlStr1 As String =
            "SELECT COUNT(GrDtID) From GrDt WHERE GrID=?;"
        Dim N As Integer, SqlStr As String =
            "DELETE * FROM Grps WHERE GrID=?;"
        Dim cmd As OleDbCommand
        Using CN = New OleDbConnection(Constr1)
            Dim CnTr As OleDbTransaction = Nothing
            Try
                CN.Open()
                CnTr = CN.BeginTransaction(IsolationLevel.ReadCommitted)
                cmd = New OleDbCommand(SqlStr1, CN, CnTr) With {.CommandType = CommandType.Text}
                With cmd.Parameters
                    .AddWithValue("?", GrID)
                End With
                N = Convert.ToInt32(cmd.ExecuteScalar)
                If N >= 1 Then
                    MsgBox("يجب أولا حذف المواعيد المحددة للمجموعه.")
                    Exit Sub
                End If
                cmd.Parameters.Clear()
                cmd = New OleDbCommand(SqlStr, CN, CnTr) With {.CommandType = CommandType.Text}
                With cmd.Parameters
                    .AddWithValue("?", GrID)
                End With
                N = cmd.ExecuteNonQuery
                CnTr.Commit()
            Catch ex As OleDbException
                MsgBox("خطأ فى الحذف : " & ex.Message)
                CnTr.Rollback()
            End Try
        End Using
    End Sub

    Private Sub Form6_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'Use KeyCode when you don't care about the modifiers, KeyData when you do.
        If e.KeyCode = Keys.S AndAlso e.Modifiers = Keys.Control Then
            If BtnSave.Enabled = True Then
                BtnSave_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.N AndAlso e.Modifiers = Keys.Control Then
            If BtnAdd.Enabled = True Then
                BtnAdd_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.A AndAlso e.Modifiers = Keys.Control Then
            If ToolStripButton6.Enabled = True Then
                ToolStripButton6_Click(sender, e)
            End If
        End If
        If e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control Then
            If BtnEdit.Enabled = True Then
                BtnEdit_Click(sender, e)
            End If
        End If
        If e.KeyData = Keys.Delete Then
            If BtnDel.Enabled = True Then
                BtnDel_Click(sender, e)
            End If
        End If
    End Sub
End Class