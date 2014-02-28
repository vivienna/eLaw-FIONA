Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports System.Xml.Linq
Imports membersarea
Imports System.Net.Mail
Imports Dba_UrlEncrption
Imports System.IO

Partial Class showcase
    Inherits System.Web.UI.Page
    Protected Shared ConnectionString As String

    Public Sub New()

        ConnectionString = clsConfigs.sGlobalConnectionString
        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If

    End Sub
    Dim ObjCS As New clsCasesSearch()
    Dim ObjInt As New clsIntelligence()
    Dim ObjUtil As New clsMyUtility()
    Dim UserName As String
    Dim URL As String = " "
    Public OpenFileName As String = "" ' use to print file name to view as PDf
    Public DownloadFileName As String = "" 'use to download the file as pdf
    Dim pageid As String = "" ' use to receive file name, to save document, for email , 
    Dim filename As String ' assign its value from PageID to read file content
    Public CaseCitator As String = "" ' print files links if case has cases referred or legislation referred
    Public HeadNote As String = "" ' print link to view headnote if case has headnote content
    Dim path As String 'read files dirctory 
    Dim s_searchExactCases As String
    Dim SearchWords As String ' highlight words assign value from s_searchExactCases
    Dim s_searchTitle As String
    Dim s_searchCaseNumber As String
    Dim s_searchJudge As String
    Dim s_searchNew As String
    Dim DisplayOption As String = "" ' recevie this var from search result if 0 or 1
    Public CasePrograssion As String = "" ' print case history if has 
    Dim c_casecitation As String = "" ' recevie value from search result if case has cases referred
    Dim c_legislationcitation As String = "" ' recevie value from search result if case has legislation referred
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyStyleSheet.Attributes.Add("href", "../css/StyleSheet.css")
        UserName = CType(Session("UserName"), String)
        If UserName = "" Then
            ' Response.Redirect("~/login.aspx")
            Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
            Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
        End If

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
                            Case "jd"
                                DisplayOption = UrlQuery(i).parvalue
                                DisplayOption = DisplayOption.Trim()
                            Case "id"
                                pageid = UrlQuery(i).parvalue

                            Case "ec"
                                s_searchExactCases = UrlQuery(i).parvalue
                                s_searchExactCases = s_searchExactCases.Replace("""", "")
                            Case "t"
                                s_searchTitle = UrlQuery(i).parvalue
                            Case "cn"
                                s_searchCaseNumber = UrlQuery(i).parvalue
                            Case "j"
                                s_searchJudge = UrlQuery(i).parvalue
                            Case "ns"
                                s_searchNew = UrlQuery(i).parvalue
                            Case "lc"
                                c_legislationcitation = UrlQuery(i).parvalue
                            Case "cc"
                                c_casecitation = UrlQuery(i).parvalue
                        End Select
                    End If
                Next
            End If
        ElseIf Request.QueryString("id") <> "" Then
            Dim TempID As String = Request.QueryString("id")
            Dim UrlDecrpt As New Dba_UrlEncrption(TempID, False)
            Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
            pageid = UrlQuery

        Else

            Response.Redirect("~/login.aspx")
        End If
        ' Sotre 
        pageid = pageid.Replace("?", "").Replace(";", "").Replace("id", "").Replace("=", "")
        fileNameStore.Text = pageid
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        URL = "showcase.aspx?info="

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        filename = pageid

        filename = Replace(filename, "?", "")
        If Right(filename, 4) <> ".xml" Then
            filename = Replace(filename, "?", "")
            filename = Replace(filename, ";", "")
            filename &= ".xml"
        End If

        '============encrpt file name for pdf and view , save cases
        Dim savepageid As String = pageid
        savepageid = savepageid.Replace(";", "")
        Dim UrlLink As New Dba_UrlEncrption(savepageid, True)
        Dim linkPrec As String = UrlLink.UrlEncrypt
        OpenFileName = "f=" & linkPrec
        DownloadFileName = "d=Y&f=" & linkPrec
        fileNameStore.Text = linkPrec
        lblid.CssClass = "invisible"
        lblid.Text = linkPrec

        '====================================================
        Dim CountCaseCitation As String = ""
        CountCaseCitation = ObjCS.CheckCaseCitator(filename.Replace(".xml", ""))
        If CountCaseCitation <> "" Then
            ' CaseCitator = "<a href='../CaseCitator.aspx?fn=" & linkPrec & "' ><img border='0' src='../img/icoViewNote.gif'/>Cases Referred</a>"
            CaseCitator = " <a href=../precMap.aspx?id=" & linkPrec & " ><img border='0' src='../img/icoViewNote.gif'/>Cases Ref : " & CountCaseCitation & "</a>"
        End If
        '=========================================================
        Dim CountLegCitation As Integer = 0
        CountLegCitation = ObjCS.CheckLegislationCitator(filename.Replace(".xml", ""))
        If CountLegCitation > 0 Then
            CaseCitator &= "<a href='../LegislationReferred.aspx?fn=" & linkPrec & "' ><img border='0' src='../img/icoViewNote.gif'/>Legislation Ref : (" & CountLegCitation & ")</a>"
            ' &= " <a href=../precMap_legis.aspx?id=" & linkPrec & " ><img border='0' src='../img/icoViewNote.gif'/>Legislation Referred</a>"
        End If
        '==============================================================
        Dim doc As String = Server.MapPath("~\xmlfiles\cases\" & filename)

        If DisplayOption = "1" Then
            ''''''''''''''''''''''''''''''''''
            If ObjInt.isValidFileForHeadnotes(filename) = True Then

                If CheckElement(doc, "HEADNOTE") Then
                    Dim IdLink1 As String = "ec=" & s_searchExactCases & "&t=" & s_searchTitle & "&cn=" & s_searchCaseNumber & "&j=" & s_searchJudge & "&ns=" & s_searchNew & "&id=" & pageid & "&lc=" & c_legislationcitation & "&cc=" & c_casecitation & "&jd=0"
                    Dim UrlFIleID1 As New Dba_UrlEncrption(IdLink1, True)
                    Dim linkFileId1 As String = UrlFIleID1.UrlEncrypt
                    HeadNote = " <a href=" & Server.UrlPathEncode(URL & linkFileId1) & "><img border='0' src='../img/icoJudgment.gif'/>Headnote</a>"
                End If
            Else
                Response.Redirect("~/login.aspx")
            End If

            ''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If s_searchExactCases = "" Then
                DisplayCase1WithoutHighlighting(filename)

            ElseIf s_searchExactCases <> "" Then
                SearchWords = ObjUtil.RefineSentence(s_searchExactCases)
                Display_Case_ExactPhrase(filename, SearchWords)
            Else
                SearchWords = ObjUtil.RefineSentence(s_searchExactCases)
                Display_Case1(filename, SearchWords)
            End If

        ElseIf DisplayOption = "0" Then
            ''''''''''''''''''''''''''''''''''''
            Dim IdLink1 As String = "ec=" & s_searchExactCases & "&t=" & s_searchTitle & "&cn=" & s_searchCaseNumber & "&j=" & s_searchJudge & "&ns=" & s_searchNew & "&id=" & pageid & "&lc=" & c_legislationcitation & "&cc=" & c_casecitation & "&jd=1"
            Dim UrlFIleID1 As New Dba_UrlEncrption(IdLink1, True)
            Dim linkFileId1 As String = UrlFIleID1.UrlEncrypt
            If CheckElement(doc, "HEADNOTE") Then
                If s_searchExactCases = "" Then
                    DisplayCase1WithoutHighlighting_HeadNotes(filename)
                ElseIf s_searchExactCases <> "" Then
                    SearchWords = Trim(s_searchExactCases)
                    Display_Case_ExactPhrase_HeadNotes(filename, SearchWords)
                Else
                    SearchWords = ObjUtil.RefineSentence(s_searchExactCases)
                    Display_Case_HeadNotes(filename, SearchWords)
                End If
                HeadNote = " <a href=" & Server.UrlPathEncode(URL & linkFileId1) & "><img border='0' src='../img/icoJudgment.gif'/>Judgment</a>"
            Else
                If s_searchExactCases = "" Then
                    DisplayCase1WithoutHighlighting(filename)
                ElseIf s_searchExactCases <> "" Then
                    SearchWords = Trim(s_searchExactCases)
                    Display_Case_ExactPhrase(filename, SearchWords)
                Else
                    SearchWords = ObjUtil.RefineSentence(s_searchExactCases)
                    Display_Case1(filename, SearchWords)
                End If
            End If

            ''''''''''''''''''''''''''''''''''''

        ElseIf DisplayOption <> "0" Or DisplayOption <> "1" Or DisplayOption = "" Then

            ''''''''''''''''''''''''''''''''''
            If ObjInt.isValidFileForHeadnotes(filename) = True Then

                Dim IdLink1 As String = "ec=" & s_searchExactCases & "&t=" & s_searchTitle & "&cn=" & s_searchCaseNumber & "&j=" & s_searchJudge & "&ns=" & s_searchNew & "&id=" & pageid & "&lc=" & c_legislationcitation & "&cc=" & c_casecitation & "&jd=1"
                Dim UrlFIleID1 As New Dba_UrlEncrption(IdLink1, True)
                Dim linkFileId1 As String = UrlFIleID1.UrlEncrypt
                If CheckElement(doc, "HEADNOTE") Then
                    DisplayCase1WithoutHighlighting_HeadNotes(filename)
                    HeadNote = " <a href=" & Server.UrlPathEncode(URL & linkFileId1) & "><img border='0' src='../img/icoJudgment.gif'/>View Judgment</a>"
                Else
                    DisplayCase1WithoutHighlighting(filename)
                End If
            Else
                Response.Redirect("~/login.aspx")
            End If
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''
        ObjUtil = Nothing
        ObjCS = Nothing
        ObjInt = Nothing
        path = ""
        SearchWords = ""
        s_searchTitle = ""
        s_searchCaseNumber = ""
        s_searchJudge = ""
        s_searchNew = ""
        CheckUserHistory(filename.Replace(".xml", ""))
        saving.Text = getSavedDoc()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If IsPostBack = False Then
            ' store file and file size to provide user daily usage
            clsMyUtility.updateQuota(filename.Trim(), Session("username").ToString(), Server.MachineName.ToString(), Request.UserHostAddress.ToString(), 1, 0)
        End If

    End Sub



    Private Function getSavedDoc() As String
        Dim sql As String = "select * from SavedCasesLinks where CaseUserName='" & Session("UserName") & "' and XmlCaseName='" & pageid & "'"
        Dim obj As New clsCasesSearch
        Dim dt = New DataTable
        dt = obj.ExecuteMyQuery(sql)
        If dt.Rows.Count > 0 Then
            'saving.Enabled = False
            saving.Attributes.Add("Saved", "saved")
            saving.Attributes.CssStyle.Add("cursor", "default")
            saving.Attributes.CssStyle.Add("text-decoration", "none")
            Return "<img border='0' src='../img/icoSave.gif'/>Saved Already"
        Else
            saving.Attributes.Add("Saved", "notSaved")
            'saving.Enabled = True
            Return "<img border='0' src='../img/icoSave.gif'/>Save"
        End If
    End Function

    Private Sub Display_Case1(ByVal Path As String, ByVal SearchWords As String)
        'actually path = filename

        Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & Path))
        Dim Pattern As String
        Dim strNewText As String = ""
        Dim objUtil As New clsMyUtility()
        Dim sbXml As New System.Text.StringBuilder
        Dim CaseCitation As String = objUtil.SplitCitation(Path)
        Dim i As Int16
        Dim JudgementBody As String
        Dim Title As String
        Dim Country As String = ""
        Dim Citation As String = ""
        Dim Court As String
        Dim Judge As String
        Dim JudgementDate As String
        Dim CaseNumber As String
        Dim CaseAppeal As String
        Dim COUNSELS As String
        Dim subject_index As String
        Dim VERDICT As String
        Dim COL As New Collections.ArrayList
        SearchWords = Trim(SearchWords)
        COL = objUtil.Tokenizer(SearchWords)
        Try
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT_NAME" Then
                            Title = "<span style='font-family:CalistoMTStdBold;font-size:18px;text-align:center;display:block;margin:20px 0;' id='title'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &")
                            sbXml.Append(Title)
                            sbXml.Append("<span style='display:block; float:right;padding:10px; background-color:#cce7f7;'>" & CaseCitation & "</span><div class='clear'></div>")
                            Title = ""
                        ElseIf xRead.Name = "COURT_TYPE" Then
                            Court = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    Court = Regex.Replace(Court, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If
                            Next
                            sbXml.Append(Court)
                            Court = ""
                        ElseIf xRead.Name = "JUDGE_NAME" Then
                            Judge = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    Judge = Regex.Replace(Judge, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next
                            sbXml.Append(Judge)
                            Judge = ""

                        ElseIf xRead.Name = "JUDGMENT_NUMBER" Then
                            CaseNumber = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    CaseNumber = Regex.Replace(CaseNumber, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next
                            sbXml.Append(CaseNumber)
                            CaseNumber = ""
                        ElseIf xRead.Name = "JUDGMENT_DATE" Then

                            JudgementDate = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    JudgementDate = Regex.Replace(JudgementDate, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next
                            sbXml.Append(JudgementDate)
                            JudgementDate = ""

                        ElseIf xRead.Name = "CASE_PROGRESSION" Then
                            Dim ReplaceWith As String
                            CaseAppeal = ""
                            Dim msg As String
                            Try
                                Dim addspan As String = ""

                                CaseAppeal = xRead.ReadInnerXml
                                If CaseAppeal.Contains("MLRH") Or CaseAppeal.Contains("MELR") Or CaseAppeal.Contains("MLRA") Then
                                    CaseAppeal = CaseAppeal.Replace("<p>Case History : </p>", "")
                                    CaseAppeal = CaseAppeal.Replace("""", "'")
                                    CaseAppeal = CaseAppeal.Replace(";", "")
                                    CaseAppeal = CaseAppeal.Replace("<br />", " <div style='width:100%;clear:both;padding:5px;'></div>")
                                    '<LINK HREF="case_notes/showcase.aspx?pageid=
                                    Pattern = "<LINK HREF='case_notes/showcase.aspx?pageid="
                                    ReplaceWith = "<span class='boxNotify1'><strong><a href='showcase.aspx?id="
                                    CaseAppeal = CaseAppeal.Replace(Pattern, ReplaceWith)
                                    'CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    Pattern = "</LINK>"
                                    ReplaceWith = "</a></strong></span>;"
                                    CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    CaseAppeal = CaseAppeal.Replace("'", """")
                                    '============================================

                                    Dim mee As New MatchEvaluator(AddressOf encrypturl)
                                    CaseAppeal = Regex.Replace(CaseAppeal, "href=""([^""]*)", mee)
                                    '============================================
                                    CasePrograssion = CaseAppeal

                                End If


                                CaseAppeal = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try

                        ElseIf xRead.Name = "COUNSELS" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try
                                COUNSELS = xRead.ReadOuterXml
                                sbXml.Append(COUNSELS)
                                COUNSELS = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        ElseIf xRead.Name = "SUBJECT_INDEX" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try
                                subject_index = xRead.ReadOuterXml
                                If subject_index.Contains("NULL") Then
                                    subject_index = subject_index.Replace("- NULL", "")
                                End If
                                sbXml.Append("<br/>")
                                sbXml.Append("<i>" & subject_index & "</i>")
                                sbXml.Append("<br/>")
                                subject_index = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        ElseIf xRead.Name = "JUDGMENT" Then
                            Dim ReplaceWith As String
                            JudgementBody = xRead.ReadOuterXml
                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If
                            Next

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayed.aspx?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayformat.asp?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationdisplayformat.asp?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""LegislationMainDisplayed.aspx?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = JudgementBody.Replace("<LINK HREF=""case_notes/showcase.aspx?pageid=", "<b><a  href=" & Chr(34) & "showcase.aspx?id=")
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")
                            '============================================
                            ' JudgementBody = Regex.Replace(JudgementBody, "("".*\?)(.*)""", encryptUrl("$1", "$2"))
                            Dim mee As New MatchEvaluator(AddressOf encrypturl)
                            JudgementBody = Regex.Replace(JudgementBody, "href=""([^""]*)", mee)
                            '============================================
                            sbXml.Append(JudgementBody)
                            JudgementBody = ""
                        End If

                End Select


            End While
        Catch ex As Exception
            lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
            TopLeftCase.Visible = False
            TopRightCase.Visible = False
        Finally
            xRead.Close()
        End Try
        ltrshowcase.Text = sbXml.ToString
        ltrshowcase = Nothing
        objUtil = Nothing
    End Sub
    Private Sub DisplayCase1WithoutHighlighting(ByVal Path As String)

        Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & Path))

        Dim Pattern As String
        Dim strNewText As String
        Dim sbXml As New System.Text.StringBuilder()
        Dim CaseCitation As String = ObjUtil.SplitCitation(Path)
        Dim i As Int16
        Dim JudgementBody As String
        Dim Title As String
        Dim Country As String
        Dim Citation As String
        Dim Court As String
        Dim Judge As String
        Dim JudgementDate As String
        Dim CaseNumber As String
        Dim CaseAppeal As String
        Dim subject_index As String
        Dim COUNSELS As String
        Try
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT_NAME" Then

                            Title = "<span style='font-family:CalistoMTStdBold;font-size:18px;text-align:center;display:block;margin:20px 0;' id='title'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &")
                            sbXml.Append(Title)

                            sbXml.Append("<span style='display:block; float:right;padding:10px; background-color:#cce7f7;'>" & CaseCitation & "</span><div class='clear'></div>")
                            Title = ""
                        ElseIf xRead.Name = "COURT_TYPE" Then
                            Court = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</b></font></p>"  'xRead.ReadOuterXml
                            sbXml.Append(Court)
                            Court = ""

                        ElseIf xRead.Name = "JUDGE_NAME" Then
                            Judge = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            sbXml.Append(Judge)
                            Judge = ""

                        ElseIf xRead.Name = "JUDGMENT_NUMBER" Then


                            CaseNumber = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            sbXml.Append(CaseNumber)
                            CaseNumber = ""
                        ElseIf xRead.Name = "JUDGMENT_DATE" Then

                            JudgementDate = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            sbXml.Append(JudgementDate)
                            JudgementDate = ""

                        ElseIf xRead.Name = "CASE_PROGRESSION" Then
                            Dim ReplaceWith As String
                            CaseAppeal = ""
                            Dim msg As String
                            Try
                                Dim addspan As String = ""

                                CaseAppeal = xRead.ReadInnerXml

                                If CaseAppeal.Contains("MLRH") Or CaseAppeal.Contains("MELR") Or CaseAppeal.Contains("MLRA") Then
                                    CaseAppeal = CaseAppeal.Replace("<p>Case History : </p>", "")
                                    CaseAppeal = CaseAppeal.Replace("""", "'")
                                    CaseAppeal = CaseAppeal.Replace(";", "")
                                    CaseAppeal = CaseAppeal.Replace("<br />", " <div style='width:100%;clear:both;padding:5px;'></div>")
                                    '<LINK HREF="case_notes/showcase.aspx?pageid=
                                    Pattern = "<LINK HREF='case_notes/showcase.aspx?pageid="
                                    ReplaceWith = "<span class='boxNotify1'><strong><a href='showcase.aspx?id="
                                    CaseAppeal = CaseAppeal.Replace(Pattern, ReplaceWith)
                                    'CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    Pattern = "</LINK>"
                                    ReplaceWith = "</a></strong></span>;"
                                    CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    CaseAppeal = CaseAppeal.Replace("'", """")
                                    '============================================

                                    Dim mee As New MatchEvaluator(AddressOf encrypturl)
                                    CaseAppeal = Regex.Replace(CaseAppeal, "href=""([^""]*)", mee)
                                    '============================================
                                    CasePrograssion = CaseAppeal

                                End If
                                CaseAppeal = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        ElseIf xRead.Name = "COUNSELS" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try
                                COUNSELS = xRead.ReadOuterXml
                                sbXml.Append(COUNSELS)
                                COUNSELS = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try


                        ElseIf xRead.Name = "SUBJECT_INDEX" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try
                                subject_index = xRead.ReadOuterXml
                                If subject_index.Contains("NULL") Then
                                    subject_index = subject_index.Replace("- NULL", "")
                                End If
                                sbXml.Append("<br/>")
                                sbXml.Append("<i>" & subject_index & "</i>")
                                sbXml.Append("<br/>")
                                subject_index = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        ElseIf xRead.Name = "JUDGMENT" Then

                            Dim ReplaceWith As String

                            JudgementBody = xRead.ReadInnerXml
                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayed.aspx?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayformat.asp?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationdisplayformat.asp?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""LegislationMainDisplayed.aspx?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = JudgementBody.Replace("<LINK HREF=""case_notes/showcase.aspx?pageid=", "<b><a  href=" & Chr(34) & "showcase.aspx?id=")
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")
                            '============================================
                            ' JudgementBody = Regex.Replace(JudgementBody, "("".*\?)(.*)""", encryptUrl("$1", "$2"))
                            Dim mee As New MatchEvaluator(AddressOf encrypturl)
                            JudgementBody = Regex.Replace(JudgementBody, "href=""([^""]*)", mee)
                            '============================================
                            sbXml.Append(JudgementBody)
                            JudgementBody = ""
                        End If

                End Select


            End While
        Catch ex As Exception
            lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
            TopLeftCase.Visible = False
            TopRightCase.Visible = False
        Finally
            xRead.Close()
        End Try

        ltrshowcase.Text = sbXml.ToString
        sbXml = Nothing

    End Sub
    ''' <summary>
    ''' Use to encrypt files name inside file content
    ''' </summary>
    ''' <param name="lnk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function encrypturl(lnk As Match) As String
        Dim txt As String = lnk.tostring
        Dim idlink1 As String = txt.Substring(txt.IndexOf("?") + 1)
        Dim urlfileid1 As New Dba_UrlEncrption(idlink1, True)
        Dim linkfileid1 As String = urlfileid1.UrlEncrypt
        Return txt.Substring(0, txt.IndexOf("?") + 1) & "info=" & linkfileid1
    End Function
    Private Sub Display_Case_ExactPhrase(ByVal Path As String, ByVal SearchWords As String)
        Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & Path))
        Dim Pattern As String
        Dim objUtil As New clsMyUtility()
        Dim sbXml As New System.Text.StringBuilder
        Dim CaseCitation As String = objUtil.SplitCitation(Path)
        Dim i As Int16
        Dim JudgementBody As String
        Dim Title As String
        Dim Court As String
        Dim Judge As String
        Dim JudgementDate As String
        Dim CaseNumber As String
        Dim CaseAppeal As String
        Dim COUNSELS As String
        Dim subject_index As String
        Dim COL As New Collections.ArrayList
        SearchWords = Trim(SearchWords)
        COL = objUtil.Tokenizer(SearchWords)
        Try
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT_NAME" Then
                            Title = "<span style='font-family:CalistoMTStdBold;font-size:18px;text-align:center;display:block;margin:20px 0; padding:0 0 0 80px;' id='title'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &").Replace(" amp;", "&")
                            sbXml.Append(Title)

                            sbXml.Append("<span style='display:block; float:right;padding:10px; background-color:#cce7f7;'>" & CaseCitation & "</span><div class='clear'></div>")
                            Title = ""
                        ElseIf xRead.Name = "COURT_TYPE" Then

                            Court = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                Court = Regex.Replace(Court, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            sbXml.Append(Court)

                            Court = ""

                        ElseIf xRead.Name = "JUDGE_NAME" Then


                            Judge = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                Judge = Regex.Replace(Judge, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            sbXml.Append(Judge)
                            Judge = ""

                        ElseIf xRead.Name = "JUDGMENT_NUMBER" Then


                            CaseNumber = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                CaseNumber = Regex.Replace(CaseNumber, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            sbXml.Append(CaseNumber)
                            CaseNumber = ""
                        ElseIf xRead.Name = "JUDGMENT_DATE" Then



                            JudgementDate = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                JudgementDate = Regex.Replace(JudgementDate, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If

                            sbXml.Append(JudgementDate)
                            JudgementDate = ""

                        ElseIf xRead.Name = "CASE_PROGRESSION" Then
                            Dim ReplaceWith As String
                            CaseAppeal = ""
                            Dim msg As String
                            Try
                                Dim addspan As String = ""

                                CaseAppeal = xRead.ReadInnerXml

                                If CaseAppeal.Contains("MLRH") Or CaseAppeal.Contains("MELR") Or CaseAppeal.Contains("MLRA") Then
                                    CaseAppeal = CaseAppeal.Replace("<p>Case History : </p>", "")
                                    CaseAppeal = CaseAppeal.Replace("""", "'")
                                    CaseAppeal = CaseAppeal.Replace(";", "")
                                    CaseAppeal = CaseAppeal.Replace("<br />", " <div style='width:100%;clear:both;padding:5px;'></div>")
                                    '<LINK HREF="case_notes/showcase.aspx?pageid=
                                    Pattern = "<LINK HREF='case_notes/showcase.aspx?pageid="
                                    ReplaceWith = "<span class='boxNotify1'><strong><a href='showcase.aspx?id="
                                    CaseAppeal = CaseAppeal.Replace(Pattern, ReplaceWith)
                                    'CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    Pattern = "</LINK>"
                                    ReplaceWith = "</a></strong></span>;"
                                    CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    CaseAppeal = CaseAppeal.Replace("'", """")
                                    '============================================

                                    Dim mee As New MatchEvaluator(AddressOf encrypturl)
                                    CaseAppeal = Regex.Replace(CaseAppeal, "href=""([^""]*)", mee)
                                    '============================================
                                    CasePrograssion = CaseAppeal

                                End If
                                CaseAppeal = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try

                        ElseIf xRead.Name = "COUNSELS" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try

                                COUNSELS &= xRead.ReadOuterXml


                                sbXml.Append(COUNSELS)
                                COUNSELS = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        ElseIf xRead.Name = "SUBJECT_INDEX" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try
                                subject_index = xRead.ReadOuterXml
                                If subject_index.Contains("NULL") Then
                                    subject_index = subject_index.Replace("- NULL", "")
                                End If
                                sbXml.Append("<br/>")
                                sbXml.Append("<i>" & subject_index & "</i>")
                                sbXml.Append("<br/>")
                                subject_index = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        ElseIf xRead.Name = "JUDGMENT" Then
                            Dim ReplaceWith As String
                            Dim FirstWord, LastWord As String
                            JudgementBody = xRead.ReadInnerXml

                            For i = 0 To COL.Count - 1
                                If i = 0 Then
                                    FirstWord = COL.Item(0)
                                Else
                                    LastWord = COL.Item(i)
                                End If
                            Next

                            FirstWord = Trim(FirstWord)
                            LastWord = Trim(LastWord)

                            If ((FirstWord <> "") And (LastWord <> "")) Then

                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & FirstWord & "\w" & LastWord & "\b"

                                JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:blue;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If


                            If SearchWords <> "" Then

                                Pattern = "\b" & SearchWords & "\b"
                                JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next
                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayed.aspx?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayformat.asp?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationdisplayformat.asp?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""LegislationMainDisplayed.aspx?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = JudgementBody.Replace("<LINK HREF=""case_notes/showcase.aspx?pageid=", "<b><a  href=" & Chr(34) & "showcase.aspx?id=")
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")
                            '============================================
                            ' JudgementBody = Regex.Replace(JudgementBody, "("".*\?)(.*)""", encryptUrl("$1", "$2"))
                            Dim mee As New MatchEvaluator(AddressOf encrypturl)
                            JudgementBody = Regex.Replace(JudgementBody, "href=""([^""]*)", mee)
                            '============================================
                            sbXml.Append(JudgementBody)

                            JudgementBody = ""
                        End If

                End Select



            End While

            ' clsMyUtility.updateQuota(Path.Replace(".xml", ""), Session("username"), Server, Request)

        Catch ex As Exception
            lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
            TopLeftCase.Visible = False
            TopRightCase.Visible = False
        Finally
            xRead.Close()
        End Try


        ltrshowcase.Text = sbXml.ToString()

        'lblPdf.Text &= "<b><a href=" & Chr(34) & "casespdfopener.aspx?f=" & filename1 & Chr(34) & " ><img border=0 src=" & Chr(34) & "images1/pdf.gif" & Chr(34) & "> <font face=Vardana size=2 color=#CC0000>" & "Printable Version</a></b></font></br>"
        sbXml = Nothing
        objUtil = Nothing
    End Sub

    Private Sub DisplayCase1WithoutHighlighting_HeadNotes(ByVal Path As String)

        Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & Path))

        Dim Pattern As String
        Dim strNewText As String
        Dim sbXml As New System.Text.StringBuilder()
        Dim CaseCitation As String = ObjUtil.SplitCitation(Path)
        Dim i As Int16
        Dim JudgementBody As String
        Dim Title As String
        Dim Country As String
        Dim Citation As String
        Dim Court As String
        Dim Judge As String
        Dim JudgementDate As String
        Dim CaseNumber As String
        Dim caseAppeal As String
        Try
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT_NAME" Then

                            Title = "<span style='font-family:CalistoMTStdBold;font-size:18px;text-align:center;display:block;margin:20px 0;' id='title'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &")
                            sbXml.Append(Title)
                            sbXml.Append("<span style='display:block; float:right;padding:10px; background-color:#cce7f7;'>" & CaseCitation & "</span><div class='clear'></div>")
                            Title = ""
                        ElseIf xRead.Name = "COURT_TYPE" Then
                            Court = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</b></font></p>"  'xRead.ReadOuterXml
                            sbXml.Append(Court)
                            Court = ""

                        ElseIf xRead.Name = "JUDGE_NAME" Then
                            Judge = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            sbXml.Append(Judge)
                            Judge = ""

                        ElseIf xRead.Name = "JUDGMENT_NUMBER" Then


                            CaseNumber = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            sbXml.Append(CaseNumber)
                            CaseNumber = ""
                        ElseIf xRead.Name = "JUDGMENT_DATE" Then

                            JudgementDate = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            sbXml.Append(JudgementDate)
                            JudgementDate = ""
                        ElseIf xRead.Name = "HEADNOTE" Then
                            Dim ReplaceWith As String
                            JudgementBody = xRead.ReadOuterXml
                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayed.aspx?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayformat.asp?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationdisplayformat.asp?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""LegislationMainDisplayed.aspx?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = JudgementBody.Replace("<LINK HREF=""case_notes/showcase.aspx?pageid=", "<b><a  href=" & Chr(34) & "showcase.aspx?id=")
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")
                            '============================================
                            ' JudgementBody = Regex.Replace(JudgementBody, "("".*\?)(.*)""", encryptUrl("$1", "$2"))
                            Dim mee As New MatchEvaluator(AddressOf encrypturl)
                            JudgementBody = Regex.Replace(JudgementBody, "href=""([^""]*)", mee)
                            '============================================
                            sbXml.Append(JudgementBody)
                            JudgementBody = ""
                        ElseIf xRead.Name = "CASE_PROGRESSION" Then
                            Dim ReplaceWith As String
                            caseAppeal = ""
                            Dim msg As String
                            Try
                                Dim addspan As String = ""

                                caseAppeal = xRead.ReadInnerXml
                                If CaseAppeal.Contains("MLRH") Or CaseAppeal.Contains("MELR") Or CaseAppeal.Contains("MLRA") Then
                                    caseAppeal = caseAppeal.Replace("<p>Case History : </p>", "")
                                    caseAppeal = caseAppeal.Replace("""", "'")
                                    caseAppeal = caseAppeal.Replace(";", "")
                                    caseAppeal = caseAppeal.Replace("<br />", " <div style='width:100%;clear:both;padding:5px;'></div>")
                                    '<LINK HREF="case_notes/showcase.aspx?pageid=
                                    Pattern = "<LINK HREF='case_notes/showcase.aspx?pageid="
                                    ReplaceWith = "<span class='boxNotify1'><strong><a href='showcase.aspx?id="
                                    caseAppeal = caseAppeal.Replace(Pattern, ReplaceWith)
                                    'CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    Pattern = "</LINK>"
                                    ReplaceWith = "</a></strong></span>;"
                                    caseAppeal = Regex.Replace(caseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    caseAppeal = caseAppeal.Replace("'", """")
                                    '============================================

                                    Dim mee As New MatchEvaluator(AddressOf encrypturl)
                                    caseAppeal = Regex.Replace(caseAppeal, "href=""([^""]*)", mee)
                                    '============================================
                                    CasePrograssion = caseAppeal

                                End If
                                caseAppeal = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        End If

                End Select


            End While
        Catch ex As Exception
            lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
            TopLeftCase.Visible = False
            TopRightCase.Visible = False
        Finally
            xRead.Close()
        End Try

        ltrshowcase.Text = sbXml.ToString
        sbXml = Nothing

    End Sub

    Private Sub Display_Case_HeadNotes(ByVal Path As String, ByVal SearchWords As String)
        'actually path = filename

        Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & Path))

        Dim Pattern As String
        Dim strNewText As String
        Dim objUtil As New clsMyUtility()
        Dim sbXml As New System.Text.StringBuilder
        Dim CaseCitation As String = objUtil.SplitCitation(Path)
        Dim i As Int16
        Dim JudgementBody As String
        Dim Title As String
        Dim Country As String
        Dim Citation As String
        Dim Court As String
        Dim Judge As String
        Dim JudgementDate As String
        Dim CaseNumber As String
        Dim CaseAppeal As String
        Dim COUNSELS As String
        Dim COL As New Collections.ArrayList

        SearchWords = Trim(SearchWords)
        COL = objUtil.Tokenizer(SearchWords)


        Try
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT_NAME" Then

                            Title = "<span style='font-family:CalistoMTStdBold;font-size:18px;text-align:center;display:block;margin:20px 0;' id='title'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &")
                            sbXml.Append(Title)
                            Title = ""
                        ElseIf xRead.Name = "COURT_TYPE" Then


                            Court = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    Court = Regex.Replace(Court, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next

                            sbXml.Append(Court)
                            Court = ""

                        ElseIf xRead.Name = "JUDGE_NAME" Then


                            Judge = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    Judge = Regex.Replace(Judge, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next


                            sbXml.Append(Judge)
                            Judge = ""

                        ElseIf xRead.Name = "JUDGMENT_NUMBER" Then


                            CaseNumber = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    CaseNumber = Regex.Replace(CaseNumber, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next


                            sbXml.Append(CaseNumber)
                            CaseNumber = ""
                        ElseIf xRead.Name = "JUDGMENT_DATE" Then



                            JudgementDate = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    JudgementDate = Regex.Replace(JudgementDate, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next
                            sbXml.Append(JudgementDate)
                            JudgementDate = ""


                        ElseIf xRead.Name = "COUNSELS" Then
                            Dim ReplaceWith As String
                            Dim msg As String
                            Try

                                COUNSELS = xRead.ReadInnerXml


                                sbXml.Append(COUNSELS)
                                COUNSELS = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try

                        ElseIf xRead.Name = "HEADNOTE" Then

                            Dim ReplaceWith As String

                            JudgementBody = xRead.ReadInnerXml
                            For i = 0 To COL.Count - 1
                                If COL.Item(i) <> " " Then
                                    Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                    JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                End If

                            Next
                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayed.aspx?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayformat.asp?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationdisplayformat.asp?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""LegislationMainDisplayed.aspx?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = JudgementBody.Replace("<LINK HREF=""case_notes/showcase.aspx?pageid=", "<b><a  href=" & Chr(34) & "showcase.aspx?id=")
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")
                            '============================================
                            ' JudgementBody = Regex.Replace(JudgementBody, "("".*\?)(.*)""", encryptUrl("$1", "$2"))
                            Dim mee As New MatchEvaluator(AddressOf encrypturl)
                            JudgementBody = Regex.Replace(JudgementBody, "href=""([^""]*)", mee)
                            '============================================
                            sbXml.Append(JudgementBody)
                            JudgementBody = ""


                        ElseIf xRead.Name = "CASE_PROGRESSION" Then
                            Dim ReplaceWith As String
                            CaseAppeal = ""
                            Dim msg As String
                            Try
                                Dim addspan As String = ""

                                CaseAppeal = xRead.ReadInnerXml
                                If CaseAppeal.Contains("MLRH") Or CaseAppeal.Contains("MELR") Or CaseAppeal.Contains("MLRA") Then
                                    CaseAppeal = CaseAppeal.Replace("<p>Case History : </p>", "")
                                    CaseAppeal = CaseAppeal.Replace("""", "'")
                                    CaseAppeal = CaseAppeal.Replace(";", "")
                                    CaseAppeal = CaseAppeal.Replace("<br />", " <div style='width:100%;clear:both;padding:5px;'></div>")
                                    '<LINK HREF="case_notes/showcase.aspx?pageid=
                                    Pattern = "<LINK HREF='case_notes/showcase.aspx?pageid="
                                    ReplaceWith = "<span class='boxNotify1'><strong><a href='showcase.aspx?id="
                                    CaseAppeal = CaseAppeal.Replace(Pattern, ReplaceWith)
                                    'CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    Pattern = "</LINK>"
                                    ReplaceWith = "</a></strong></span>;"
                                    CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    CaseAppeal = CaseAppeal.Replace("'", """")
                                    '============================================

                                    Dim mee As New MatchEvaluator(AddressOf encrypturl)
                                    CaseAppeal = Regex.Replace(CaseAppeal, "href=""([^""]*)", mee)
                                    '============================================
                                    CasePrograssion = CaseAppeal

                                End If
                                CaseAppeal = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try
                        End If

                End Select


            End While
        Catch ex As Exception
            lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
            TopLeftCase.Visible = False
            TopRightCase.Visible = False
        Finally
            xRead.Close()
        End Try



        ltrshowcase.Text = sbXml.ToString
        'lblPdf.Text &= "<b><a href=" & Chr(34) & "casespdfopener.aspx?f=" & filename1 & Chr(34) & " ><img border=0 src=" & Chr(34) & "images1/pdf.gif" & Chr(34) & "> <font face=Vardana size=2 color=#CC0000>" & "Printable Version</a></b></font></br>"
        ltrshowcase = Nothing
        objUtil = Nothing
    End Sub
    Private Sub Display_Case_ExactPhrase_HeadNotes(ByVal Path As String, ByVal SearchWords As String)
        'actually path = filename

        Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("~\xmlfiles\cases\" & Path))

        Dim Pattern As String
        Dim strNewText As String
        Dim objUtil As New clsMyUtility()
        Dim sbXml As New System.Text.StringBuilder
        Dim CaseCitation As String = objUtil.SplitCitation(Path)
        Dim i As Int16
        Dim JudgementBody As String
        Dim Title As String
        Dim Country As String
        Dim Citation As String
        Dim Court As String
        Dim Judge As String
        Dim JudgementDate As String
        Dim CaseNumber As String
        Dim CaseAppeal As String
        Dim COUNSELS As String
        Dim COL As New Collections.ArrayList

        SearchWords = Trim(SearchWords)
        COL = objUtil.Tokenizer(SearchWords)
        Try
            While xRead.Read()
                Select Case xRead.NodeType
                    Case XmlNodeType.Element
                        If xRead.Name = "JUDGMENT_NAME" Then
                            Title = "<span style='font-family:CalistoMTStdBold;font-size:18px;text-align:center;display:block;margin:20px 0;' id='title'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &")
                            sbXml.Append(Title)
                            sbXml.Append("<span style='display:block; float:right;padding:10px; background-color:#cce7f7;'>" & CaseCitation & "</span><div class='clear'></div>")
                            Title = ""
                        ElseIf xRead.Name = "COURT_TYPE" Then
                            Court = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                Court = Regex.Replace(Court, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            sbXml.Append(Court)
                            Court = ""
                        ElseIf xRead.Name = "JUDGE_NAME" Then
                            Judge = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                Judge = Regex.Replace(Judge, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            sbXml.Append(Judge)
                            Judge = ""

                        ElseIf xRead.Name = "JUDGMENT_NUMBER" Then


                            CaseNumber = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml

                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                CaseNumber = Regex.Replace(CaseNumber, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            sbXml.Append(CaseNumber)
                            CaseNumber = ""
                        ElseIf xRead.Name = "JUDGMENT_DATE" Then



                            JudgementDate = "<span style='-webkit-transition:1s;-moz-transition:1s;transition:1s;font-family:CalistoMTStdRegular;font-size:16px;display:block;'>" & Trim(xRead.ReadInnerXml) & "</span>"  'xRead.ReadOuterXml
                            If SearchWords <> "" Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & SearchWords & "\b"
                                JudgementDate = Regex.Replace(JudgementDate, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If

                            sbXml.Append(JudgementDate)
                            JudgementDate = ""

                        ElseIf xRead.Name = "HEADNOTE" Then

                            Dim ReplaceWith As String
                            Dim FirstWord, LastWord As String
                            JudgementBody = xRead.ReadInnerXml

                            For i = 0 To COL.Count - 1
                                If i = 0 Then
                                    FirstWord = COL.Item(0)
                                Else
                                    LastWord = COL.Item(i)
                                End If
                            Next

                            FirstWord = Trim(FirstWord)
                            LastWord = Trim(LastWord)

                            If ((FirstWord <> "") And (LastWord <> "")) Then
                                'Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                Pattern = "\b" & FirstWord & "\w" & LastWord & "\b"
                                JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:blue;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            If SearchWords <> "" Then

                                Pattern = "\b" & SearchWords & "\b"
                                JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & SearchWords & "</b></span>", RegexOptions.IgnoreCase)
                            End If
                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayed.aspx?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationSectiondisplayformat.asp?", "<b><a  href=""../legislationSectionDisplayed.aspx?fn=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""legislationdisplayformat.asp?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = Regex.Replace(JudgementBody, "<LINK HREF=""LegislationMainDisplayed.aspx?", "<b><a  href=""../LegislationMainDisplayed.aspx?id=", RegexOptions.IgnoreCase)
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")

                            JudgementBody = JudgementBody.Replace("<LINK HREF=""case_notes/showcase.aspx?pageid=", "<b><a  href=" & Chr(34) & "showcase.aspx?id=")
                            JudgementBody = Regex.Replace(JudgementBody, "</LINK>", "</a></b>")
                            '============================================
                            ' JudgementBody = Regex.Replace(JudgementBody, "("".*\?)(.*)""", encryptUrl("$1", "$2"))
                            Dim mee As New MatchEvaluator(AddressOf encrypturl)
                            JudgementBody = Regex.Replace(JudgementBody, "href=""([^""]*)", mee)
                            '============================================
                            sbXml.Append(JudgementBody)

                            JudgementBody = ""


                        ElseIf xRead.Name = "CASE_PROGRESSION" Then
                            Dim ReplaceWith As String
                            CaseAppeal = ""
                            Dim msg As String
                            Try
                                Dim addspan As String = ""

                                CaseAppeal = xRead.ReadInnerXml
                                If CaseAppeal.Contains("MLRH") Or CaseAppeal.Contains("MELR") Or CaseAppeal.Contains("MLRA") Then
                                    CaseAppeal = CaseAppeal.Replace("<p>Case History : </p>", "")
                                    CaseAppeal = CaseAppeal.Replace("""", "'")
                                    CaseAppeal = CaseAppeal.Replace(";", "")
                                    CaseAppeal = CaseAppeal.Replace("<br />", " <div style='width:100%;clear:both;padding:5px;'></div>")
                                    '<LINK HREF="case_notes/showcase.aspx?pageid=
                                    Pattern = "<LINK HREF='case_notes/showcase.aspx?pageid="
                                    ReplaceWith = "<span class='boxNotify1'><strong><a href='showcase.aspx?id="
                                    CaseAppeal = CaseAppeal.Replace(Pattern, ReplaceWith)
                                    'CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    Pattern = "</LINK>"
                                    ReplaceWith = "</a></strong></span>;"
                                    CaseAppeal = Regex.Replace(CaseAppeal, Pattern, ReplaceWith, RegexOptions.IgnoreCase)
                                    CaseAppeal = CaseAppeal.Replace("'", """")
                                    '============================================

                                    Dim mee As New MatchEvaluator(AddressOf encrypturl)
                                    CaseAppeal = Regex.Replace(CaseAppeal, "href=""([^""]*)", mee)
                                    '============================================
                                    CasePrograssion = CaseAppeal

                                End If
                                CaseAppeal = ""
                            Catch err As Exception
                                msg = err.Message()

                            Finally
                                ReplaceWith = ""
                                msg = ""

                            End Try


                        End If

                End Select



            End While

        Catch ex As Exception
            lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
            TopLeftCase.Visible = False
            TopRightCase.Visible = False
        Finally
            xRead.Close()
        End Try


        ltrshowcase.Text = sbXml.ToString()

        'lblPdf.Text &= "<b><a href=" & Chr(34) & "casespdfopener.aspx?f=" & filename1 & Chr(34) & " ><img border=0 src=" & Chr(34) & "images1/pdf.gif" & Chr(34) & "> <font face=Vardana size=2 color=#CC0000>" & "Printable Version</a></b></font></br>"
        sbXml = Nothing
        objUtil = Nothing
    End Sub



    <System.Web.Services.WebMethod()> _
    Public Shared Function saveDoc(ByVal pageid As String) As Boolean
        Dim user As String
        Dim docname As String
        Dim docDate As DateTime = DateTime.Now.ToString
        Dim title As String
        Dim obj As New clsCasesSearch
        title = Nothing
        docname = pageid
        user = (New showcase).Session("UserName")
        Dim sql As String
        Dim UrlDecrpt As New Dba_UrlEncrption(docname, False)
        Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
        'Dim mysqlreader As SqlDataReader
        sql = "SELECT TITLE from cases WHERE DATAFILENAME='" & UrlQuery & ".xml'"
        title = obj.ExecuteMyQuery(sql).Rows(0).Item(0).ToString()
        If title.Length > 100 Then
            title = Left(title, 100)
        End If
        title = title.Replace("'", "''")
        If Not UrlQuery = Nothing Then
            sql = "INSERT INTO SavedCasesLinks (CaseUserName,XmlCaseName,CaseSavedDate,CaseJudgeName) values ( '" & user & "','" & UrlQuery & "','" & docDate & "','" & title & "' )"

            If (obj.UpdateRecord2(sql)) Then
                Return True
            Else
                Return False
            End If

        Else
            Return False
        End If

        Return False


    End Function



    Private Sub SearchData()

        Dim Obj As New clsSearch
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

        If Len(Trim(txtExactPhrase.Text)) = 0 Then ' And Len(Trim(txtCounsel.Text)) = 0 Then
            lblMsg.Text = "*At least one of the red fields must be filled" ' "Enter the Keywords for Searching Cases in All or any of the words for searching"
            txtExactPhrase.BackColor = Drawing.Color.FromArgb(255, 220, 220)
            Exit Sub
        End If

        SearchExactCases = Trim(txtExactPhrase.Text)
        SearchNotCases = Trim(txtNotCases.Text)

        Dim count1 As Integer = CountWords(SearchExactCases)
        If count1 >= 2 Then
            If ddlprox.SelectedValue = "" Then
                If ddlproxmity.SelectedValue = 3 Then
                    iProximityLevel = 3
                ElseIf ddlproxmity.SelectedValue = 2 Then
                    iProximityLevel = 2
                ElseIf ddlproxmity.SelectedValue = 1 Then
                    iProximityLevel = 1

                End If
            ElseIf ddlprox.SelectedValue <> "" Then
                iProximityWithin = Convert.ToInt16(Trim(ddlprox.SelectedValue))
                iProximityLevel = 4
            End If
        Else
            iProximityLevel = 0
        End If

        If cbIndustrialCourt.Checked = True Then
            SearchIndustrialCourt = 1
        End If
        SearchTypeinNo = rbs.SelectedValue
        Year1 = 1894
        Year2 = Date.Now.Year
        Dim UrlEncrption As String = "ec=" & SearchExactCases & "&nc=" & SearchNotCases & "&y1=" & Year1 & "&y2=" & Year2 & "&crt=" & SearchCourt & "&icrt=" & SearchIndustrialCourt & "&cn=" & SearchCaseNumber & "&j=" & SearchJudge & "&srchtpe=" & SearchTypeinNo & "&prxOpt=" & iProximityLevel & "&legT=&prxOptWit=" & iProximityWithin
        Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
        Dim link As String = UrlLink.UrlEncrypt
        URL = "~/casessearchresult1.aspx?info=" & link

        'URL = "~/casessearchresult1.aspx?ec=" & SearchExactCases & "&nc=" & SearchNotCases & "&y1=" & Year1 & "&y2=" & Year2 & "&crt=" & SearchCourt & "&icrt=" & SearchIndustrialCourt & "&cn=" & SearchCaseNumber & "&j=" & SearchJudge & "&srchtpe=" & SearchTypeinNo & "&prxOpt=" & iProximityLevel & "&prxOptThesaurus=" & iOption & "&legT=" & "&prxOptWit=" & iProximityWithin
        URL = Server.UrlPathEncode(URL)
        Response.Redirect(URL)

    End Sub
    Public Function CountWords(ByVal value As String) As Integer
        ' Count matches.
        Dim collection As MatchCollection = Regex.Matches(value, "\S+")
        Return collection.Count
    End Function
    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        SearchData()
    End Sub
    ''''''Check for headnotes tags if exist
    Function CheckElement(ByVal filename As String, ByVal name As String) As Boolean
        Dim checkHeadnotesTag As Boolean = False
        Dim doc As New System.Xml.XmlDocument

        Try
            doc.Load(filename)
            Dim list = doc.GetElementsByTagName(name)
            'list = doc.GetElementsByTagName("COURT_TYPE")
            For Each item As System.Xml.XmlElement In list
                If item.Name = name Then
                    checkHeadnotesTag = True
                End If

            Next
            'Return doc.Descendants(name).Any()

        Catch ex As Exception
            checkHeadnotesTag = False
        End Try
        Return checkHeadnotesTag
    End Function

    Private Sub CheckUserHistory(ByVal fn As String)
        Dim sql As String = "select * from CasesViewHistory where UserName='" & Session("UserName") & "' and DateFileName='" & fn & "'"
        Dim obj As New clsCasesSearch
        Dim dt = New DataTable
        Dim docDate As DateTime = DateTime.Now.ToString
        dt = obj.ExecuteMyQuery(sql)

        If dt.Rows.Count > 0 Then
            Dim CountViewtime As Integer = dt.Rows(0).Item("NumberViewTimes")
            CountViewtime = CountViewtime + 1
            Dim updatelastview As String = "update CasesViewHistory set LastViewDate='" & docDate & "',NumberViewTimes=" & CountViewtime & "  where DateFileName='" & fn & "' and UserName='" & Session("UserName") & "'"
            obj.UpdateRecord(updatelastview)
        Else
            Dim sqltitle As String
            sqltitle = "SELECT TITLE from cases WHERE DATAFILENAME='" & fn & ".xml'"
            dt = obj.ExecuteMyQuery(sqltitle)
            If dt.Rows.Count > 0 Then
                Title = dt.Rows(0).Item(0)
                'Title = obj.ExecuteMyQuery(sqltitle).Rows(0).Item(0).ToString()
                If Title.Length > 100 Then
                    Title = Left(Title, 100)
                End If
                Title = Title.Replace("'", "''")
                ' Title = Title.Replace("&AMP;", "&").Replace("&amp;", "&").Replace(" amp;", " &")
                Dim InsertViewHistory As String = "INSERT INTO [dbo].[CasesViewHistory]" & _
              " ([DateFileName],[UserName] ,[LastViewDate] ,[NumberViewTimes],[CaseTitle]) " & _
                " VALUES('" & fn & "' ,'" & Session("username") & "','" & docDate & "','1','" & Title & "')"
                obj.AddRecord(InsertViewHistory)
            End If

        End If

    End Sub
End Class
