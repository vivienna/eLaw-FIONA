Imports System.Data

Namespace membersarea

    Partial Class load_gloassry
        Inherits System.Web.UI.Page

        Dim CharSortBy As String
        Dim obj As New clsSearch
        Dim UserName As String
        Dim CharSortBy_glo As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim Terms As String
            Dim DT As New DataTable
            Dim i As Int16
            Dim RowCount As Integer
            Dim SBDetails As New System.Text.StringBuilder
            Terms = Request.QueryString("tab")
            Dim filename As String
            Dim legLink As String = ""
            DT = obj.getGlossaryDefinations(Terms)
            RowCount = DT.Rows.Count()
            filename = DT.Rows(i).Item(0)
            filename = filename.Replace(".xml", "")
            For i = 0 To (RowCount - 1)
                '------------------------------------------
                Dim IdLink As String = "fn=" & filename & "&sw=&sn=" & DT.Rows(i).Item(2)
                Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
                Dim linkFileId As String = UrlFIleID.UrlEncrypt

                legLink = "LegislationSectionDisplayed.aspx?info=" & linkFileId

                '-------------------------------------------


                Response.Write("<span style=""display:inline;"">" & i + 1 & ")&nbsp;</span>")
                Response.Write("<span style=""display:inline;"">" & DT.Rows(i).Item(1) & "</br>")
                Response.Write("<a href=" & legLink & ">" & DT.Rows(i).Item(3) & "</br>  " & DT.Rows(i).Item(5) & "  " & DT.Rows(i).Item(4) & "</a> </span>")
            Next
            SBDetails = Nothing
            RowCount = 0
            DT = Nothing
            i = 0
            Terms = ""
        End Sub

    End Class
End Namespace
