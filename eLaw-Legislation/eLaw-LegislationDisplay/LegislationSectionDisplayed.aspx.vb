Imports System.Xml
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data
Imports Dba_UrlEncrption

Namespace membersarea


    Partial Class LegislationSectionDisplayed1
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
        Dim fileName As String
        Dim SearchWords As String
        Dim SearchKeyword As String = "" '///////////////////////////// use this one for title finding
        Dim SectionNo As String
        Public ActNot As String
        Public LegilationNavgation As String ' use for navgation between 
        Public Shared FileNameSection As String
        Public ListSection As String ' rerive and append its value to listAllSection
        Dim listAllSection As New StringBuilder ' Store section title, No 
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim Type As String = "" ' type of request to display , section content, preamble ....
            Dim LegislationSection As String
            Dim SCHEDULENo As String
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                'Server.Transfer("login.aspx")
                Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If

            '==============================Received Data===================
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
                                Case "fn"
                                    fileName = UrlQuery(i).parvalue
                                Case "sw"
                                    SearchKeyword = UrlQuery(i).parvalue
                                Case "tp"
                                    Type = UrlQuery(i).parvalue
                                Case "sn"
                                    SectionNo = UrlQuery(i).parvalue
                                    SectionNo = Trim(SectionNo)
                                Case "schNO"
                                    SCHEDULENo = UrlQuery(i).parvalue
                                    SCHEDULENo = Convert.ToInt32(SCHEDULENo)

                            End Select
                        End If
                    Next
                End If
            ElseIf Request.QueryString("id") <> "" Then
                Dim TempID As String = Request.QueryString("id")
                Dim UrlDecrpt As New Dba_UrlEncrption(TempID, False)
                Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
                fileName = UrlQuery
            ElseIf Request.QueryString("fn") <> "" Then
                fileName = Request.QueryString("fn")
                Type = Request.QueryString("tp")
                SCHEDULENo = Convert.ToInt32(Request.QueryString("schNO"))
            Else

                Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If
            '==============================================================

            SearchWords = SearchKeyword

            LegislationSection = Replace(fileName, ".xml", "") & ";" & Replace(SectionNo, ".", "")
            Dim ObjCS As New clsCasesSearch()
            If ObjCS.CheckActNoterUp(LegislationSection) = True Then
                Dim IdLink As String = LegislationSection.Trim()
                Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
                Dim linkFileId As String = UrlFIleID.UrlEncrypt
                ActNot = "<a href=LegislationReferredCases.aspx?fn=" & linkFileId & " >Cases Referred</a>"
                'ActNot = "<a href=LegislationReferredCases.aspx?fn=" & linkFileId & " > Legislation Citation </a>"
            End If
            If Type <> "" Then
                fileName = fileName & ".xml"
                If Type = "loa" Then
                    DisplayLegislation_LOA(fileName, SearchWords)
                    Navigation()
                    AllSection(fileName, SearchWords)
                    Exit Sub
                End If
                If Type = "pre" Then
                    DisplayLegislation_PRE(fileName, SearchWords)
                    Navigation()
                    AllSection(fileName, SearchWords)
                    Exit Sub
                End If
                If Type = "sch" Then
                    DisplayLegislation_SCH(fileName, SearchWords, SCHEDULENo)
                    Navigation()
                    AllSection(fileName, SearchWords)
                    Exit Sub
                End If
            ElseIf SectionNo <> "" Then
                fileName = fileName & ".xml"
                DisplayLegislation_Sec(fileName, SearchWords, SectionNo)

            Else
                Dim Identifier As Char = ";"
                Dim InfoArr() As String
                'fileName = Replace(fileName, "?", "")
                InfoArr = Split(fileName, ";")
                fileName = InfoArr(0)
                fileName = Replace(fileName, "?", "")
                SectionNo = InfoArr(1)
                SectionNo = SectionNo.Replace("%20", " ")
                DisplayLegislation_Sec(fileName, SearchWords, SectionNo)
            End If
            AllSection(fileName, SearchWords)
            Navigation()
            If Me.IsPostBack = False Then
                lblMsg.EnableViewState = False
                lblXml.EnableViewState = False
                lblPageTop.EnableViewState = False

            End If
            ObjCS = Nothing
            Type = ""
        End Sub
        Private Sub DisplayLegislation_LOA(ByVal FileName As String, ByVal SearchWords As String)

            Dim Title As String
            Dim Number As String
            Dim ListOfAmendments As String

            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName))
            Dim LegislationDisplayFormat As New System.Text.StringBuilder()
            SearchWords = Trim(SearchWords)
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "TITLE" Then
                                Title = Trim(xRead.ReadInnerXml)
                                lblPageTop.Text = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Title & "</b></font></p></br>"
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Number & "</b></font></p></br>"

                            ElseIf xRead.Name = "NUMBER" Then
                                Number = Trim(xRead.ReadInnerXml)
                            ElseIf xRead.Name = "COUNTRY" Then
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p></br>"
                            ElseIf xRead.Name = "LISTOFAMENDMENTS" Then
                                ListOfAmendments = xRead.ReadOuterXml
                                ListOfAmendments = Regex.Replace(ListOfAmendments, "</?(a|A).*?>", "")
                            End If

                    End Select

                End While
            Catch ex As Exception
                lblMsg.Text = ex.Message
            Finally
                xRead.Close()
                Title = ""
                Number = ""

            End Try
            lblXml.Text = ListOfAmendments

        End Sub

        Private Sub DisplayLegislation_PRE(ByVal FileName As String, ByVal SearchWords As String)

            Dim Title As String
            Dim Number As String
            Dim Preamble As String
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName))
            Dim LegislationDisplayFormat As New System.Text.StringBuilder()
            SearchWords = Trim(SearchWords)
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "TITLE" Then
                                Title = Trim(xRead.ReadInnerXml)
                                lblPageTop.Text = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Title & "</b></font></p></br>"
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Number & "</b></font></p></br>"

                            ElseIf xRead.Name = "NUMBER" Then

                                Number = Trim(xRead.ReadInnerXml)

                            ElseIf xRead.Name = "COUNTRY" Then
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p></br>"


                            ElseIf xRead.Name = "PREAMBLE" Then
                                Preamble = xRead.ReadOuterXml

                            End If

                    End Select

                End While
            Catch ex As Exception
                lblMsg.Text = ex.Message
            Finally
                xRead.Close()
                Title = ""
                Number = ""

            End Try

            lblXml.Text = Preamble
            Preamble = ""
        End Sub

        Private Sub DisplayLegislation_SCH(ByVal FileName As String, ByVal SearchWords As String, ByVal schNo As Integer)

            Dim Title As String
            Dim Number As String
            Dim Schedule As String
            Dim CountSch As Integer
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName))
            Dim LegislationDisplayFormat As New System.Text.StringBuilder()
            SearchWords = Trim(SearchWords)
            CountSch = 0
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "TITLE" Then
                                Title = Trim(xRead.ReadInnerXml)
                                lblPageTop.Text = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Title & "</b></font></p></br>"
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Number & "</b></font></p></br>"
                            ElseIf xRead.Name = "NUMBER" Then
                                Number = Trim(xRead.ReadInnerXml)
                            ElseIf xRead.Name = "COUNTRY" Then
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p></br>"
                            ElseIf xRead.Name = "SCHEDULE" Then
                                CountSch = CountSch + 1
                                If CountSch = schNo Then
                                    Schedule = "<div style='float:right'><a href='#' class='fontSizePlus'>A+</a> | <a href='#' class='fontReset'>Reset</a> | <a href='#' class='fontSizeMinus'>A-</a></div><div style='clear:both'></div>"
                                    Schedule &= "<div class='intro'>" & xRead.ReadOuterXml & "</div>"
                                End If
                            End If
                    End Select
                End While
            Catch ex As Exception
                lblMsg.Text = ex.Message
            Finally
                xRead.Close()
                Title = ""
                Number = ""

            End Try

            lblXml.Text = Schedule
            Schedule = ""
        End Sub



        Private Sub DisplayLegislation_Sec(ByVal FileName As String, ByVal SearchWords As String, ByVal SecNo As String)

            Dim Title As String
            Dim Number As String
            Dim ResultSection As String
            If FileName.Contains("xml") = False Then
                FileName = FileName & ".xml"
            End If
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName))
            Dim Nodelst As XmlNodeList
            Dim NodelstSection As XmlNodeList
            Dim XDoc As New XmlDocument()
            Try
                XDoc.Load(Server.MapPath("xmlFiles\legislation\" & FileName))
                Dim Count As Int16 = 0
                Dim CountSection As Int16 = 0
                Dim currentSectionNo As Integer = 0
                Dim CurSecNo As Int16 = 0
                Dim i As Integer
                Dim PrevSecNoFLG As Boolean = False
                Dim NextSecNoFLG As Boolean = False
                Dim SectionFound As Boolean = False
                SearchWords = Trim(SearchWords)


                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "TITLE" Then
                                Title = Trim(xRead.ReadInnerXml)

                                lblPageTop.Text = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Title & "</b></font></p></br>"
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Number & "</b></font></p></br>"
                                Exit While ' As there is no use of parsing the whole legislation xml file as we get our required info
                            ElseIf xRead.Name = "NUMBER" Then
                                Number = Trim(xRead.ReadInnerXml)
                            ElseIf xRead.Name = "COUNTRY" Then
                                lblPageTop.Text &= "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 ><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p></br>"
                            End If
                    End Select
                End While
                ' Find specific  section 
                NodelstSection = XDoc.GetElementsByTagName("SECTION")
                Nodelst = XDoc.GetElementsByTagName("SNO")

                For i = 0 To Nodelst.Count - 1
                    Dim TemSection As String
                    TemSection = Nodelst(i).InnerText
                    If TemSection.Contains(SecNo) = True Then
                        TemSection = NodelstSection(i).OuterXml
                        TemSection = Regex.Replace(TemSection, "</?(a|A).*?>", "")
                        ResultSection = "<div id='printdiv' style='float:right'>" & ActNot & " | <a href='#' class='fontSizePlus'>A+</a> | <a href='#' class='fontReset'>Reset</a> | <a href='#' class='fontSizeMinus'>A-</a></div><div style='clear:both'></div>"
                        ResultSection &= "<div class='intro'>" & TemSection & "</div>"
                        ResultSection = ResultSection.Replace("<TERM>", "<font face=Vardana size=3 color=blue>&nbsp;")
                        ResultSection = ResultSection.Replace("</TERM>", "</font>&nbsp;")

                        Exit For
                    End If
                Next i
                If ResultSection = "" Then
                    ResultSection = "Section Not avaiable ,Please search for other section"
                End If
            Catch ex As Exception
                lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents.Legislation Details will Be Update Soon !</span></div>"
            Finally
                xRead.Close()
                Title = ""
                Number = ""
            End Try
            lblXml.Text = ResultSection
        End Sub

        Sub AllSection(ByVal FileName As String, ByVal SW As String)
            Dim SectionTitle As String
            Dim SectionNo As String
            If FileName.Contains("xml") = False Then
                FileName = FileName & ".xml"
            End If
            Try
                Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName))
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "SNO" Then
                                SectionNo = Trim(xRead.ReadInnerXml)
                                Dim IdLink As String = FileName.Replace(".xml", "").Trim() & "-" & SectionNo
                                Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
                                Dim linkFileId As String = UrlFIleID.UrlEncrypt
                                listAllSection.Append("<li onclick=LoadContent('" & linkFileId & "')><a style='cursor:pointer'><small> " & SectionNo & "</small><div>")
                            End If
                            If xRead.Name = "ST" Then
                                SectionTitle = Trim(xRead.ReadInnerXml)
                                If SearchWords <> "" Then
                                    SectionTitle = Regex.Replace(SectionTitle, SearchWords, "<span class='hWord'>" & SearchWords & "</span>", RegexOptions.IgnoreCase)
                                End If
                                listAllSection.Append(SectionTitle & "</div></a></li>")
                            End If
                    End Select
                End While
            Catch ex As Exception
                lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Section not available!<br> Sorry for any inconvenience caused.</span></div>"
            End Try
            ListSection = listAllSection.ToString
        End Sub
        Private Sub Navigation()
            LegilationNavgation = ""
            fileName = fileName.Replace(".xml", "")
            Dim UrlEncrption As String = "fn=" & fileName & "&tp=loa"
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            Dim UrlEncrption1 As String = "fn=" & fileName & "&tp=pre"
            Dim UrlLink1 As New Dba_UrlEncrption(UrlEncrption1, True)
            Dim link1 As String = UrlLink1.UrlEncrypt
            Dim UrlEncrptionpage As String = "id=" & fileName & "&tp=pre"
            Dim UrlLinkpage As New Dba_UrlEncrption(UrlEncrptionpage, True)
            Dim linkpage As String = UrlLinkpage.UrlEncrypt
            LegilationNavgation &= "&nbsp;&nbsp;<span><a href=" & Chr(34) & "LegislationSectionDisplayed.aspx?info=" & link & Chr(34) & "><img border='0' src='GUI/NewDesign/img/iconListOfAmends.png'/>List of Amendments</a></span>  "
            LegilationNavgation &= "&nbsp;&nbsp;<span><a href=" & Chr(34) & "LegislationSectionDisplayed.aspx?info=" & link1 & Chr(34) & "><img border='0' src='GUI/NewDesign/img/iconListOfPreamble.png'/>Preamble</a></span>  "
            LegilationNavgation &= "&nbsp;&nbsp;<span><a href=" & Chr(34) & "LegislationMainDisplayed.aspx?info=" & linkpage & Chr(34) & ">Legislation Main Page</a></span>  "



        End Sub
        Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
            Dim URL As String
            Dim obj As New clsSearch
            Dim SearchNotlegislation As String = ""
            Dim S_SreachType As String = ""
            Dim searchFTS As String = ""
            Dim searchLegislation As String = ""
            Dim searchCountry As String = ""
            Dim searchActs As String = ""
            Dim searchActNumber As String = ""
            Dim iProximityWithin As String = ""
            Dim iProximityLevel As String = ""

            Dim iOption As String = " "

            'If Len(Trim(txtLegislation.Text)) = 0 And Len(Trim(txtFTS.Text)) = 0 And rblSearchType.SelectedItem.Text = "" And cblCountries.SelectedItem.Text = "" Then

            SearchNotlegislation = Trim(txtNotCases.Text)
            searchFTS = Trim(txtFTS.Text)
            S_SreachType = rbs.SelectedValue

            If searchFTS.Split(" ").Length() < 2 Then
                iProximityLevel = 0
                iOption = 0
            Else
                If ddlprox.SelectedValue = "" Then
                    If ddlproxmity.SelectedValue = 3 Then
                        iProximityLevel = 3
                    ElseIf ddlproxmity.SelectedValue = 2 Then
                        iProximityLevel = 2
                    ElseIf ddlproxmity.SelectedValue = 1 Then
                        iProximityLevel = 1

                    End If
                ElseIf ddlprox.SelectedValue <> "" Then
                    iProximityWithin = Trim(ddlprox.SelectedValue)
                    iProximityLevel = 4

                End If

                iOption = 0      ' Convert.ToInt16(Trim(ddl_phrase.SelectedValue)) ' /// 1=inflection; 2= thesaurus
            End If
            Dim UrlEncrption As String = "ft=" & searchFTS & "&nc=" & SearchNotlegislation & "&st=" & S_SreachType & "&an=" & searchActNumber & "&prxOpt=" & iProximityLevel & "&prxOptWit=" & iProximityWithin
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt



            URL = "legislationsearchresult.aspx?info=" & link

            URL = Server.UrlPathEncode(URL)
            Response.Redirect(URL)
        End Sub
        <System.Web.Services.WebMethod()> _
        Public Shared Function getSection() As ArrayList
            Dim suggestions As New ArrayList
            Dim Nodelst As XmlNodeList
            Dim XDoc As New XmlDocument()
            Dim Strfile As String = FileNameSection

            Dim Count As Int16 = 0
            Dim SectionNo As String
            Dim i As Integer

            XDoc.Load(HttpContext.Current.Server.MapPath("xmlFiles\legislation\" & Strfile))
            Nodelst = XDoc.GetElementsByTagName("SNO")
            'For i = 0 To Nodelst.Count - 1
            For i = 0 To Nodelst.Count - 1
                SectionNo = Nodelst(i).InnerXml()
                suggestions.Add(SectionNo)
            Next i
            'For i = 0 To dt.Rows.Count - 1
            '    suggestions.Add(dt.Rows(i).Item(0).ToString.Trim())
            'Next
            Return suggestions
        End Function

        <System.Web.Services.WebMethod()> _
        Public Shared Function getSectionCont(ByVal SecReceiv As String) As String
            Dim SectionContent As String
            Dim ReceivedSec As String
            Dim TempContent As String
            Dim InfoArr() As String
            Dim LegislationSection As String
            Dim CheckActNot As String
            Dim UrlDecrpt As New Dba_UrlEncrption(SecReceiv, False)
            Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
            'fileName = Replace(fileName, "?", "")
            InfoArr = Split(UrlQuery, "-")
            FileNameSection = InfoArr(0)
            ReceivedSec = InfoArr(1)

            If Left(FileNameSection, 3) <> "xml" Then
                FileNameSection = FileNameSection & ".xml"
            End If
            Dim xRead As XmlTextReader = New XmlTextReader(HttpContext.Current.Server.MapPath("xmlFiles\legislation\" & FileNameSection))
            Try


                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element

                            If xRead.Name = "SECTION" Then
                                TempContent = xRead.ReadInnerXml
                                If TempContent.Contains("<SNO>" & ReceivedSec & "</SNO>") = True Then
                                    TempContent = Regex.Replace(TempContent, "</?(a|A).*?>", "")
                                    LegislationSection = InfoArr(0) & ";" & InfoArr(1).Replace(".", "")
                                    Dim ObjCS As New clsCasesSearch()
                                    If ObjCS.CheckActNoterUp(LegislationSection) = True Then
                                        Dim UrlLink As New Dba_UrlEncrption(LegislationSection, True)
                                        Dim linkPrec As String = UrlLink.UrlEncrypt
                                        CheckActNot = "<a href=LegislationReferredCases.aspx?fn=" & linkPrec & " >Cases Referred</a>"


                                    End If
                                    SectionContent = "<div id='printdiv' style='float:right'>" & CheckActNot & " | <a href='#' class='fontSizePlus'>A+</a> | <a href='#' class='fontReset'>Reset</a> | <a href='#' class='fontSizeMinus'>A-</a></div><div style='clear:both'></div>"
                                    SectionContent &= "<div class='intro'>" & TempContent & "</div>"
                                    SectionContent = SectionContent.Replace("<TERM>", "<font face=Vardana size=3 color=blue>&nbsp;")
                                    SectionContent = SectionContent.Replace("</TERM>", "</font>&nbsp;")

                                    Exit While
                                End If
                            End If

                    End Select

                End While
            Catch ex As Exception

            End Try


            Return SectionContent
        End Function


    End Class

End Namespace
