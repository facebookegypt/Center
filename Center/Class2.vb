﻿Public Class Class2
    Inherits Form
    Private ReadOnly Btn1 As Button = New Button With {.Dock = DockStyle.Bottom, .Text = "Save", .Name = "Btn1"}
    Public Txt1 As TextBox = New TextBox With {.Dock = DockStyle.Top, .Text = String.Empty, .Name = "Txt1"}

    Private Sub Btn1_Click(sender As Object, e As EventArgs)
        My.Settings.Save()
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

End Class
