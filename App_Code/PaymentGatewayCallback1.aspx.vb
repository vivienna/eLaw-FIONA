Imports System.Diagnostics
Imports System.Data


Namespace membersarea

Public Class PayementGatewayCallback
    Inherits System.Web.UI.Page


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim TestingValue As String
    Dim TrasactionValue As String
    Dim BillNo As Long
    Dim TrasactionStatus As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        If Request.QueryString("MC_log") <> "" Then
            'TestingValue = CStr(Server.UrlDecode(Request.QueryString("MC_log")))
            TestingValue = CStr(Request.QueryString("MC_log"))
        End If

        If TestingValue <> "" Then
            Me.TestingCallback()

        End If


        If Request.QueryString("M_TrasactionValue") <> "" Then
            TrasactionValue = CStr(Server.UrlDecode(Request.QueryString("M_TrasactionValue")))
        End If

        If Request.QueryString("cartId") <> "" Then
            BillNo = CLng(Server.UrlDecode(Request.QueryString("cartId")))
        End If

        'If Request.QueryString("transStatus") <> "" Then
        'TrasactionStatus = CStr(Server.UrlDecode(Request.QueryString("transStatus")))
        'End If
        TrasactionStatus = Request.Form("transStatus")
        Me.TestingCallback1(TrasactionStatus)

        TestingValue = Request.Form("MC_log")
        Me.TestingCallback1(TestingValue)

        'Poid = getsess("oid")               ' if not session, get from eway return
        'If poid = "" Then
        '    shoperror(getlang("langCheckoutProcessed"))
        '    poid = Request("oid")
        '    setsess("oid", poid)
        'End If

    End Sub

    Private Sub TestingCallback()
        Dim Query As String
        Dim obj As New clsUsers
        Dim err As String
        Dim Log As New EventLog
        Query = "insert into tblTestingCallback (FLD1) "
        Query &= "values ('" & TestingValue & "') "
        Try
            obj.AddRecord(Query)

        Catch ex As Exception
            err = ex.Message()

            Log.Source = "Credit card Callback"
            Log.WriteEntry(err, EventLogEntryType.Error)
        Finally
            Log.Source = "Credit card Callback"
            Log.WriteEntry("Callback was successful", EventLogEntryType.SuccessAudit)

            Query = ""
            obj = Nothing
            err = ""
        End Try

    End Sub

    Private Sub TestingCallback1(ByVal Value As String)
        Dim Query As String
        Dim obj As New clsUsers
        Dim err As String
        Dim Log As New EventLog
        Query = "insert into tblTestingCallback (FLD1) "
        Query &= "values ('" & Value & "') "
        Try
            obj.AddRecord(Query)

        Catch ex As Exception
            err = ex.Message()

            Log.Source = "Credit card Callback"
            Log.WriteEntry(err, EventLogEntryType.Error)
        Finally
            Log.Source = "Credit card Callback"
            Log.WriteEntry("Callback was successful", EventLogEntryType.SuccessAudit)

            Query = ""
            obj = Nothing
            err = ""
        End Try

    End Sub


    Private Sub LogTransaction()
        Dim Query As String
        Dim obj As New clsUsers
        Dim err As String
        Query = "insert into tblCreditCardTransactionDetail (Bil,LogData,TransactionStatus) "
        Query &= "values (" & BillNo & ",'" & TrasactionValue & "','" & TrasactionStatus & " ) "
        Try
            obj.AddRecord(Query)

        Catch ex As Exception
            err = ex.Message()
        Finally
            Query = ""
            obj = Nothing
            err = ""
        End Try

    End Sub


    Private Sub CreateNewUser()
        Dim Query As String
        Dim obj As New clsUsers
        Dim err As String

        Query = "insert into tblCreditCardTransactionDetail (Bil,LogData,TransactionStatus) "
        Query &= "values (" & BillNo & ",'" & TrasactionValue & "','" & TrasactionStatus & " ) "
        Try
            obj.AddRecord(Query)

        Catch ex As Exception
            err = ex.Message()
        Finally
            Query = ""
            obj = Nothing
            err = ""
        End Try

    End Sub



End Class

End Namespace


