Imports membersarea
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Partial Class ForGetPassword
    Inherits System.Web.UI.Page
    Dim objUsers As New clsUsers
    Public errormsg As String
    Dim Email As String
    Dim Fn As String
    Dim E_pw As String
    Dim E_username As String
    Public E_Success As String
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            errormsg = ""
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim DT As New DataTable
        Dim flg As Boolean
        Email = txtemail.Text
        errormsg = ""
        Dim Query = "SELECT First_Name,[User_Name],[Password],[Email_ID] FROM master_users where  Email_ID='" & Email & "'"
        DT = objUsers.FetchDataSet(Query)
        Try
            If DT.Rows.Count > 0 Then
                E_username = DT.Rows(0).Item("User_Name")
                Fn = DT.Rows(0).Item("First_Name")
                E_pw = DT.Rows(0).Item("Password")
                flg = SendEmail(Email)

            Else
                errormsg = " <span class='validationLogInTrial'>We can not find" & Email & " In Our Records, Please Provide Your Email </span>"
            End If
        Catch ERR As Exception
            Dim msg As String
            msg = ERR.Message

        End Try
        If flg = True Then
            errormsg = ""
            txtemail.Text = ""
            Button1.Enabled = False

            E_Success = "<span class='validationLogInTrial' style='width:550px;'>An email has been sent to your email address, Thank you for keeping in touch with eLaw</span> "
        Else
            E_Success = "<span class='validationLogInTrial' style='width:550px;'>An error occurred , Please try later or contact us for futher information </span> "
        End If
    End Sub
    Protected Function SendEmail(ByVal toAddress As String) As Boolean
        Dim result As Boolean = False
        Dim GetBody As String = PerpareBodyMessage(Fn, E_username, E_pw, Email)
        Try
            Dim mail As New MailMessage()
            'Dim fn As System.Net.Mime.ContentDisposition
            Dim SmtpServer As New SmtpClient("mail.elaw.my", 25)
            mail.From = New MailAddress("subscribe@elaw.my", "Request Password-elaw.my ")
            mail.[To].Add(toAddress)
            '   mail.[To].Add("dba_1988@ymail.com")
            mail.Subject = "Password Request ,Elaw.my"
            mail.IsBodyHtml = True
            mail.Body = GetBody
            SmtpServer.EnableSsl = False
            SmtpServer.UseDefaultCredentials = True
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpServer.Credentials = New System.Net.NetworkCredential("ali@elaw.my", "Ali@2013")
            SmtpServer.Send(mail)
            result = True
        Catch ex As Exception
            result = False
        End Try
        Return result
    End Function
    Function PerpareBodyMessage(ByVal Fn As String, ByVal us As String, ByVal userpassword As String, ByVal emial As String) As String
        Dim line As String

        Dim FileEmail = Server.MapPath("~\EmailMessage\EmailForgetPassword.html")
        Using reader As StreamReader = New StreamReader(FileEmail)
            ' Read one line from file
            line = reader.ReadToEnd
        End Using
        'Dim Email_date As String = DateTime.Now.Day & "/" & DateTime.Now.Month & "/" & DateTime.Now.Year
        line = line.Replace("{{Name}}", Fn)
        line = line.Replace("{{username}}", us)
        line = line.Replace("{{password}}", userpassword)
        line = line.Replace("{{email}}", emial)
        Return line
    End Function
End Class
