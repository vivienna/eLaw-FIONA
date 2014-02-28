
'/******************************************************************************/
'/*	Developer 	    : Usman Sarwar           								   */
'/*	Company     	: The Digital Library Sdn. Bhd.		    	    				        */
'/* Date Modified	: 18-6-2013 		        		    			     */  
'/*	Description		: open practice notes file , xml parse and display it              */
'/*	Version			: 1.0											            */
'/*******************************************************************************/


Imports System.Xml
Imports System.IO
Imports System.Text.RegularExpressions
Imports Dba_UrlEncrption


Namespace membersarea


    Partial Class PracticeNotesMainDisplayed
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

        Dim URL As String
        Dim filename1 As String
        Dim s_Fts As String
        Dim UserName As String
        Dim HeadnotesFlag As Boolean = False
        Dim fileName As String = ""
        Public EmailConfirm As String = ""
        Public DownloadFileName As String = ""
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim path As String = ""
            Dim SearchWords As String = ""
            Dim s_searchNew As String = ""
            Dim ObjUtil As New clsMyUtility()
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                'Server.Transfer("login.aspx")
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
                                    s_Fts = UrlQuery(i).parvalue
                                Case "ns"
                                    s_searchNew = UrlQuery(i).parvalue

                                Case "id"
                                    fileName = UrlQuery(i).parvalue
                            End Select
                        End If
                    Next
                End If
            Else
               ' Response.Redirect("~/login.aspx")
			    Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If

            fileName = Replace(fileName, "?", "")
            If IsPostBack = False Then
                lblPdf.EnableViewState = False
            End If
            If Right(fileName, 4) <> ".xml" Then
                fileName = Replace(fileName, "?", "")
                fileName = Replace(fileName, ";", "")
                fileName &= ".xml"
            End If
            SearchWords = Trim(s_Fts)
            SearchWords = ObjUtil.RefineSentence(SearchWords)
            Display_Case1(fileName, SearchWords)
            DownloadFileName = "d=Y&f=" & fileName.Replace(".xml", "").Trim()
            ObjUtil = Nothing


        End Sub


        Private Sub Display_Case1(ByVal Path As String, ByVal SearchWords As String)
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\Practicenotes\" & Path))
            Dim Pattern As String
            Dim objUtil As New clsMyUtility()
            Dim sbXml As New System.Text.StringBuilder()
            Dim COL As New Collections.ArrayList()
            Dim i As Int16
            Dim JudgementBody As String = ""
            Dim Title As String
            Dim Citation As String
            Dim Court As String
            SearchWords = Trim(SearchWords)
            COL = objUtil.Tokenizer(SearchWords)
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "TITLE" Then

                                Title = "<p align='center' id='title' style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 color=#465877 ><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p>"  'xRead.ReadOuterXml

                                For i = 0 To COL.Count - 1
                                    If COL.Item(i) <> " " Then
                                        Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                        Title = Regex.Replace(Title, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                    End If

                                Next

                                sbXml.Append(Title)
                                Title = ""
                            ElseIf xRead.Name = "ISSUE" Then


                                Court = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 color=#465877><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p>"  'xRead.ReadOuterXml

                                For i = 0 To COL.Count - 1
                                    If COL.Item(i) <> " " Then
                                        Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                        Court = Regex.Replace(Court, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                    End If

                                Next

                                sbXml.Append(Court)
                                Court = ""
                            ElseIf xRead.Name = "TYPE" Then

                                Citation = Trim(xRead.ReadInnerXml)
                                If Citation = "" Then

                                Else
                                    Citation = "<font face=Vardana size=2 color=#000000><b>" & Citation & "</b></font>&nbsp &nbsp</br>"


                                End If

                            ElseIf xRead.Name = "NUMBER" Then

                                Citation = Trim(xRead.ReadInnerXml)
                                If Citation = "" Then

                                Else
                                    Citation = "<font face=Vardana size=2 color=#000000><b>Number: " & Citation & "</b></font>&nbsp &nbsp</br>"


                                End If

                            ElseIf xRead.Name = "YEAR" Then

                                Citation = Trim(xRead.ReadInnerXml)
                                If Citation = "" Then

                                Else
                                    Citation = "<font face=Vardana size=2 color=#000000><b>Year: " & Citation & "</b></font>&nbsp &nbsp</br>"
                                End If
                                sbXml.Append(Citation)
                            ElseIf xRead.Name = "TEXT" Then
                                JudgementBody &= xRead.ReadOuterXml
                                For i = 0 To COL.Count - 1
                                    If COL.Item(i) <> " " Then
                                        Pattern = "\b" & Trim(COL.Item(i)) & "\b"
                                        JudgementBody = Regex.Replace(JudgementBody, Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & COL.Item(i) & "</b></span>", RegexOptions.IgnoreCase)
                                    End If
                                Next
                                sbXml.Append(JudgementBody)
                                JudgementBody = ""
                            End If
                    End Select
                End While
            Catch ex As Exception
                ' lblMsg.Text = ex.Message
            Finally
                Dim UrlDecrpt As New Dba_UrlEncrption(Regex.Replace(Path, ".xml", "", RegexOptions.IgnoreCase).Trim(), True)
                Dim UrlQuery As String = UrlDecrpt.UrlEncrypt
                sbXml.Append("<div style='display:none' id='lblid'>" & UrlQuery & "</div>")

                xRead.Close()
            End Try



            lblXml.Text = sbXml.ToString
            sbXml = Nothing
            objUtil = Nothing
        End Sub

    End Class

End Namespace


