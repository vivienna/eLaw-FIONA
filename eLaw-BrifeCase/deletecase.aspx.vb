
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.IO

Partial Class deletecase
    Inherits System.Web.UI.Page
    
    Protected Shared ConnectionString As String

    Public Sub New()

        ConnectionString = clsConfigs.sGlobalConnectionString
        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If

    End Sub

    Dim UserName As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sql_conn As New SqlConnection(ConnectionString)
        Dim strpage_id As String = ""
        UserName = CType(Session("UserName"), String)
        If UserName = "" Then

        End If

        'btnsave.CssClass = "invisible"
        Dim DeleteCase As String = ""
        If Request.QueryString("pageid") <> "" Then
            DeleteCase = Request.QueryString("pageid")

        Else
            If Request.Form("Selectedvalues") <> "" Then
                strpage_id = CStr(Server.UrlDecode(Request.Form("Selectedvalues")))
            Else
                Response.Write("success")
            End If
            If Request.Form("is_ajax") = "1" Then


            Else
                Response.Redirect("~/login.aspx")
            End If

        End If
        If DeleteCase <> "" Then
            sql_conn.Open()
            Dim strdelete As String = "delete from SavedCasesLinks where CaseID='" & DeleteCase & "' and CaseUserName='" & UserName & "'"
            Dim mysqlcmd As New SqlCommand(strdelete, sql_conn)
            mysqlcmd.ExecuteNonQuery()
            Response.Redirect("user_cases.aspx")
        Else
            Dim SplitIds As String() = strpage_id.Split(New Char() {","c})
            For Each ID As String In SplitIds
                If ID <> "all" Then
                    'deelete_files_html(ID, UserName)
                    Try
                        sql_conn.Open()
                        Dim strdelete As String = "delete from SavedCasesLinks where CaseID='" & ID & "' and CaseUserName='" & UserName & "'"
                        Dim mysqlcmd As New SqlCommand(strdelete, sql_conn)
                        mysqlcmd.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    Finally
                        sql_conn.Close()
                    End Try
                End If
            Next
            Response.Write("success")
        End If

        




    End Sub

    Sub delete_notes(ByVal del_case_note_id As Integer, ByVal del_user_id As String)

        Dim sql_conn As New SqlConnection(ConnectionString)

        Try
            sql_conn.Open()
            Dim str_delete_note As String = "delete from SavedCasesRemark where RemarkUserName='" & del_user_id & "' and CaseRemarkId='" & del_case_note_id & "'"
            Dim mysqlcmd_del As New SqlCommand(str_delete_note, sql_conn)
            mysqlcmd_del.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sql_conn.Close()
        End Try





    End Sub
    Sub deelete_files_html(ByVal strpage_id As String, ByVal user_id As String)
        Dim file_name As String = ""
        Dim sql_conn As New SqlConnection(ConnectionString)
        sql_conn.Open()
        Try


            Dim strselectcase = "SELECT HtmlCaseName FROM SavedCasesLinks where CaseID= " & strpage_id & " AND  CaseUserName = '" & user_id & "'"
            Dim mysqlcmd2 As New SqlCommand(strselectcase, sql_conn)
            Dim mysqlreader2 As SqlDataReader
            mysqlreader2 = mysqlcmd2.ExecuteReader()
            If mysqlreader2.HasRows Then
                While mysqlreader2.Read
                    file_name = mysqlreader2(0)
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sql_conn.Close()
        End Try

        Dim path As String = Server.MapPath("~") & "\xmlfiles\notes"
        If (Directory.Exists(path)) Then
        Else
            My.Computer.FileSystem.CreateDirectory(Server.MapPath("~") & "\xmlfiles\notes")
        End If
        Try
            If File.Exists(path & "\" & file_name) Then
                File.Delete(path & "\" & file_name)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try





    End Sub
End Class
