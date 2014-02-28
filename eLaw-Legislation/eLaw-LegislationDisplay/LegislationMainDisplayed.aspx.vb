'/**********************************************************************/
'/*	Developer 	    : Modify By Ali 								   */
'/*	Company     	: The Digital Library Sdn. Bhd.		    					   */
'/* Date Modified	: latest update 4-12-2013	        					*/  
'/*	Description		: Legislation Main page which contains sections etc         */
'/*	Version			: 1.0											   */
'/**********************************************************************/

Imports System.Xml
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data
Imports Dba_UrlEncrption

Namespace membersarea


    Partial Class LegislationMainDisplayed1
        Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub



#End Region

        Dim UserName As String
        Dim ActNo As String
        Dim Country As String
        Dim fileName As String
        Public GetSubSidairy As String ' To View Subsidaries inside browse tab , and also to load 1st 10 result 
        Public CountSubSidairy As Integer ' Count number of subsidaries files related to act ,if has result print GetSubSidairy value
        Public GetAmdment As String ' ' To View Amdment inside browse tab , and also to load 1st 10 result , plus print link to view as classic view  
        Public CountAmd As Integer ' Count number of Amdment files related to act ,if has result print GetSubSidairy value
        Public MoreSubsiary As String ' ,  print link to view all subsidaries files 
        Public MoreAmd As String ' ,  print link to view all Amdment files 
        Public ListSchudle As String ' print Schudle content while reading act files
        Public ListPremple As String ' print link to view Premple related to current act
        Public ListAmendments As String ' print link to view Amendments related to current act
        Public PartStyle As String ' make shortcut to view legislation by parts , print anchor
        Public ListSection As String ' Store section title, links, Number in browse tab , use with jquery function 
        Public mainact As String 'the is for the mainid paramerters to keep the main act 
        Dim SearchWords As String ' Store words for highlight
        
        
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            Dim objUtil As New clsMyUtility()
            Dim ObjCs As New clsCasesSearch()
            Dim Temp As String
            Dim TLfile As String ' Store file name for timeline
            Dim TLsel As String ' store current select file from timeline 
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                'Response.Redirect("login.aspx")
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
                                Case "id"
                                    fileName = UrlQuery(i).parvalue
                                Case "sw"
                                    SearchWords = UrlQuery(i).parvalue
                                    SearchWords = objUtil.RefineSentence(SearchWords)
                                Case "mainid"
                                    mainact = UrlQuery(i).parvalue

                            End Select
                        End If
                    Next
                    If mainact <> "" Then
                        Dim encurl As New Dba_UrlEncrption(mainact, True)
                        Dim encryptedacturl As String = encurl.UrlEncrypt
                        mainacthidden.Value = encryptedacturl
                    End If
                End If
            ElseIf Request.QueryString("id") <> "" Then
                fileName = Request.QueryString("id")
                Dim UrlDecrpt As New Dba_UrlEncrption(fileName, False)
                Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
                fileName = UrlQuery

                mainact = Request.QueryString("mainid")
                If mainact <> "" Then
                    Dim encurl As New Dba_UrlEncrption(mainact, True)
                    Dim encryptedacturl As String = encurl.UrlEncrypt
                    mainacthidden.Value = mainact

                End If

            Else

                'Response.Redirect("~/login.aspx")
                Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If
            'My.Computer.FileSystem.WriteAllText("c:\abdo\checkleg.txt", mainact & vbCrLf, True)
            '========================================
            Dim IdLink As String = fileName
            Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
            Dim linkFileId As String = UrlFIleID.UrlEncrypt

            Id.Text = linkFileId
            TLfile = Request.QueryString("TLF")
            TLsel = Request.QueryString("TLS")
            If TLfile <> "" Then
                If TLsel <> "" Then
                    TLF.Attributes.Add("sel", TLsel)
                End If
                TLF.Text = TLfile
            End If

            If mainacthidden.Value.Trim() = "" Then
                mainacthidden.Value = ""
            End If
            If Me.IsPostBack = False Then
                DisplayLegislationNewFormat(fileName, SearchWords, mainacthidden.Value)
                Dim URL As String
                '==============Check First If Act No Not Empty==
                If ActNo <> "" Then
                    CountSubSidairy = ObjCs.CheckSubsidiaryActForMainLegislation1(ActNo, Trim(Country))
                    If CountSubSidairy > 0 Then
                        ActNo = ActNo.Replace(" ", "_")
                        Dim IdLinks As String = "t=s&no=" & ActNo & "&ctry=" & Country & "&DivId=0"
                        Dim UrlFIles As New Dba_UrlEncrption(IdLinks, True)
                        Dim linkFiles As String = UrlFIles.UrlEncrypt
                        URL = "LegislationSubDisplay.aspx?info=" & linkFiles
                        GetSubSidairy = URL & "&DivId=1"
                        MoreSubsiary = "<a href='" & URL & "'>View More Subsidiary Acts</a>"
                    Else
                        CountSubSidairy = 0
                        MoreSubsiary = "<a href='#'>No Result For Subsidiary Acts</a>"
                    End If
                    CountAmd = ObjCs.CheckAmendingActForMainLegislation1(ActNo, Trim(Country))
                    If CountAmd > 0 Then
                        ActNo = ActNo.Replace(" ", "_")
                        Dim IdLinka As String = "t=a&no=" & ActNo & "&ctry=" & Country & "&DivId=0"
                        Dim UrlFIlea As New Dba_UrlEncrption(IdLinka, True)
                        Dim linkFilea As String = UrlFIlea.UrlEncrypt

                        URL = "LegislationSubDisplay.aspx?info=" & linkFilea
                        GetAmdment = URL & "&DivId=1"
                        MoreAmd = "<a href='" & URL & "'>View More Amendment</a>"
                    Else
                        CountAmd = 0
                        MoreAmd = "<a href='#'>No Result For Amendment</a>"
                    End If

                End If
                lblMsg.EnableViewState = False
                lblXml.EnableViewState = False
                lblPageTop.EnableViewState = False
            End If '' end if postback

            ObjCs = Nothing

            If IsPostBack = False Then
                'Store file size for use daily usage
                clsMyUtility.updateQuota(fileName.Trim(), Session("username").ToString(), Server.MachineName.ToString(), Request.UserHostAddress.ToString(), 2, 0)
            End If
        End Sub

        Sub DisplayLegislationNewFormat(ByVal FileName As String, ByVal SearchWords As String, ByVal strmainact As String)
            Dim objUtil As New clsMyUtility()
            Dim Title As String
            Dim Number As String
            Dim RoyalAssent As String
            Dim TableInParliament As String
            Dim InforceFrom As String
            Dim PrincipalActNo As String
            Dim PrincipalActTitle As String
            Dim GazetteDate As String
            Dim ListOfAmendments As String
            Dim Preamble As String
            Dim Section As String
            Dim Schedule As String
            Dim PartNumber As String
            Dim PartTitle As String
            Dim copyPart As String
            Dim TempSection As String
            Dim i As Int16
            Dim ArrLst As New ArrayList()
            Dim noShorLinks As Int16 = 1
            Dim tmpPartNo As String
            Dim listAllSection As New StringBuilder
            Dim CountSchedule As Integer
            Dim COL As New Collections.ArrayList
            SearchWords = Trim(SearchWords)
            COL = objUtil.Tokenizer(SearchWords)
            Dim FirstWord, Lastword As String
            Dim SeCount As Integer = 0 ' Count All Section 
            'Clear File Name from other chars 

            FileName = FileName.Replace("?", "").Replace(";", "")
            If FileName.IndexOf(".xml", 0, FileName.Length, StringComparison.CurrentCultureIgnoreCase) = -1 Then
                FileName &= ".xml"
            End If
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName))
            Dim strNewText As String
            Dim LegislationDisplayFormat As New System.Text.StringBuilder()
            Dim ShortLinks As New System.Text.StringBuilder()
            Dim Nodelst As XmlNodeList
            Dim firstTime As Boolean = True
            FileName = FileName.Replace(".xml", "")
            SearchWords = Trim(SearchWords)
            '''' for main act
            Dim IdLinkmain As String = "id=" & strmainact & "&mainid=" & strmainact
            Dim legLinkmain As String = "LegislationMainDisplayed.aspx?" & IdLinkmain
            Dim flgchk As Boolean = False
            If strmainact.ToString().Trim() <> "" Then
                flgchk = True
            End If
            CountSchedule = 0
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "TITLE" Then
                                Title = Trim(xRead.ReadInnerXml)
                                lblPageTop.Text = Title
                                If flgchk = True Then
                                    lblPageTop.Text &= "&nbsp;&nbsp;" & "[" & Number & " ]" & "       <a href=" & legLinkmain & " style='text-align:right;'> Main Legislation</a>"
                                Else
                                    lblPageTop.Text &= "&nbsp;&nbsp;" & "[" & Number & " ]"
                                End If

                            ElseIf xRead.Name = "NUMBER" Then
                                Number = Trim(xRead.ReadInnerXml)
                                ActNo = Number
                            ElseIf xRead.Name = "COUNTRY" Then
                                Country = Trim(xRead.ReadInnerXml)
                            ElseIf xRead.Name = "ROYALASSENT" Then
                                RoyalAssent = Trim(xRead.ReadInnerXml)
                                If RoyalAssent <> "" Then
                                    LegislationDisplayFormat.Append("<p align=justify style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=2 ><i>" & RoyalAssent & "</i></font></p></br>")
                                End If
                            ElseIf xRead.Name = "TABLEINPARLIAMENT" Then

                                TableInParliament = Trim(xRead.ReadInnerXml)
                                If TableInParliament <> "" Then
                                    LegislationDisplayFormat.Append("<p align=justify style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=2 ><i>" & TableInParliament & "</i></font></p></br>")
                                End If
                            ElseIf xRead.Name = "INFORCEFROM" Then

                                InforceFrom = Trim(xRead.ReadInnerXml)
                                If InforceFrom <> "" Then
                                    LegislationDisplayFormat.Append("<p align=justify style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=2 ><i>" & InforceFrom & "</i></font></p></br>")
                                End If

                            ElseIf xRead.Name = "PRINCIPALACTNO" Then

                                'LegislationDisplayFormat.Append("<p align=justify style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=2 ><i>" & Trim(xRead.ReadInnerXml) & "</i></font></p></br>")

                            ElseIf xRead.Name = "PRINCIPALACTTITLE" Then

                                'LegislationDisplayFormat.Append("<p align=justify style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=2 ><i>" & Trim(xRead.ReadInnerXml) & "</i></font></p></br>")
                            ElseIf xRead.Name = "GAZETTEDATE" Then


                                GazetteDate = Trim(xRead.ReadInnerXml)
                                If GazetteDate <> "" Then
                                    LegislationDisplayFormat.Append("<p align=justify style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=2 ><i>" & GazetteDate & "</i></font></p></br>")
                                End If

                            ElseIf xRead.Name = "LISTOFAMENDMENTS" Then
                                ListOfAmendments = Trim(xRead.ReadInnerXml)
                                If ListOfAmendments <> "" Then
                                    ArrLst.Add("List Of Amendments")
                                    Dim UrlEncrption As String = "fn=" & FileName & "&tp=loa"
                                    Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
                                    Dim link As String = UrlLink.UrlEncrypt
                                    ListAmendments = "<a  href=" & Chr(34) & "LegislationSectionDisplayed.aspx?info=" & link & Chr(34) & "><img border='0' src='GUI/NewDesign/img/iconListOfAmends.png'/>List of Amendments</a>"

                                End If
                            ElseIf xRead.Name = "PREAMBLE" Then
                                Preamble = Trim(xRead.ReadInnerXml)
                                If Preamble <> "" Then
                                    ArrLst.Add("Preamble")
                                    Dim UrlEncrption1 As String = "fn=" & FileName & "&tp=pre"
                                    Dim UrlLink1 As New Dba_UrlEncrption(UrlEncrption1, True)
                                    Dim link1 As String = UrlLink1.UrlEncrypt
                                    ListPremple = "<a  href=" & Chr(34) & "LegislationSectionDisplayed.aspx?info=" & link1 & Chr(34) & "><img border='0' src='GUI/NewDesign/img/iconListOfPreamble.png'/>List of Preamble</a>"


                                End If
                            ElseIf xRead.Name = "PARTNUMBER" Then

                                PartNumber = Trim(xRead.ReadInnerXml)
                                tmpPartNo = PartNumber

                                If PartNumber <> "" Then

                                    PartNumber = " <a " & "class=" & Chr(34) & "s_out" & Chr(34) & " name=" & noShorLinks & "></a><p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & ">" & " <font face=Vardana size=3 color=#465877><b>" & PartNumber & "</b> </font></p></br>"
                                    LegislationDisplayFormat.Append(PartNumber)

                                End If

                            ElseIf xRead.Name = "PARTTITLE" Then

                                PartTitle = Trim(xRead.ReadInnerXml)

                                ShortLinks.Append("<li><a href=" & Chr(34) & "#" & noShorLinks & Chr(34) & "> <span id='no'>" & tmpPartNo & "</span></a></li>")
                                ShortLinks = ShortLinks.Replace("PART", "")
                                ShortLinks = ShortLinks.Replace("CHAPTER", "")
                                ShortLinks = ShortLinks.Replace("ORDER", "")
                                If PartTitle <> "" Then
                                    copyPart = PartTitle
                                    PartTitle = " <a " & "class=" & Chr(34) & "s_out" & Chr(34) & " name=" & noShorLinks & "></a><p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & ">" & " <font face=Vardana size=3 color=#465877><b>" & PartTitle & "</b> </font></p></br>"
                                    LegislationDisplayFormat.Append(PartTitle)
                                    listAllSection.Append("<div align='center' id='parttitle'>" & copyPart & "</div>")
                                End If
                                noShorLinks += 1

                            ElseIf xRead.Name = "SUBPARTNUMBER" Then

                                PartNumber = Trim(xRead.ReadInnerXml)
                                If PartNumber <> "" Then
                                    PartNumber = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & ">" & " <font face=Vardana size=3 color=#000000><b>" & PartNumber & "</b> </font></p></br>"
                                    LegislationDisplayFormat.Append(PartNumber)
                                End If

                            ElseIf xRead.Name = "SUBPARTTITLE" Then
                                PartTitle = Trim(xRead.ReadInnerXml)
                                If PartTitle <> "" Then
                                    PartTitle = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & ">" & " <font face=Vardana size=3 color=#000000><b>" & PartTitle & "</b> </font></p></br>"
                                    LegislationDisplayFormat.Append(PartTitle)
                                End If

                            ElseIf xRead.Name = "SUBSUBPARTNUMBER" Then

                                PartNumber = Trim(xRead.ReadInnerXml)
                                If PartNumber <> "" Then
                                    PartNumber = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & ">" & " <font face=Vardana size=3 color=#000000><b>" & PartNumber & "</b> </font></p></br>"
                                    LegislationDisplayFormat.Append(PartNumber)
                                End If
                            ElseIf xRead.Name = "SUBSUBPARTTITLE" Then
                                PartTitle = Trim(xRead.ReadInnerXml)
                                If PartTitle <> "" Then
                                    PartTitle = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & ">" & " <font face=Vardana size=3 color=#000000><b>" & PartTitle & "</b> </font></p></br>"
                                    LegislationDisplayFormat.Append(PartTitle)
                                End If

                            ElseIf xRead.Name = "SECTION" Then
                                TempSection = xRead.ReadOuterXml.ToString()


                                Dim reader2 As XmlReader = XmlReader.Create(New StringReader(TempSection))
                                Dim SecNo As String = ""
                                Dim SecTitle As String = ""
                                While reader2.Read()
                                    If reader2.Name = "SNO" Then
                                        SecNo = Trim(reader2.ReadInnerXml)
                                        Exit While
                                    End If
                                End While
                                While reader2.Read()
                                    If reader2.Name = "ST" Then
                                        SecTitle = Trim(reader2.ReadInnerXml)
                                        SecTitle = SecTitle.Replace("&quot;", "")
                                        Exit While
                                    End If

                                End While

                                If SecNo <> "" And SecTitle <> "" Then
                                    FileName = FileName.Replace(".xml", "")

                                    For i = 0 To COL.Count - 1
                                        If i = 0 Then
                                            FirstWord = COL.Item(0)
                                        Else
                                            Lastword = COL.Item(i)
                                        End If
                                    Next
                                    TempSection = TempSection.Replace(SecTitle, " ")

                                    If SearchWords <> "" Then

                                        SecTitle = Regex.Replace(SecTitle, SearchWords, "<span class='hWord'>" & SearchWords & "</span>", RegexOptions.IgnoreCase)

                                        For i = 0 To COL.Count - 1
                                            If COL.Item(i) <> " " Then

                                                TempSection = Regex.Replace(TempSection, Trim(COL.Item(i)), "<span class='hWord'>" & Trim(COL.Item(i)) & "</span>", RegexOptions.IgnoreCase)
                                            End If

                                        Next
                                    End If
                                    TempSection = Regex.Replace(TempSection, "</?(a|A).*?>", "")
                                    Dim link As String = "fn=" & FileName & "&sn=" & SecNo
                                    Dim UrlLink As New Dba_UrlEncrption(link, True)
                                    Dim linkPrec As String = UrlLink.UrlEncrypt
                                    SeCount = SeCount + 1
                                    Section = "<div class='boxResult' > " & _
                                             "<div   style='width:100%;background-color:white;'><div style='float:left;width:5%;padding-top:10px;padding-bottom:10px;'><input type='checkbox' value='" & linkPrec & "' name='sendItems' id='" & SeCount & SecNo.Replace(" ", "_") & "'></div> <div onclick=show_sec_content('" & linkPrec & "','" & SeCount & SecNo.Replace(" ", "_") & "') class='titleBox' >" & _
                                             "<div class='boxCheck10' style='padding-bottom:10px;float:right;' id='SDimg" & SeCount & SecNo.Replace(".", "").Replace(" ", "_") & "'><img src='img/arrowEx1.png'/></div>" & _
                                             "<span id='SDfind" & SeCount & SecNo.Replace(".", "").Replace(" ", "_") & "'>" & SecNo & " " & SecTitle & "</span></div><div class='SDclear' id ='SD" & SeCount & SecNo.Replace(".", "").Replace(" ", "_") & "' style='display:none;width:90%;'>" & _
                                    "<div id='printdiv" & SeCount & SecNo.Replace(".", "").Replace(" ", "_") & "' style='float:right'><span id='locatcitation'></span> | <a href='#'class='fontReset' onclick=printDiv('PR" & SeCount & SecNo.Replace(".", "").Replace(" ", "_") & "')>Print</a>  | <a href='#' class='fontSizePlus'>A+</a> | <a href='#' class='fontReset'>Reset</a> | <a href='#' class='fontSizeMinus'>A-</a></div><div style='clear:both'></div>" & _
                                     "<div class='intro' id ='PR" & SeCount & SecNo.Replace(".", "").Replace(" ", "_") & "'>" & Regex.Replace(Regex.Replace(TempSection, "<st>[^<]*</st>", "", RegexOptions.IgnoreCase), "<sno>[^<]*</sno>", "", RegexOptions.IgnoreCase) & "</div></div></div></div> <div class='clear'></div>"
                                    listAllSection.Append("<li onclick=show_sec_content('" & linkPrec & "','" & SeCount & SecNo.Replace(" ", "_") & "') id='a" & SeCount & SecNo.Replace(".", "") & "'><a style='cursor:pointer'><small>" & SecNo & " </small><div>" & SecTitle & "</div></a></li>")
                                    LegislationDisplayFormat.Append("<Unknow>" & Section & "</Unknow>")
                                    Section = ""
                                    TempSection = ""

                                End If
                            ElseIf xRead.Name = "SCHEDULE" Then
                                CountSchedule = CountSchedule + 1
                                Schedule = Trim(xRead.ReadInnerXml)
                                If Schedule <> "" Then
                                    ListSchudle = "<a href='#' id='gotosch'><img border='0' src='GUI/NewDesign/img/iconListSchedule.png'/>List Schedule</a>"
                                    Schedule = "<div class='boxResult' onclick=show_am_content('" & FileName & "','sch'," & CountSchedule & ")> <div class='boxCheck1' id='SHimg" & CountSchedule & "' ><img src='img/arrowEx1.png'/></div>" & _
                                  "<div  class='resultTitleCase' style='width:90%;'> " & _
                                  "<span id='SHfind" & CountSchedule & "'>SCHEDULE No :" & CountSchedule & "</span><div class='SHclear' id ='SH" & CountSchedule & "' style='display:none;'>" & _
                                  "</div></div>" & _
                                  "</div> <div class='clear'></div>"
                                    LegislationDisplayFormat.Append(Schedule)
                                End If


                            End If

                    End Select


                End While
            Catch ex As Exception
                lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Section not available!<br> Sorry for any inconvenience caused.</span></div>"
            Finally

                xRead.Close()

                Title = ""
                Number = ""

                RoyalAssent = ""
                TableInParliament = ""
                InforceFrom = ""
                PrincipalActNo = ""
                PrincipalActTitle = ""
                GazetteDate = ""
                ListOfAmendments = ""
                Preamble = ""
                Section = ""
                Schedule = ""
                PartNumber = ""
                PartTitle = ""
                tmpPartNo = ""
                noShorLinks = 0
                i = 0


                xRead = Nothing
                strNewText = Nothing
                Nodelst = Nothing


            End Try

            lblXml.Text = LegislationDisplayFormat.ToString & "<br><br><br><br><br><br>"
            ListSection = listAllSection.ToString()
            PartStyle = ShortLinks.ToString
            LegislationDisplayFormat = Nothing

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
        ' load 10 top amendment in browse tab
        <System.Web.Services.WebMethod()> _
        Public Shared Function getAmendments(ByVal ActNo As String) As ArrayList
            Dim sql As String
            Dim list As New ArrayList
            Dim templist As New ArrayList
            Dim table As New DataTable
            Dim obj As New clsCasesSearch
            Dim UrlDecrpt As New Dba_UrlEncrption(ActNo, False)
            Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
            ActNo = UrlQuery
            Try

                sql = "select number,country from Legislation where datafilename like '" & ActNo & ".xml'"
                table = obj.ExecuteMyQuery(sql)
                sql = "select NUMBER,YEAR,TITLE,datafilename from  Legislation where  datafilename LIKE '%MY_AMEN%' and PRINCIPALACTNO  LIKE '" & table.Rows(0).Item(0).ToString().Trim() & "' and country='" & table.Rows(0).Item(1).ToString().Trim() & "' order by year asc"
                table = obj.ExecuteMyQuery(sql)
                For i = 0 To table.Rows.Count - 1
                    templist.Add(table.Rows(i).Item(0).ToString().Trim())
                    templist.Add(table.Rows(i).Item(1).ToString().Trim())
                    templist.Add(table.Rows(i).Item(2).ToString().Trim())
                    Dim IdLink As String = table.Rows(i).Item(3).ToString().Replace(".xml", "").Trim()
                    Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
                    Dim linkFileId As String = UrlFIleID.UrlEncrypt
                    templist.Add(linkFileId)
                    list.Add(templist.ToArray())
                    templist.Clear()
                Next

            Catch ex As Exception
                'lblMsg.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents , legislation Details will Be Upload Soon !</span></div>"
            End Try

            Return list
        End Function
        ' Load section content 
        <System.Web.Services.WebMethod()> _
        Public Shared Function sendJSON(ByVal id As String, ByVal sec As String) As String
            Dim returnValue As String = ""
            Dim ObjCS As New clsCasesSearch()
            Dim UrlDecrpt As New Dba_UrlEncrption(id, False)
            Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
            Dim LegislationSection As String = Replace(UrlQuery, ".xml", "").Replace("fn=", "").Replace("&sn=", ";").Replace(".", "")
            LegislationSection = LegislationSection.Replace(" ", "")
            If ObjCS.CheckActNoterUp(LegislationSection) = True Then
                Dim UrlLink As New Dba_UrlEncrption(LegislationSection, True)
                Dim linkPrec As String = UrlLink.UrlEncrypt
                returnValue = " <a href=precMap_legis.aspx?id=" & linkPrec & " > Cases Referred </a>"
            Else
                returnValue = ""
            End If

            Return returnValue
        End Function

    End Class

End Namespace
