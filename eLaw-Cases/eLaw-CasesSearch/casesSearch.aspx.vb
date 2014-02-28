'/**********************************************************************/
'/*	Developer 	    : Usman Sarwar  								   */
'/*	Company     	: Elaw Sdn Bhd		    					   */
'/* Date Modified	: 27 June 2005  								    */  
'/*	Description		: Case Search page                                */
'/*	Version			: 1.0											   */
'/**********************************************************************/
Imports System.IO.Compression
Imports System.Data
Imports System
Imports System.EventArgs
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography
Imports System.Net

Namespace membersarea


    Partial Class casesSearch
        Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Dim UserName As String ' assign to seesion login
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("login.aspx?returnUrl=" & returnUrl)
            End If
            If IsPostBack = False Then
                btnSearch.EnableViewState = False
                lblMsg.EnableViewState = False
                fill_JudgementYears()
            End If
            If Not IsPostBack Then
                CheckBoxList1.Items.Clear()
                CheckBoxList2.Items.Clear()
                Dim query As String = ""
                query = "SELECT  distinct (  LEVEL1 )  FROM upload_subjectindex  WHERE LEVEL1 is not null ORDER BY LEVEL1"
                Dim obj As New clsCasesSearch
                Dim dt As New DataTable
                dt = obj.ExecuteMyQuery(query)
                For i = 0 To dt.Rows.Count - 1
                    Dim ii As New ListItem
                    ii.Text = StrConv(dt.Rows(i).Item(0).ToString(), VbStrConv.ProperCase)
                    ii.Value = dt.Rows(i).Item(0).ToString()
                    CheckBoxList1.Items.Add(ii)
                Next

            End If
        End Sub

        Private Sub fill_JudgementYears()
            Dim TypeList As New ArrayList
            Dim i As Int16
            Dim iCount As Int16 = 0

            Dim obj As New clsIntelligence
            Dim StYear, EndYear As Int16
            StYear = obj.CasesStartingYear
            EndYear = obj.CasesEndingYear

            For i = EndYear To StYear Step -1
                TypeList.Add(i)
                iCount = iCount + 1
            Next i

            ddlDate1.DataSource = TypeList
            ddlDate1.DataBind()

            ddlDate2.DataSource = TypeList
            ddlDate2.DataBind()


            ddlDate1.SelectedIndex = iCount - 1
            ddlDate2.SelectedIndex = 0
            TypeList = Nothing
            obj = Nothing
        End Sub

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Me.SearchData()
        End Sub

        Private Sub SearchData()

            Dim Obj As New clsSearch
            Dim objUtil As New clsMyUtility
            Dim SearchExactCases As String = ""
            Dim SearchNotCases As String = ""
            Dim SearchTypeinNo As Byte = 0
            Dim SearchCaseNumber As String = ""
            Dim SearchJudge As String = ""
            Dim Year1 As String = ""
            Dim Year2 As String = ""
            Dim URL As String = ""
            Dim iProximityLevel As String = ""
            Dim iProximityWithin As String = ""
            Dim sWord As String = ""
            Dim iOption As String = ""
            Dim SearchIndustrialCourt As Byte = 0
            Dim SearchCourt As Byte = 0
            Dim SubjectIndex As String
            Dim Subindex1 As String
            Dim subindex2 As String
            SubjectIndex = Label1.Text
            Dim list As New ArrayList
            Dim list2 As New ArrayList
            Dim listindexid As New ArrayList
            Dim Counsel As String = ""
            Dim CjudiciallyCon As Integer ' Cases Judicially Considered
            Dim OpSearchWithin As Integer = 0
            ''for subject index
            For Each li As ListItem In CheckBoxList1.Items
                If li.Selected Then
                    list.Add(li.Value)
                End If

            Next
            For Each li2 As ListItem In CheckBoxList2.Items
                If li2.Selected And li2.Text.Contains("   ") = False Then
                    list2.Add(li2.Value.Replace("'", "") & li2.Text.Replace("'", ""))
                Else
                    If li2.Selected And li2.Text.Contains("   ") = True Then
                        list2.Add(li2.Value.Replace("'", ""))
                    End If
                End If
            Next
            If (list2.Count > 0) Then
                subindex2 = "('" & String.Join("','", list2.ToArray()) & "')"
                Dim query As String = ""
                query = "SELECT  distinct subj_index_id  FROM subjectindex_unique  where replace(concat(level1,level2),'''','') in " & subindex2
                Dim obj1 As New clsCasesSearch
                Dim dt As New DataTable
                dt = obj1.ExecuteMyQuery(query)
                For i = 0 To dt.Rows.Count - 1
                    listindexid.Add(dt.Rows(i).Item(0).ToString())
                Next
                If listindexid.Count > 1 Then
                    subindex2 = String.Join(",", listindexid.ToArray())
                Else
                    subindex2 = listindexid.ToArray().ToString()
                End If
                Subindex1 = ""

            End If
            If list.Count > 0 Then ' it at least selected index 1 
            Else
                Subindex1 = ""
                subindex2 = ""
            End If
            If Len(Trim(txtExactPhrase.Text)) = 0 And Len(Trim(txtCaseNumber.Text)) = 0 And Len(Trim(txtJudgeName.Text)) = 0 And Len(Trim(legTitle.Text)) = 0 And Len(Trim(txtCounsel.Text)) = 0 Then
                lblMsg.Text = "*At least one of the red fields must be filled" ' "Enter the Keywords for Searching Cases in All or any of the words for searching"
                txtExactPhrase.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                legTitle.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                If txtNotCases.Text.Trim() = "" Then
                    txtCaseNumber.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                    txtJudgeName.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                Else
                    txtCaseNumber.BackColor = Drawing.Color.White
                    txtJudgeName.BackColor = Drawing.Color.White
                End If
                Exit Sub
            End If
            If Len(ddlDate1.SelectedItem.Text) > 1 And Len(ddlDate2.SelectedItem.Text) > 1 Then
                Year1 = ddlDate1.SelectedItem.Text
                Year2 = ddlDate2.SelectedItem.Text
            End If
            '================check Years Compare in case user missed select 
            If CInt(Year1) > CInt(Year2) Then
                Year1 = Year1 Xor Year2
                Year2 = Year1 Xor Year2
                Year1 = Year1 Xor Year2
            End If
            SearchExactCases = Trim(txtExactPhrase.Text)
            ' remove first char if chr = - or + or \ to avoid search by oprators 
            If SearchExactCases <> "" Then
                SearchExactCases = clsMyUtility.RemoveFirstIndex(Trim(txtExactPhrase.Text), "-,+,\")
            End If

            '================================== Finding And clean  Search Operators w/p , w/s, w/n 
            SearchExactCases = objUtil.CheckReaptedOp(SearchExactCases)
            OpSearchWithin = objUtil.GetSearchWithin(SearchExactCases)
            SearchExactCases = objUtil.ClearOprator(SearchExactCases)
            '==================================
            SearchNotCases = Trim(txtNotCases.Text)
            SearchCaseNumber = Trim(txtCaseNumber.Text)
            SearchJudge = Trim(txtJudgeName.Text)
            Counsel = Trim(txtCounsel.Text)
            'count number of words if more than 2 words then apply proxmity search
            Dim count1 As Integer = CountWords(SearchExactCases)
            '========================================================
            Dim S_dd_selected As Integer
            S_dd_selected = ddlsrch.SelectedIndex
            Select Case S_dd_selected
                Case 0
                    SearchTypeinNo = 1   ' Judgment
                Case 1
                    SearchTypeinNo = 2 ' Title
                Case 2 And count1 >= 2
                    iProximityWithin = 3   ' 3 words
                    iProximityLevel = 4
                    SearchTypeinNo = 1
                Case 3 And count1 >= 2
                    iProximityWithin = 4  ' 4 words
                    iProximityLevel = 4
                    SearchTypeinNo = 1
                Case 4 And count1 >= 2
                    iProximityWithin = 5  '5 words
                    iProximityLevel = 4
                    SearchTypeinNo = 1
                Case 5 And count1 >= 2
                    iProximityLevel = 1   ' Sentence
                    SearchTypeinNo = 1
                Case 6 And count1 >= 2
                    iProximityLevel = 2   ' Paragraph
                    SearchTypeinNo = 1
                Case 7 And count1 >= 2
                    iProximityLevel = 3   ' Page
                    SearchTypeinNo = 1
                Case Else
                    SearchTypeinNo = 1
                    iProximityLevel = 0
            End Select
            '================= Search Operators w/p , w/s, w/n 
            If OpSearchWithin > 0 And count1 >= 2 Then
                If OpSearchWithin = 1 Then
                    iProximityLevel = 1
                ElseIf OpSearchWithin = 2 Then
                    iProximityLevel = 2
                Else
                    iProximityWithin = OpSearchWithin
                    iProximityLevel = 4
                End If
                SearchTypeinNo = 1
            End If


            '========================================================
            If Len(ddlCourts.SelectedItem.Text) > 1 Then
                Dim court As String
                court = ddlCourts.SelectedItem.Text
                SearchCourt = Convert.ToInt16(ddlCourts.SelectedIndex)
            End If
            If cbIndustrialCourt.Checked = True Then
                SearchIndustrialCourt = 1
            End If
            '============= Cases Judicially Considered============
            If CJudcinially.SelectedIndex > 0 Then
                CjudiciallyCon = CJudcinially.SelectedIndex.ToString()
            End If
            ' SearchExactCases = SearchExactCases.Replace(" ", "+")
            Dim legSearch As String = legTitle.Text.Trim()

            SearchExactCases = SearchExactCases.Replace("&", "__")
            SearchExactCases = SearchExactCases.Replace("&", "")
            SearchExactCases = SearchExactCases.Replace("=", "")
            SearchNotCases = SearchNotCases.Replace("&", "")
            SearchNotCases = SearchNotCases.Replace("=", "")
            SearchCaseNumber = SearchCaseNumber.Replace("&", "")
            SearchCaseNumber = SearchCaseNumber.Replace("=", "")
            SearchJudge = SearchJudge.Replace("=", "")
            SearchJudge = SearchJudge.Replace("&", "")

            Dim UrlEncrption As String = "ec=" & SearchExactCases & "&nc=" & SearchNotCases & "&y1=" & Year1 & "&y2=" & Year2 & "&crt=" & SearchCourt & "&icrt=" & SearchIndustrialCourt & "&cn=" & SearchCaseNumber & "&j=" & SearchJudge & "&srchtpe=" & SearchTypeinNo & "&prxOpt=" & iProximityLevel & "&legT=" & legSearch & "&prxOptWit=" & iProximityWithin & "&sub1=" & Subindex1 & "&sub2=" & subindex2 & "&counsel=" & Counsel & "&cjc=" & CjudiciallyCon
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            URL = "casessearchresult1.aspx?info=" & link
            URL = Server.UrlPathEncode(URL)
            Response.Redirect(URL)

        End Sub
        Public Function getsubjectindex1() As List(Of String)
            Dim listlev1 As New List(Of String)
            For Each li As ListItem In CheckBoxList1.Items
                If li.Selected Then
                    listlev1.Add(li.Value)
                End If
            Next
            Return listlev1

        End Function
        ' Count words for proxmity search 
        Public Function CountWords(ByVal value As String) As Integer
            ' Count matches.
            Dim collection As MatchCollection = Regex.Matches(value, "\S+")
            Return collection.Count
        End Function

        Private Sub appendCitationYears(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CitPub.SelectedIndexChanged
            Dim query As String = ""

            If Not CitPub.SelectedIndex = 0 Then
                If CitPub.SelectedIndex = 4 Or CitPub.SelectedIndex = 7 Then
                    query = "select distinct(SUBSTRING(clj, 5, 4)) from CasesIndustrialCourt where clj like '%" & CitPub.SelectedItem.ToString & "%' and clj <> '' order by 1 desc "
                ElseIf CitPub.SelectedIndex = 5 Then
                    query = "select distinct(SUBSTRING(mlj, 5, 4)) from CasesIndustrialCourt where mlj like '%" & CitPub.SelectedItem.ToString & "%' and mlj <> '' order by 1 desc"
                ElseIf CitPub.SelectedIndex = 6 Then
                    query = "select distinct(SUBSTRING(amr, 5, 4)) from CasesIndustrialCourt where amr like '%" & CitPub.SelectedItem.ToString & "%' and amr <>'' order by 1 desc"
                Else
                    query = "select JUDGEMENTYEAR from CasesIndustrialCourt where datafilename like '%" & CitPub.SelectedItem.ToString & "%'  GROUP BY JUDGEMENTYEAR order by JUDGEMENTYEAR desc"
                End If

            End If

            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            'MsgBox(dt.Rows.Count)
            al.Add("")
            For i = 0 To dt.Rows.Count - 1
                If (IsNumeric((dt.Rows(i).Item(0)))) Then
                    al.Add(dt.Rows(i).Item(0))
                End If
            Next
            CitYear.DataSource = al
            CitYear.DataBind()
            CitYear.SelectedIndex = 0
            CitVol.DataSource = New ArrayList
            CitVol.DataBind()
            CitYear.AutoPostBack = True
            
            'AddHandler CitYear.SelectedIndexChanged, AddressOf onYearChange
        End Sub
        Private Sub onYearChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles CitYear.SelectedIndexChanged
            Dim yr As String = CType(sender, DropDownList).SelectedValue
            Dim query As String = ""
            If CitPub.SelectedIndex = 4 Or CitPub.SelectedIndex = 7 Then

                query = "select distinct left(clj,CHARINDEX('_',clj,11)-1) from CasesIndustrialCourt where clj like '%" & CitYear.SelectedItem.ToString & "_%'  and clj like '%" & CitPub.SelectedItem.ToString & "%' and clj <> '' order by 1 asc"
            ElseIf CitPub.SelectedIndex = 5 Then
                query = "select distinct left(mlj,CHARINDEX('_',mlj,11)-1) from CasesIndustrialCourt where mlj like '%" & CitYear.SelectedItem.ToString & "_%'  and mlj like '%" & CitPub.SelectedItem.ToString & "%' and (mlj <> '' and DATALENGTH(mlj)>12) order by 1 asc"
            ElseIf CitPub.SelectedIndex = 6 Then
                query = "select distinct left(amr,CHARINDEX('_',amr,11)-1) from CasesIndustrialCourt where amr like '%" & CitYear.SelectedItem.ToString & "_%'  and amr like '%" & CitPub.SelectedItem.ToString & "%' and amr <> '' order by 1 asc"
            Else
                query = "select distinct left(datafilename,CHARINDEX('_',datafilename,11)-1) from CasesIndustrialCourt where JUDGEMENTYEAR like '%" & CitYear.SelectedItem.ToString & "%'  and datafilename like '%" & CitPub.SelectedItem.ToString & "%' order by 1 asc"
            End If
            'Dim query As String = "select datafilename from cases where judgementyear like '%" & yr & "%' group by " ' to be specified

            'MsgBox(query)
            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            al.Add("")
            If CitPub.SelectedIndex > 3 Then
                For i = 0 To dt.Rows.Count - 1
                    al.Add(dt.Rows(i).Item(0).ToString.Substring(9))
                Next
            Else
                For i = 0 To dt.Rows.Count - 1
                    al.Add(dt.Rows(i).Item(0).ToString.Substring(10))
                Next
            End If
           
            'MsgBox(dt.ToString)
            CitVol.DataSource = al
            CitVol.DataBind()
            CitVol.SelectedIndex = 0
            CitVol.AutoPostBack = True
            'CitVol
            'AddHandler CitVol.SelectedIndexChanged, AddressOf onVolChange
        End Sub
        <System.Web.Services.WebMethod()> _
        Public Shared Function sendJSON(ByVal pubAjax As String, ByVal yearAjax As String, ByVal volAjax As String) As ArrayList
            Dim suggestions As New ArrayList
            Dim query As String = ""
            If pubAjax = "CLJ" Or pubAjax = "ILR" Then
                query = "select right(clj, charindex('_', reverse(clj)) - 1) from CasesIndustrialCourt where clj like '%" & pubAjax & "[_]" & yearAjax & "[_]" & volAjax & "[_]%' order by 1"
            ElseIf pubAjax = "MLJ" Then
                query = "select right(MLJ, charindex('_', reverse(MLJ)) - 1) from CasesIndustrialCourt where MLJ like '%" & pubAjax & "[_]" & yearAjax & "[_]" & volAjax & "[_]%' order by 1"
            ElseIf pubAjax = "AMR" Then
                query = "select right(AMR, charindex('_', reverse(AMR)) - 1) from CasesIndustrialCourt where AMR like '%" & pubAjax & "[_]" & yearAjax & "[_]" & volAjax & "[_]%' order by 1"
            Else
                query = "select right(DATAFILENAME, charindex('_', reverse(DATAFILENAME)) - 1) from CasesIndustrialCourt where datafilename like '%" & pubAjax & "[_]" & yearAjax & "[_]" & volAjax & "[_]%' order by 1"
            End If

            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            'Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            For i = 0 To dt.Rows.Count - 1
                suggestions.Add(dt.Rows(i).Item(0).ToString.Replace(".xml", "").Trim())
            Next
            Return suggestions
        End Function

        <System.Web.Services.WebMethod()> _
        Public Shared Function Getautocomplate(ByVal auto As String) As ArrayList
            Dim suggestions As New ArrayList
            Dim tolow As String
            Dim query As String = "select DISTINCT top 5 Phrase  from  SearchEngain_FrequencySearches where Phrase like '" & auto & "%' and flag=0 order by 1"
            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            'Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            For i = 0 To dt.Rows.Count - 1
                tolow = dt.Rows(i).Item(0).ToString.Trim()
                tolow = tolow.ToLower
                suggestions.Add(Left(tolow, 50))
            Next
            Return suggestions
        End Function
        <System.Web.Services.WebMethod()> _
        Public Shared Function GetCitation(ByVal pubAjax As String, ByVal yearAjax As String, ByVal volAjax As String, ByVal pg As String) As String

            Dim UrlEncrption As String = "yy=" & yearAjax & "&vol=" & volAjax & "&pub=" & pubAjax & "&pg=" & pg & "&srt="
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            Dim URL = "casessearchresult1.aspx?info=" & link

            'HttpContext.Current.Response.Redirect(URL)
            Return URL
        End Function

        Protected Sub CheckBoxList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles CheckBoxList1.SelectedIndexChanged
            Dim chk As Boolean
            If IsPostBack Then
                CheckBoxList2.Items.Clear()
                '    Dim res As String
                For Each li As ListItem In CheckBoxList1.Items

                    If li.Selected Then
                        chk = exist1in2(li.Value)
                        If chk Then
                            Dim nothingvar As Boolean
                        Else
                            Static qq As Integer
                            qq = qq + 1


                            Dim myii As New ListItem
                            myii.Text = "    " & li.Value & "     "
                            myii.Value = li.Value
                            myii.Selected = True

                            CheckBoxList2.Items.Add(myii)


                            Dim query As String = ""
                            query = "SELECT  distinct level2   FROM upload_subjectindex  where level2 is not null and level1='" & myii.Value & "' order by level2"
                            Dim obj As New clsCasesSearch
                            Dim dt As New DataTable
                            dt = obj.ExecuteMyQuery(query)
                            For i = 0 To dt.Rows.Count - 1
                               

                                Dim myi As New ListItem
                                myi.Text = dt.Rows(i).Item(0).ToString()
                                myi.Value = li.Value
                                myi.Selected = True
                                CheckBoxList2.Items.Add(myi)
                            Next


                        End If

                    Else

                        chk = exist1in2(li.Value)
                        If chk Then

                            While IsNothing(CheckBoxList2.Items.FindByValue(li.Value)) = False
                                CheckBoxList2.Items.Remove(CheckBoxList2.Items.FindByValue(li.Value))
                            End While

                        End If


                    End If

                Next

            End If
        End Sub

        Function exist1in2(ByVal chkvar As String) As Boolean
            chkvar = chkvar.ToLower
            For Each li2 As ListItem In CheckBoxList2.Items
                '            If li2.Selected Then
                If li2.Value.ToLower.Contains(chkvar) Then

                    Return True

                End If

            Next
            Return False
        End Function

       
        
    End Class
End Namespace



