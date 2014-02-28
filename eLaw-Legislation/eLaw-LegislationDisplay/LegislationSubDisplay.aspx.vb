Imports System.Data
Imports Dba_UrlEncrption

Namespace membersarea

    Partial Class LegislationSubDisplay1
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

        Dim UserName As String
        Dim MinActNumber As String
        Dim MaxActNumber As String
        Dim ActNo As String
        Dim LegType As String
        Dim SortType As String
        Dim Country As String
        Dim loadResult As Integer = 0
        Dim SortedBy As String
        Dim LoadJqueryRequest As String
        Dim GetSortby As String = "0"
        Dim thisPageUrl As String = ""
        Dim PageCurrent As Int16
        Dim RecordCount As Int32 'totalRec or MaxRec
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            UserName = CType(Session("UserName"), String)
            thisPageUrl = Request.Url.ToString()
            If UserName = "" Then
               ' Server.Transfer("login.aspx")
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
                    '"t=s&no=" & ActNo & "&ctry=" & Country & "&DivId=1"
                    For i = 0 To UrlQuery.Count - 1
                        If UrlQuery(i).exist = True Then
                            Select Case UrlQuery(i).parvar
                                Case "t"
                                    LegType = UrlQuery(i).parvalue
                                Case "no"
                                    ActNo = UrlQuery(i).parvalue
                                    ActNo = ActNo.Replace(" ", "_")
                                Case "ctry"
                                    Country = UrlQuery(i).parvalue
                                Case "divid"
                                    loadResult = UrlQuery(i).parvalue
                                Case "srtp"
                                    GetSortby = UrlQuery(i).parvalue
                                Case "Page"
                                    PageCurrent = UrlQuery(i).parvalue
                                Case "rc"
                                    RecordCount = UrlQuery(i).parvalue
                            End Select
                        End If
                    Next
                End If
            Else

                'Response.Redirect("~/login.aspx")
				 Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If

            '========================================






            loadResult = CInt(Request.QueryString("DivId"))
            If Request.QueryString("DivId") = "" Then
                loadResult = 0
            Else
                loadResult = CInt(Request.QueryString("DivId"))
            End If

            If Request.QueryString("srtp") <> "" Then



                If GetSortby <> "" Then
                    If GetSortby = "1" Then

                        SortedBy = " order by TITLE DESC"
                    ElseIf GetSortby = "2" Then

                        SortedBy = " order by NUMBER"
                    ElseIf GetSortby = "3" Then

                        SortedBy = "order by (select top 1 DATAFILENAME from LEGISLATION where datafilename like '%PUAS%') asc"
                    ElseIf GetSortby = "4" Then

                        SortedBy = " order by (select top 1 DATAFILENAME from LEGISLATION where DATAFILENAME like '%PUBS%') asc"
                    ElseIf GetSortby = "6" Then

                        SortedBy = " order by NUMBER desc"
                    Else

                    End If


                Else

                End If
            End If
            '==================================

            If Request.QueryString("j") <> "" Then
                LoadJqueryRequest = Request.QueryString("j")
            Else

            End If

            If LoadJqueryRequest <> "sub" Then
                'to display all resutl including paging
                lblTb1Jquery.Visible = False
                GetLegislationByCharSort(LegType, SortType)

            ElseIf LoadJqueryRequest = "sub" Then
                'To display first 10 resutl 
                GetLegislationByCharSortJquery(LegType, SortType)
            Else
                Server.Transfer("login.aspx")
            End If
            If Me.IsPostBack = False Then
                fill_TitleSort()
            End If
        End Sub


        Private Sub GetLegislationByCharSort(ByVal Legtype As String, ByVal SortType As String)
            Dim PageFirst As Int16

            Dim PageSize As Byte  'showRec
            Dim PageCount As Int16

            Dim LastPage As Int16
            Dim CurrentRecord As Int32
            Dim FirstRecord As Byte = 1
            Dim FieldCount As Byte = 4
            Dim Counter As Int16
            Dim URLpage As String


            Dim strSQL As String
            Dim RecordsShown As Int32
            Dim LoopRecordCount As Int32
            Dim pageNum As Int16
            Dim TopNavigation As New System.Text.StringBuilder()
            Dim BottomNavigation As New System.Text.StringBuilder()
            Dim Records As New System.Text.StringBuilder()
            Dim objCasesDb As New clsCasesSearch()
            Dim SqlQuery As String

            Dim markShowPage As Int16
            Dim I, J As Integer
            Dim DT As New DataTable()
            Dim LegislationType As String

            Dim CountryQuery As String
            Dim ObjUtil As New clsMyUtility()
            Dim FileName As String = ""
            PageSize = 10 ' this ammount of rec to show
            If Legtype = "a" Then
                LegislationType = " datafilename LIKE '%MY_AMEN%' and  PRINCIPALACTNO like'" & ActNo & "' AND COUNTRY='" & Country & "'"
            ElseIf Legtype = "s" Then
                LegislationType = " (datafilename "
                LegislationType &= " LIKE '%MY_PUBS%' or datafilename "
                LegislationType &= " LIKE '%MY_PUAS%') and PRINCIPALACTNO  like'" & ActNo & "' AND COUNTRY='" & Country & "'"
            End If
            If PageCurrent = 0 Then
                PageCurrent = 1
            End If
            
            If CurrentRecord = 0 Then
                CurrentRecord = 1

            End If
            If PageCurrent > 1 Then
                CurrentRecord = PageSize * (PageCurrent - 1) ' means pg2= 20 * (2-1) ,pg3=  30 * (3-1)
                CurrentRecord = CurrentRecord + 1 ' because 30 = 30+1 i.e CurRec = lastRec +1
            End If
            SqlQuery = LegislationType & SortedBy
            strSQL = "select counting from LEGISLATION where  " & SqlQuery
            DT = objCasesDb.ExecuteMyQuery(strSQL)
            RecordCount = DT.Rows.Count()
            PageCount = CInt(Math.Ceiling(CDbl(RecordCount) / PageSize)) 'recordCount to double because ceiling requies it
            If PageCount > 100 Then
                LastPage = 100 ' because of business logic
            Else
                LastPage = PageCount


            End If
            If PageCurrent < 1 Then
                PageCurrent = 1
            End If
            If PageCurrent >= PageCount Then
                PageCurrent = LastPage 'PageCount
                'Exit Sub

            End If

            lblLegislationFound1.Text = RecordCount

            lblPgNo.Text = "<b>" & PageCurrent & "</b> of <b> " & PageCount & "</b>"
            If PageCurrent > PageCount Then PageCurrent = PageCount
            ' Lesser than firstPage than first page
            If PageCurrent < 1 Then PageCurrent = 1



            If RecordCount <= 0 Then
                lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
                'lblTopNavigation.Text = ""
                lblBottomNavigation.Text = ""
                lblTb.Text = ""
                btnSearch.Enabled = False
                lblPgNo.Text = ""
                Exit Sub
            End If
            Dim CurRecForFetch As Int32
            strSQL = "Select DATAFILENAME from LEGISLATION where  " & SqlQuery

            CurRecForFetch = 10 * (PageCurrent - 1)
            'DT = ObjCS.ExecuteMyQuery(StrSQL)
            DT = objCasesDb.ExecuteMyQuery(strSQL, "Legislation", CurRecForFetch)

            Dim FileCond As String = ""
            If DT.Rows.Count > 0 Then
                FileCond = "DATAFILENAME='" & DT.Rows(0).Item(0) & "'"
                For J = 1 To DT.Rows.Count - 1
                    FileCond &= " or DATAFILENAME='" & DT.Rows(J).Item(0) & "'"
                Next
            End If

            strSQL = "select DATAFILENAME,TITLE,NUMBER,SUBJECT ,COUNTRY ,INFORCEFROM ,PREAMBLE from LEGISLATION where " & FileCond
            DT = objCasesDb.ExecuteMyQuery(strSQL)


            Dim StartTime, EndTime, ResultTime As Int32
            EndTime = System.DateTime.Now.Millisecond
            ResultTime = (EndTime - StartTime)
            If ResultTime > 0 Then
                lblSec.Text = ResultTime / 1000
            Else
                lblSec.Text = "0.09"
            End If
            If PageCount > 0 Then
                If PageCurrent >= 1 Then

                    J = 0

                    Dim RC As Int16 = DT.Rows.Count - 1
                    For Counter = 0 To RC
                        FileName = DT.Rows(Counter).Item(0)
                        Dim IdLink As String = FileName.Replace(".xml", "")
                        Dim UrlFIleID As New Dba_UrlEncrption(IdLink.TrimEnd(), True)
                        Dim linkFileId As String = UrlFIleID.UrlEncrypt

                        If Counter = RecordCount Then
                            GoTo allRecDisplayed
                        End If


                        If J = 1 Then


                            Records.Append(" <div class='boxResult'>")
                            Records.Append("<div class='boxCheck1' id ='divPreamle" & Counter & "' onclick='show_legis_preamble(" & Counter & ")'><img src='img/arrowEx1.png'/></div>")
                            Records.Append("<div class='resultTitleCase' style='width:90%;'>")
                            Records.Append("<p class='tTitleCase'>&nbsp;<a href='LegislationMainDisplayed.aspx?id=" & linkFileId & "'>" & DT.Rows(Counter).Item(1) & "</a></p>")
                            Records.Append("<p class='SDclear'  id ='ShowPreamle" & Counter & "' style='display:none;'> Date of Preamble:" & "<br/>" & Regex.Replace(DT.Rows(Counter).Item(6), "<[^>]*>", "") & "</p>")
                            Records.Append("<p class='title2Case'> <span style='color:#f47200;font-size:14px;font-weight:700'>&nbsp;" & DT.Rows(Counter).Item(2) & "&nbsp;Subject;&nbsp;" & DT.Rows(Counter).Item(3) & " </span></p>")
                            Records.Append("<p class='tItalic'>&nbsp;" & DT.Rows(Counter).Item(5) & "</p></div>")
                            Records.Append(" <div class='clear'></div>")
                            Records.Append(" </div>")

                        End If

                        If J = 0 Then
                            Records.Append(" <div class='boxResult'>")
                            Records.Append("<div class='boxCheck1' id ='divPreamle" & Counter & "' onclick='show_legis_preamble(" & Counter & ")'><img src='img/arrowEx1.png'/></div>")
                            Records.Append("<div class='resultTitleCase' style='width:90%;'>")
                            Records.Append("<p class='tTitleCase'>&nbsp;<a href='LegislationMainDisplayed.aspx?id=" & linkFileId & "'>" & DT.Rows(Counter).Item(1) & "</a></p>")
                            Records.Append("<p class='SDclear'  id ='ShowPreamle" & Counter & "' style='display:none;'> Date of Preamble:" & "<br/>" & Regex.Replace(DT.Rows(Counter).Item(6), "<[^>]*>", "") & "</p>")
                            Records.Append("<p class='title2Case'> <span style='color:#f47200;font-size:14px;font-weight:700'>&nbsp;" & DT.Rows(Counter).Item(2) & "&nbsp;Subject;&nbsp;" & DT.Rows(Counter).Item(3) & " </span></p>")
                            Records.Append("<p class='tItalic'>&nbsp;" & DT.Rows(Counter).Item(5) & "</p></div>")
                            Records.Append(" <div class='clear'></div>")
                            Records.Append(" </div>")


                        End If

                        J = J + 1
                        If J = 2 Then
                            J = 0
                        End If

                    Next Counter

allRecDisplayed:
                    CurrentRecord = Counter ' i.e 60 or 90 because we loop through it
                    Records.Append("</table>")
                    lblTb.Text = Records.ToString

                End If



            End If


            'lblTopNavigation.Text = ""
            lblBottomNavigation.Text = ""


            URLpage = "legislationSubDisplay.aspx?info="

            If PageCount > 1 Then

                If PageCurrent > 1 Then
                    Dim IdLinkPage As String = "DivId=" & loadResult & "&no=" & ActNo & "&t=" & Legtype & "&ctry=" & Country & "&srtp=" & 1 & "&Page=1" 'SortType
                    Dim UrlFIlePage As New Dba_UrlEncrption(IdLinkPage.TrimEnd(), True)
                    Dim linkFilepabe As String = UrlFIlePage.UrlEncrypt
                    Dim IdLinkPage1 As String = "DivId=" & loadResult & "&no=" & ActNo & "&t=" & Legtype & "&ctry=" & Country & "&srtp=" & 1 & "&Page=" & PageCurrent - 1
                    Dim UrlFIlePage1 As New Dba_UrlEncrption(IdLinkPage1.TrimEnd(), True)
                    Dim linkFilepabe1 As String = UrlFIlePage1.UrlEncrypt

                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabe) & Chr(34) & "><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a>&nbsp;&nbsp;")
                    ' End If
                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabe1) & Chr(34) & "> <img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a> &nbsp;")

                End If
                If ((PageCurrent - (PageCurrent Mod 10)) + 10) > PageCount Then
                    For I = PageCurrent - (PageCurrent Mod 10) To PageCount
                        If Not I = 0 Then
                            Dim IdLinkPage1 As String = "DivId=" & loadResult & "&no=" & ActNo & "&t=" & Legtype & "&ctry=" & Country & "&srtp=" & 1 & "&Page=" & I
                            Dim UrlFIlePage1 As New Dba_UrlEncrption(IdLinkPage1.TrimEnd(), True)
                            Dim linkFilepabe1 As String = UrlFIlePage1.UrlEncrypt

                            If I = PageCurrent Then
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabe1) & Chr(34) & " class='currentPage'>" & I & " </a>")
                            Else
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabe1) & Chr(34) & " >" & I & " </a>")
                            End If
                        End If
                    Next I
                Else
                    For I = PageCurrent - (PageCurrent Mod 10) To (PageCurrent - (PageCurrent Mod 10)) + 10
                        If Not I = 0 Then
                            Dim IdLinkPagei As String = "DivId=" & loadResult & "&no=" & ActNo & "&t=" & Legtype & "&ctry=" & Country & "&srtp=" & 1 & "&Page=" & I
                            Dim UrlFIlePagei As New Dba_UrlEncrption(IdLinkPagei.TrimEnd(), True)
                            Dim linkFilepabei As String = UrlFIlePagei.UrlEncrypt

                            If I = PageCurrent Then
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabei) & Chr(34) & " class='currentPage'>" & I & " </a>")
                            Else
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabei) & Chr(34) & " >" & I & " </a>")
                            End If

                        End If

                    Next I

                    Dim IdLinkPageLast As String = "DivId=" & loadResult & "&no=" & ActNo & "&t=" & Legtype & "&ctry=" & Country & "&srtp=" & 1 & "&Page=" & PageCurrent + 1
                    Dim UrlFIlePageLast As New Dba_UrlEncrption(IdLinkPageLast.TrimEnd(), True)
                    Dim linkFilepabeLast As String = UrlFIlePageLast.UrlEncrypt
                    Dim IdLinkPageLast1 As String = "DivId=" & loadResult & "&no=" & ActNo & "&t=" & Legtype & "&ctry=" & Country & "&srtp=" & 1 & "&Page=" & PageCount
                    Dim UrlFIlePageLast1 As New Dba_UrlEncrption(IdLinkPageLast1.TrimEnd(), True)
                    Dim linkFilepabeLast1 As String = UrlFIlePageLast1.UrlEncrypt

                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabeLast) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>&nbsp;&nbsp;")

                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & linkFilepabeLast1) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>")

                End If


            End If

            'If PageCount > 1 Then
            '    'lblTopNavigation.Text &= "<font color=#506a96 size=2> <a href=casessearchresult4.aspx?page=1 >First</a>&nbsp;&nbsp;"
            '    'lblTopNavigation.Text &= "<a href=casessearchresult4.aspx?page=" & PageCurrent - 1 & " >Prev</a> </font>&nbsp;&nbsp;"
            '    'lblTopNavigation.Text &= "<font color=#506a96 size=1>"


            '    If PageCurrent > 1 Then


            '        TopNavigation.Append("<font size=1 face=Verdana color=" & Chr(34) & "#000000" & Chr(34) & " >" & URLpage & "1 >First</a>&nbsp;&nbsp;")
            '        TopNavigation.Append(URLpage & PageCurrent - 1 & " >Prev</a> </font>&nbsp;&nbsp;")


            '        If PageSize <= PageCount Then
            '            TopNavigation.Append("<font size=1 face=Verdana color=#000000>" & URLpage & PageCurrent - PageSize & " >Prev Set</a>&nbsp;&nbsp;")

            '        End If





            '    End If

            '    TopNavigation.Append("<font size=2 face=Verdana color=" & Chr(34) & "#000000" & Chr(34) & " >")

            '    If PageCount <= 10 Then
            '        For I = 1 To PageCount

            '            TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '            TopNavigation.Append("&nbsp;&nbsp;")


            '        Next I
            '    End If

            '    If PageCount > 10 Then
            '        'If PageCurrent < 20 Then
            '        '    For I = 1 To 20
            '        '        lblTopNavigation.Text &= "<a href=casessearchresult2.aspx?page=" & I & " >" & I & " </a>"
            '        '        lblTopNavigation.Text &= "&nbsp;&nbsp;"
            '        '    Next I
            '        'End If

            '        If (PageCurrent >= 1 And PageCurrent < 10) And PageCount < 10 Then
            '            For I = 1 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 1 And PageCurrent < 10) And PageCount >= 10 Then '' eg: curPg=30 pgCount=50
            '            For I = 1 To 10
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 10 And PageCurrent < 20) And PageCount < 20 Then
            '            For I = 10 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")
            '            Next I
            '        End If

            '        If (PageCurrent >= 10 And PageCurrent < 20) And PageCount >= 20 Then '' eg: curPg=30 pgCount=50
            '            For I = 10 To 20
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 20 And PageCurrent < 30) And PageCount < 30 Then '' eg: curPg=30 pgCount=50
            '            For I = 20 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 20 And PageCurrent < 30) And PageCount >= 30 Then '' eg: curPg=30 pgCount=50
            '            For I = 20 To 30
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If


            '        If (PageCurrent >= 30 And PageCurrent < 40) And PageCount < 40 Then '' eg: curPg=30 pgCount=50
            '            For I = 30 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 30 And PageCurrent < 40) And PageCount >= 40 Then '' eg: curPg=30 pgCount=50
            '            For I = 30 To 40
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If


            '        If (PageCurrent >= 40 And PageCurrent < 50) And PageCount < 50 Then '' eg: curPg=30 pgCount=50
            '            For I = 40 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I

            '        End If

            '        If (PageCurrent >= 40 And PageCurrent < 50) And PageCount >= 50 Then '' eg: curPg=30 pgCount=50
            '            For I = 40 To 50
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I

            '        End If

            '        If (PageCurrent >= 50 And PageCurrent < 60) And PageCount < 60 Then '' eg: curPg=30 pgCount=50
            '            For I = 50 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 50 And PageCurrent < 60) And PageCount >= 60 Then '' eg: curPg=30 pgCount=50
            '            For I = 50 To 60
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If


            '        If (PageCurrent >= 60 And PageCurrent < 70) And PageCount < 70 Then '' eg: curPg=30 pgCount=50
            '            For I = 60 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 60 And PageCurrent < 70) And PageCount >= 70 Then '' eg: curPg=30 pgCount=50
            '            For I = 60 To 70
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 70 And PageCurrent < 80) And PageCount < 80 Then '' eg: curPg=30 pgCount=50
            '            For I = 70 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 70 And PageCurrent < 80) And PageCount >= 80 Then '' eg: curPg=30 pgCount=50
            '            For I = 70 To 80
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 80 And PageCurrent < 90) And PageCount < 90 Then '' eg: curPg=30 pgCount=50
            '            For I = 80 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If

            '        If (PageCurrent >= 80 And PageCurrent < 90) And PageCount >= 90 Then '' eg: curPg=30 pgCount=50
            '            For I = 80 To 90
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '        End If
            '        If (PageCurrent >= 90 And PageCurrent < 100) And PageCount < 100 Then '' eg: curPg=30 pgCount=50
            '            For I = 90 To PageCount
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")


            '            Next I
            '            TopNavigation.Append("</br> <b>These are the records for 100 pages, to reduce</b>")
            '        End If

            '        If (PageCurrent >= 90 And PageCurrent < 100) And PageCount >= 100 Then '' eg: curPg=30 pgCount=50
            '            For I = 90 To 100
            '                TopNavigation.Append(URLpage & I & " >" & I & " </a>")
            '                TopNavigation.Append("&nbsp;&nbsp;")

            '            Next I
            '            TopNavigation.Append("</br> <b>These are the records for 100 pages, to reduce</b>")
            '        End If

            '    End If





            'End If
            'TopNavigation.Append("</font>")



            'If PageCount > 1 Then
            '    TopNavigation.Append("<font  size=1> " & URLpage & PageCurrent + 1 & " >Next</a>&nbsp;&nbsp;")
            '    TopNavigation.Append(URLpage & LastPage & " >Last</a> </font>&nbsp;&nbsp;")

            '    If PageSize <= PageCount Then
            '        TopNavigation.Append("<font size=1 face=Verdana color=#000000>" & URLpage & PageSize + PageCurrent & " >Next Set</a>&nbsp;&nbsp;")

            '    End If

            'End If

            'lblTopNavigation.Text = TopNavigation.ToString
            lblBottomNavigation.Text = TopNavigation.ToString



            DT = Nothing
            PageFirst = 0
            RecordCount = 0
            PageSize = 0
            PageCount = 0
            PageCurrent = 0
            CurrentRecord = 0
            FirstRecord = 0
            FieldCount = 0
            Counter = 0

            strSQL = ""
            RecordsShown = 0
            LoopRecordCount = 0
            pageNum = 0

            markShowPage = 0
            I = 0
            J = 0

        End Sub
        Private Sub GetLegislationByCharSortJquery(ByVal Legtype As String, ByVal SortType As String)
            Dim PageFirst As Int16
            Dim RecordCount As Int32 'totalRec or MaxRec
            Dim PageSize As Byte  'showRec
            Dim PageCount As Int16
            Dim PageCurrent As Int16
            Dim LastPage As Int16
            Dim CurrentRecord As Int32
            Dim FirstRecord As Byte = 1
            Dim FieldCount As Byte = 4
            Dim Counter As Int16
            Dim URLpage As String


            Dim strSQL As String
            Dim RecordsShown As Int32
            Dim LoopRecordCount As Int32
            Dim pageNum As Int16
            Dim TopNavigation As New System.Text.StringBuilder()
            Dim BottomNavigation As New System.Text.StringBuilder()
            Dim Records As New System.Text.StringBuilder()
            Dim objCasesDb As New clsCasesSearch()
            Dim SqlQuery As String

            Dim markShowPage As Int16
            Dim I, J As Integer
            Dim DT As New DataTable()
            Dim LegislationType As String
            Dim SortedBy As String
            Dim CountryQuery As String
            Dim ObjUtil As New clsMyUtility()




            PageSize = 10 ' this ammount of rec to show


            If Legtype = "a" Then
                LegislationType = " datafilename LIKE '%MY_AMEN%' and  PRINCIPALACTNO like'" & ActNo & "' AND COUNTRY='" & Country & "'"
            ElseIf Legtype = "s" Then
                LegislationType = " (datafilename "
                LegislationType &= " LIKE '%MY_PUBS%' or datafilename "
                LegislationType &= " LIKE '%MY_PUAS%') and PRINCIPALACTNO  like'" & ActNo & "' AND COUNTRY='" & Country & "'"
            End If

            If Request.QueryString("page") = "" Then
                PageCurrent = 1
            Else
                PageCurrent = CInt(Request.QueryString("page"))

            End If

            
            If CurrentRecord = 0 Then
                CurrentRecord = 1
                'ElseIf CurrentRecord > 1 And PageCurrent > 1 Then
            End If
            If PageCurrent > 1 Then
                CurrentRecord = PageSize * (PageCurrent - 1) ' means pg2= 20 * (2-1) ,pg3=  30 * (3-1)
                CurrentRecord = CurrentRecord + 1 ' because 30 = 30+1 i.e CurRec = lastRec +1
            End If


            If SortType = "a" Then
                SortedBy = "Order by number"

            ElseIf SortType = "ta" Then
                SortedBy = " Order by title asc"

            Else

                SortedBy = " Order by title asc"

            End If
            SqlQuery = LegislationType & SortedBy
            strSQL = "select counting from LEGISLATION where  " & SqlQuery
            DT = objCasesDb.ExecuteMyQuery(strSQL)
            RecordCount = DT.Rows.Count()
            PageCount = CInt(Math.Ceiling(CDbl(RecordCount) / PageSize)) 'recordCount to double because ceiling requies it

            If PageCount > 100 Then
                LastPage = 100 ' because of business logic
            Else
                LastPage = PageCount


            End If


            If PageCurrent < 1 Then
                PageCurrent = 1
            End If

            If PageCurrent >= PageCount Then
                PageCurrent = LastPage

            End If

            lblLegislationFound1.Text = RecordCount

            lblPgNo.Text = "<b>" & PageCurrent & "</b> of <b> " & PageCount & "</b>"
            If PageCurrent > PageCount Then PageCurrent = PageCount
            ' Lesser than firstPage than first page
            If PageCurrent < 1 Then PageCurrent = 1



            If RecordCount <= 0 Then
                lblMsg.Text = "<font color=#CC3300><strong>Sorry No Record Found in our Library!</strong></font>"
                'lblTopNavigation.Text = ""
                lblBottomNavigation.Text = ""
                lblTb.Text = ""
                'btnSearch.Enabled = False
                lblPgNo.Text = ""
                Exit Sub
            End If

            Dim CurRecForFetch As Int32
            strSQL = "Select DATAFILENAME  from LEGISLATION where  " & SqlQuery

            CurRecForFetch = 10 * (PageCurrent - 1)
            'DT = ObjCS.ExecuteMyQuery(StrSQL)
            DT = objCasesDb.ExecuteMyQuery(strSQL, "Legislation", CurRecForFetch)

            Dim FileCond As String = ""
            If DT.Rows.Count > 0 Then
                FileCond = "DATAFILENAME='" & DT.Rows(0).Item(0) & "'"
                For J = 1 To DT.Rows.Count - 1
                    FileCond &= " or DATAFILENAME='" & DT.Rows(J).Item(0) & "'"
                Next
            End If

            strSQL = "select DATAFILENAME,TITLE,NUMBER,SUBJECT ,COUNTRY ,INFORCEFROM from LEGISLATION where " & FileCond
            DT = objCasesDb.ExecuteMyQuery(strSQL)

            Dim headnotesLen As Int16
            If PageCount > 0 Then

                lblTb.Text = ""
                If PageCurrent >= 1 Then

                    J = 0

                    Dim RC As Int16 = DT.Rows.Count - 1
                    For Counter = 0 To RC
                        Dim FileName As String = DT.Rows(Counter).Item(0)
                        FileName = FileName.Replace(".xml", "").Trim()
                        If Counter = RecordCount Then
                            GoTo allRecDisplayed
                        End If

                        Dim IdLink As String = FileName.Trim()
                        Dim UrlFIleID As New Dba_UrlEncrption(IdLink.TrimEnd(), True)
                        Dim linkFileId As String = UrlFIleID.UrlEncrypt
                        If J = 1 Then

                            Records.Append("<div class='boxRecentResult'>")
                            Records.Append(" <h2 class='title2Case2' style='color:#f47200;font-size:12px;font-weight:600'><a href='LegislationMainDisplayed.aspx?id=" & linkFileId & "'>" & DT.Rows(Counter).Item(1) & "</a></h2>")
                            Records.Append("<p class='title3Case'><a href='#'><strong>" & DT.Rows(Counter).Item(2) & "</strong>&nbsp;;" & DT.Rows(Counter).Item(3) & "</a>&nbsp;")
                            Records.Append("<a style='color:#f47200; font-weight:bold;' href='#'>" & DT.Rows(Counter).Item(5) & ";")
                            Records.Append(DT.Rows(Counter).Item(4) & " </a></p>")
                            Records.Append(" </div>")

                        End If

                        If J = 0 Then

                            Records.Append("<div class='boxRecentResult'>")
                            Records.Append(" <h2 class='title2Case2' style='color:#f47200;font-size:12px;font-weight:600'><a href='LegislationMainDisplayed.aspx?id=" & linkFileId & "'>" & DT.Rows(Counter).Item(1) & "</a></h2>")
                            Records.Append("<p class='title3Case'><a href='#'><strong>" & DT.Rows(Counter).Item(2) & "</strong>&nbsp;;" & DT.Rows(Counter).Item(3) & "</a>&nbsp;")
                            Records.Append("<a style='color:#f47200; font-weight:bold;' href='#'>" & DT.Rows(Counter).Item(5) & ";")
                            Records.Append(DT.Rows(Counter).Item(4) & " </a></p>")
                            Records.Append(" </div>")
                        End If

                        J = J + 1
                        If J = 2 Then
                            J = 0
                        End If
                    Next Counter

allRecDisplayed:
                    CurrentRecord = Counter ' i.e 60 or 90 because we loop through it
                    Records.Append("</table>")
                    lblTb1Jquery.Text = Records.ToString

                End If

            End If

            lblBottomNavigation.Text = ""

            lblBottomNavigation.Text = TopNavigation.ToString
            DT = Nothing
            PageFirst = 0
            RecordCount = 0
            PageSize = 0
            PageCount = 0
            PageCurrent = 0
            CurrentRecord = 0
            FirstRecord = 0
            FieldCount = 0
            Counter = 0

            strSQL = ""
            RecordsShown = 0
            LoopRecordCount = 0
            pageNum = 0

            markShowPage = 0
            I = 0
            J = 0

        End Sub
        Private Sub GetLegislationByNumbers(ByVal MinValue As Int16, ByVal MaxValue As Int16, ByVal Legtype As String, ByVal SortType As String)
            Dim PageFirst As Int16
            Dim RecordCount As Int32 'totalRec or MaxRec
            Dim PageSize As Byte  'showRec
            Dim PageCount As Int16
            Dim PageCurrent As Int16
            Dim LastPage As Int16
            Dim CurrentRecord As Int32
            Dim FirstRecord As Byte = 1
            Dim FieldCount As Byte = 4
            Dim Counter As Int16
            Dim URLpage As String


            Dim strSQL As String
            Dim RecordsShown As Int32
            Dim LoopRecordCount As Int32
            Dim pageNum As Int16
            Dim TopNavigation As New System.Text.StringBuilder()
            Dim BottomNavigation As New System.Text.StringBuilder()
            Dim Records As New System.Text.StringBuilder()
            Dim objCasesDb As New clsCasesSearch()
            Dim SqlQuery As String

            Dim markShowPage As Int16
            Dim I, J As Integer
            Dim DT As New DataTable()
            Dim LegislationType As String
            Dim SortedBy As String
            Dim CountryQuery As String
            Dim ObjUtil As New clsMyUtility()
            Dim ObjInt As New clsIntelligence()

            Dim DataFileName As String
            Dim Title As String
            Dim Number As String
            Dim Subject As String
            Dim Country As String
            Dim InforceFrom As String



            PageSize = 10 ' this ammount of rec to show
            Legtype = Replace(Legtype, Chr(34), "")
            If Legtype = "pa" Then
                'LegislationType = " and LEGISLATIONTYPE like '%FS_ACT%' "
                LegislationType = " LEGISLATIONTYPE like '%FS_ACT%' "
            ElseIf Legtype = "aa" Then
                'LegislationType = " and LEGISLATIONTYPE like '%FS_AME%' "
                LegislationType = " LEGISLATIONTYPE like '%FS_AME%' "
            ElseIf Legtype = "bls" Then
                'LegislationType = " and LEGISLATIONTYPE like '%FS_BIL%' "
                LegislationType = " LEGISLATIONTYPE like '%FS_BIL%' "
            ElseIf Legtype = "sa" Then
                'LegislationType = " and LEGISLATIONTYPE like '%FS_ACT%' "
            End If

            If Request.QueryString("page") = "" Then
                PageCurrent = 1
            Else
                PageCurrent = CInt(Request.QueryString("page"))

            End If

            '        CurrentRecord = CType(Session("curRec"), Int32)


            If CurrentRecord = 0 Then
                CurrentRecord = 1
                'ElseIf CurrentRecord > 1 And PageCurrent > 1 Then
            End If
            If PageCurrent > 1 Then
                CurrentRecord = PageSize * (PageCurrent - 1) ' means pg2= 20 * (2-1) ,pg3=  30 * (3-1)
                CurrentRecord = CurrentRecord + 1 ' because 30 = 30+1 i.e CurRec = lastRec +1
            End If


            If SortType = "a" Then
                SortedBy = " order by number"

            ElseIf SortType = "ta" Then
                SortedBy = " order by title asc"


            Else

                SortedBy = " order by pagenumber"

            End If

            'If SelectedCountries = "All" Then
            '    ''This Condition means that all countries or fetch all type of countries
            '    If SortType <> "a" And SortType <> "ta" Then
            '        SortType = " order by country"
            '    End If
            'Else

            '    CountryQuery = " and  " & ObjUtil.CountryParser(SelectedCountries, "Country", "or")
            '    If SortType <> "a" And SortType <> "ta" Then
            '        SortType = " order by country"
            '    End If
            '    'Sql &= CountryQuery
            'End If


            '        SqlQuery = " TITLE like '" & SortBy & "%' " & LegislationType & CountryQuery & SortedBy


            'SqlQuery = " Pagenumber > " & MinValue - 1 & " and Pagenumber < " & MaxValue - 1 & LegislationType & " order by pagenumber " 'SortedBy

            'SqlQuery = LegislationType & " AND  Pagenumber > " & MinValue - 1 & " and Pagenumber < " & MaxValue - 1 & " order by pagenumber "   'SortedBy
            SqlQuery = LegislationType & " AND  Pagenumber > " & MinValue - 1 & " and Pagenumber < " & MaxValue - 1 & SortedBy

            strSQL = "select counting from LEGISLATION where  " & SqlQuery


            'strSQL = sqlQuery
            'strSQL = "select DATAFILENAME,TITLE,NUMBER,SUBJECT,COUNTRY from LEGISLATION where  " & sqlQuery


            DT = objCasesDb.ExecuteMyQuery(strSQL)

            RecordCount = DT.Rows.Count()
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
                PageCurrent = LastPage 'PageCount
                'Exit Sub

            End If

            lblLegislationFound1.Text = RecordCount

            lblPgNo.Text = "<b>" & PageCurrent & "</b> of <b> " & PageCount & "</b>"





            ' i.e If currentpage got greater than totalpages then make currentpage the total page,i.e lastPage
            If PageCurrent > PageCount Then PageCurrent = PageCount
            ' Lesser than firstPage than first page
            If PageCurrent < 1 Then PageCurrent = 1



            If RecordCount <= 0 Then
                lblMsg.Text = "Sorry No Record Found in our Library!"
                'lblTopNavigation.Text = ""
                lblBottomNavigation.Text = ""
                lblTb.Text = ""
                'btnSearch.Enabled = False
                lblPgNo.Text = ""
                Exit Sub
            End If


            '||||||||||
            '        DataGrid1.CurrentPageIndex = PageCurrent


            'Dim arrDBData(FieldCount, RecordCount) As String
            Dim CurRecForFetch As Int32
            strSQL = "Select DATAFILENAME from LEGISLATION where  " & SqlQuery


            CurRecForFetch = 10 * (PageCurrent - 1)


            'DT = ObjCS.ExecuteMyQuery(StrSQL)
            DT = objCasesDb.ExecuteMyQuery(strSQL, "Legislation", CurRecForFetch)

            Dim FileCond As String = ""
            If DT.Rows.Count > 0 Then
                FileCond = "DATAFILENAME='" & DT.Rows(0).Item(0) & "'"
                For J = 1 To DT.Rows.Count - 1
                    FileCond &= " or DATAFILENAME='" & DT.Rows(J).Item(0) & "'"
                Next
            End If

            strSQL = "select DATAFILENAME,TITLE,NUMBER,SUBJECT ,COUNTRY ,INFORCEFROM from LEGISLATION where " & FileCond
            DT = objCasesDb.ExecuteMyQuery(strSQL)



            Dim headnotesLen As Int16
            If PageCount > 0 Then

                lblTb.Text = ""




                If PageCurrent >= 1 Then

                    J = 0
                    'lblTbl.Text = "<table border=1 cellpadding=1 cellspacing=0  bordercolor=#111111 width=92% >"
                    Records.Append("<table border=0 cellpadding=1 cellspacing=1 width=100% >")

                    '                lblTbl.Text += "<tr bgColor=#506a96><td width=5%><font color=#FFFFFF size=3><b>Rec#</b></font></td>"
                    Records.Append("<tr bgColor=#465877>")
                    Records.Append("<td width=85% align=center><font color=#FFFFFF size=1><b>Legislation Description</b></font></td>")
                    Records.Append("<td width=15% align=center><font color=#FFFFFF size=1><b>Country</b></font></td></tr>")


                    '        For Counter = CurrentRecord - 1 To (PageSize * PageCurrent) - 1 '' 31 to 60 or 61 to 90
                    For Counter = 0 To DT.Rows.Count - 1
                        If Counter = RecordCount Then
                            GoTo allRecDisplayed
                        End If
                        'DATAFILENAME,TITLE,NUMBER,SUBJECT ,COUNTRY ,INFORCEFROM  
                        DataFileName = DT.Rows(Counter).Item(0)
                        Title = DT.Rows(Counter).Item(1)
                        Number = DT.Rows(Counter).Item(2)
                        Subject = DT.Rows(Counter).Item(3)
                        Country = DT.Rows(Counter).Item(4)
                        InforceFrom = DT.Rows(Counter).Item(5)


                        If J = 1 Then

                            Records.Append("<tr>")

                            Records.Append("<td width=85% bgColor=#FFFFFF></br>" & Counter + 1 & ".&nbsp;<a href=LegislationMainDisplayed.aspx?id=" & DataFileName & " class=" & Chr(34) & "nav3" & Chr(34) & ">" & Title & " </a></br>")
                            Records.Append("<font face=Verdana color=#07236 size=1> By &nbsp;<b>" & Number & "</b></br>")
                            Records.Append("<i>" & Subject & "</i></font>&nbsp;&nbsp;")
                            Records.Append("<i>" & InforceFrom & "</i>")
                            Records.Append("</font></td>")
                            Records.Append("<td width=15% bgColor=#FFFFFF align=center><b><font color=" & ObjInt.GetCountryColor(Country) & " size=2><center>" & Country & "<center></font></b></td>")
                            Records.Append("</tr>")
                        End If

                        If J = 0 Then
                            Records.Append("<tr>")

                            Records.Append("<td width=85% bgColor=#F0F8FF></br>" & Counter + 1 & ".&nbsp;<a href=LegislationMainDisplayed.aspx?id=" & DataFileName & " class=" & Chr(34) & "nav3" & Chr(34) & ">" & Title & " </a></br>")
                            Records.Append("<font face=Verdana color=#007236 size=1> By &nbsp;<b>" & Number & "</b></br>")
                            Records.Append("<i>" & Subject & "</i></font>&nbsp;&nbsp;")
                            Records.Append("<i>" & InforceFrom & "</i>")
                            Records.Append("</font></td>")
                            Records.Append("<td width=15% bgColor=#F0F8FF align=center><b><font color=" & ObjInt.GetCountryColor(Country) & " size=2><center>" & Country & "<center></font></b></td>")
                            Records.Append("</tr>")
                        End If

                        J = J + 1
                        If J = 2 Then
                            J = 0
                        End If

                        'For I = 0 To FieldCount - 1
                        '    lblTbl.Text += DS.Tables(0).Rows(Counter).Item(I)
                        'Next I
                        '                   lblTbl.Text += "</br>"

                    Next Counter

allRecDisplayed:
                    CurrentRecord = Counter ' i.e 60 or 90 because we loop through it
                    Records.Append("</table>")
                    lblTb.Text = Records.ToString

                End If



            End If


            'lblTopNavigation.Text = ""
            lblBottomNavigation.Text = ""


            'Dim LinkTemplate As String = "<a href='casessearchresult1.aspx?GenreID=" & s_searchCases.ToString & "&Page=%%%'>%%%</a"

            '        URLpage = "<a href=legislationBrowseresult.aspx?page="

            URLpage = "LegislationSubDisplay.aspx?no=" & ActNo & "&tp=" & Legtype & "&srtp=" & SortType & "&ctry=" & Country & "&Page="

            If PageCount > 1 Then

                If PageCurrent > 1 Then

                    ' If PageCurrent >= 10 Then
                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & "1") & Chr(34) & "><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a>&nbsp;&nbsp;")
                    ' End If
                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & (PageCurrent - 1)) & Chr(34) & "> <img border='0' style='padding-top:2px;' align='top' src='img/leftArrow.png'/></a> &nbsp;")

                End If
                If ((PageCurrent - (PageCurrent Mod 10)) + 10) > PageCount Then
                    For I = PageCurrent - (PageCurrent Mod 10) To PageCount
                        If Not I = 0 Then
                            If I = PageCurrent Then
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " class='currentPage'>" & I & " </a>")
                            Else
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " >" & I & " </a>")
                            End If
                        End If
                    Next I
                Else
                    For I = PageCurrent - (PageCurrent Mod 10) To (PageCurrent - (PageCurrent Mod 10)) + 10
                        If Not I = 0 Then
                            If I = PageCurrent Then
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " class='currentPage'>" & I & " </a>")
                            Else
                                TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " >" & I & " </a>")
                            End If

                        End If

                    Next I
                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & PageCurrent + 1) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>&nbsp;&nbsp;")

                    TopNavigation.Append("<a  href=" & Chr(34) & Server.UrlPathEncode(URLpage & PageCount) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='img/rightArrow.png'/></a>")

                End If


            End If
            lblBottomNavigation.Text = TopNavigation.ToString



            DT = Nothing
            PageFirst = 0
            RecordCount = 0
            PageSize = 0
            PageCount = 0
            PageCurrent = 0
            CurrentRecord = 0
            FirstRecord = 0
            FieldCount = 0
            Counter = 0

            strSQL = ""
            RecordsShown = 0
            LoopRecordCount = 0
            pageNum = 0

            markShowPage = 0
            I = 0
            J = 0

        End Sub




        Private Sub fill_TitleSort()
            Dim TypeList As New ArrayList
            '  TypeList.Add("")
            TypeList.Add("Ascending Order")
            TypeList.Add("Descending Order")
            TypeList.Add("ACT")
            TypeList.Add("PU(A)")
            TypeList.Add("PU(B)")
            'TypeList.Add("India")

            ddlTitle.DataSource = TypeList
            ddlTitle.DataBind()
            ddlTitle.SelectedIndex = Integer.Parse(GetSortby)

        End Sub
        Private Sub ddlTitle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTitle.SelectedIndexChanged
            Dim seltdTitleSort As String
            ''Dim objUtil As New clsMyUtility
            'Dim Sql As String
            Dim UrlPage As String
            seltdTitleSort = sender.SelectedIndex.ToString()
            'seltdTitleSort = ddlTitle.SelectedIndex
            'If seltdTitleSort = 0 Then
            '    SortedBy = "1"


            'ElseIf seltdTitleSort = 1 Then

            '    SortedBy = "2"

            'ElseIf seltdTitleSort = 2 Then
            '    SortedBy = "3"

            'ElseIf seltdTitleSort = 5 Then
            '    SortedBy = "4"

            'ElseIf seltdTitleSort = 3 Then
            '    SortedBy = "5"

            'ElseIf seltdTitleSort = 4 Then
            '    SortedBy = "6"

            'End If
            'SortedBy = ((ddlTitle.SelectedIndex) + 1).ToString()
            UrlPage = Server.UrlPathEncode("LegislationSubDisplay.aspx?no=" & ActNo & "&t=" & LegType & "&ctry=" & Country & "&srtp=" & seltdTitleSort & "&DivId=1")
            'UrlPage = "legislationsearchresult.aspx?ft=" & s_searchFTS & "&t=" & s_searchLegislation & "&an=" & s_searchACTNumber & "&prxOpt=" & i_searchProximityOption & "&prxThesaurus=" & s_wordForThesaurus & "&prxOptThesaurus=" & i_SearchOptionThesaurus & "&ns=" & s_searchNew & "&srt=" & S_SortBy & "&rc=" & S_RecordCount & "&chsrt=" & CharSortBy

            'UrlPage = Server.UrlPathEncode(UrlPage)
            UrlPage = Server.UrlPathEncode(UrlPage)
            Server.Transfer(UrlPage)


        End Sub

        Private Sub saveSrch(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveSearch.ServerClick
            Dim obj As New clsCasesSearch
            Dim sql As String
            Dim title As String
            If saveWithName.Text = "" Then
                title = ActNo
            Else
                title = saveWithName.Text
            End If
            'MsgBox(saveWithName.Text)
            'MsgBox(pageUrl)
            'type 0 is cases , 1 is legislation
            sql = "insert into SavedSearch(Username,Title,Link,Time,Type) values('" & Session("UserName") & "','" & title & "','" & thisPageUrl & "',GETDATE(),1 )"
            Dim res As Boolean = obj.UpdateRecord(sql)
            'If res Then
            '    MsgBox("successful")
            'Else
            '    MsgBox("failed")
            'End If
            Response.Redirect(thisPageUrl) 'can be removed to reduce load if a solution is made for contacting the server without redirecting
        End Sub
    End Class

End Namespace
