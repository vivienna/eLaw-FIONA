<%@ Import Namespace="Dba_UrlEncrption" %>
<%@ Import Namespace="membersarea" %>
<%@ Import Namespace="membersarea.clsMyUtility" %>
<script runat="server">

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        loadData()
    End Sub
    
    Sub loadData()
        
        Dim ConnectionString As String = clsConfigs.sGlobalConnectionString '"Data Source=(LocalDB)\v11.0;AttachDbFilename=""C:\Users\CME\Desktop\PrecedentMap\PrecedentMap\App_Data\rcases_Data.mdf"";Integrated Security=True"
        Dim rowID As String = ""
        Dim ori As Boolean = False
        Dim fn As String = ""
        Dim title As String = ""
        Dim NewSearch As String = ""
        Dim ObjUt As New clsMyUtility
        Dim S_NewSearch As String
        Dim LeftTitle As String = ""
        Dim ResultTypeQ As String = Request.QueryString("type")
        Dim ResultQ As String = Request.QueryString("R")
        Dim classicView As String = Request.QueryString("classic")  ' classic =1 , new design =0 
        Dim ReturnClassic As String = ""
        If (Request.QueryString("id") <> "") Then
            fn = Server.UrlDecode(Request.QueryString("id"))
        Else
            Response.Redirect("~/login.aspx")
        End If
        NewSearch = Request.QueryString("sIn")
        If NewSearch <> "" Then
            S_NewSearch = ObjUt.Contains_Parser_notDetailed(NewSearch, "BOOLEANTEXT", "and")
        End If
        If ResultQ <> "" Then
            
        End If
        If classicView <> "" Then ' display classic, or Map
            If classicView = 1 Then
                ReturnClassic = "classic"
            End If
            
            If classicView = 0 Then
                ReturnClassic = "new"
            End If
        End If
        Dim Query As String
        Dim UrlDecrpt As New Dba_UrlEncrption(fn, False)
        Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
        If UrlQuery <> "" Then
            rowID=UrlQuery 
        Else
            Response.Redirect("~/login.aspx")
        End If
        
        If (Request.QueryString("t") = "ref") Then
            ori = True
        End If
        
        If ori Then
            LeftTitle = "Case Referred By"
            If NewSearch <> "" Then
                Query = "SELECT  distinct(RootCitation),ReferredTitle, RootTitle, Court, TYPE FROM refcases WHERE FileLinkTO = '" & rowID & "'" & _
                   " AND RootCitation IN (SELECT REPLACE(DATAFILENAME,'.xml','') as datafiles from casesindustrialcourt where " & S_NewSearch & ")"
            Else
                Query = "SELECT distinct(RootCitation),ReferredTitle,  RootTitle, refcases.Court, TYPE,JUDGEMENTDATE,CITATION FROM refcases,casesindustrialcourt WHERE FileLinkTO = '" & rowID & "'" & _
                        " and casesindustrialcourt.DATAFILENAME=RootCitation +'.xml' "
            End If
        Else
            LeftTitle = "Case Refers To"
            If NewSearch <> "" Then
                Query = "SELECT distinct(FileLinkTO),RootTitle,   ReferredTitle, refcases.Court, TYPE ,JUDGEMENTDATE,CITATION FROM refcases ,casesindustrialcourt WHERE RootCitation = '" & rowID & "'  and  casesindustrialcourt.DATAFILENAME=FileLinkTO +'.xml' " & _
                " AND FileLinkTO IN (SELECT REPLACE(DATAFILENAME,'.xml','') as datafiles from casesindustrialcourt where " & S_NewSearch & ")"
            Else
                Query = "SELECT distinct(FileLinkTO),RootTitle, ReferredTitle, refcases.Court, TYPE,JUDGEMENTDATE,CITATION FROM refcases,casesindustrialcourt WHERE RootCitation = '" & rowID & "'" & _
                    " and casesindustrialcourt.DATAFILENAME=FileLinkTO +'.xml' "
            End If
        End If
        'Dim Query As String = "SELECT RootTitle, FileLinkTO, ReferredTitle, Court, TYPE FROM refcases WHERE RootCitation = '" & rowID & "'"
        Dim conn As New System.Data.SqlClient.SqlConnection(ConnectionString)
        Dim Reader As System.Data.SqlClient.SqlDataReader
        Dim Result As String = ""
        Dim Found As Boolean
        Found = False

        Dim cmd As New System.Data.SqlClient.SqlCommand(Query, conn)
        conn.Open()
        Try
            Dim counter As Integer = 1
            Reader = cmd.ExecuteReader
            If Reader.HasRows Then
                While Reader.Read()
                    If counter = 1 Then
                        Response.Write("<table id='datatbl' class='hidden' data-title='" & Reader(1) & "'>")
                    End If
                    title = Reader(2)
                    title = title.Replace("&Amp;", "&amp;")
                    Dim IdLink As String = Reader(0)
                    Dim UrlLink As New Dba_UrlEncrption(IdLink, True)
                    Dim linkPrec As String = UrlLink.UrlEncrypt
                    Response.Write("<tr id=" & counter & "><td class='fileLinkTo'>" & linkPrec & "</td><td class='refTitle'>" & Reader(2) & "</td><td class='court'>" & Reader(3) & "</td> <td class='type'>" & Reader(4) & "</td><td class='date'>" & Reader(5) & "</td><td class='cite'>" & Reader(6) & "</td></tr>")
                    counter = counter + 1
                End While
                Response.Write("</table><span id='refine'>Refined Search: " & NewSearch & "</span><span id='LeftTitle'>" & LeftTitle & "</span><span id='classicview' class='hidden'>" & ReturnClassic & "</span>")
            Else
                Response.Write("<table id='datatbl' class='hidden' data-title='There is no Related Result to your search'>")
                Response.Write("</table><span id='refine'>Refined Search: " & NewSearch & "</span><span id='LeftTitle'>" & LeftTitle & "</span><span id='classicview' class='hidden'>" & ReturnClassic & "</span>")
            End If
            
        Catch ex As Exception
            Response.Write("<table id='datatbl' class='hidden' data-title='There is no Related Result to your search'>")
            Response.Write("</table><span id='refine'>Refined Search: " & NewSearch & "</span><span id='LeftTitle'>" & LeftTitle & "</span><span id='classicview' class='hidden'>" & ReturnClassic & "</span>")
        End Try
       
    End Sub
    
</script>