
'/******************************************************************************/
'/*	Developer 	    : Modify By Mohammed         								   */
'/*	Company     	: The Digital Library Sdn. Bhd.		    	    				        */
'/* Date Modified	: 3/1/2014		        		    			     */  
'/*	Description		: practice notes search  result page                                */
'/*	Version			: 1.0											            */
'/*******************************************************************************/

Imports System
Imports System.Data
Imports Dba_UrlEncrption

Namespace membersarea

    Partial Class PracticeNotesSearchresult
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

        Dim PageCurrent As Int16
        Dim UserName As String
        Dim s_searchFTS As String
        Dim SearchNotW As String
        Dim proximity As Integer
        Dim proxmityWinthin As Integer
        Dim SearchProximityCollection As String
        Dim S_searchType As Byte
        Dim s_searchNew As String
        Dim S_SortBy As String
        Dim S_RecordCount As Int32
        Dim TitleSortIndex As Byte
        Dim SearchField As String
        Dim objUtil As New clsMyUtility

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Page.SmartNavigation = True
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
'                Server.Transfer("login.aspx")
 Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If


            '===========Received Data================
            Dim EncrptUrl As String
            If Request.QueryString("info") <> "" Then
                EncrptUrl = Request.QueryString("info")
                Dim UrlDecrpt As New Dba_UrlEncrption(EncrptUrl, False)
                Dim UrlQuery As New List(Of urlbreaker)
                UrlQuery = UrlDecrpt.UrlDetails
                If UrlQuery.Count > 0 Then

                    For i = 0 To UrlQuery.Count - 1
                        If UrlQuery(i).exist = True Then
                            Select Case UrlQuery(i).parvar
                                Case "ft"
                                    s_searchFTS = UrlQuery(i).parvalue
                                Case "nw"
                                    SearchNotW = UrlQuery(i).parvalue
                                    SearchNotW = objUtil.RefineSentence(SearchNotW)
                                Case "ppl"
                                    proximity = CInt(CStr(UrlQuery(i).parvalue))
                                Case "pplw"
                                    proxmityWinthin = CInt(CStr(UrlQuery(i).parvalue))
                                Case "ns"
                                    s_searchNew = UrlQuery(i).parvalue
                                Case "srt"
                                    S_SortBy = UrlQuery(i).parvalue
                                Case "Stype"
                                    S_searchType = UrlQuery(i).parvalue
                                    If S_searchType = 1 Then
                                        SearchField = "BOOLEANTEXT"
                                    ElseIf S_searchType = 2 Then
                                        SearchField = "FORMTITLE"
                                    Else
                                        SearchField = "BOOLEANTEXT"
                                    End If
                                Case "page"
                                    PageCurrent = UrlQuery(i).parvalue
                                Case "rc"
                                    S_RecordCount = UrlQuery(i).parvalue
                            End Select
                        End If
                    Next
                End If
            Else

               ' Response.Redirect("~/login.aspx")
			    Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If

            If PageCurrent = 0 Then
                PageCurrent = 1
            End If


            '============== Sort Result================

            If S_SortBy = "" Then
                S_SortBy &= " order by case"
                S_SortBy &= " when " & objUtil.Contains_Parser_notDetailed(s_searchFTS, SearchField) & " then '1' "
                S_SortBy &= " else '2' end "
                TitleSortIndex = 2
            Else
                Select Case S_SortBy
                    Case 0
                        S_SortBy = " order by title ASC "
                        TitleSortIndex = 0

                    Case 1
                        S_SortBy = " order by title desc "
                        TitleSortIndex = 1

                    Case Else
                        S_SortBy = " order by title ASC "

                End Select
            End If
            If Me.IsPostBack = False Then
                lblTopNavigation.EnableViewState = False
                lblTopNavigation.EnableViewState = False
                lblTbl.EnableViewState = False
                lblLegislationFound2.EnableViewState = False
                lblSortBy.EnableViewState = False
                btnSearch.EnableViewState = False
                lblPgNo.EnableViewState = False
                lblMsg.EnableViewState = False
                SearchWords()
                ddlTitle.SelectedIndex = TitleSortIndex
                ddlTitle.AutoPostBack = True
            End If
        End Sub

        Private Sub SearchWords()
            Dim objUtil As New clsMyUtility
            Dim Sql As String
            Sql = Me.legislation_Circuit
            myBindData(Sql)
        End Sub

        '''==========================================================================================



        '''==========================================================================================
        Private Sub myBindData(ByVal sqlQuery As String)
            Dim PageFirst As Int16
            Dim RecordCount As Int32 'totalRec or MaxRec
            Dim PageSize As Byte  'showRec
            Dim PageCount As Int16
            Dim LastPage As Int16
            Dim CurrentRecord As Int32
            Dim Counter As Int16
            Dim I, J As Integer
            Dim StartTime, EndTime, ResultTime As Int32

            Dim CurRecForFetch As Int32

            Dim DT As New DataTable
            Dim TopNavigation As New System.Text.StringBuilder
            Dim Records As New System.Text.StringBuilder
            Dim ObjCS As New clsCasesSearch
            Dim ObjInt As New clsIntelligence

            Dim DataFileName As String
            Dim Title As String
            Dim number As String
            Dim Country As String
            Dim Place As String

            Dim Issue As String
            Dim StrSQL As String
            Dim URLpage As String

            PageSize = 10 ' this ammount of rec to show

            If CurrentRecord = 0 Then
                CurrentRecord = 1
                'ElseIf CurrentRecord > 1 And PageCurrent > 1 Then
            End If
            If PageCurrent > 1 Then
                CurrentRecord = PageSize * (PageCurrent - 1) ' means pg2= 20 * (2-1) ,pg3=  30 * (3-1)
                CurrentRecord = CurrentRecord + 1 ' because 30 = 30+1 i.e CurRec = lastRec +1
            End If

            StartTime = System.DateTime.Now.Millisecond
            StrSQL = "select counting from reference_PRACTICENOTES where  " & sqlQuery
            RecordCount = S_RecordCount
            If RecordCount = 0 Then
                DT = ObjCS.ExecuteMyQuery(StrSQL)
                RecordCount = DT.Rows.Count()
                S_RecordCount = RecordCount
            End If

            EndTime = System.DateTime.Now.Millisecond
            ResultTime = (EndTime - StartTime)
            If ResultTime > 0 Then
                lblSec.Text = ResultTime / 1000
            Else
                lblSec.Text = "0.09"
            End If

            If RecordCount <= 0 Then

                lblTbl.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Match Your Search!</span></div>"
                lblSortBy.Text = ""

                lblBottomNavigation.Text = ""
                lblPgNo.Text = "0"

                lblLegislationFound2.Text = "<strong>Your search return no result!</strong>"

                btnAct.Enabled = False

                txtSearch.Enabled = False
                btnSearch.Enabled = False
                ' txtSearch1.Enabled = False
                'btnSearch1.Enabled = False

                ddlTitle.Enabled = False


                Exit Sub
            End If

            PageCount = CInt(Math.Ceiling(CDbl(RecordCount) / PageSize)) 'recordCount to double because ceiling requies it
            If RecordCount = 1 Then
                lblLegislationFound2.Text = "Search within this<strong><font color=#CC3300> " & RecordCount & " </font></strong>result"
            End If
            If RecordCount > 1 Then
                lblLegislationFound2.Text = "Search within these<strong><font color=#CC3300> " & RecordCount & " </font></strong>results"
            End If
            lblPgNo.Text = "<b>" & PageCurrent & "</b> of <b> " & PageCount & "</b>"
            StrSQL = "Select DATAFILENAME,TITLE,NUMBER, PLACE, ISSUE, COUNTRY from reference_PRACTICENOTES where  " & sqlQuery
            CurRecForFetch = 10 * (PageCurrent - 1)
            'DT = ObjCS.ExecuteMyQuery(StrSQL)
            DT = ObjCS.ExecuteMyQuery(StrSQL, "reference_PRACTICENOTES", CurRecForFetch)
            s_searchFTS = s_searchFTS.Replace(" ", "+")
            URLpage = "PracticeNotesMainDisplayed.aspx?info="

            If PageCount > 0 Then

                lblTbl.Text = ""


                If PageCurrent >= 1 Then

                    J = 0



                    'For Counter = CurrentRecord - 1 To (PageSize * PageCurrent) - 1 '' 31 to 60 or 61 to 90
                    Dim RC As Int16 = DT.Rows.Count - 1
                    For Counter = 0 To RC

                        If Counter = RecordCount Then
                            GoTo allRecDisplayed
                        End If
                        'DATAFILENAME,FORMTITLE,NUMBER,SUBJECT ,COUNTRY ,INFORCEFROM  
                        DataFileName = DT.Rows(Counter).Item(0)
                        Title = DT.Rows(Counter).Item(1) ' TYPE
                        number = DT.Rows(Counter).Item(2) ' TYPE
                        Place = DT.Rows(Counter).Item(3)
                        Issue = DT.Rows(Counter).Item(4)
                        Country = DT.Rows(Counter).Item(5)
                        Dim UrlEncrptionId As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&id=" & Regex.Replace(DataFileName, ".xml", "", RegexOptions.IgnoreCase).Trim()
                        Dim UrlLinkId As New Dba_UrlEncrption(UrlEncrptionId, True)
                        Dim linkId As String = UrlLinkId.UrlEncrypt
                        Records.Append("<div class='boxResult' onmouseout='style.backgroundColor='#ebebeb'' onmouseover='style.backgroundColor='#ffe5d6';'>")
                        Records.Append("   <div class='tTitleCase'><div class='title2Case'></div>")
                        Records.Append("<h1><br/><a href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkId) & Chr(34) & " class=" & Chr(34) & "nav3" & Chr(34) & ">" & Title & " </a></h1>")
                        Records.Append("<h2 style='color:#f47200;font-size:14px;font-weight:700'>" & StrConv(number, VbStrConv.ProperCase) & "&nbsp;&nbsp;")
                        Records.Append("" & StrConv(Place, VbStrConv.ProperCase) & "&nbsp;&nbsp;")
                        Records.Append("" & StrConv(Issue, VbStrConv.ProperCase) & "</h2>&nbsp;&nbsp;")
                        Records.Append("</div><div class='clear1'></div></div>")

                    Next Counter

allRecDisplayed:
                    CurrentRecord = Counter ' i.e 60 or 90 because we loop through it

                    lblTbl.Text = Records.ToString

                End If
            End If


            lblTopNavigation.Text = ""
            lblBottomNavigation.Text = ""
            Dim AT As String
            AT = "<a  href="

            URLpage = "PracticeNotesSearchresult.aspx?info="

            If PageCurrent > 1 Then
                Dim UrlEncrption1 As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & TitleSortIndex & "&rc=" & S_RecordCount & "&Stype=" & S_searchType & "&page=1"
                Dim UrlLink1 As New Dba_UrlEncrption(UrlEncrption1, True)
                Dim link1 As String = UrlLink1.UrlEncrypt
                Dim UrlEncrption As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & TitleSortIndex & "&rc=" & S_RecordCount & "&Stype=" & S_searchType & "&page=" & PageCurrent - 1
                Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                Dim link As String = UrlLink.UrlEncrypt

                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & link1) & Chr(34) & "><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a>&nbsp;&nbsp;")
                ' End If
                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & "> <img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a> &nbsp;")

            End If
            If ((PageCurrent - (PageCurrent Mod 10)) + 10) > PageCount Then
                For I = PageCurrent - (PageCurrent Mod 10) To PageCount
                    If Not I = 0 Then
                        Dim UrlEncrption As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & TitleSortIndex & "&rc=" & S_RecordCount & "&Stype=" & S_searchType & "&page=" & I
                        Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                        Dim link As String = UrlLink.UrlEncrypt

                        If I = PageCurrent Then
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & " class='currentPage'>" & I & " </a>")
                        Else
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & link) & Chr(34) & " >" & I & " </a>")
                        End If
                    End If
                Next I
            Else
                For I = PageCurrent - (PageCurrent Mod 10) To (PageCurrent - (PageCurrent Mod 10)) + 10
                    If Not I = 0 Then
                        Dim UrlEncrptionpage As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & TitleSortIndex & "&rc=" & S_RecordCount & "&Stype=" & S_searchType & "&page=" & I
                        Dim UrlLinkpage As New Dba_UrlEncrption(UrlEncrptionpage, True)
                        Dim linkpage As String = UrlLinkpage.UrlEncrypt

                        If I = PageCurrent Then
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & linkpage) & Chr(34) & " class='currentPage'>" & I & " </a>")
                        Else
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & linkpage) & Chr(34) & " >" & I & " </a>")
                        End If

                    End If

                Next I
                Dim UrlEncrptionLast As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & TitleSortIndex & "&rc=" & S_RecordCount & "&Stype=" & S_searchType & "&page=" & PageCurrent + 1
                Dim UrlLinkLast As New Dba_UrlEncrption(UrlEncrptionLast, True)
                Dim linkLast As String = UrlLinkLast.UrlEncrypt
                Dim UrlEncrptionLast1 As String = "ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & TitleSortIndex & "&rc=" & S_RecordCount & "&Stype=" & S_searchType & "&page=" & PageCount
                Dim UrlLinkLast1 As New Dba_UrlEncrption(UrlEncrptionLast1, True)
                Dim linkLast1 As String = UrlLinkLast1.UrlEncrypt
                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & linkLast) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>&nbsp;&nbsp;")

                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & linkLast1) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>")

            End If
            lblTopNavigation.Text = TopNavigation.ToString
            lblBottomNavigation.Text = TopNavigation.ToString




            DT = Nothing
            ObjCS = Nothing
            ObjInt = Nothing
            Records = Nothing
            TopNavigation = Nothing

            PageFirst = 0
            RecordCount = 0
            PageSize = 0
            PageCount = 0
            PageCurrent = 0
            CurrentRecord = 0
            Counter = 0
            I = 0
            J = 0
            StartTime = 0
            EndTime = 0
            ResultTime = 0
            StrSQL = ""
            DataFileName = ""
            Title = ""
            number = ""
            Country = ""
            Issue = ""
            Place = ""

        End Sub
        Private Function legislation_Circuit() As String
            Dim SqlForm As String
            Dim strSearchFTS As String
            Dim StrSearchallWords As String
            Dim StrSearchAnyWords As String

            Dim sendProxmitySentence As String
            Dim StrPerpareTokenizer As String
            Dim StrSearchProximity As String = ""
            Dim StrSearchThesaurusInflection As String = ""
            Dim StrSearchThesaurusInflection1 As String = ""
            Dim newSearch As String
            '------------- Get Information------------------------------------------
            strSearchFTS = s_searchFTS
            StrSearchallWords = s_searchFTS
            StrSearchAnyWords = s_searchFTS
            newSearch = s_searchNew
            '------------------------------------------------------------------------
            '---------------- Prepare Query------------------------------------------
            If SearchNotW <> "" Then
                SearchNotW = objUtil.Contains_Parser_notDetailed(SearchNotW, SearchField, "or") '' for not
            End If
            If strSearchFTS <> "" Then
                strSearchFTS = objUtil.Contains_Parser_notDetailed(strSearchFTS, SearchField)
                StrSearchallWords = objUtil.Contains_Parser_notDetailed(s_searchFTS, SearchField, "and")
                StrSearchAnyWords = objUtil.Contains_Parser_notDetailed(s_searchFTS, SearchField, "or")
            End If
            If SearchNotW <> "" And strSearchFTS <> "" Then
                SqlForm = " (( " & strSearchFTS & " ) or ( " & StrSearchallWords & " ) or (" & StrSearchAnyWords & ") ) and NOT(" & SearchNotW & " )"
            End If
            If SearchNotW = "" And strSearchFTS <> "" Then
                SqlForm = "( ( " & strSearchFTS & " ) or ( " & StrSearchallWords & " ) or  ( " & StrSearchAnyWords & " ) )"
            End If
            '------------------------------------------------------------------------
            If s_searchFTS <> "" And S_searchType = 1 Then

                Dim count1 As Integer = CountWords(s_searchFTS)
                If count1 >= 2 Then
                    sendProxmitySentence = objUtil.RefineSentence(s_searchFTS)
                    StrPerpareTokenizer = objUtil.PerpareTokenizer(sendProxmitySentence)
                    If proximity = 1 Then ' sentence
                        StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, 100, SearchField)
                    ElseIf (proximity = 2) Then 'paragraph
                        StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, 300, SearchField)
                    ElseIf (proximity = 3) Then 'page
                        StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, 1000, SearchField)
                    ElseIf (proximity = 4) Then 'page
                        StrSearchProximity = objUtil.Contains_Parser_proximity(StrPerpareTokenizer, proxmityWinthin, SearchField)
                    End If
                    StrSearchThesaurusInflection = objUtil.Contains_Parser_Proximity_Inflection(StrPerpareTokenizer, SearchField)
                    StrSearchThesaurusInflection1 = objUtil.Contains_Parser_Proximity_Thesaurus(StrPerpareTokenizer, SearchField)

                End If
            End If
            '-----------------------New Search--------------------------
            If s_searchNew <> "" Then
                newSearch = objUtil.Contains_Parser_notDetailed(Trim(s_searchNew), "BOOLEANTEXT", "and")
                SqlForm &= " and ( " & newSearch & " )"
            End If
            '---------------Proximity-----------------------------------
            If StrSearchProximity <> "" Then
                If SqlForm = "" Then
                    SqlForm &= " ( " & StrSearchProximity & " )"
                Else
                    SqlForm &= " or ( " & StrSearchProximity & " )"
                End If
            End If
            '--------------------------------------------------------
            ''------------------------------------
            '/// inflection and thesaurus
            If StrSearchThesaurusInflection <> "" Then
                If SqlForm = "" Then
                    SqlForm &= " ( " & StrSearchThesaurusInflection & " ) and  (" & StrSearchThesaurusInflection1 & ")"
                Else
                    SqlForm &= " and ( " & StrSearchThesaurusInflection & " ) and  (" & StrSearchThesaurusInflection1 & ")"
                End If
            End If
            If S_SortBy <> "" Then
                SqlForm &= S_SortBy
            End If
            strSearchFTS = ""
            newSearch = ""
            Return SqlForm
        End Function
        Public Function CountWords(ByVal value As String) As Integer
            ' Count matches.
            Dim collection As MatchCollection = Regex.Matches(value, "\S+")
            Return collection.Count
        End Function
        Private Sub ddlTitle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTitle.SelectedIndexChanged
            Dim UrlPage As String
            S_SortBy = ddlTitle.SelectedIndex
            UrlPage = "PracticeNotesSearchresult.aspx?ft=" & s_searchFTS & "&ns=" & s_searchNew & "&srt=" & S_SortBy & "&rc=" & S_RecordCount & "&Stype=" & S_searchType
            UrlPage = Server.UrlPathEncode(UrlPage)
            Server.Transfer(UrlPage)


        End Sub
        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

            NewSearch()
        End Sub

        Private Sub NewSearch()
            Dim UrlPage As String
            If txtSearch.Text = "" Then
                lblMsg.Text = "Please insert keywords for searching."
                Exit Sub
            End If
            If Len(Trim(txtSearch.Text)) <> 0 Then
                s_searchNew = Trim(txtSearch.Text)
            End If
            UrlPage = "PracticeNotesSearchresult.aspx?ft=" & s_searchFTS & "&t=&an=&ns=" & s_searchNew
            UrlPage = Server.UrlPathEncode(UrlPage)
            Server.Transfer(UrlPage)

        End Sub
        Private Sub newSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newSearchButton.Click
            Dim s As String = ""
            s = Trim(newSearchText.Text)
            Dim URLpage As String = ""
            If s <> "" Then

                URLpage = "PracticeNotesSearchresult.aspx?ft=" & s & "&Stype=1"
                Response.Redirect(URLpage)
            Else
                newSearchText.Attributes.Add("placeholder", "Please insert keywords")
            End If
        End Sub
        Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged

            NewSearch()
        End Sub


    End Class

End Namespace
