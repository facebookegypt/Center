Public Class Class2
    Inherits Form
    Private ReadOnly Btn1 As Button = New Button With {.Dock = DockStyle.Bottom, .Text = "Save", .Name = "Btn1"}
    Public Txt1 As TextBox = New TextBox With {.Dock = DockStyle.Top, .Text = String.Empty, .Name = "Txt1"}
    Public Shared Property Pro As String = Nothing

    Private Sub Btn1_Click(sender As Object, e As EventArgs)
        My.Settings.Save()
        Pro = Txt1.Text
        Close()
    End Sub
    Sub New()
        FormBorderStyle = FormBorderStyle.None
        AcceptButton = Btn1
        Width = 450
        Height = 100
        Me.Controls.Add(Btn1)
        Me.Controls.Add(Txt1)
        AddHandler Btn1.Click, AddressOf Btn1_Click
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'Class2
        '
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Name = "Class2"
        Me.ResumeLayout(False)

    End Sub

    Private Sub Class2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Class2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler Btn1.Click, AddressOf Btn1_Click
        Dispose()
    End Sub
End Class
