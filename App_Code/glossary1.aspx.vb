Imports System.Data
Namespace membersarea

    Public Class glossary11
        Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
        Protected WithEvents txtSearch As System.Web.UI.WebControls.TextBox
        Protected WithEvents Button1 As System.Web.UI.WebControls.Button
        Protected WithEvents Requiredfieldvalidator1 As System.Web.UI.WebControls.RequiredFieldValidator
        Protected WithEvents lblSortBy As System.Web.UI.WebControls.Label
        Protected WithEvents lbWords As System.Web.UI.WebControls.ListBox
        Protected WithEvents lblDefinations As System.Web.UI.WebControls.Label

        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Dim obj As New clsSearch
        Dim UserName As String
        Dim CharSortBy As String
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                Server.Transfer("login.aspx")
            End If

            If Request.QueryString("srt") <> "" Then

                CharSortBy = CStr(Request.QueryString("srt"))

            End If

            If CharSortBy = "" Then
                CharSortBy = "A"
            End If


            If IsPostBack = False Then

                lblMsg.EnableViewState = False
                lblSortBy.EnableViewState = False
                lblDefinations.EnableViewState = False

                lbWords.AutoPostBack = True
                '            lblSortBy.Text = Me.SortingOrderByAlphabets
                WordList()
            End If
            lblSortBy.Text = Me.SortingOrderByAlphabets

        End Sub



        Private Function SortingOrderByAlphabets() As String
            Dim URLpage As String
            Dim i As Byte
            Dim SbLetters As New System.Text.StringBuilder
            Dim Result As String


            URLpage = "<a class=" & Chr(34) & "nav3" & Chr(34) & " href=" & Chr(34) & "glossary.aspx?srt="

            'SbLetters.Append("<font size=2 face=Verdana color=" & Chr(34) & "#fedf5e" & Chr(34) & " >")
            For i = 65 To 90
                SbLetters.Append("&nbsp;" & URLpage & Chr(i) & Chr(34) & " >" & Chr(i) & "</a>&nbsp;")
            Next
            'SbLetters.Append("</font>")
            Result = SbLetters.ToString

            SbLetters = Nothing
            URLpage = ""
            i = 0

            Return Result

        End Function




        Private Sub lbAlphabets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)


            lbWords.DataSource = obj.getGlossarySortBy(CharSortBy)
            lbWords.DataTextField = "Terms"
            lbWords.DataValueField = "Terms"
            lbWords.DataBind()

            'obj = Nothing
        End Sub

        Private Sub WordList()
            lbWords.DataSource = obj.getGlossarySortBy(CharSortBy)
            lbWords.DataTextField = "Terms"
            lbWords.DataValueField = "Terms"
            lbWords.DataBind()

        End Sub

        Private Sub getSortedWordlist()
            lbWords.DataSource = obj.getGlossarySortBy(CharSortBy)
            lbWords.DataTextField = "Terms"
            lbWords.DataValueField = "Terms"
            lbWords.DataBind()

        End Sub


        Private Sub lbWords_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbWords.SelectedIndexChanged
            Dim Terms As String
            Dim DT As New DataTable
            Dim i As Int16
            Dim RowCount As Integer
            Dim SBDetails As New System.Text.StringBuilder

            Terms = lbWords.SelectedItem.Text

            DT = obj.getGlossaryDefinations(Terms)
            RowCount = DT.Rows.Count()
            lblDefinations.Text = ""
            ''/** NOTE
            'DT.Rows(i).Item(0)=DATAFILENAME
            'DT.Rows(i).Item(1)=DEFINATIONS
            'DT.Rows(i).Item(2)=SNO
            'DT.Rows(i).Item(3) = ST
            'DT.Rows(i).Item(4)=NUMBER
            'DT.Rows(i).Item(5)=TITLE
            ''         **/

            For i = 0 To (RowCount - 1)
                SBDetails.Append(i + 1 & " - <b>" & Terms & "</b></br>")
                SBDetails.Append("<b>Definations:<b>" & DT.Rows(i).Item(1) & "</br>")
                SBDetails.Append("<a href=" & Chr(34) & "LegislationSectionDisplayed.aspx?fn=" & DT.Rows(i).Item(0) & "&sn=" & DT.Rows(i).Item(2) & Chr(34) & " >" & DT.Rows(i).Item(3) & "</br>  " & DT.Rows(i).Item(5) & "  " & DT.Rows(i).Item(4) & "</a> </br></br>")

            Next

            lblDefinations.Text = SBDetails.ToString


            SBDetails = Nothing
            RowCount = 0
            DT = Nothing
            i = 0
            Terms = ""
        End Sub

        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

            ManualSearch()
        End Sub

        Private Sub ManualSearch()
            Dim Terms As String
            Dim DT As New DataTable
            Dim i As Int16
            Dim RowCount As Integer
            Dim SBDetails As New System.Text.StringBuilder

            If Len(txtSearch.Text) > 250 Then
                lblMsg.Text = "Exceeding the range for word. Please insert the valid word."

            End If

            If IsNumeric(txtSearch.Text) Then
                lblMsg.Text = "Please insert the valid word."
            End If



            Terms = txtSearch.Text
            Try
                DT = obj.getGlossaryDefinations(Terms)
                RowCount = DT.Rows.Count()
                lblDefinations.Text = ""
                ''/** NOTE
                'DT.Rows(i).Item(0)=DATAFILENAME
                'DT.Rows(i).Item(1)=DEFINATIONS
                'DT.Rows(i).Item(2)=SNO
                'DT.Rows(i).Item(3) = ST
                'DT.Rows(i).Item(4)=NUMBER
                'DT.Rows(i).Item(5)=TITLE
                ''         **/

                For i = 0 To (RowCount - 1)
                    SBDetails.Append(i + 1 & " - <b>" & Terms & "</b></br>")
                    SBDetails.Append("<b>Definations:<b>" & DT.Rows(i).Item(1) & "</br>")
                    SBDetails.Append("<a href=" & Chr(34) & "LegislationSectionDisplayed.aspx?fn=" & DT.Rows(i).Item(0) & "&sn=" & DT.Rows(i).Item(2) & Chr(34) & " >" & DT.Rows(i).Item(3) & "</br>  " & DT.Rows(i).Item(5) & "  " & DT.Rows(i).Item(4) & "</a> </br></br>")

                Next

            Catch err As Exception
            Finally
                lblDefinations.Text = SBDetails.ToString
                SBDetails = Nothing
                RowCount = 0
                DT = Nothing
                i = 0
                Terms = ""

            End Try


        End Sub

        Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
            ManualSearch()
        End Sub

        Protected Overrides Sub Finalize()
            CharSortBy = ""
            obj = Nothing
            UserName = ""
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
