Imports System.Xml
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data

Namespace membersarea


    Partial Class load_phrase
        Inherits System.Web.UI.Page
        Dim CharSortBy As String
        Dim obj As New clsCasesSearch() '  clsSearch()
        Dim UserName As String
        Dim Query As String
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
           
            Dim Word As String
            Word = Request.QueryString("tab")
            Word = Replace(Word, " ", "_")
            MeaningFile_word_ph(Word & ".xml")
        End Sub
        Private Sub MeaningFile_word_ph(ByVal Filename As String)
            'actually path = filename

            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\words_phrases\" & Filename))

            '      Dim Pattern As String
            '       Dim strNewText As String
            '        Dim objUtil As New clsMyUtility()
            Dim sbXml As New System.Text.StringBuilder()



            Dim Def As String
            Dim Title As String
            Dim Reference As String


            '        COL = objUtil.Tokenizer(SearchWords)
            Try
                While xRead.Read()
                    Select Case xRead.NodeType
                        Case XmlNodeType.Element
                            If xRead.Name = "title" Then

                                Title = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 color=#465877 ><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p>"  'xRead.ReadOuterXml
                                sbXml.Append(Title)
                                Title = ""
                            ElseIf xRead.Name = "Reference" Then
                                Reference = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 color=#465877><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p>"  'xRead.ReadOuterXml
                                sbXml.Append(Reference)
                                Reference = ""

                            ElseIf xRead.Name = "Def" Then
                                Def = "<p align=center style=" & Chr(34) & "MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px" & Chr(34) & "><font face=Vardana size=3 color=#465877><b>" & Trim(xRead.ReadInnerXml) & "</b></font></p>" 'xRead.ReadOuterXml
                                sbXml.Append(Def)
                                Def = ""


                            End If

                    End Select


                End While
            Catch ex As Exception
                Response.Write(ex.Message)
            Finally
                xRead.Close()
            End Try

            Response.Write(sbXml.ToString)
            sbXml = Nothing

        End Sub
    End Class
End Namespace