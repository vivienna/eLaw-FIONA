'/**********************************************************************/
'/*	Developer 	    : Usman Sarwar  								   */
'/*	Company     	: The Digital Library Sdn. Bhd.		    					   */
'/* Date Modified	: 28/2/2013								*/  
'/*	Description		: open legal Forms                                */
'/*	Version			: 1.0											   */
'/**********************************************************************/
Imports System
Imports System.Data
Namespace membersarea

    Partial Class practiceNotesSearch
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
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
                'Server.Transfer("login.aspx")
				 Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If
            If IsPostBack = False Then
                btnSearch.EnableViewState = False
                lblMsg.EnableViewState = False
            End If

        End Sub

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Me.SearchData()
        End Sub


        Private Sub SearchData()
            Dim Obj As New clsSearch
            Dim SearchFTS As String
            Dim SearchNotW As String
            Dim SearchType As Byte
            Dim iProximityWithin As String
            Dim iProximityLevel As String
            Dim URL As String
            If Len(Trim(txtFTS.Text)) = 0 And Len(Trim(txtNotform.Text)) = 0 Then
                lblMsg.Text = "Enter the Keywords for Searching Articles"
                Exit Sub
            End If
            Try
                SearchFTS = Trim(txtFTS.Text)
                SearchNotW = Trim(txtNotform.Text)
                SearchFTS = SearchFTS.Replace(" ", "+")
                Dim count1 As Integer = CountWords(SearchFTS)
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
                End If
            Catch er As Exception
            End Try
            SearchType = rbs.SelectedValue
            Dim UrlEncrption As String = "ft=" & SearchFTS & "&nw=" & SearchNotW & "&ppl=" & iProximityLevel & "&pplw=" & iProximityWithin & "&Stype=" & SearchType
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt
            URL = "PracticeNotesSearchresult.aspx?info=" & link
            URL = Server.UrlPathEncode(URL)
            Response.Redirect(URL)
        End Sub

        Public Function CountWords(ByVal value As String) As Integer
            ' Count matches.
            Dim collection As MatchCollection = Regex.Matches(value, "\S+")
            Return collection.Count
        End Function
        <System.Web.Services.WebMethod()> _
        Public Shared Function exactAutoComplete(ByVal auto As String) As ArrayList
            Dim suggestions As New ArrayList
            Dim query As String = "select DISTINCT top 5 TITLE  from  reference_PRACTICENOTES where TITLE like '" & Trim(auto) & "%'  order by 1 asc"
            Dim obj As New clsCasesSearch
            Dim dt As New DataTable()
            'Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            For i = 0 To dt.Rows.Count - 1
                suggestions.Add(dt.Rows(i).Item(0).ToString.Trim())
            Next
            Return suggestions
        End Function


    End Class

End Namespace
