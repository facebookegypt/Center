Imports System.Data.OleDb

Public Class Class3
    Inherits Form
    Private Property IC As Class1 = New Class1
    Private Property Dt1 As DataTable = New DataTable
    Private Property Constr1 As String = IC.ConStr
    Private CboGrps As ComboBox = New ComboBox
    Public Property GrID1 As Integer
    Sub New()
        MaximizeBox = False
        MinimizeBox = False
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        WindowState = FormWindowState.Normal
        StartPosition = FormStartPosition.CenterParent
        KeyPreview = True
        Width = 150
        Height = 59
        BackColor = Color.WhiteSmoke
        RightToLeft = RightToLeft.Yes
    End Sub
    Private Sub CboGrps_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = ChrW(Keys.Escape) Then Close()
        If e.KeyChar = ChrW(Keys.Enter) Then
            Dim RUSure As MsgBoxResult =
            MsgBox("هل ترغب فى نقل الطالب الي المجموعه" & vbCrLf & CboGrps.Text & vbCrLf & "؟",
                    MsgBoxStyle.MsgBoxRight + MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.YesNoCancel)
            If RUSure = MsgBoxResult.Yes Then
                Dim N As Integer
                Dim S As List(Of Integer) = New List(Of Integer)
                Dim SqlStr As String =
                    <SQL>UPDATE GrSt SET GrID=? WHERE GrSt.StID=?;</SQL>.Value
                Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                        CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
                    With CMD.Parameters
                        .AddWithValue("?", GrID1)
                        .AddWithValue("?", Form2.StID1)
                    End With
                    CN.Open()
                    N = CMD.ExecuteNonQuery
                End Using
                Dim SqlStr1 As String =
                "Select GrDt.GrDtID From GrSt INNER Join GrDt On GrSt.GrID = GrDt.GrID Where (((GrSt.StID) = ?)) Order By GrDt.GrDt1;"
                Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                        CMD As OleDbCommand = New OleDbCommand(SqlStr1, CN) With
                        {.CommandType = CommandType.Text}
                    CMD.Parameters.AddWithValue("?", Form2.StID1)
                    CN.Open()
                    Using Rdr As OleDbDataReader = CMD.ExecuteReader
                        If Rdr.HasRows Then
                            While Rdr.Read
                                S.AddRange({Rdr.GetInt32(0)})
                            End While
                        End If
                    End Using
                End Using
                Dim SqlStr2 As String = "UPDATE Attnd SET GrDtID=? WHERE StID=?;"
                If S.Count >= 1 Then
                    For I As Integer = 0 To S.Count - 1
                        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = Constr1},
                        CMD As OleDbCommand = New OleDbCommand(SqlStr2, CN) With {.CommandType = CommandType.Text}
                            CMD.Parameters.AddWithValue("?", S(I))
                            CMD.Parameters.AddWithValue("?", Form2.StID1)
                            CN.Open()
                            CMD.ExecuteNonQuery()
                            CMD.Parameters.Clear()
                        End Using
                    Next
                End If
                MsgBox("تم تعديل المجموعة.")
                Application.DoEvents()
                Close()
            Else
                MsgBox("لم يكتمل النقل")
                Application.DoEvents()
                Close()
            End If
        End If

    End Sub
    Private Sub CboGrps_SelectionChangeCommitted(sender As Object, e As EventArgs)
        GrID1 = Convert.ToInt32(CboGrps.SelectedValue)

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
        If e.KeyChar = ChrW(Keys.Escape) Then
            RemoveHandler CboGrps.SelectionChangeCommitted, AddressOf CboGrps_SelectionChangeCommitted
            RemoveHandler CboGrps.KeyPress, AddressOf CboGrps_KeyPress
            Close()
        End If
    End Sub

    Private Sub Class3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "المجموعات"
        Dim sqlstr As String = <sql>SELECT GrDt.GrID, Grps.GrNm FROM Grps INNER JOIN GrDt ON Grps.GrID = GrDt.GrID 
            GROUP BY GrDt.GrID, Grps.GrNm;</sql>.Value
        With CboGrps
            .Font = New Font("Arial", 13.25, FontStyle.Bold)
            .Dock = DockStyle.Top
            .DropDownStyle = ComboBoxStyle.DropDownList
            .ResumeLayout(False)
        End With
        Controls.Add(CboGrps)
        AddHandler CboGrps.SelectionChangeCommitted, AddressOf CboGrps_SelectionChangeCommitted
        AddHandler CboGrps.KeyPress, AddressOf CboGrps_KeyPress
        IC.GetGrps(Dt1, Constr1, sqlstr, CboGrps, "GrNm", "GrID")
    End Sub
End Class
