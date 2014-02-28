Imports System.IO
Imports System.Data
Imports Microsoft.Office.Interop.Word
Imports Microsoft.Office.Interop
Imports System.Xml

Imports System.Data.SqlClient
Imports Dba_UrlEncrption

Namespace membersarea

    Partial Class FormMainDisplayed
        Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        Private ConnectionString As String
        Private Shared CnString As String
        Public Sub New()

            ConnectionString = clsConfigs.sGlobalConnectionString

            CnString = ConnectionString
            If ConnectionString = "" Then
                Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
            End If

        End Sub
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region
        Dim Path As String
        Public EmailConfirm As String = ""
        Dim FileName As String = ""
        Dim FormNo As String = ""
        Dim FormTitle As String
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim UserName As String
            MyStyleSheet.Attributes.Add("href", "css/StyleSheet.css")
            UserName = CType(Session("UserName"), String)
            If UserName = "" Then
               ' Server.Transfer("login.aspx")
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
                                    FileName = UrlQuery(i).parvalue
                                Case "No"
                                    FormNo = UrlQuery(i).parvalue
                                    FormNo = FormNo.Replace("_", " ")
                            End Select
                        End If
                    Next
                End If
            Else
                'Response.Redirect("~/login.aspx")
				 Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If

            Try
                Path = Server.MapPath("xmlFiles\legislation\" & FileName & ".xml")

                If isFileExist(Path) = True Then
                    DisplayLegislation_Sec(FileName, Trim(FormNo))

                Else
                    lblXml.Text = "<div align='center' style='padding: 100px'><span class='tfont1'>Sorry No Relevant Documents Found , We Will Notify The Problem Soon !</span></div>"
                End If

            Catch ex As Exception
                ' lblMsg.Text = ex.Message
            End Try
        End Sub
        Private Sub DisplayLegislation_Sec(ByVal FileName As String, ByVal No As String)



            Dim ResultSection As String
            Dim xRead As XmlTextReader = New XmlTextReader(Server.MapPath("xmlFiles\legislation\" & FileName & ".xml"))
            While xRead.Read()
                If xRead.Name = "FORM" Then
                    ResultSection = xRead.ReadOuterXml
                    ResultSection = ResultSection.Replace("> ", ">")
                    ResultSection = ResultSection.Replace(" <", "<")
                    If ResultSection.Contains("<FORMNUMBER>" & No & "</FORMNUMBER>") = True Then
                        Exit While
                    End If
                End If
            End While
            lblXml.Text = ResultSection
            No = No.Replace(" ", "_")


            Dim UrlLink As New Dba_UrlEncrption(FileName & "_" & No, True)
            Dim UrlQuery As String = UrlLink.UrlEncrypt
            lblid.Text = UrlQuery
            lblid.CssClass = "invisible"
            ResultSection = ""
        End Sub


        Private Sub ConvertWordTohtml(ByVal path As String)
            Try
                Dim Path1 As String = Server.MapPath("xmlFiles\Forms\")
                Dim WordApp As New Microsoft.Office.Interop.Word.ApplicationClass()
                Dim WordDoc As New Microsoft.Office.Interop.Word.Document()
                Dim DocNoParam As Object = Type.Missing
                Dim DocFileName As Object = path
                'File Path
                Dim DocReadOnly As Object = False
                Dim DocVisible As Object = False
                Dim SaveToFormat As Object = ""


                ' Open the document by passing the path
                WordDoc = WordApp.Documents.Open(DocFileName, DocNoParam, DocReadOnly, DocNoParam, DocNoParam, DocNoParam, _
                 DocNoParam, DocNoParam, DocNoParam, DocNoParam, DocVisible, DocNoParam, _
                 DocNoParam, DocNoParam, DocNoParam, DocNoParam)
                WordDoc.Activate()


                SaveToFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML
                Dim SavePath = Path1 + "test.html"


                WordDoc.SaveAs(SavePath, SaveToFormat, DocNoParam, DocNoParam, DocNoParam, DocNoParam, _
                 DocNoParam, DocNoParam, DocNoParam, DocNoParam, DocNoParam, DocNoParam, _
                 DocNoParam, DocNoParam, DocNoParam, DocNoParam)
                ' Since we don't want to save changes to the original document, Close the document, save no changes
                Dim SaveChanges As Object = False
                WordApp.Quit(SaveChanges, DocNoParam, DocNoParam)


            Catch generatedExceptionName As Exception
                Throw
            End Try
        End Sub

        Private Function isFileExist(ByVal path As String) As Boolean

            Dim flg As Boolean
            flg = File.Exists(path)
            Return flg
        End Function



        Private Sub ReadDoc(ByVal Path As String, ByVal FormName As String)

            Dim FullFilePath As String = Path
            Dim file As New FileInfo(FullFilePath)
            FormNo = FormNo.Replace(" ", "_")
            '''''''''''''''''''''''''''''''''''''''''''
            ' Get Form title for file name 

            Dim query As String = "SELECT TOP 1 [FORMTITLE]  "
            query &= " FROM Reference_form "
            query &= " where DATAFILENAME =@formID"

            Dim conn As New SqlConnection(CnString)
            Dim cmd = New SqlCommand(query, conn)
            cmd.Parameters.Add("@formID", SqlDbType.VarChar, 200)
            cmd.Parameters("@formID").Value = FormName
            Dim Reader As SqlDataReader
            Try
                conn.Open()
                Reader = cmd.ExecuteReader()
                While Reader.Read()
                    FormTitle = Reader(0)
                End While
            Catch err As Exception
            Finally
                conn.Close()
                conn = Nothing
                Reader = Nothing
                cmd = Nothing
            End Try
            '''''''''''''''''''''''''''''''''''''''''''
            If FormTitle <> "" Then
                FormTitle = FormTitle.Replace(" ", "_")
            Else
                FormTitle = "Lgislation_Form_" & FormNo
            End If
            If file.Exists Then
                Response.ContentType = "application/vnd.ms-word"
                Response.AddHeader("Content-Disposition", "inline; filename=" & FormTitle & ".doc")
                Response.AddHeader("Content-Length", file.Length.ToString())
                Response.TransmitFile(file.FullName)
            End If
        End Sub

        Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
            FormNo = FormNo.Replace(" ", "_")
            FileName = FileName & "_"
            FileName = FileName.Replace(".xml", "")
            Dim FormDoc As String = FileName & FormNo & ".doc"
            Path = Server.MapPath("xmlFiles\forms\" & FormDoc)
            Call ReadDoc(Path, FormDoc)
        End Sub

    End Class

End Namespace
