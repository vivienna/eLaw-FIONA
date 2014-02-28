
'/**********************************************************************/
'@	Developer 	    : Modify By MOhammed Ali , Abdo, Zubair								   
'@	Company     	: eLaw          		    					   
'@ Date Modified	: 29 Oct 2013 								     
'@	Description		: Case Search result page                          
'@	Version			: 1.0	
'@ todo            
'   1- Reduce overhead
'       a- get/post method variables
'       b- searching variables
'       c- searching query overhead
'       d- searching condition overhead
'       e- html overhead
'       f- search other ways to optmize.
'   2-mapping for keywords
'       a- map keywords for better result, 
'       b- possible to use stored procedure etc
'       c- 
'   3- to show an icon to represent, the case is already in member's list
'/**********************************************************************/
Imports membersarea.clsFTSEngine
Imports System.Data
Imports System.IO.Compression
Imports System.Security.Cryptography
Imports System.IO
Imports Dba_UrlEncrption
Imports System.Xml

Namespace membersarea
    Partial Class CasesSearchResult11
        Inherits System.Web.UI.Page
        'Protected WithEvents lblDebug As System.Web.UI.WebControls.Label

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


        Dim s_searchExactCases As String
        Dim s_searchNotCases As String
        Dim s_searchCaseNumber As String
        Dim S_SearchJudge As String
        Dim S_SearchCourt As Byte
        Dim S_SearchIndustrialCourt As Byte
        Dim S_SearchType As Byte ' Headnotes/HEadnotes+Judgement
        Dim s_searchNew As String ' Get New search add to search query
        Dim PageCurrent As Int16
        Dim s_SortBy As String
        Dim s_Year1 As String
        Dim s_Year2 As String
        Dim flafLink As Boolean
        Dim SearchField As String 'search inside booleantext/title
        Dim i_searchProximityOption As Integer ' // if 0 means no proximity. 1 = sentence. 2= paragraph; 3= others
        Dim i_searchProximitywith As Integer '// proxmity by words distance 

        Dim UserName As String ' assign to current session user name
        Dim S_RecordCount As Int32
        Dim TitleSortIndex As Byte ' current select sort result by 
        Dim objUtil As New clsMyUtility
        Dim pageUrl As String = "" ' use to save search query into user's saved search
        Dim CitationYear As String = ""
        Dim CitationVolume As String = ""
        Dim CitationPub As String = ""
        Dim CitationPage As String = ""
        Dim filterBy As String = "0" ' current select filter result by 
        Dim selFltrIndx As Integer = 0
        Dim SaveQuerySE As String = "" ' save search words to give user mosgesstion in search cases
        Dim legTitle As String = "" ' get act title related to cases 
        Dim sRelevance As String ' this var not use for current time , it used to sort result by relevance query 
        Dim StrSearchExactWords As String
        Dim StrSearchWords As String
        Dim StrSearchAnyWords As String
        Dim subjindex1 As String = ""
        Dim subjindex2 As String = ""
        Dim S_searchTable As String = "" ' define which table search either cases or Industrial cases
        Dim S_Counsel As String = ""
        Dim S_cjc As Integer ' Cases Judicially Considered

        'Reduce Page Load By removing all white Space during page load 
        ' this function will handle this needs .Done by Mohammed Ali 3-12-2013
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            Dim REGEX_BETWEEN_TAGS As New Regex(">\s+<", RegexOptions.Compiled)
            Dim REGEX_LINE_BREAKS As New Regex("\n\s+", RegexOptions.Compiled)
            Using htmlwriter As New HtmlTextWriter(New System.IO.StringWriter())
                'Me.Page.Title = CommonFunctions.HtmlEncode(Me.Page.Title)
                MyBase.Render(htmlwriter)
                Dim html As String = htmlwriter.InnerWriter.ToString()
                html = REGEX_BETWEEN_TAGS.Replace(html, "> <")
                html = REGEX_LINE_BREAKS.Replace(html, String.Empty)
                writer.Write(html.Trim())
            End Using

        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            pageUrl = Request.Url.ToString
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                'return url
                Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
                'end of return url
            End If
            ' enable Gzip to reduce page size up to 70% 
            ' Done By Mohammed Ali
            If Request.Headers("Accept-encoding") IsNot Nothing AndAlso Request.Headers("Accept-encoding").Contains("gzip") Then
                Response.Filter = New GZipStream(Response.Filter, CompressionMode.Compress, True)
                Response.AppendHeader("Content-encoding", "gzip")
            ElseIf Request.Headers("Accept-encoding") IsNot Nothing AndAlso Request.Headers("Accept-encoding").Contains("deflate") Then
                Response.Filter = New DeflateStream(Response.Filter, CompressionMode.Compress, True)
                Response.AppendHeader("Content-encoding", "deflate")
            End If
            '\**********************************
            'Description for QueryString

            '  ec =  ExactPhrase   
            '  y1 =  Year1 
            '  y2 =  Year2 
            '  crt = Court  
            '  icrt = Industrial Court  
            '  t =  Title of Case 
            '  cn =  CaseNumber  
            '  j =   Judge "
            '  lrr=  Related Case Searc
            '' legT = legislation title
            'sub1 = subject index level 1 
            ' sub2 subject index level 2
            'cjc = cases Judicially Considered
            '\**********************************

            '==============================Received Data===================
            Dim EncrptUrl As String
            If Request.QueryString("info") <> "" Then
                EncrptUrl = Request.QueryString("info")
                pageUrl = Request.QueryString("info")
                Dim UrlDecrpt As New Dba_UrlEncrption(EncrptUrl, False)
                Dim UrlQuery As New List(Of urlbreaker)
                UrlQuery = UrlDecrpt.UrlDetails
                If UrlQuery.Count > 0 Then

                    For i = 0 To UrlQuery.Count - 1
                        If UrlQuery(i).exist = True Then
                            Select Case UrlQuery(i).parvar
                                Case "ec"
                                    s_searchExactCases = UrlQuery(i).parvalue
                                Case "nc"
                                    s_searchNotCases = UrlQuery(i).parvalue
                                Case "y1"
                                    s_Year1 = UrlQuery(i).parvalue
                                Case "y2"
                                    s_Year2 = UrlQuery(i).parvalue

                                Case "crt"
                                    S_SearchCourt = UrlQuery(i).parvalue
                                Case "icrt"
                                    S_SearchIndustrialCourt = UrlQuery(i).parvalue
                                    If S_SearchIndustrialCourt = 1 Then
                                        S_searchTable = "CasesIndustrialCourt"
                                    End If
                                Case "cn"
                                    s_searchCaseNumber = UrlQuery(i).parvalue
                                Case "j"
                                    S_SearchJudge = UrlQuery(i).parvalue
                                Case "srchtpe"
                                    S_SearchType = UrlQuery(i).parvalue

                                    If S_SearchType = 1 Then
                                        SearchField = "BOOLEANTEXT"
                                    ElseIf S_SearchType = 2 Then
                                        SearchField = "title"
                                    ElseIf S_SearchType = 3 Then
                                        SearchField = "BOOLEANTEXT"
                                    End If
                                Case "prxOpt"
                                    i_searchProximityOption = UrlQuery(i).parvalue
                                Case "legT"
                                    legTitle = UrlQuery(i).parvalue
                                Case "prxOptWit"
                                    i_searchProximitywith = UrlQuery(i).parvalue
                                Case "page"
                                    PageCurrent = UrlQuery(i).parvalue
                                Case "ns"
                                    s_searchNew = UrlQuery(i).parvalue
                                Case "srt"
                                    s_SortBy = UrlQuery(i).parvalue
                                Case "fltr"
                                    filterBy = UrlQuery(i).parvalue
                                Case "yy"
                                    CitationYear = UrlQuery(i).parvalue
                                Case "vol"
                                    CitationVolume = UrlQuery(i).parvalue
                                Case "pub"
                                    CitationPub = UrlQuery(i).parvalue
                                    S_searchTable = "CasesIndustrialCourt"
                                Case "pg"
                                    CitationPage = UrlQuery(i).parvalue
                                Case "sub1"
                                    subjindex1 = UrlQuery(i).parvalue
                                Case "sub2"
                                    subjindex2 = UrlQuery(i).parvalue
                                Case "counsel"
                                    S_Counsel = UrlQuery(i).parvalue
                                Case "cjc"
                                    S_cjc = UrlQuery(i).parvalue
                            End Select
                        End If
                    Next
                End If
            Else

                'return url by abdo
                Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
                'end of return url

                ' Response.Redirect("login.aspx")
            End If
            '==============================================================

            If CitationYear <> "" Then
                GoTo sortingBy

            End If

sortingBy:
            If s_SortBy = "" And s_searchExactCases <> "" Then
                s_SortBy = " order by case "
                s_SortBy &= " when " & objUtil.Contains_Parser_notDetailed(s_searchExactCases, SearchField) & " then '1'"
                ' if seaerch query does not asking for exact phrase by checking if string contain """" 
                If s_searchExactCases.Contains("""") = False Then
                    s_SortBy &= " when " & objUtil.Contains_Parser_notDetailed(s_searchExactCases, SearchField, "and") & " then '2' "
                    's_SortBy &= " when " & objUtil.Contains_Parser_notDetailed(s_searchExactCases, SearchField, "or") & " then '3'"
                    s_SortBy &= "else '3' end  "
                    s_SortBy &= " , judgementyear desc"
                Else
                    s_SortBy &= "else '2' end  "
                End If
                TitleSortIndex = 0

            Else

                Select Case s_SortBy
                    Case "1"  ' Federal /Superme Court
                        s_SortBy = " order by "
                        s_SortBy &= "case" & vbNewLine
                        s_SortBy &= "	when CASETYPE like '%MLRA%' and (COURT like '%federal%' or COURT like '%supreme%') then '1'" & vbNewLine
                        s_SortBy &= "	when CASETYPE like '%MLRA%' then '2'" & vbNewLine
                        s_SortBy &= "	else '3'" & vbNewLine
                        s_SortBy &= "END, JUDGEMENTYEAR desc"
                        TitleSortIndex = 1
                    Case "2"  'appleal court
                        s_SortBy = " order by "
                        s_SortBy &= "case"
                        s_SortBy &= "	when CASETYPE like '%MLRA%' and COURT like '%appeal%' then '1'"
                        s_SortBy &= "	when CASETYPE like '%MLRA%' then '2'"
                        s_SortBy &= "	else '3'"
                        s_SortBy &= "END, JUDGEMENTYEAR desc"

                        TitleSortIndex = 2
                    Case "3" 'high court
                        s_SortBy = "  order by " & vbNewLine
                        s_SortBy &= "case" & vbNewLine
                        s_SortBy &= "	when CASETYPE like '%MLRH%' or COURT like '%high%' then '1'" & vbNewLine
                        s_SortBy &= "	else '2'" & vbNewLine
                        s_SortBy &= "END, JUDGEMENTYEAR desc"
                        's_SortBy = "  order by JUDGE asc"
                        TitleSortIndex = 3
                    Case "4"   'industrial court
                        s_SortBy = " order by " & vbNewLine
                        s_SortBy &= "case" & vbNewLine
                        s_SortBy &= "	when CASETYPE like '%MELR%' then '1'" & vbNewLine
                        s_SortBy &= "	else '2'" & vbNewLine
                        s_SortBy &= "END, JUDGEMENTYEAR desc"
                        TitleSortIndex = 4
                    Case "5"   'Jugde Name
                        s_SortBy = "  order by JUDGE asc"
                        s_SortBy &= ", JUDGEMENTYEAR desc"
                        TitleSortIndex = 5
                    Case "6"   'Title Acs
                        s_SortBy = "  order by TITLE asc"
                        s_SortBy &= ", JUDGEMENTYEAR desc"
                        TitleSortIndex = 6
                    Case "7"   'Title desc
                        s_SortBy = "  order by TITLE desc"
                        s_SortBy &= ", JUDGEMENTYEAR desc"
                        TitleSortIndex = 7
                    Case Else
                        s_SortBy = " order by judgementyear desc"
                        TitleSortIndex = 0

                End Select

            End If

            'If filterBy = "" Then
            '    filterBy = "0"
            'End If


            ''''''''' filter by '''''''''
            ''''''''' changes by zubair ''''''''''

            Select Case filterBy

                Case "1"
                    ' filter by MRLA with federal and supreme court
                    filterBy = " and CASETYPE like '%MLRA%' and (COURT like '%federal%' or COURT like '%supreme') "
                    toggleSortCourts(False)
                    selFltrIndx = 1
                Case "2"
                    ' filter by MRLA with appeal court
                    filterBy = " and CASETYPE like '%MLRA%' and COURT like '%appeal%' "
                    toggleSortCourts(False)
                    selFltrIndx = 2
                Case "3"
                    '   ' filter by Industrial cases
                    filterBy = " and CASETYPE like '%MLRH%' "
                    selFltrIndx = 3
                    toggleSortCourts(False)
                Case "4"
                    filterBy = " and CASETYPE like '%MELR%' "
                    toggleSortCourts(False)
                    selFltrIndx = 4
                Case Else
                    filterBy = ""
                    toggleSortCourts(True)

            End Select



            '//////////////////////////////////////////////////////////////////////////////////////////////////

            If PageCurrent = 0 Then
                PageCurrent = 1
            End If
            If S_RecordCount = 0 Then
                S_RecordCount = 0
            End If

            Dim tempSql As String = ""

            If IsPostBack = False Then
                SearchWords()
                lblTbl.EnableViewState = False

                lblCaseFound2.EnableViewState = False

                btnSearch.EnableViewState = False
                lblPgNo.EnableViewState = False
                lblMsg.EnableViewState = False


                ddlTitle.SelectedIndex = TitleSortIndex
                sfilter.SelectedIndex = selFltrIndx

                ddlTitle.AutoPostBack = True
                sfilter.AutoPostBack = True

            End If

        End Sub
        Private Sub toggleSortCourts(ByVal t As Boolean)
            ddlTitle.Items.Item(4).Enabled = t
            ddlTitle.Items.Item(5).Enabled = t
            ddlTitle.Items.Item(6).Enabled = t
            ddlTitle.Items.Item(7).Enabled = t

        End Sub

        Private Sub SearchWords()

            Dim Sql As String
            Dim tempSql As String = ""
            ' When search come from citation ,build query here , not need to go to FTS function 
            Dim iSupportedPlatform As Integer = ElawPlatforms.pc
            If CitationPub = "CLJ" Or CitationPub = "ILR" Then
                If CitationYear <> "" Then
                    If CitationPub <> "" Then
                        tempSql &= "  CLJ like '%" & CitationPub & "_" & CitationYear & "%'"
                    End If
                    If CitationVolume <> "" Then
                        tempSql &= " and CLJ like '%" & CitationVolume & "%'"
                    End If
                    If CitationPage <> "" Then
                        tempSql = "  CLJ = '" & CitationPub & "_" & CitationYear & "_" & CitationVolume & "_" & CitationPage & "'"
                    End If
                    Sql = tempSql
                End If
            ElseIf CitationPub = "MLJ" Then
                If CitationYear <> "" Then
                    If CitationPub <> "" Then
                        tempSql &= "  MLJ like '%" & CitationPub & "_" & CitationYear & "%'"
                    End If
                    If CitationVolume <> "" Then
                        tempSql &= " and MLJ like '%" & CitationVolume & "%'"
                    End If
                    If CitationPage <> "" Then
                        tempSql = "  MLJ = '" & CitationPub & "_" & CitationYear & "_" & CitationVolume & "_" & CitationPage & "'"
                    End If
                    Sql = tempSql
                End If

            ElseIf CitationPub = "AMR" Then
                If CitationYear <> "" Then
                    If CitationPub <> "" Then
                        tempSql &= "  AMR like '%" & CitationPub & "_" & CitationYear & "%'"
                    End If
                    If CitationVolume <> "" Then
                        tempSql &= " and AMR like '%" & CitationVolume & "%'"
                    End If
                    If CitationPage <> "" Then
                        tempSql = "  AMR = '" & CitationPub & "_" & CitationYear & "_" & CitationVolume & "_" & CitationPage & "'"
                    End If
                    Sql = tempSql
                End If

            Else
                If CitationYear <> "" Then
                    tempSql &= " JUDGEMENTYEAR like '" & CitationYear & "'"
                    If CitationVolume <> "" Then
                        tempSql &= " and SortValue like '%" & CitationVolume & "%'"
                    End If
                    If CitationPub <> "" Then
                        tempSql &= " and CASETYPE = '" & CitationPub & "'"
                    End If
                    If CitationPage <> "" Then
                        tempSql &= " and pageno =" & CitationPage
                    End If
                    Sql = tempSql
                Else
                    Sql = Me.FTS_Circuits()
                End If

            End If


            'Response.Write(Sql)
            If Sql = "" Then

                '// Now handling in this function as previously it was in the FTS_Circuit

                lblPgNo.Text = "0"
                lblTbl.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Match Your Search!</span></div>"

                lblCaseFound2.Text = "0"



                txtSearch.Enabled = False
                btnSearch.Enabled = False
                ddlTitle.Enabled = False
                sfilter.Enabled = False
                ' ddlCourts.Enabled = False
                ddlTitle.Enabled = False

                Exit Sub
            End If

            myBindData(Sql)

        End Sub


        Private Sub myBindData(ByVal sqlQuery As String)

            Dim PageFirst As Int16
            Dim RecordCount As Int32 'totalRec or MaxRec
            Dim PageSize As Byte  'showRec
            Dim PageCount As Int16
            Dim LastPage As Int16
            Dim CurrentRecord As Int32
            Dim FirstRecord As Byte = 1
            Dim Counter As Int16
            Dim URLpage As String
            Dim URL As String
            Dim File As String
            Dim Title As String
            Dim Number As String
            Dim JudgementDate As String
            'Dim JudgementYear As Integer
            Dim Headnotes As String
            Dim Court As String
            Dim Judge As String
            Dim FileCitation As String
            Dim Citation As String
            Dim CsaeCitationFlag As String ' check if case has citation so send par to caseview 1 , if not 0
            Dim legCiationFlag As String ' check if case has legislation citation so send par to caseview 1 , if not 0
            Dim legislationCitation As String
            Dim URLCaseDisplayed As String
            Dim URLHeadnotesDisplayed As String
            Dim checkDisplayJandH As String
            Dim strSQL As String
            Dim StrHighlightWords As String = objUtil.RefineSentence(s_searchExactCases)
            Dim I, J As Integer
            Dim ExactPraseSeach As Boolean ' use to check if search words already save into search enginee table or not
            Dim StartTime, EndTime, ResultTime As Int32
            'Dim DS As New DataSet()
            Dim ResultGrid As New System.Text.StringBuilder
            Dim TopNavigation As New System.Text.StringBuilder
            Dim ObjInt As New clsIntelligence
            Dim ObjCS As New clsCasesSearch
            Dim DT As New DataTable




            '================== Cases Judicially Considered ============
            Dim CountCaseCitation As Integer = 0
            Dim CountCaseCitationRefd As Integer = 0
            Dim CountCaseCitationFoll As Integer = 0
            Dim CountCaseCitationDist As Integer = 0
            Dim CountCaseCitationLeg As Integer = 0
            Dim cjcType As String = ""
            Dim cjcSetBackGround As String = ""
            Dim cjCaseType As String = ""
            '==========================================================



            lblTbl.Text = ""
            PageSize = 10 ' Show this no. of records on every page
            If CurrentRecord = 0 Then
                CurrentRecord = 1
                'ElseIf CurrentRecord > 1 And PageCurrent > 1 Then
            End If
            If PageCurrent > 1 Then
                CurrentRecord = PageSize * (PageCurrent - 1) ' means pg2= 30 * (2-1) ,pg3=  30 * (3-1)
                CurrentRecord = CurrentRecord + 1 ' because 30 = 30+1 i.e CurRec = lastRec +1
            End If
            StartTime = System.DateTime.Now.Millisecond
            '================== Replace Search Talbe According To User Select Plus Citation 
            If S_searchTable <> "" Then
                strSQL = "select counting from CasesIndustrialCourt where " & sqlQuery  '' this is only for counting we need some field to count because reader is not working i choose small data field
            Else
                strSQL = "select counting from cases where " & sqlQuery  '' this is only for counting we need some field to count because reader is not working i choose small data field
            End If

            'My.Computer.FileSystem.WriteAllText("C:\Elaw\AliAli\log.txt", strSQL & vbCrLf, True)
            RecordCount = S_RecordCount
            If RecordCount = 0 Then
                DT = ObjCS.ExecuteMyQuery(strSQL)
                RecordCount = DT.Rows.Count()
                S_RecordCount = RecordCount
                If (S_RecordCount = 0) Then
                    lblBottomNavigation.Text = ""
                    lblPgNo.Text = "0"
                    lblCaseFound2.Text = " <span class='tfont1'>0</span>&nbsp;results&nbsp;"
                    lblTbl.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Match Your Search!</span></div>"
                    txtSearch.Enabled = False
                    btnSearch.Enabled = False
                    ddlTitle.Enabled = False
                    sfilter.Enabled = False
                    'ddlTitle.Enabled = False
                    Exit Sub
                End If

            End If
            PageCount = CInt(Math.Ceiling(CDbl(RecordCount) / PageSize)) 'recordCount to double because ceiling requies it
            If PageCount > 100 Then
                LastPage = 100 ' because of business logic
            Else
                LastPage = PageCount
            End If


            ' Lesser than firstPage than first page
            If PageCurrent < 1 Then
                PageCurrent = 1
            End If
            ' i.e If currentpage got greater than totalpages then make currentpage the total page
            ',i.e lastPage. this is used with next as well next set
            If PageCurrent >= PageCount Then
                PageCurrent = PageCount
            End If



            If RecordCount = 1 Then
                lblCaseFound2.Text = "<span class='tfont1'>" & RecordCount & "</span>&nbsp;results&nbsp;"

            End If
            If RecordCount > 1 Then
                lblCaseFound2.Text = " <span class='tfont1'>" & RecordCount & "</span>&nbsp;results&nbsp;"

            End If

            If PageCount = 1 Then

                lblBottomNavigation.Visible = False
            End If

            '/// feeeling lucky - usman 20110516

            If RecordCount <= 0 Then

                lblBottomNavigation.Text = ""
                lblPgNo.Text = "0"
                lblCaseFound2.Text = " <span class='tfont1'>0</span>&nbsp;results&nbsp;"
                lblTbl.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Match Your Search!</span></div>"
                txtSearch.Enabled = False
                btnSearch.Enabled = False
                ddlTitle.Enabled = False
                Exit Sub
            End If
            If PageCurrent = 1 Then

            End If
            '//////////////////////////////////////////////////////////////////////////////////64
            Dim lstcid As New List(Of String)
            If S_searchTable = "" Then
                S_searchTable = "cases"
            End If
            strSQL = "select DATAFILENAME from " & S_searchTable & " where " & sqlQuery
            Dim CurRecForFetch As Int32
            CurRecForFetch = 10 * (PageCurrent - 1)
            DT = ObjCS.ExecuteMyQuery(strSQL, S_searchTable, CurRecForFetch)
            Dim FileCond As String = ""
            If DT.Rows.Count > 0 Then
                For J = 0 To DT.Rows.Count - 1
                    lstcid.Add(DT.Rows(J).Item(0))
                Next
            End If
            FileCond = " datafilename in ('" & String.Join("','", lstcid) & "')"
            If S_cjc = 1 Then
                cjCaseType = " where  type='foll' "
            ElseIf S_cjc = 2 Then
                cjCaseType = " where (TYPE='dist' or TYPE='ovrld' or TYPE='not foll') "
            Else
                cjCaseType = " "
            End If
            strSQL = "select DATAFILENAME,TITLE,CASENUMBER,JUDGE,COURT,JUDGEMENTDATE,SUBSTRING(headnotes,0,100) as 'headnotes_small',CITATION, JUDGEMENTYEAR,CountcitationRefd, CountcitationFoll,CountcitationDist,Countcitation,Countcitationleg,type from  " & S_searchTable & _
                      " left join (select count(distinct(RootCitation))  as CountcitationRefd,FileLinkTO from refcases where TYPE='refd' group by FileLinkTO ) " & _
                    " tb2 on  Replace(DATAFILENAME,'.xml','')=tb2.FileLinkTO " & _
                    " left join (select count(distinct(RootCitation))  as Countcitationfoll,FileLinkTO from refcases where TYPE='foll' group by FileLinkTO ) " & _
                    " tb4 on  Replace(DATAFILENAME,'.xml','')=tb4.FileLinkTO " & _
                    " left join (select count(distinct(RootCitation))  as Countcitationdist,FileLinkTO from refcases where TYPE='dist' or TYPE='ovrld' or TYPE='not foll' group by FileLinkTO ) " & _
                    " tb5 on  Replace(DATAFILENAME,'.xml','')=tb5.FileLinkTO " & _
                    " left join (select count(distinct(FileLinkTO))  as Countcitation,RootCitation from refcases  group by RootCitation  ) " & _
                    " tb6 on  Replace(DATAFILENAME,'.xml','')=tb6.RootCitation " & _
                    "left join (select [TYPE],FileLinkTO from refcases " & cjCaseType & "  group by TYPE,FileLinkTO ) " & _
                    " tb7 on  Replace(DATAFILENAME,'.xml','')=tb7.FileLinkTO  " & _
                    " left join (SELECT  count(distinct(ReferredCitaion))as CountcitationLeg , root_citation FROM REF_LEG_TB group by root_citation) " & _
                    " tb3 on Replace(DATAFILENAME,'.xml','')=tb3.root_citation  " & _
                      " where " & FileCond & filterBy & s_SortBy
            DT = ObjCS.ExecuteMyQuery(strSQL)
            '///////////////////////////////////////////////////////////////////////////////////
            EndTime = System.DateTime.Now.Millisecond
            ResultTime = (EndTime - StartTime)
            lblSec.Text = ResultTime / 1000
            lblBottomNavigation.Text = ""
            Dim AT As String
            AT = "<a  href="
            URLpage = "casessearchresult1.aspx?info="
            If PageCount > 1 Then

                If PageCurrent > 1 Then
                    Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & filterBy & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page=1"
                    Dim UrlEncrption1 As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & filterBy & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page=" & PageCurrent - 1
                    Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                    Dim link As String = UrlLink.UrlEncrypt
                    Dim UrlLink1 As New Dba_UrlEncrption(UrlEncrption1, True)
                    Dim link1 As String = UrlLink1.UrlEncrypt
                    TopNavigation.Append("<li>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png' alt='Next'/><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png' alt='Next'/></a></li>")
                    TopNavigation.Append("<li>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & link1) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png' alt='Next'/></a></li>")
                End If
                '//////////////////////////////////////////////////////////////////////// 64
                If ((PageCurrent - (PageCurrent Mod 10)) + 10) > PageCount Then
                    For I = PageCurrent - (PageCurrent Mod 10) To PageCount
                        Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & selFltrIndx & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page=" & I
                        Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                        Dim link As String = UrlLink.UrlEncrypt
                        If Not I = 0 Then
                            If I = PageCurrent Then
                                TopNavigation.Append("<li  class='currentPage'>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & " >" & I & " </a></li>")
                            Else
                                TopNavigation.Append("<li>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & " >" & I & " </a></li>")
                            End If
                        End If

                    Next I
                Else
                    For I = PageCurrent - (PageCurrent Mod 10) To (PageCurrent - (PageCurrent Mod 10)) + 10
                        Dim UrlEncrptionpage As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & selFltrIndx & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page=" & I
                        Dim UrlLinkpage As New Dba_UrlEncrption(UrlEncrptionpage, True)
                        Dim linkpage As String = UrlLinkpage.UrlEncrypt
                        If Not I = 0 Then
                            If I = PageCurrent Then
                                TopNavigation.Append("<li class='currentPage'>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & linkpage) & Chr(34) & " >" & I & " </a></li>")
                            Else
                                TopNavigation.Append("<li>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & linkpage) & Chr(34) & " >" & I & " </a></li>")
                            End If


                        End If
                    Next I

                    Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & selFltrIndx & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page=" & PageCurrent + 1

                    Dim UrlEncrption1 As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & selFltrIndx & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page=" & PageCount

                    Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                    Dim link As String = UrlLink.UrlEncrypt
                    Dim UrlLink1 As New Dba_UrlEncrption(UrlEncrption1, True)
                    Dim link1 As String = UrlLink1.UrlEncrypt


                    TopNavigation.Append("<li>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a></li>")

                    TopNavigation.Append("<li>" & AT & Chr(34) & Server.UrlPathEncode(URLpage & link1) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a></li>")

                End If

            End If

skipotherloop:



            URLCaseDisplayed = "case_notes/showcase.aspx?info="


            If PageCount > 0 Then

                lblTbl.Text = ""

                If PageCurrent >= 1 Then

                    J = 0


                    Dim RC As Int16 = DT.Rows.Count - 1
                    For Counter = 0 To RC '' 31 to 60 or 61 to 90

                        If Counter = RecordCount Then
                            GoTo allRecDisplayed
                        End If

                        File = DT.Rows(Counter).Item(0)
                        Dim FileReplace As String = Trim(File.Replace(".xml", ""))
                        'FileReplace = clsMyUtility.Encrypt_QueryString(FileReplace)
                        Title = DT.Rows(Counter).Item(1)
                        If Title.Length > 100 Then
                            Title = Left(Title, 100) & "...."
                        End If
                        Number = DT.Rows(Counter).Item(2)
                        Judge = DT.Rows(Counter).Item(3)
                        Court = DT.Rows(Counter).Item(4)
                        Court = ToProperCase(Court)
                        JudgementDate = DT.Rows(Counter).Item(5)
                        JudgementDate = JudgementDate.Replace("<p>", "")
                        JudgementDate = JudgementDate.Replace("</p>", "")
                        FileCitation = DT.Rows(Counter).Item(7)


                        If Not IsDBNull(DT.Rows(Counter).Item(6)) Then
                            Headnotes = DT.Rows(Counter).Item(6)
                            If Headnotes.Length > 50 Then
                                checkDisplayJandH = "&jd=0"
                            Else
                                checkDisplayJandH = "&jd=1"
                            End If

                        End If
                        '================ Cjc Area =====================
                        If Not IsDBNull(DT.Rows(Counter).Item(9)) Then
                            CountCaseCitationRefd = CInt((DT.Rows(Counter).Item(9)))
                        End If
                        If Not IsDBNull(DT.Rows(Counter).Item(10)) Then
                            CountCaseCitationFoll = CInt((DT.Rows(Counter).Item(10)))
                        End If
                        If Not IsDBNull(DT.Rows(Counter).Item(11)) Then
                            CountCaseCitationDist = CInt((DT.Rows(Counter).Item(11)))
                        End If
                        If Not IsDBNull(DT.Rows(Counter).Item(12)) Then
                            CountCaseCitation = CInt((DT.Rows(Counter).Item(12)))
                        End If
                        If Not IsDBNull(DT.Rows(Counter).Item(13)) Then
                            CountCaseCitationLeg = CInt((DT.Rows(Counter).Item(13)))
                        End If
                        If Not IsDBNull(DT.Rows(Counter).Item(14)) Then
                            cjcType = DT.Rows(Counter).Item(14).ToString()
                        End If
                        '===============================================================
                        If J = 1 Then
                            Title = Replace(Title, "''", "'")
                            Citation = ""
                            legislationCitation = ""
                            Dim UrlLink As New Dba_UrlEncrption(FileReplace, True)
                            Dim linkPrec As String = UrlLink.UrlEncrypt
                            ' check if case has case's referred then print the link 
                            '======================= Cjc Area ================================
                            Citation = "<div class='cjc' style='clear:both'><span style='color: #0000ff; font-size:0.9em'>" & FileCitation & "</span>&nbsp;&nbsp;"


                            If CountCaseCitationFoll > 0 Or CountCaseCitationRefd > 0 Or CountCaseCitationDist > 0 Then

                                Citation &= " <span class='label'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >Treatments :</a></span>&nbsp;"
                            End If
                            If CountCaseCitationFoll > 0 Then
                                Citation &= "<span class='positive'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >" & CountCaseCitationFoll & " positive</a> </span>&nbsp;"
                            End If
                            If CountCaseCitationRefd > 0 Then
                                Citation &= "<span class='neutral'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >" & CountCaseCitationRefd & " neutral</a> </span>&nbsp;"
                            End If
                            If CountCaseCitationDist > 0 Then
                                Citation &= "<span class='negative'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >" & CountCaseCitationDist & " negative</a></span>&nbsp"
                            End If



                            If CountCaseCitation > 0 Then
                                Citation &= "&nbsp;&nbsp;<span class='Leglisation'><a href='precMap.aspx?id=" & linkPrec & "' >Cases Referred(" & CountCaseCitation & ")</a></span>"
                                CsaeCitationFlag = "1"
                            End If
                            ' check if case has legislation  referred then print the link 
                            If CountCaseCitationLeg > 0 Then
                                Citation &= "&nbsp;&nbsp;<span class='Leglisation'><a href='LegislationReferred.aspx?fn=" & linkPrec & "' >Legislation Referred(" & CountCaseCitationLeg & ")</a></span>"
                                legCiationFlag = "1"
                            End If
                            Citation &= "</div>"
                            'If cjcType.ToLower() = "refd" Then
                            '    cjcSetBackGround = "style='background-color: #FF9933;'"
                            'ElseIf cjcType.ToLower() = "foll" Then
                            '    cjcSetBackGround = "style='background-color: #009900;'"
                            'ElseIf cjcType.ToLower() = "dist" Or cjcType.ToLower() = "not foll" Then
                            '    cjcSetBackGround = "style='background-color: #FF0000;'"
                            'End If
                            '==========================================================

                            Dim IdLink As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&id=" & Trim(FileReplace) & checkDisplayJandH & "&lc=" & legCiationFlag & "&cc=" & CsaeCitationFlag
                            Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
                            Dim linkFileId As String = UrlFIleID.UrlEncrypt

                            ResultGrid.Append("<div class='boxResult' >")
                            ResultGrid.Append("<div class='boxCheck'" & cjcSetBackGround & "><input type='checkbox' name='item0[]' value='" & Trim(linkPrec) & "'></div>")
                            ResultGrid.Append("<div class='resultTitleCase' style=' width:95%;'>")
                            ResultGrid.Append("<p class='tTitleCase' >" & AT & Chr(34) & Server.UrlPathEncode(URLCaseDisplayed & linkFileId) & Chr(34) & " id='" & Trim(linkPrec) & "' data-title='" & Title & "'>" & Title & " </a></p>")
                            ResultGrid.Append("<p class='title2Case' style='color:#f47200;'>" & Court & "&nbsp;&nbsp;<span style='color:#000000;'>" & JudgementDate & "</span></p>")
                            'subject index 
                            Dim strsubjectindex As String = getsubjectIndex(FileReplace & ".xml")
                            If strsubjectindex <> "" Then
                                ResultGrid.Append("<p class='title2Case' style='color:black;'> " & strsubjectindex & "</p>")
                            End If
                            'end of subject index
                            ResultGrid.Append("<p class='title2Case' style='color:black;'>" & Select100Words(File, s_searchExactCases, StrHighlightWords) & "</p>")
                            ResultGrid.Append(Citation & " &nbsp;&nbsp;" & legislationCitation & "</div>")
                            ResultGrid.Append("<div class='clear'></div></div>")
                            CsaeCitationFlag = ""
                            legCiationFlag = ""

                        End If

                        If J = 0 Then
                            Headnotes = ""
                            Title = Replace(Title, "''", "'")
                            Citation = ""
                            legislationCitation = ""
                            Dim UrlLink As New Dba_UrlEncrption(FileReplace, True)
                            Dim linkPrec As String = UrlLink.UrlEncrypt
                            '==================================
                            Citation = "<div class='cjc' style='clear:both'><span style='color: #0000ff; font-size:0.9em'>" & FileCitation & "</span>&nbsp;&nbsp;"

                            If CountCaseCitationFoll > 0 Or CountCaseCitationRefd > 0 Or CountCaseCitationDist > 0 Then

                                Citation &= " <span class='label'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >Treatments :</a></span>&nbsp;"
                            End If
                            If CountCaseCitationFoll > 0 Then
                                Citation &= "<span class='positive'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >" & CountCaseCitationFoll & " positive</a> </span>&nbsp;"
                            End If
                            If CountCaseCitationRefd > 0 Then
                                Citation &= "<span class='neutral'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >" & CountCaseCitationRefd & " neutral</a> </span>&nbsp;"
                            End If
                            If CountCaseCitationDist > 0 Then
                                Citation &= "<span class='negative'><a href='precMap.aspx?id=" & linkPrec & "&cls=0&t=ref' >" & CountCaseCitationDist & " negative</a></span>&nbsp"
                            End If
                            If CountCaseCitation > 0 Then
                                Citation &= "&nbsp;&nbsp;<span class='Leglisation'><a href='precMap.aspx?id=" & linkPrec & "' >Cases Referred(" & CountCaseCitation & ")</a></span>"
                                CsaeCitationFlag = "1"
                            End If
                            ' check if case has legislation  referred then print the link 
                            If CountCaseCitationLeg > 0 Then
                                Citation &= "&nbsp;&nbsp;<span class='Leglisation'><a href='LegislationReferred.aspx?fn=" & linkPrec & "' >Legislation Referred(" & CountCaseCitationLeg & ")</a></span>"
                                legCiationFlag = "1"
                            End If
                            Citation &= "</div>"
                            'If cjcType.ToLower() = "refd" Then
                            '    cjcSetBackGround = "style='background-color: #FF9933;'"
                            'ElseIf cjcType.ToLower() = "foll" Then
                            '    cjcSetBackGround = "style='background-color: #009900;'"
                            'ElseIf cjcType.ToLower() = "dist" Or cjcType.ToLower() = "not foll" Then
                            '    cjcSetBackGround = "style='background-color: #FF0000;'"
                            'End If
                            '====================================================================
                            Dim IdLink1 As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&id=" & Trim(FileReplace) & checkDisplayJandH & "&lc=" & legCiationFlag & "&cc=" & CsaeCitationFlag
                            Dim UrlFIleID1 As New Dba_UrlEncrption(IdLink1, True)
                            Dim linkFileId1 As String = UrlFIleID1.UrlEncrypt

                            ResultGrid.Append("<div class='boxResult'>")
                            ResultGrid.Append("<div class='boxCheck' " & cjcSetBackGround & "><input type='checkbox' name='item0[]' value='" & Trim(linkPrec) & "'></div>")
                            ResultGrid.Append("<div class='resultTitleCase' style=' width:95%;'>")
                            ResultGrid.Append("<p class='tTitleCase'>" & AT & Chr(34) & Server.UrlPathEncode(URLCaseDisplayed & linkFileId1) & Chr(34) & " id='" & Trim(linkPrec) & "' data-title='nothing'>" & Title & " </a></p>")
                            ResultGrid.Append("<p class='title2Case' style='color:#f47200;'>" & Court & " &nbsp;&nbsp; <span style='color:#000000;'>" & JudgementDate & "</span></p>")

                            'abdo work only check on local host
                            'Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                            ' If returnUrl.ToString().ToLower().Contains("localhost") Then
                            Dim strsubjectindex As String = getsubjectIndex(FileReplace & ".xml")
                            If strsubjectindex <> "" Then
                                ResultGrid.Append("<p class='title2Case' style='color:black;'> " & strsubjectindex & "</p>")
                            End If

                            'End If

                            'abdo work only check on local host
                            ResultGrid.Append("<p class='title2Case' style='color:black;'>" & Select100Words(File, s_searchExactCases, StrHighlightWords) & "</p>")
                            ResultGrid.Append(Citation & "</div>")
                            'ResultGrid.Append("<div class='verdictBox'><p class='verdictBold'>&nbsp;</p><span class='tWhite'><a href='#'>Application Allowed</a></span></div>")

                            ResultGrid.Append("<div class='clear'></div></div>")
                            CsaeCitationFlag = ""
                            legCiationFlag = ""

                        End If
                        '=========== End of cjc Area==============
                        CountCaseCitation = 0
                        CountCaseCitationFoll = 0
                        CountCaseCitationDist = 0
                        CountCaseCitationRefd = 0
                        CountCaseCitationLeg = 0
                        cjcType = ""
                        cjcSetBackGround = ""
                        '===================================

                        J = J + 1
                        If J = 2 Then
                            J = 0
                        End If



                    Next Counter

allRecDisplayed:
                    CurrentRecord = Counter


                    lblTbl.Text = ResultGrid.ToString
                End If
            End If
            lblBottomNavigation.Text = TopNavigation.ToString

            ExactPraseSeach = ObjCS.ExactPhraseAutoComplete(s_searchExactCases, "SearchEngain_FrequencySearches", 0)
            DT = Nothing
            ObjInt = Nothing
            ObjCS = Nothing
            ResultGrid = Nothing
            TopNavigation = Nothing
            ExactPraseSeach = False

            PageFirst = 0
            RecordCount = 0
            PageSize = 0
            PageCount = 0
            LastPage = 0
            PageCurrent = 0
            CurrentRecord = 0
            FirstRecord = 0
            Counter = 0

            ' URLpage = ""
            strSQL = ""

            URLpage = ""
            URL = ""
            File = ""
            Title = ""
            Number = ""
            JudgementDate = ""
            Headnotes = ""
            Court = ""
            Judge = ""

            Citation = ""
            URLCaseDisplayed = ""
            URLHeadnotesDisplayed = ""

            I = 0
            J = 0
            StartTime = 0
            EndTime = 0
            ResultTime = 0
        End Sub



        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            s_searchExactCases = Trim(txtExactPhrase.Text)
            Dim URLpage As String
            If s_searchExactCases <> "" Then
                Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=&crt=&icrt=&cn=&j=&prxOpt=" & 3 & "&ns=&srchtpe=1&counsel=&cjc="
                Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                Dim link As String = UrlLink.UrlEncrypt
                URLpage = "casessearchresult1.aspx?info=" & link

                Response.Redirect(URLpage)
            Else
                txtExactPhrase.Attributes.Add("placeholder", "Please insert keywords")
            End If
        End Sub
        Private Sub saveSrch(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveSearch.ServerClick
            Dim obj As New clsCasesSearch
            Dim sql As String
            Dim title As String
            If saveWithName.Text = "" Then
                title = s_searchExactCases
            Else
                title = saveWithName.Text
            End If



            'type 0 is cases , 1 is legislation
            sql = "insert into SavedSearch(Username,Title,Link,Time,Type) values('" & Session("UserName") & "','" & title & "','" & pageUrl & "',GETDATE(),0 )"
            Dim res As Boolean = obj.UpdateRecord(sql)

            Response.Redirect("casessearchresult1.aspx?info=" & pageUrl) 'can be removed to reduce load if a solution is made for contacting the server without redirecting
        End Sub


        Private Function FTS_Circuits() As String
            Dim objUtil As New clsMyUtility
            Dim Sql As String = ""

            Dim StrSearchNotWords As String
            Dim StrSearchProximity As String = ""
            Dim StrSearchThesaurusInflection As String = ""
            Dim StrSearchThesaurusInflection1 As String = ""
            Dim StrPerpareTokenizer As String = ""
            Dim StrNewSearch As String
            Dim newSearch As String
            Dim SearchCourt As String
            Dim Judge As String
            Dim CaseNumber As String
            Dim sendProxmitySentence As String
            Dim sProximitySearchOption As Boolean = False
            Dim StrSubindex1 As String = subjindex1
            Dim StrSubindex2 As String = subjindex2
            Dim CountDoublequates As Integer
            Dim Str_Counsel As String = S_Counsel
            StrSearchExactWords = s_searchExactCases
            StrSearchWords = s_searchExactCases
            StrSearchAnyWords = s_searchExactCases
            StrSearchNotWords = s_searchNotCases
            StrNewSearch = s_searchNew
            Dim SearchOperator As Boolean = False
            '==========update this part for opertors 
            ' check if StrsearchExact has double quotes then apply search by opertors for extact search 
            If StrSearchExactWords <> "" Then
                CountDoublequates = objUtil.CountCharacter(StrSearchExactWords, """")
                ' if StrsearchExact empty of + ,-,\ then apply normal query
                If (StrSearchExactWords.Contains("+") = False And StrSearchExactWords.Contains("-") = False And StrSearchExactWords.Contains("\") = False) And CountDoublequates = 0 Then
                    StrSearchExactWords = objUtil.Contains_Parser_notDetailed(s_searchExactCases, SearchField)
                    StrSearchWords = objUtil.Contains_Parser_notDetailed(s_searchExactCases, SearchField, "and")
                    StrSearchAnyWords = objUtil.Contains_Parser_notDetailed(s_searchExactCases, SearchField, "or")
                    sProximitySearchOption = True
                ElseIf CountDoublequates > 1 Then
                    StrSearchExactWords = objUtil.Contains_Parser_operatorSearch(StrSearchExactWords, SearchField)
                    SearchOperator = True
                Else
                    If StrSearchExactWords <> "" Then
                        StrSearchExactWords = objUtil.Contains_Parser_operatorSearch(StrSearchExactWords, SearchField)
                        SearchOperator = True
                    End If
                End If
            End If


            If StrSearchNotWords <> "" Then
                StrSearchNotWords = objUtil.Contains_Parser_notDetailed(StrSearchNotWords, SearchField, "or") '' for not
            End If

            If StrSearchExactWords <> "" And S_SearchType = 1 And sProximitySearchOption = True Then
                sendProxmitySentence = objUtil.RefineSentence(s_searchExactCases)
                StrPerpareTokenizer = objUtil.PerpareTokenizer(sendProxmitySentence)
                '    ''// here distance for sentence, paragraph and page is approximate as it was mentioned in the documents
                If i_searchProximityOption = 1 Then ' sentence

                    StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, 15, SearchField)

                ElseIf (i_searchProximityOption = 2) Then 'paragraph

                    StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, 50, SearchField)

                ElseIf (i_searchProximityOption = 3) Then 'page

                    StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, 300, SearchField)
                ElseIf i_searchProximityOption = 4 Then

                    StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, i_searchProximitywith, SearchField)

                End If
                StrSearchThesaurusInflection = objUtil.Contains_Parser_Proximity_Inflection(StrPerpareTokenizer, SearchField)
                StrSearchThesaurusInflection1 = objUtil.Contains_Parser_Proximity_Thesaurus(StrPerpareTokenizer, SearchField)
            End If
            If SearchOperator = False Then

                If StrSearchNotWords <> "" And (StrSearchExactWords <> "" And S_SearchType = 1) Then

                    Sql = " (( " & StrSearchExactWords & " ) or ( " & StrSearchWords & " ) or (" & StrSearchAnyWords & ")) and NOT(" & StrSearchNotWords & " ) "

                End If
                If StrSearchNotWords = "" And (StrSearchExactWords <> "" And S_SearchType = 1) Then

                    Sql = "( ( " & StrSearchExactWords & " ) or ( " & StrSearchWords & " ) or (" & StrSearchAnyWords & "))"
                End If
                '===========================Title===================================
                If StrSearchNotWords <> "" And (StrSearchExactWords <> "" And S_SearchType = 2) Then

                    Sql = " (( " & StrSearchExactWords & " ) or ( " & StrSearchWords & " )) and NOT(" & StrSearchNotWords & " )"

                End If
                If StrSearchNotWords = "" And (StrSearchExactWords <> "" And S_SearchType = 2) Then

                    Sql = "( ( " & StrSearchExactWords & " ) or ( " & StrSearchWords & " ) )"
                End If
            Else
                Sql = StrSearchExactWords
            End If
            '====================================
            '====================================
            If StrNewSearch <> "" Then
                newSearch = objUtil.Contains_Parser_notDetailed(Trim(s_searchNew), "BOOLEANTEXT", "and")
                If Sql = "" Then
                    Sql &= " ( " & newSearch & " )"
                Else
                    Sql &= " and ( " & newSearch & " )"
                End If


            End If
            ''------------------------------------
            '========== proximity======================
            If StrSearchProximity <> "" And StrSearchProximity <> " " Then
                If Sql = "" Then
                    Sql &= " ( " & StrSearchProximity & " )"
                Else
                    Sql &= " and ( " & StrSearchProximity & " )"
                End If
            End If
            ''------------------------------------
            '====== inflection and thesaurus===========
            If StrSearchThesaurusInflection <> "" Then
                If Sql = "" Then
                    Sql &= " ( " & StrSearchThesaurusInflection & " ) and  (" & StrSearchThesaurusInflection1 & ")"
                Else
                    Sql &= " and ( " & StrSearchThesaurusInflection & " ) and  (" & StrSearchThesaurusInflection1 & ")"
                End If
            End If

            '--------------------------------------------------------

            Judge = S_SearchJudge
            CaseNumber = s_searchCaseNumber

            Dim judgeArgs = ""
            If Judge <> "" Then
                judgeArgs = " ( "
                Dim tempjdg As String() = Judge.Split(",")
                For i = 0 To tempjdg.Length() - 1
                    If (i <> 0) Then
                        judgeArgs &= " and "
                    End If
                    judgeArgs &= "judge like '%" & tempjdg(i).Trim() & "%'"
                Next
                judgeArgs &= " ) "
            End If
skipTitle:
            '==========Judge===========
            If Judge <> "" Then
                Judge = judgeArgs
                If Sql = "" Then
                    Sql = judgeArgs
                Else
                    Sql &= " and " & judgeArgs
                End If
            End If
            '========== Case Number ===========
            If CaseNumber <> "" Then
                If Sql = "" Then
                    Sql = " CASENUMBER like '%" & s_searchCaseNumber & "%'  "
                Else
                    Sql &= "and ( CASENUMBER like '%" & s_searchCaseNumber & "%' ) "
                End If
            End If
            '======== 
            '========== Case Counsel ===========
            If Str_Counsel <> "" Then
                Str_Counsel = objUtil.Contains_Parser_notDetailed(S_Counsel, "COUNSEL", "and")
                If Sql = "" Then
                    Sql = " (" & Str_Counsel & ")  "
                Else
                    Sql &= "and (" & Str_Counsel & ") "
                End If
            End If
            '======== 

            '==================Subject Index ============================
            If StrSubindex2 <> "" Then

                If Sql = "" Then
                    Sql = " (DATAFILENAME in ( select mlrcitation from upload_subjectindex where subindexid in (" & StrSubindex2 & ") ) )"

                Else
                    ' Response.Write("((" & StrSubindex1 & ") or " & "(" & StrSubindex2 & "))")
                    Sql &= " and (DATAFILENAME in ( select mlrcitation from upload_subjectindex where subindexid in (" & StrSubindex2 & ") ) )"
                    'Sql &= " and  ((" & StrSubindex1 & ") or " & "(" & StrSubindex2 & "))  "
                    'Response.Write(" and  ((" & StrSubindex1 & ") or " & "(" & StrSubindex2 & "))  ")
                End If

            End If
            '==================== Cases Judicially Considered=============================\
            If S_cjc > 0 Then
                ' 1=Applied (Followed)----Green 
                '2= Referred(Refered) ----Brown, 
                '3=overruld (Overruled, Dis, Not Follow)---- Red 
                Dim Scjc As String = ""
                If S_cjc = 1 Then
                    Scjc = " where type='foll' "
                ElseIf S_cjc = 2 Then
                    Scjc = " where type='not foll' or type='dist' or type='ovrld' "
                Else
                    Scjc = ""
                End If
                If Scjc <> "" Then
                    If Sql = "" Then
                        Sql = " (DATAFILENAME in ( select distinct(RootCitation +'.xml') as cjCase from refcases " & Scjc & " ) )"
                    Else

                        Sql &= " and (DATAFILENAME in ( select distinct(RootCitation +'.xml') as cjCase from refcases " & Scjc & " ) )"
                    End If
                End If

            End If
            '================================================================================
            '//////////////////////////////////////////////////////////////////////////////////////64

            Dim legSql As String = ""
            Dim legDT = New DataTable
            Dim legObj = New clsCasesSearch
            If legTitle <> "" Then
                legTitle = objUtil.Contains_Parser_notDetailed(Trim(legTitle), "b.TITLE", "and")
                'legSql = "select a.DATAFILENAME, a.ATTRIBUTEVALUE from cases_links as a, LEGISLATION as b where b.TITLE like '%" & legTitle & "%' and a.ATTRIBUTEVALUE like '%' +left(b.DATAFILENAME,len(b.DATAFILENAME)-4)+ '%'"
                If Trim(Sql) = "" Then
                    'DATAFILENAME IN(select RTRIM(LTRIM(a.root_citation +'.xml')) as DATAFILENAME from ref_leg_tb as a, LEGISLATION as b where ( " & legTitle & " )  and replace(RTRIM(LTRIM(b.DATAFILENAME)),'.xml','') = SUBSTRING(a.ReferredCitaion,0, CHARINDEX(';',a.ReferredCitaion)))
                    Sql = " ( DATAFILENAME IN(select RTRIM(LTRIM(a.root_citation +'.xml')) as DATAFILENAME from ref_leg_tb as a, LEGISLATION as b where ( " & legTitle & " )  and replace(RTRIM(LTRIM(b.DATAFILENAME)),'.xml','') = SUBSTRING(a.ReferredCitaion,0, CHARINDEX(';',a.ReferredCitaion))))"
                Else
                    Sql &= " and DATAFILENAME IN(select RTRIM(LTRIM(a.root_citation +'.xml')) as DATAFILENAME from ref_leg_tb as a, LEGISLATION as b where ( " & legTitle & " )  and replace(RTRIM(LTRIM(b.DATAFILENAME)),'.xml','') = SUBSTRING(a.ReferredCitaion,0, CHARINDEX(';',a.ReferredCitaion))) "
                End If

            End If

            '//////////////////////////////////////////////////////-------

            If Sql = "" Then

                lblPgNo.Text = "0"
                lblTbl.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Document Found in Elaw Digital Library!</span></div>"
                lblCaseFound2.Text = "0"
                txtSearch.Enabled = False
                btnSearch.Enabled = False
                ddlTitle.Enabled = False
                ddlTitle.Enabled = False
                'ddlCourts.Enabled = False
                ddlTitle.Enabled = False
                Exit Function
            End If


            If (S_SearchCourt > 0 And S_SearchCourt <= 5) Then
                SearchCourt = "and ( " & objUtil.CourtForCountries("Malaysia", S_SearchCourt) & " )"
                SearchCourt = SearchCourt.Replace("court like '%Supreme Court%'", "(court like '%Supreme Court%' or court like '%Federal%')")
                Sql &= SearchCourt

            End If

            If (S_SearchCourt = 6) Then

                'No criteria as it will search for all courts

            End If

SkipCourt:
            'Dim Language As String
            'Language = "and (language='English')"
            'Sql &= Language

            ' // which means the years are mentioned
            If (s_Year1 <> "" And s_Year1 <> "") Then
                Dim year As String
                Dim StYear, EndYear As Int16
                Dim obj As New membersarea.clsIntelligence

                StYear = obj.CasesStartingYear
                EndYear = obj.CasesEndingYear

                If (StYear = s_Year1) And (EndYear = s_Year2) Then
                    year = " "
                Else
                    year = " and ( CONVERT(INT,JUDGEMENTYEAR) >= " & s_Year1 & " and CONVERT(INT,JUDGEMENTYEAR) <= " & s_Year2 & " )"

                End If

                Sql &= year
                year = ""
            End If

            If filterBy <> "" Then
                Sql &= " " & filterBy
            End If

            If s_SortBy = "" And StrSearchExactWords <> "" Then
                Sql &= sRelevance
            Else
                Sql &= s_SortBy
            End If

            ''/*  Explicitly Destructing the values.  */
            objUtil = Nothing
            StrSearchExactWords = ""

            StrSearchWords = ""
            StrSearchAnyWords = ""
            StrSearchNotWords = ""
            newSearch = ""
            StrNewSearch = ""

            Judge = ""
            CaseNumber = ""
            'legTitle = ""

            Return Sql
        End Function

        Private Sub ddlTitle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTitle.SelectedIndexChanged
            Dim seltdTitleSort As String
            Dim objUtil As New clsMyUtility
            Dim UrlPage As String
            seltdTitleSort = ddlTitle.SelectedItem.Text
            S_SearchIndustrialCourt = 4
            s_SortBy = ddlTitle.SelectedIndex.ToString()
            filterBy = sfilter.SelectedIndex.ToString()
            S_SearchCourt = 6
            Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & s_SortBy & "&fltr=" & filterBy & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page="

            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            UrlPage = "casessearchresult1.aspx?info=" & link ' s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & s_SortBy & "&fltr=" & sfilter.SelectedIndex.ToString() & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&page="
            UrlPage = Server.UrlPathEncode(UrlPage)
            Response.Redirect(UrlPage)
        End Sub

        Private Sub sfilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sfilter.SelectedIndexChanged
            Dim seltdTitleSort As String
            Dim objUtil As New clsMyUtility
            Dim UrlPage As String
            seltdTitleSort = sfilter.SelectedItem.Text
            If sfilter.SelectedIndex = 4 Then
                S_SearchIndustrialCourt = 1
            End If

            filterBy = sfilter.SelectedIndex.ToString()
            S_SearchCourt = 6
            Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & TitleSortIndex & "&fltr=" & filterBy & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page="

            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            UrlPage = "casessearchresult1.aspx?info=" & link '& s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&ns=" & s_searchNew & "&srchtpe=" & S_SearchType & "&srt=" & ddlTitle.SelectedIndex.ToString() & "&fltr=" & filterBy & "&rc=" & S_RecordCount & "&prxOpt=" & i_searchProximityOption & "&page="
            UrlPage = Server.UrlPathEncode(UrlPage)
            Response.Redirect(UrlPage)
        End Sub


        Protected lstSearchResult As ArrayList

        Private Sub NewSearch()
            Dim UrlPage As String
            If txtSearch.Text = "" Then
                lblMsg.Text = "Please insert keywords for searching."
                Exit Sub
            End If
            If (Len(Trim(txtSearch.Text)) <> 0) And (Len(Trim(s_searchNew)) <> 0) Then
                s_searchNew = Trim(txtSearch.Text) + " " + s_searchNew '// for having history of search together

            ElseIf (Len(Trim(txtSearch.Text)) <> 0) Then
                s_searchNew = Trim(txtSearch.Text)
            End If

            Dim UrlEncrption As String = "ec=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&srchtpe=" & S_SearchType & "&prxOpt=" & i_searchProximityOption & "&legT=" & legTitle.Trim() & "&prxOptWit=" & i_searchProximitywith & "&ns=" & s_searchNew & "&yy=" + CitationYear + "&vol=" + CitationVolume + "&pub=" + CitationPub + "&pg=" + CitationPage & "&counsel=" & S_Counsel & "&cjc=" & S_cjc & "&page="

            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            '=" & s_searchExactCases & "&nc=" & s_searchNotCases & "&y1=" & s_Year1 & "&y2=" & s_Year2 & "&legT=" & legTitle.Trim() & "&crt=" & S_SearchCourt & "&icrt=" & S_SearchIndustrialCourt & "&cn=" & s_searchCaseNumber & "&j=" & S_SearchJudge & "&srt=" & s_SortBy & "&srchtpe=" & S_SearchType & "&ns=" & s_searchNew & "&prxOpt=" & i_searchProximityOption & "&prxOptWit=" & i_searchProximitywith & "&page="
            UrlPage = "casessearchresult1.aspx?info=" & link
            UrlPage = Server.UrlPathEncode(UrlPage)
            Response.Redirect(UrlPage)
            'Server.Transfer(UrlPage)

        End Sub


        ' retrun case title as titleCase 
        Function ToProperCase(ByVal str As String) As String
            Dim myTI As System.Globalization.TextInfo
            myTI = New System.Globalization.CultureInfo("en-US", False).TextInfo
            str = str.ToLower
            str = myTI.ToTitleCase(str)
            Return str
        End Function
        Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
            Me.NewSearch()
        End Sub
        Function getsubjectIndex(ByVal strfilename As String) As String
            ' Dim subfilename, sublevel1, sublevel2 As String
            Dim strsubjectindex As New ArrayList
            Dim query As String = ""
            Try

                query = "  select mlrcitation ,level1,level2 from upload_subjectindex where mlrcitation='" & strfilename & "'"
                Dim obj As New clsCasesSearch
                Dim dt As New DataTable
                dt = obj.ExecuteMyQuery(query)

                For i = 0 To dt.Rows.Count - 1

                    If dt.Rows(i).Item(2).ToString().Trim() <> "" Then
                        strsubjectindex.Add(dt.Rows(i).Item(1) & " - " & dt.Rows(i).Item(2))
                    Else
                        strsubjectindex.Add(dt.Rows(i).Item(1))
                    End If

                Next

                If strsubjectindex.Count > 0 Then
                    Return String.Join(" / ", strsubjectindex.ToArray())
                End If
                Return ""

            Catch ex As Exception
                Return ""
            End Try
        End Function

        Function Select100Words(ByVal JGFile As String, ByVal SW As String, ByVal SByWords As String) As String
            If SW <> "" Then
            Else
                Return ""
            End If
            If s_searchNew <> "" Then
                s_searchNew = objUtil.RefineSentence(s_searchNew)
                SByWords = SByWords & " " & s_searchNew
            End If
            'SW = objUtil.RefineSentence(SW)
            'My.Computer.FileSystem.WriteAllText("C:\Elaw\AliAli\log.txt", SW & vbCrLf, True)
            If File.Exists(Server.MapPath("~\xmlfiles\cases\" & JGFile)) = False Then
                Return ""
            End If
            Dim JudgementBody As String
            Dim AllCase As New StringBuilder
            Dim StrWordHighlight As String
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & JGFile))
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "HEADNOTE" Then
                                AllCase.Append(xRead.ReadInnerXml)
                            ElseIf xRead.Name = "JUDGMENT" Then
                                AllCase.Append(xRead.ReadInnerXml)
                            End If
                    End Select
                End While
                JudgementBody = Regex.Replace(AllCase.ToString, "<.*?>", " ")
                Dim clsWordHighlight As New WordHighlight(JudgementBody, SW, SByWords)
                StrWordHighlight = clsWordHighlight.GetHighlight()
                clsWordHighlight = Nothing
                Return StrWordHighlight
            Catch ex As Exception
                Return ""
            End Try
            Return StrWordHighlight
        End Function


        <System.Web.Services.WebMethod()> _
        Public Shared Function getToolTip(ByVal SecReceiv As String) As String
            Dim UrlDecrpt As New Dba_UrlEncrption(SecReceiv, False)
            Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
            Dim xRead As XmlTextReader = New XmlTextReader(HttpContext.Current.Server.MapPath("~\xmlfiles\cases\" & UrlQuery & ".xml"))
            Dim TempHeadNote As String = ""
            Dim tempJud As String = ""
            Dim endTagStartPosition As Integer = 0
            Dim TempJudment As String = ""
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT" Then
                            tempJud = xRead.ReadInnerXml
                            TempJudment = tempJud
                        End If
                End Select
            End While
            'If TempHeadNote.Length > 700 Then
            '    TempHeadNote = TempHeadNote.Substring(0, 700)
            '    TempHeadNote = Regex.Replace(TempHeadNote, "</?(a|A|LINK).*?>", "")
            '    endTagStartPosition = TempHeadNote.LastIndexOf("</")
            '    ' Remove the identified section if it is valid. 
            '    If endTagStartPosition >= 0 Then
            '        TempHeadNote = TempHeadNote.Substring(0, endTagStartPosition)
            '        TempHeadNote = TempHeadNote.Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("</b>", "")
            '    End If
            'Else
            '    TempHeadNote = Regex.Replace(TempHeadNote, "</?(a|A|LINK).*?>", "")
            '    TempHeadNote = TempHeadNote.Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("</b>", "")
            'End If
            'endTagStartPosition = 0

            If tempJud.Length > 800 Then
                tempJud = Regex.Replace(tempJud.ToString, "<.*?>", " ")
                tempJud = tempJud.Substring(0, 800)
                tempJud = Regex.Replace(tempJud, "</?(a|A|LINK).*?>", "")
                'endTagStartPosition = tempJud.LastIndexOf("</")
                'If endTagStartPosition >= 0 Then
                ' tempJud = tempJud.Substring(0, endTagStartPosition)
            Else
                tempJud = Regex.Replace(tempJud.ToString, "<.*?>", " ")
                tempJud = Regex.Replace(tempJud, "</?(a|A|LINK).*?>", "")
            End If
            'tempJud = tempJud.Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("</b>", "")
            'End If
            'If tempJud.Length < 300 Then
            '    tempJud = TempJudment
            '    tempJud = tempJud.Substring(0, 1500)
            '    tempJud = Regex.Replace(tempJud, "</?(a|A|LINK).*?>", "")
            '    endTagStartPosition = tempJud.LastIndexOf("</")
            '    If endTagStartPosition >= 0 Then
            '        tempJud = tempJud.Substring(0, endTagStartPosition)
            '    End If
            '    tempJud = tempJud.Replace("<p>", "").Replace("</P>", "").Replace("<b>", "").Replace("</b>", "")
            'End If
            Dim StrToolTip As String = tempJud
            Return StrToolTip
        End Function

    End Class

End Namespace
