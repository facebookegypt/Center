Imports System.Data.OleDb

Public Class Form4
    Private Dt1 As DataTable
    Public Property GroupsDic1 As Dictionary(Of Integer, String)
    Public Property IU As Class1 = New Class1
    Public Property Constr1 As String = IU.ConStr
    Public Property GrID As Integer
    Public Property TaskID As Integer
    Private DG1 As DataGridView = New DataGridView With
        {.Name = "DGV2", .BorderStyle = BorderStyle.None, .RightToLeft = RightToLeft.Yes,
        .AllowUserToAddRows = False, .Dock = DockStyle.Fill, .EnableHeadersVisualStyles = True,
        .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, .RowHeadersVisible = False,
        .BackgroundColor = Color.WhiteSmoke}
    Private Sub GetGrps(ByVal SqlStr As String,
                        ByVal combobox As ComboBox,
                        Optional DisMem As String = "Value",
                        Optional ValMem As String = "Key")
        Dt1 = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Using CN = New OleDbConnection With {.ConnectionString = Constr1()},
            CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt1)
        End Using
        With combobox
            .BeginUpdate()
            .DataSource = Dt1.DefaultView
            .DisplayMember = DisMem
            .ValueMember = ValMem
            .SelectedIndex = -1
            .EndUpdate()
            .Update()
        End With
    End Sub
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        DoubleBuffered = True
        Text = "اعدادات المجموعات"
        GetGrps("SELECT GrID,GrNm FROM Grps;", ComboBox2, "GrNm", "GrID")
        GetGrps("SELECT TaskID,TaskNm FROM Tsks;", ComboBox3, "TaskNm", "TaskID")
        With DateTimePicker1
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "MMMMyyyy"
            .ShowUpDown = True
        End With
        With DateTimePicker2
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "dddd dd/MMMM/yyyy hh:mm tt"
            .ShowUpDown = True
        End With
        WindowState = FormWindowState.Maximized
    End Sub
    Private Sub Form4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Frm As New Form6
        Frm.ShowDialog()
        GetGrps("SELECT GrID,GrNm FROM Grps;", ComboBox2, "GrNm", "GrID")
    End Sub
    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        If IsNothing(ComboBox2.SelectedItem) Then
            MsgBox("من فضلك اختر المجموعة أولا.")
            Exit Sub
        End If
        If IsNothing(ComboBox3.SelectedItem) Then
            MsgBox("من فضلك اختر الأعمال.")
            Exit Sub
        End If
        Dim SqlStr1 As String =
            "INSERT INTO GrDT (GrID,Mnm,GrDt1,GrDt2,TaskID,DtCrtd,DtMdfd) VALUES (?,?,?,?,?,?,?);"
        Dim N As Integer
        Using CN = New OleDbConnection(Constr1),
                cmd = New OleDbCommand(SqlStr1, CN) With {.CommandType = CommandType.Text}
            With cmd.Parameters
                .AddWithValue("?", GrID)
                .AddWithValue("?", DateTimePicker1.Value.Date)
                .AddWithValue("?", DateTimePicker2.Value.Date)
                .AddWithValue("?", DateTimePicker2.Value.TimeOfDay)
                .AddWithValue("?", TaskID)
                .AddWithValue("?", Now.Date)
                .AddWithValue("?", Now.Date)
            End With
            Try
                CN.Open()
                N = cmd.ExecuteNonQuery()
                MsgBox("تم الحفظ بنجاح.")
            Catch ex As Exception
            End Try
        End Using
    End Sub
    Private Sub ComboBox2_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox2.SelectionChangeCommitted
        GrID = Convert.ToInt32(ComboBox2.SelectedValue)
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
            .ReadOnly = True
            .AutoGenerateColumns = False
            .DataSource = Nothing
            .DataSource = New BindingSource(Dt2.DefaultView, Nothing)
        End With
        GroupBox1.Controls.Add(DG1)
    End Sub
    Private Sub ComboBox3_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBox3.SelectionChangeCommitted
        TaskID = Convert.ToInt32(ComboBox3.SelectedValue)
    End Sub
    Private Sub ToolStripButton10_Click(sender As Object, e As EventArgs) Handles ToolStripButton10.Click
        Dim SqlStr As String =
        <sql>SELECT GrDt.GrDtID, GrDt.GrID, Grps.GrNm, Grps.Lnm, Grps.SubNm, GrDt.Mnm, GrDt.GrDt1, GrDt.GrDt2, Tsks.TaskNm 
        FROM Tsks INNER JOIN (GrDt INNER JOIN Grps ON GrDt.GrID = Grps.GrID) ON Tsks.TaskID = GrDt.TaskID;</sql>.Value
        GetGrpsDts(SqlStr)
        Dim Dg4 As New DataGridViewTextBoxColumn With
            {.Name = "Mnm", .ValueType = GetType(Date), .DataPropertyName = "Mnm", .HeaderText = "الشهر"}
        Dg4.DefaultCellStyle.Format = "MMMMyyyy"
        Dim Dg5 As New DataGridViewTextBoxColumn With
            {.Name = "GrDt1", .ValueType = GetType(Date), .DataPropertyName = "GrDt1", .HeaderText = "اليوم"}
        Dg5.DefaultCellStyle.Format = "dddd dd/MMMM"
        Dim Dg6 As New DataGridViewTextBoxColumn With
            {.Name = "GrDt2", .ValueType = GetType(Date), .DataPropertyName = "GrDt2", .HeaderText = "الساعة"}
        Dg6.DefaultCellStyle.Format = "hh:mm tt"
        If DG1.Columns.Count <= 0 Then
            DG1.Columns.Insert(0, Dg4)
            DG1.Columns.Insert(1, Dg5)
            DG1.Columns.Insert(2, Dg6)
        End If

    End Sub
End Class