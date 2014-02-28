<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="membersarea" %>
<script runat="server">

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        loadData()
    End Sub
    
    Sub loadData()
        
        Dim ConnectionString As String = clsConfigs.sGlobalConnectionString '"Data Source=(LocalDB)\v11.0;AttachDbFilename=""C:\Users\CME\Desktop\PrecedentMap\PrecedentMap\App_Data\rcases_Data.mdf"";Integrated Security=True"
        Dim rowID As String = ""
        Dim fn As String = ""
        Dim ori As Boolean = False
        Dim Query As String
        Dim CL As Integer ' difference between cases & Legislation for Display cl=0 legislation, cl=1 case
        Dim flgSecT As Integer ' seaerch for section title if legistion referred ID is case ID
        Dim leftTitle As String = ""
        If (Request.QueryString("id") <> "") Then
            fn = Server.UrlDecode(Request.QueryString("id"))
        Else
            Response.Redirect("~/login.aspx")
        End If
        Dim UrlDecrpt As New Dba_UrlEncrption(fn, False)
        Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
        If UrlQuery <> "" Then
            rowID = UrlQuery
        Else
            Response.Redirect("~/login.aspx")
        End If
        
        If (Request.QueryString("t") = "ref") Then
            ori = True
        End If
        'Request.QueryString("t") = "ref"
        'If ori Then
        '    Query = "SELECT ReferredTitle, RootCitation, RootTitle, Court, TYPE FROM refcases WHERE FileLinkTO = '" & rowID & "'"
        'Else
        '    Query = "SELECT RootTitle, FileLinkTO, ReferredTitle, Court, TYPE FROM refcases WHERE RootCitation = '" & rowID & "'"
        'End If
        
         
        If rowID.ToUpper.Contains("MY") Then
            ori = True
            CL = 0
        Else
            ori = False
            CL = 1
        End If
        If ori Then
             leftTitle="Act Referred By"
            Query = "SELECT refferredtitle,root_citation,root_title,'Act Title :  ' + refferredtitle, ReferredCitaion, '-' FROM ref_leg_tb WHERE ReferredCitaion like '%" & rowID & "%' group by refferredtitle,root_citation,root_title, refferredtitle, ReferredCitaion"
        Else
            flgSecT = 1
            leftTitle = "Case Refers To"
            Query = "SELECT root_title,ReferredCitaion,refferredtitle,root_title, '-' FROM ref_leg_tb WHERE root_citation = '" & rowID & "' group by root_title,ReferredCitaion,refferredtitle"  '         
        End If
        'Dim Query As String = "SELECT RootTitle, FileLinkTO, ReferredTitle, Court, TYPE FROM refcases WHERE RootCitation = '" & rowID & "'"
        
        Dim conn As New System.Data.SqlClient.SqlConnection(ConnectionString)
        Dim Reader As System.Data.SqlClient.SqlDataReader
        Dim Result As String = ""
        Dim Found As Boolean
        Dim SplitDBfn() As String
        Dim legislationFiles As String
        Dim sectionNo As String
        Dim DT As New DataTable
        Dim ObjCS As New clsCasesSearch()
        Dim SecTitle As String = ""
        Found = False
        Dim cmd As New System.Data.SqlClient.SqlCommand(Query, conn)
        conn.Open()
        Dim rowName As String = cmd.ExecuteScalar().ToString()
        Dim counter As Integer = 1
        Response.Write("<table id='datatbl' class='hidden' data-title='" & rowName & "'>")
        Reader = cmd.ExecuteReader
        While Reader.Read()
            Dim IdLink As String = Reader(1)
            Dim UrlLink As New Dba_UrlEncrption(IdLink, True)
            Dim linkPrec As String = UrlLink.UrlEncrypt
            If flgSecT = 1 Then
                legislationFiles= Reader(1).ToString()
                SplitDBfn = legislationFiles.Split(New Char() {";"c})
                legislationFiles = SplitDBfn(0)
                sectionNo = SplitDBfn(1)
                DT = ObjCS.getLegislationInfo(legislationFiles, sectionNo)
                If DT.Rows.Count = 1 Then
                    If Not IsDBNull(DT.Rows(0).Item(0)) Then
                        SecTitle = DT.Rows(0).Item(0)
                    End If
                End If
                
            End If
            
            Response.Write("<tr id=" & counter & "><td class='fileLinkTo'>" & linkPrec & "</td><td class='refTitle'>" & Reader(2) & "</td><td class='court'>" & IIf(SecTitle <> "", sectionNo & ". " & SecTitle, Reader(3)) & "</td> <td class='type'>----</td></tr>")
            counter = counter + 1
        End While
        Response.Write("</table>")
        Response.Write("<span id='CL'style='display:none;'>" & CL & "</span> <span id='leftTitle'>" & leftTitle & "</span>")
    End Sub
    
</script>
