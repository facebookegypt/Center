Imports System.Data.OleDb
Imports Microsoft.Office.Interop.Access
Imports Tulpep.NotificationWindow

Public Class Class1
    Public Property PopupNotifier1 = New PopupNotifier With
        {
        .ShowCloseButton = True,
        .ShowOptionsButton = True,
        .TitleFont = New Font("Times New Roman", 12, FontStyle.Bold),
        .TitleColor = Color.Black,
        .TitlePadding = New Padding(5),
        .ContentFont = New Font("Arial", 10.25, FontStyle.Bold),
        .ContentColor = Color.White,
        .IsRightToLeft = True,
        .TitleText = "مرحبا فى برنامج C E N T E R",
        .Delay = 5000,
        .BorderColor = Color.Red,
        .HeaderColor = Color.Red,
        .BodyColor = Color.DeepSkyBlue,
        .GradientPower = 10
    }
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
    Public Function LstUpdt(ByVal Bckpfldr As String) As Date
        Try
            Dim Iu As New Class1
            Dim Done As Boolean = Iu.CompRepair(Bckpfldr)
            If Done Then
                My.Settings.LstBckPDt = Now.Date
            End If
        Catch ex As Exception
            My.Settings.LstBckPDt = New Date(2022, 9, 20)
        End Try
        Return My.Settings.LstBckPDt
    End Function
    Public Function ConStr() As String
        Dim ConStr1 As String = My.Settings.Provider
        If String.IsNullOrEmpty(ConStr1) Then
            GetConStr()
        Else
            ConStr1 = My.Settings.Provider & IO.Path.Combine(Application.StartupPath, My.Settings.dbNm) & ";" &
                        "Jet OLEDB:Database Password= " & My.Settings.dbPass & ";" &
                        "Persist Security Info=False;"
        End If
        Return ConStr1
    End Function
    Private Function GetConStr() As String
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
        Else
            Return String.Empty
            Exit Function
        End If
        Return Rslt
    End Function
    Public Function CompRepair(Optional Compactedfil As String = "") As Boolean
        'expression .CompactDatabase(SrcName, DstName, DstLocale, Options, password)
        'This will Compact & Repair MSAccess2007 Database to the same location with the same name.
        'CloseCN()
        'If CN.State = ConnectionState.Open Then CN.Close()
        Dim Result As Boolean = False
        Cursor.Current = Cursors.WaitCursor
        'Compact & Repair needs the Database File (*.accdb) to be closed.

        Dim Di As IO.DirectoryInfo = New IO.DirectoryInfo(Compactedfil)
        If Not Di.Exists Then
            Di.Create()
        End If
        Dim MyAccDB As New Dao.DBEngine()
        Compactedfil = IO.Path.Combine(Application.StartupPath, "BackUp_" & Now.Date.ToShortDateString.Replace("/", "_") & ".accdb")
        Try
            MyAccDB.CompactDatabase(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm),
                                    Compactedfil,
                                    Dao.LanguageConstants.dbLangGeneral,
                                    Dao.DatabaseTypeEnum.dbVersion120, ";pwd=" & My.Settings.dbPass)
        Catch ex As Exception
            MsgBox("Error 1: " & ex.Message, MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
            End
            Return False
            Exit Function
        End Try
        Try
            My.Computer.FileSystem.DeleteFile(IO.Path.Combine(Application.StartupPath, My.Settings.dbNm))
            Result = True
        Catch ex As Exception
            MsgBox("Error :  " & ex.Message, MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
            Return Result
            Exit Function
        End Try
        Try
            Rename(Compactedfil, IO.Path.Combine(Application.StartupPath, My.Settings.dbNm))
            Result = True
        Catch ex As Exception
            MsgBox("Error : " & ex.Message, MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
            Return Result
            Exit Function
        End Try
        Application.DoEvents()
        Return Result
        Cursor.Current = Cursors.Default
    End Function
    Public Function GetIDFrmTbl(ByVal TblNm As String, ByVal Fld As String, ByVal Fld1 As String, ByVal Fld1Val As String) As List(Of Integer)
        Dim Lst As List(Of Integer) = New List(Of Integer)
        Dim SqlStr As String =
            <sql>SELECT( <%= Fld %> ) FROM <%= TblNm %> WHERE <%= Fld1 %>=?;</sql>.Value
        Using cn As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr()},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, cn) With {.CommandType = CommandType.Text}
            CMD.Parameters.AddWithValue("?", Fld1Val)
            cn.Open()
            Using Rdr As OleDbDataReader = CMD.ExecuteReader
                If Rdr.HasRows Then
                    While Rdr.Read
                        Lst.Add(Rdr.GetInt32(0))
                    End While
                End If
            End Using
        End Using
        Return Lst
    End Function
    Public Function GetData(ByVal SqlStr As String) As DataTable
        Dim Dt1 As DataTable = New DataTable With {.Locale = Globalization.CultureInfo.CurrentCulture}
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr()},
            CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
            DataAdapter1 As OleDbDataAdapter = New OleDbDataAdapter(CMD)
            DataAdapter1.Fill(Dt1)
        End Using
        Return Dt1
    End Function
    Public Sub GetGrps(ByVal DT1 As DataTable,
                       ByVal Constr1 As String,
                       ByVal SqlStr As String,
                        ByVal combobox As ComboBox,
                        Optional ByVal DisMem As String = "Value",
                        Optional ByVal ValMem As String = "Key")
        DT1 = New DataTable With {.Locale = Globalization.CultureInfo.InvariantCulture}
        Try
            Using CN = New OleDbConnection With {.ConnectionString = Constr1},
                CMD = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text},
                DataAdapter1 = New OleDbDataAdapter(CMD)
                DataAdapter1.Fill(DT1)
            End Using
        Catch ex As OleDbException
            MsgBox("مشكلة فى التعرف علي قاعدة البيانات : " & vbCrLf & ex.Message, _
                   MsgBoxStyle.MsgBoxRtlReading + MsgBoxStyle.MsgBoxRight + MsgBoxStyle.Critical)
            Class2.ShowDialog()
        End Try
        With combobox
            .BeginUpdate()
            .DataSource = DT1.DefaultView
            .DisplayMember = DisMem
            .ValueMember = ValMem
            .SelectedIndex = -1
            .EndUpdate()
            .Update()
        End With
    End Sub
    Public Function GetCount1(ByVal SqlStr As String) As Object
        Dim Rslt As String = String.Empty
        Dim Obj As Object
        Using CN As OleDbConnection = New OleDbConnection With {.ConnectionString = ConStr()},
                CMD As OleDbCommand = New OleDbCommand(SqlStr, CN) With {.CommandType = CommandType.Text}
            CN.Open()
            Obj = CMD.ExecuteScalar
            Return Obj
        End Using
    End Function
End Class
