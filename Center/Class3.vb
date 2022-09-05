Public Class Class3
    Inherits Form
    Public DG As DataGridView = New DataGridView With {.Name = "DGNew", .BorderStyle = BorderStyle.None,
        .RightToLeft = RightToLeft.Yes, .AllowUserToAddRows = True, .Dock = DockStyle.Fill, .ColumnCount = 3,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, .RowHeadersVisible = False,
        .BackgroundColor = Color.WhiteSmoke},
        Dt1 As DataTable, I As Class1 = New Class1
    Private Sub ConDGV(ByVal SqlStr As String)
        With DG
            .DataSource = Nothing
            .AutoGenerateColumns = False
            .Columns(0).Name = "LID"
            .Columns(0).HeaderText = "كود الصف"
            .Columns(0).DataPropertyName = "LID"
            .Columns(0).ReadOnly = True

            .Columns(1).Name = "LNm"
            .Columns(1).HeaderText = "الصف الدراسي"
            .Columns(1).DataPropertyName = "LNm"
            .Columns(1).ReadOnly = False

            .Columns(2).Name = "SubNm"
            .Columns(2).HeaderText = "المادة الدراسية"
            .Columns(2).DataPropertyName = "SubNm"
            .Columns(2).ReadOnly = False

            .DataSource = New BindingSource(I.GetData(SqlStr), Nothing)
        End With
    End Sub
    Private Sub DG_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs)
        Dim AreYouSure As MsgBoxResult =
            MsgBox("هل ترغب فى حفظ التغييرات?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight)
        If AreYouSure = MsgBoxResult.Yes Then
            Dim DGRow As DataGridViewRow = DG.CurrentRow
            Dim SqlStr As String
            If DGRow.Cells(0).Value Is DBNull.Value Then
                SqlStr =
                "INSERT INTO Lvls(Lnm,SubNm,DtCrtd,DtMdfd) VALUES (?,?,?,?);"

                Using cn As OleDb.OleDbConnection = New OleDb.OleDbConnection With {.ConnectionString = I.GetConStr},
                    CMD As OleDb.OleDbCommand = New OleDb.OleDbCommand(SqlStr, cn) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", DGRow.Cells(1).Value.ToString)
                        .AddWithValue("?", DGRow.Cells(2).Value.ToString)
                        .AddWithValue("?", Now.Date)
                        .AddWithValue("?", Now.Date)
                    End With
                    cn.Open()
                    CMD.ExecuteNonQuery()
                End Using
            Else
                SqlStr =
                    "UPDATE Lvls SET Lnm=?,SubNm=?, DtMdfd=? WHERE LID=?;"

                Using cn As OleDb.OleDbConnection = New OleDb.OleDbConnection With {.ConnectionString = I.GetConStr},
                    CMD As OleDb.OleDbCommand = New OleDb.OleDbCommand(SqlStr, cn) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", DGRow.Cells(1).Value.ToString)
                        .AddWithValue("?", DGRow.Cells(2).Value.ToString)
                        .AddWithValue("?", Now.Date)
                        .AddWithValue("?", Convert.ToInt32(DGRow.Cells(0).Value))
                    End With
                    cn.Open()
                    CMD.ExecuteNonQuery()
                End Using
            End If
            Close()
        End If
    End Sub
    Sub New()
        FormBorderStyle = FormBorderStyle.None
        KeyPreview = True
        Width = 450
        Height = 400
        BackColor = Color.WhiteSmoke
        ConDGV("SELECT * FROM Lvls;")
        Controls.Add(DG)
        AddHandler DG.CellEndEdit, AddressOf DG_CellEndEdit
        DG.Invalidate(True)
        DG.Update()
        DG.Refresh()
    End Sub
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'Class3
        '
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Name = "Class3"
        Me.ResumeLayout(False)

    End Sub

    Private Sub Class3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
End Class
