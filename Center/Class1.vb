Imports System.Data.OleDb
Imports Microsoft.Win32

Public Class Class1
    Private Function DbFound() As Boolean
        If IO.File.Exists(Application.StartupPath & "\Center.accdb") Then
            Return True
        Else
            MsgBox("ملف قاعدة البيانات غير موجود" & vbCrLf & "برجاء الاتصال بـ " & vbCrLf & "9977 1220 010",
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical, "Center App.")
            Return False
        End If
    End Function
    Private Sub Btn1_Click(sender As Object, e As EventArgs)
        My.Settings.Save()
    End Sub
    Public Function GetConStr() As String
        Dim ConStr As String = "مشكلة فى ملفات الأكسس"
        Dim Ilist As List(Of String) = New List(Of String)
        Ilist = FindProvider()
        Dim Pro As String = String.Empty
        If DbFound() Then
            If String.IsNullOrEmpty(My.Settings.Provider) Then
                If Ilist.Count > 1 Then
                    Class2.Txt1.Text = String.Join(",", Ilist)
                    Class2.ShowDialog()
                    Pro = Class2.Txt1.Text
                    My.Settings.Provider = Pro
                    My.Settings.Save()
                    ConStr = Pro & IO.Path.Combine(Application.StartupPath, My.Settings.dbNm) & ";" &
                    "Jet OLEDB:Database Password= " & My.Settings.dbPass & ";" &
                    "Persist Security Info=False;"
                Else
                    ConStr = FindProvider(0) & IO.Path.Combine(Application.StartupPath, My.Settings.dbNm) & ";" &
                        "Jet OLEDB:Database Password= " & My.Settings.dbPass & ";" &
                        "Persist Security Info=False;"
                End If
            Else
                ConStr = My.Settings.Provider & IO.Path.Combine(Application.StartupPath, My.Settings.dbNm) & ";" &
                        "Jet OLEDB:Database Password= " & My.Settings.dbPass & ";" &
                        "Persist Security Info=False;"
            End If
        End If
        Return ConStr
    End Function
    Private Function FindProvider() As List(Of String)
        Dim Provider As String = String.Empty
        Dim Provider1 As New List(Of String)
        Dim reader = OleDbEnumerator.GetRootEnumerator()
        Dim list As List(Of String) = New List(Of String)
        While reader.Read()
            For i = 0 To reader.FieldCount - 1
                'Debug.WriteLine(reader.GetName(i))
                If reader.GetName(i) = "SOURCES_NAME" Then
                    list.Add(reader.GetValue(i).ToString())
                    'Debug.WriteLine(reader.GetValue(i).ToString())
                End If
            Next
        End While
        reader.Close()
        For Each St As String In list
            If St.StartsWith("Microsoft.") Then
                Provider1.Add("Provider=" & St & ";Data Source=")
            End If
        Next
        Return Provider1
        'If Provider1.Count > 1 Then
        ' Class2.Txt1.Text = String.Join(",", Provider1)
        ' Class2.ShowDialog()
        ' My.Settings.Provider = Class2.Txt1.Text
        ' My.Settings.Save()
        ' End If
        'Return Class2.Txt1.Text & ";Data Source="
    End Function
End Class
