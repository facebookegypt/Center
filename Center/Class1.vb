﻿Imports System.Data.OleDb
Imports Microsoft.Office.Interop.Access
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
    End Function
    Private DefaultLocalFolder As IO.DirectoryInfo
    Public Function OfdOpn(ByVal Desc1 As String) As String
        'compact And repair database
        Dim Rslt As String = String.Empty
        Dim OFD As New FolderBrowserDialog
        Dim FolderName As String = String.Empty
        With OFD
            .Description = Desc1
            .ShowNewFolderButton = True
        End With
        Dim result As DialogResult = OFD.ShowDialog()
        If (result = DialogResult.OK) Then
            FolderName = OFD.SelectedPath
            Rslt = FolderName
            DefaultLocalFolder = New IO.DirectoryInfo(Rslt)
            My.Settings.LocalBackUpFolder = DefaultLocalFolder.FullName
            My.Settings.Save()
        End If
        Return Rslt
    End Function
    Public Function CompRepair(Optional NewLocation As String = "") As Boolean
        'expression .CompactDatabase(SrcName, DstName, DstLocale, Options, password)
        'This will Compact & Repair MSAccess2007 Database to the same location with the same name.
        'CloseCN()
        'If CN.State = ConnectionState.Open Then CN.Close()
        Dim Result As Boolean = False
        Cursor.Current = Cursors.WaitCursor
        'Compact & Repair needs the Database File (*.accdb) to be closed.
        Dim MyAccDB As New Dao.DBEngine()
        Dim Compactedfil As String = Application.StartupPath & "\BackUp\" & Now.Date.ToShortDateString.Replace("/", "_") & ".accdb"
        Try
            Debug.WriteLine(MyAccDB.Version.ToString)
            MyAccDB.CompactDatabase(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm),
                                    Compactedfil,
                                    Dao.LanguageConstants.dbLangGeneral,
                                    Dao.DatabaseTypeEnum.dbVersion120, ";pwd=" & My.Settings.dbPass)
        Catch ex As Exception
            MsgBox("Error : " & ex.Message)
            Return False
            Exit Function
        End Try
        Try
            My.Computer.FileSystem.DeleteFile(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm))
            Result = True
        Catch ex As Exception
            MsgBox("Error :  " & ex.Message)
            Return Result
            Exit Function
        End Try
        Try
            Rename(Compactedfil, IO.Path.Combine(Application.StartupPath, My.Settings.dbNm))
            Result = True
        Catch ex As Exception
            MsgBox("Error : " & ex.Message)
            Return Result
            Exit Function
        End Try
        Application.DoEvents()
        Return Result
        Cursor.Current = Cursors.Default
    End Function
End Class
