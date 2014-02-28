'/**********************************************************************/
'/*	Developer 	    : Usman Sarwar  								   */
'/*	Company     	: The Digital Library Sdn. Bhd.		    					   */
'/* Date Modified	: 28/2/2013		        					*/  
'/*	Description		: Law dictionay                                      */
'/*	Version			: 1.0											   */
'/**********************************************************************/

Imports System.Net
Imports System.Xml
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data

Namespace membersarea

    Partial Class LawDictionary1
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

        Dim CharSortBy As String
        Dim obj As New clsSearch
        Dim UserName As String
        Dim CharSortBy_glo As String
        Dim CharSortBy_ph As String
        Dim CharSortBy_Translate As String
        Dim Query As String

        Dim Translate As String
        Dim loadTranslate As String
        Public wordPhrase As String
        Dim obj_ph As New clsCasesSearch()
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Response.Expires = 0
            Response.Cache.SetNoStore()
            Response.AppendHeader("Pragma", "no-cache")
            UserName = CType(Session("UserName"), String)

            If UserName = "" Then
               ' Server.Transfer("login.aspx")
			    Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If
           

            If Request.QueryString("srt") <> "" Then
                hidLastTab.Value = Request.QueryString("tab")
                CharSortBy = CStr(Request.QueryString("srt"))

            End If
            If Request.QueryString("srt_ph") <> "" Then
                hidLastTab.Value = Request.QueryString("tab")
                CharSortBy_ph = CStr(Request.QueryString("srt_ph"))

            End If
            If CharSortBy = "" Then
                CharSortBy = "A"
            End If
            If Request.QueryString("srt_glo") <> "" Then
                hidLastTab.Value = Request.QueryString("tab")
                CharSortBy_glo = CStr(Request.QueryString("srt_glo"))

            End If
            If Request.QueryString("trans") <> "" Then
                hidLastTab.Value = Request.QueryString("tab")

                Translate = CStr(Request.QueryString("trans"))
                
            End If
            If Request.QueryString("str_trans") <> "" Then
                hidLastTab.Value = Request.QueryString("tab")

                CharSortBy_Translate = CStr(Request.QueryString("str_trans"))

            End If
            If CharSortBy_glo = "" Then
                CharSortBy_glo = "A"
            End If
            If CharSortBy_ph = "" Then
                CharSortBy_ph = "A"
            End If

           
            Query = "select TITLE from Reference_wordAndPhrase where title like '" & CharSortBy_ph & "%'"

            If IsPostBack = False Then
                lblMsg.EnableViewState = False
                lblSortBy.EnableViewState = False
                lblMeaning.EnableViewState = False
                lblSortBy.Text = Me.SortingOrderByAlphabets("srt", 0)
                lblMsg_glossary.EnableViewState = False
                lblSortBy_glossary.EnableViewState = False
                WordList_word_ph()
                 WordList()
                WordList_glo()
                lblSortByword_ph.Text = Me.SortingOrderByAlphabets_word_ph()
                WordList_word_Translate()
            Else
            End If
            lblSortBy_glossary.Text = Me.SortingOrderByAlphabets_glo
            lblSortBy.Text = Me.SortingOrderByAlphabets("srt", 0)
            lblSortByword_ph.Text = Me.SortingOrderByAlphabets_word_ph
            SortBytranslate.Text = Me.SortingOrderByAlphabetsTranslate
            ClientScript.RegisterStartupScript(Me.[GetType](), "selecttab", "$('.tabsBox li:eq(' +" & hidLastTab.Value & "+ ')').addClass('current1');", True)
        End Sub
        Private Function SortingOrderByAlphabets(ByVal str As String, ByVal tab As Integer) As String
            Dim i As Byte
            Dim SbLetters As New System.Text.StringBuilder
            Dim Result As String
            For i = 65 To 90
                SbLetters.Append(" <a  style='cursor:pointer'  onclick=""LoadContent('" & Chr(i) & "')"" >" & Chr(i) & "</a> ")
            Next
            Result = SbLetters.ToString
            SbLetters = Nothing
            i = 0
            Return Result
        End Function
        Private Function SortingOrderByAlphabetsTranslate() As String
            Dim i As Byte
            Dim SbLetters As New System.Text.StringBuilder
            Dim Result As String
            For i = 65 To 90
                SbLetters.Append(" <a  style='cursor:pointer'  onclick=""LoadTranslator('" & Chr(i) & "')"" >" & Chr(i) & "</a> ")
            Next
            Result = SbLetters.ToString
            SbLetters = Nothing
            i = 0
            Return Result
        End Function
        Private Sub WordList_word_Translate()
            Dim QueryTranslate = "select English from reference_translation where English like 'A%' "
            lbTranslate.DataSource = obj_ph.ExecuteMyQuery(QueryTranslate) 'obj.getLawDictionarySortBy(CharSortBy)
            lbTranslate.DataTextField = "English"
            lbTranslate.DataValueField = "English"
            lbTranslate.DataBind()

        End Sub
        Private Sub lbAlphabets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            lbWords.DataSource = obj.getLawDictionarySortBy(CharSortBy)
            lbWords.DataTextField = "word"
            lbWords.DataValueField = "word"
            lbWords.DataBind()


        End Sub

        Private Sub WordList()
            lbWords.DataSource = obj.getLawDictionarySortBy(CharSortBy)
            lbWords.DataTextField = "word"
            lbWords.DataValueField = "word"
            lbWords.DataBind()

        End Sub

        Private Sub getSortedWordlist()
            lbWords.DataSource = obj.getLawDictionarySortBy(CharSortBy)
            lbWords.DataTextField = "word"
            lbWords.DataValueField = "word"
            lbWords.DataBind()

        End Sub



        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

            ManualSearch()
        End Sub
        Private Sub ManualSearch()
            Dim Word As String


            If Len(txtSearch.Text) > 250 Then
                lblMsg.Text = "Exceeding the range for word. Please insert the valid word."
            End If

            If IsNumeric(txtSearch.Text) Then
                lblMsg.Text = "Please insert the valid word."
            End If


            lblMeaning.Text = ""
            Word = txtSearch.Text
            Try
                lblMeaning.Text = obj.getLawDictionaryMeaning(1, Word)
            Catch err As Exception
            Finally
                '   obj = Nothing
            End Try

        End Sub

        Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
            ManualSearch()
        End Sub


        Private Function SortingOrderByAlphabets_glo() As String
            Dim i As Byte
            Dim SbLetters As New System.Text.StringBuilder
            Dim Result As String
            For i = 65 To 90
                SbLetters.Append(" <a  style='cursor:pointer' onclick=""LoadContentwp('" & Chr(i) & "')"" >" & Chr(i) & "</a> ")
            Next
            Result = SbLetters.ToString
            SbLetters = Nothing
            i = 0
            Return Result

        End Function

        Private Sub lbAlphabets_glo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)


            lbWords_glossary.DataSource = obj.getGlossarySortBy(CharSortBy_glo)
            lbWords_glossary.DataTextField = "Terms"
            lbWords_glossary.DataValueField = "Terms"
            lbWords_glossary.DataBind()
            hidLastTab.Value = 1
            'obj = Nothing
        End Sub
        Private Sub WordList_glo()

            lbWords_glossary.DataSource = obj.getGlossarySortBy(CharSortBy_glo)
            lbWords_glossary.DataTextField = "Terms"
            lbWords_glossary.DataValueField = "Terms"
            lbWords_glossary.DataBind()

        End Sub
        Private Sub getSortedWordlist_glo()
            lbWords_glossary.DataSource = obj.getGlossarySortBy(CharSortBy_glo)
            lbWords_glossary.DataTextField = "Terms"
            lbWords_glossary.DataValueField = "Terms"
            lbWords_glossary.DataBind()

        End Sub
       
        
        Private Sub ManualSearch_glo()
            Dim Terms As String
            Dim DT As New DataTable
            Dim i As Int16
            Dim RowCount As Integer
            Dim SBDetails As New System.Text.StringBuilder

            If Len(txtSearch_glossary.Text) > 250 Then
                lblMsg_glossary.Text = "Exceeding the range for word. Please insert the valid word."

            End If

            If IsNumeric(txtSearch_glossary.Text) Then
                lblMsg_glossary.Text = "Please insert the valid word."
            End If



            Terms = txtSearch_glossary.Text
            Try
                DT = obj.getGlossaryDefinations(Terms)
                RowCount = DT.Rows.Count()
                For i = 0 To (RowCount - 1)
                    SBDetails.Append(i + 1 & " - <b>" & Terms & "</b></br>")
                    SBDetails.Append("<b>Definations:<b>" & DT.Rows(i).Item(1) & "</br>")
                    SBDetails.Append("<a href=" & Chr(34) & "LegislationSectionDisplayed.aspx?fn=" & DT.Rows(i).Item(0) & "&sn=" & DT.Rows(i).Item(2) & Chr(34) & " >" & DT.Rows(i).Item(3) & "</br>  " & DT.Rows(i).Item(5) & "  " & DT.Rows(i).Item(4) & "</a> </br></br>")

                Next

            Catch err As Exception
            Finally
                'lblDefinations.Text = SBDetails.ToString
                SBDetails = Nothing
                RowCount = 0
                DT = Nothing
                i = 0
                Terms = ""

            End Try


        End Sub
        Private Sub txtSearch_glossary_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch_glossary.TextChanged
            ManualSearch_glo()
        End Sub



        Private Function SortingOrderByAlphabets_word_ph() As String
            Dim i As Byte
            Dim SbLetters As New System.Text.StringBuilder()
            Dim Result As String
            For i = 65 To 90
                SbLetters.Append(" <a  style='cursor:pointer' onclick=""LoadContentglo('" & Chr(i) & "')"" >" & Chr(i) & "</a> ")
            Next
            Result = SbLetters.ToString
            SbLetters = Nothing
            i = 0

            Return Result

        End Function

        Private Sub WordList_word_ph()
            lbWords_word_ph.DataSource = obj_ph.ExecuteMyQuery(Query) 'obj.getLawDictionarySortBy(CharSortBy)
            lbWords_word_ph.DataTextField = "TITLE"
            lbWords_word_ph.DataValueField = "TITLE"
            lbWords_word_ph.DataBind()
        End Sub
        
        Private Sub lbWords_word_ph_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbWords_word_ph.SelectedIndexChanged

            Dim Word As String
            Word = lbWords_word_ph.SelectedItem.Text
            Word = Replace(Word, " ", "_")
           
        End Sub
        
        Private Sub btn_word_ph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_word_ph.Click

            ManualSearch_word_ph()
        End Sub
        Private Sub ManualSearch_word_ph()
            Dim Word As String
            Dim ManualQuery As String
            Dim DT As New Data.DataTable()

            If Len(txtSearch_word_ph.Text) > 250 Then
                lblMsgword_ph.Text = "Exceeding the range for word. Please insert the valid word."
            End If

            If IsNumeric(txtSearch_word_ph.Text) Then
                lblMsgword_ph.Text = "Please insert the valid word."
            End If
            Word = txtSearch_word_ph.Text

            ManualQuery = "select TITLE from Reference_wordAndPhrase where title = '" & Word & "'"

            Try
                DT = obj_ph.ExecuteMyQuery(ManualQuery)
                wordPhrase = DT.Rows(0).Item(0).ToString()
            Catch err As Exception
            Finally
                '   obj = Nothing
            End Try

        End Sub

        Protected Overrides Sub Finalize()
            CharSortBy = ""
            CharSortBy_glo = ""
            CharSortBy_ph = ""
            obj = Nothing
            obj_ph = Nothing
            UserName = ""
            Query = ""
            MyBase.Finalize()
        End Sub

        <System.Web.Services.WebMethod()> _
        Public Shared Function GetautocomplateTrems(ByVal auto As String) As ArrayList
            Dim suggestions As New ArrayList
            Dim dt As New DataTable
            Dim obj As New clsSearch
            Dim objs As New clsCasesSearch
            dt = obj.getGlossaryDefinations(auto)
            'RowCount = dt.Rows.Count()
            Dim query As String = "select DISTINCT top 10 Terms  from  LEGTERMDEF where Terms like '" & Trim(auto) & "%'  order by 1"
            dt = objs.ExecuteMyQuery(query)
            For i = 0 To dt.Rows.Count - 1
                suggestions.Add(dt.Rows(i).Item(0).ToString.Trim())
            Next
            Return suggestions
        End Function

        <System.Web.Services.WebMethod()> _
        Public Shared Function Getglo(ByVal auto As String) As String
            Dim suggestions As New StringBuilder
            Dim DT As New DataTable
            Dim Query1 = "select TITLE from Reference_wordAndPhrase where title like '" & auto & "%'"
            Dim obj_ph As New clsCasesSearch()
            DT = obj_ph.ExecuteMyQuery(Query1)
            For i = 0 To DT.Rows.Count - 1
                suggestions.Append("<option value=" & DT.Rows(i).Item(0).ToString.Trim() & ">" & DT.Rows(i).Item(0).ToString.Trim() & "</option>")
            Next
            Return suggestions.ToString
        End Function
        <System.Web.Services.WebMethod()> _
        Public Shared Function Getlegd(ByVal auto As String) As String
            Dim suggestions As New StringBuilder
            Dim obj As New clsSearch

            Dim DT As New DataTable
            DT = obj.getLawDictionarySortBy(auto)

            For i = 0 To DT.Rows.Count - 1
                suggestions.Append("<option value=" & DT.Rows(i).Item(0).ToString.Trim() & ">" & DT.Rows(i).Item(0).ToString.Trim() & "</option>")
            Next
            Return suggestions.ToString
        End Function
        <System.Web.Services.WebMethod()> _
        Public Shared Function Getwp(ByVal auto As String) As String
            Dim suggestions As New StringBuilder
            Dim obj As New clsSearch

            Dim DT As New DataTable
            DT = obj.getGlossarySortBy(auto)

            For i = 0 To DT.Rows.Count - 1
                suggestions.Append("<option value=" & DT.Rows(i).Item(0).ToString.Trim() & ">" & DT.Rows(i).Item(0).ToString.Trim() & "</option>")
            Next
            Return suggestions.ToString
        End Function
        <System.Web.Services.WebMethod()> _
        Public Shared Function GetTranslator(ByVal auto As Char, ByVal lang As Byte, ByVal word As String) As String
            Dim suggestions As New StringBuilder
            Dim Translate As Byte = lang
            Dim loadTranslate As String = ""
            Dim CharSortBy_Translate As Char = auto
            Dim obj As New clsCasesSearch()
            Dim DT As New DataTable
            Dim QueryTranslate As String
            Dim ValueWords As String
            If Translate = "2" Then
                loadTranslate = "Malay"
            Else
                loadTranslate = "English"
            End If
            If CharSortBy_Translate = "" Then
                CharSortBy_Translate = "A"
            End If
            If word <> "" Then
                Dim Translate_Word As String = ""
                If loadTranslate = "English" Then
                    Translate_Word = "Malay"
                ElseIf loadTranslate = "Malay" Then
                    Translate_Word = "English"
                End If
                word = Replace(word, "_", " ")
                Dim Query As String = "select " & Translate_Word & " from reference_translation where " & loadTranslate & " = '" & word & "' "
                DT = obj.ExecuteMyQuery(Query)
                suggestions.Append(DT.Rows(0).Item(0).ToString.Trim())
            Else
                QueryTranslate = "SELECT " & loadTranslate & " FROM reference_translation where " & loadTranslate & " like '" & CharSortBy_Translate & "%'  order by " & loadTranslate & " asc"

                DT = obj.ExecuteMyQuery(QueryTranslate)
                For i = 0 To DT.Rows.Count - 1
                    ValueWords = Replace(DT.Rows(i).Item(0).ToString(), " ", "_")
                    suggestions.Append("<option value=" & ValueWords & ">" & DT.Rows(i).Item(0).ToString.Trim() & "</option>")
                Next
            End If
           
            Return suggestions.ToString
        End Function
        
       
    End Class

End Namespace
