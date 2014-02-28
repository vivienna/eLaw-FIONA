Imports System.Data
Namespace membersarea


    Partial Class load_words
        Inherits System.Web.UI.Page
        Dim CharSortBy As String
        Dim obj As New clsSearch
        Dim UserName As String
        Dim CharSortBy_glo As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim Word As String
            Dim terms As String
            terms = Request.QueryString("tab")

            Word = terms
            Response.Write(obj.getLawDictionaryMeaning(1, Word))
        End Sub
    End Class
End Namespace