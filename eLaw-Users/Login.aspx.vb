Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports membersarea
Imports System.Net.Mail
Imports System.IO

Partial Class ElawLogin
    Inherits System.Web.UI.Page
    Private ConnectionString As String
    Private Shared CnString As String
    Dim IPAddress As String
    Public Sub New()
        ConnectionString = clsConfigs.sGlobalConnectionString
        CnString = ConnectionString
        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If

    End Sub
    Dim UserName As String ' Assign this value to seesion 
    Dim objUsers As New clsUsers
    Dim ActivationCode As String ' used for first time to acivete free trial for  user account 
    Dim UserEmail As String ' Save User email to send him/her email in case account expired 
    Dim ExpiryDate As Date ' Get Expiry Date and compare to current Date
    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        Dim fLg As Boolean
        Dim Expired As Integer
        Dim Status As Byte
        Dim onlineStatus As String ' check current status for user before login 
        Dim Password As String
        Dim msg As String
        Dim IPAddress As String
        Dim isSiteSubscribed As Boolean = False
        Dim RemoteAddress As String = Request.UserHostAddress
        Dim ExpiredDetail As New ArrayList
        Dim ExtendDate As Date ' used only for single account which we give 7 days even after account expired
        Dim DT As New DataTable
        Dim FlgLogin As Boolean = False ' USER ACCOUNT TYPE  false=Single , Free , true = Group 
        Dim MutliLoginAcc As Integer ' Store number of session given by admin for current use 
        Dim MultiLoginStatus As Integer
        UserName = Trim(txtUsername.Text)
        Password = Trim(txtPassword.Text)
        If (Len(UserName) = 0 Or Len(Password) = 0) Then
            lblMsg.Visible = True
            lblMsg.Text = "Username or password required"
        End If
        Try
            fLg = objUsers.getLoginAuthenticated(UserName, Password)
            If fLg = False Then
                lblMsg.Visible = True
                lblMsg.Text = "Either username or password is not correct."
                Exit Sub
            End If
            
            Dim FlgIpRange As Boolean
            Dim flgIp As Boolean
            flgIp = objUsers.IsExistInIpRange(UserName)
            If flgIp = True Then
                FlgIpRange = objUsers.CheckIpRange(UserName, RemoteAddress)
                If FlgIpRange = False Then
                    lblMsg.Visible = True
                    lblMsg.Text = "Unfortunately, Your are trying to login from IP: " & Request.UserHostAddress.ToString & " which is not listed under your Ip range"
                    Exit Sub
                End If
            End If


            'Return user expiry infromation in arraylist
            ExpiredDetail = objUsers.ExpiryDateAuthentication(UserName)
            If ExpiredDetail.Count > 0 Then
                Expired = ExpiredDetail(0)
                If Expired = 1 Then
                    ExtendDate = ExpiredDetail(2)
                    ExtendDate = ExtendDate.AddDays(ExpiredDetail(1))
                    If DateTime.Compare(ExtendDate, Date.Now) <= 0 Then
                        ExpiryDate = ExpiredDetail(2)
                        DT = objUsers.GetUserEmailByUserName(UserName)
                        If Not IsDBNull(DT.Rows(0).Item(0)) Then
                            SendReminder(DT.Rows(0).Item(1), DT.Rows(0).Item(0))
                        End If
                        DT = Nothing
                        Response.Redirect("~/expiredAcc.html")
                        Exit Sub
                    End If
                ElseIf Expired = 2 Then
                    lblMsg.Visible = True
                    lblMsg.Text = "Unfortunately, there is a problem accessing your account. Please call us at 1300-88-3529 (eLaw)."
                    Exit Sub
                End If
            End If
            Status = objUsers.getUserStatus(UserName)
            If Status = 1 Then
                lblMsg.Visible = True
                lblMsg.Text = "Your account has not been activated. Please click on the activation link sent to your email. Should you require assistance, please call us at 1300-88-3529 (eLaw)"
                Exit Sub
            ElseIf Status = 2 Then
                lblMsg.Visible = True
                lblMsg.Text = "Unfortunately, there is a problem accessing your account. Please call us at 1300-88-3529 (eLaw)."
                Exit Sub
            End If
            
normal:

        Catch Err As Exception
            msg = Err.Message
        Finally
        End Try
        

        If fLg = True Then
            'check if user group or single , group =true , single=false
            If objUsers.isUserCorporate(UserName) = False Then
                'Get User Account for login Report 
                FlgLogin = True
                'get user status 
                onlineStatus = objUsers.getUserOnlineStatus(Trim(UserName))
                If String.Compare(onlineStatus, "Online1", True) = 0 Then
                    lblMsg.Visible = True
                    'objUsers.updateOnlineUserStatus(UserName, "Online2")
                    lblMsg.Text = "Unfortunately, the system has detected too many simultaneous connections. Please try logging in again shortly"
                    objUsers = Nothing
                    fLg = False
                    UserName = ""
                    Exit Sub
                ElseIf String.Compare(onlineStatus, "Online", True) = 0 Then
                    objUsers.updateOnlineUserStatus(UserName, "Online1")
                ElseIf String.Compare(onlineStatus, "Offline", True) = 0 Then
                    objUsers.updateOnlineUserStatus(UserName, "Online")
                End If
            Else
                FlgLogin = True
                onlineStatus = objUsers.getUserOnlineStatus(Trim(UserName))
                MutliLoginAcc = objUsers.GetMultiSessionGroup(Trim(UserName))
                If MutliLoginAcc > 0 Then
                    MultiLoginStatus = objUsers.ProcessMultiSession(Trim(UserName), onlineStatus, MutliLoginAcc)
                    If MultiLoginStatus = 1 Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Unfortunately, the system has detected too many simultaneous connections. Please try logging in again shortly"
                        objUsers = Nothing
                        fLg = False
                        UserName = ""
                        Exit Sub
                    End If
                End If

            End If
            Session.Add("USERNAME", UserName)
            If rememberme.Checked Then
                Response.Cookies("UserName").Expires = DateTime.Now.AddMonths(2)
                Response.Cookies("Password").Expires = DateTime.Now.AddMonths(2)
            Else
                Response.Cookies("UserNameUserName").Expires = DateTime.Now.AddMonths(-1)
                Response.Cookies("Password").Expires = DateTime.Now.AddMonths(-1)
            End If
            Response.Cookies("UserName").Value = txtUsername.Text
            Response.Cookies("Password").Value = txtPassword.Text


            'return url by abdo
            Dim returnUrl = Request.QueryString("returnUrl")
            If String.IsNullOrEmpty(returnUrl) Then
                'Add Login Record for Login Report
                IPAddress = Request.UserHostAddress.ToString()
                If FlgLogin = True Then
                    fLg = objUsers.AddLogin(UserName, "Single", IPAddress)
                Else
                    fLg = objUsers.AddLogin(UserName, "Group", IPAddress)
                End If
                returnUrl = "~\casesSearch.aspx"
            End If
            'end of reurn url by abdo
            objUsers = Nothing
            fLg = False
            UserName = ""
            Response.Redirect(returnUrl)
        End If
    End Sub

    Function isAllowedIP(ByVal strpubIP As String) As Boolean

        Dim query As String = "SELECT macaddress,pcname,ip,lock,username FROM allowedip where ip='" & strpubIP & "' and lock = 0"
        Dim obj As New clsCasesSearch
        Dim dt As New DataTable
        dt = obj.ExecuteMyQuery(query)

        If dt.Rows.Count > 0 Then

            Dim ipsession As String = dt.Rows(0).Item("username")

            Session("USERNAME") = ipsession



            'return url by abdo
            Dim returnUrl = Request.QueryString("returnUrl")
            If String.IsNullOrEmpty(returnUrl) Then
                returnUrl = "casessearch.aspx"
            End If
            'end of reurn url by abdo
            Response.Redirect(returnUrl)


            Return True
        End If

        Return False
    End Function
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        '
        Dim strip As String = Request.UserHostAddress
        isAllowedIP(strip)
        UserName = CStr(Session.Item("UserName"))
        If UserName <> "" And UserName = Trim(txtUsername.Text) Then
            Response.Redirect("index1.aspx")
        End If

        'to allowed ip


        ''''''''

        lblMsg.Visible = False
        ''''''''''''''''''''''''''''Remember Us and PW
        '
        If Request.QueryString("code") <> "" Then
            'Recieved encrypted  Code 
            ActivationCode = Request.QueryString("code")
            'function to decrypt the code 
            Dim UrlDecrpt As New Dba_UrlEncrption(ActivationCode, False)
            Dim UrlQuery As String = UrlDecrpt.UrlDecrypt
            'send details to perpare user information
            Call PerpareInformation(UrlQuery)
        End If
        If Not IsPostBack Then
            If ((Not (Request.Cookies("UserName")) Is Nothing) AndAlso (Not (Request.Cookies("Password")) Is Nothing)) Then
                txtUsername.Text = Request.Cookies("UserName").Value
                txtPassword.Text = Request.Cookies("Password").Value
            End If
        End If
    End Sub
    ''' <summary>
    ''' Call this function for first time user to activate user account 
    ''' </summary>
    ''' <param name="Info"></param>
    ''' <remarks></remarks>
    Sub PerpareInformation(ByVal Info As String)
        Dim objClsUser As New clsUsers
        Dim ArrInfo() As String = Split(Info, "#")
        UserEmail = ArrInfo(0)
        Dim flg As Boolean
        UserName = ArrInfo(1)
        Dim CID As String = ArrInfo(2)
        Dim Userid As Integer = ArrInfo(3)
        txtUsername.Text = UserName
        flg = objClsUser.UpdateAccountTypeStatus("Free", CID, CID, Userid)
    End Sub
    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        txtUsername.Text = ""
        txtPassword.Text = ""
    End Sub
    ''' <summary>
    ''' send an email when user account already expired 
    ''' </summary>
    ''' <param name="toAddress"></param>
    ''' <param name="Fn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function SendReminder(ByVal toAddress As String, ByVal Fn As String) As Boolean
        Dim result As Boolean = False
        Dim GetBody As String = PerpareBodyMessage(Fn)
        Try
            Dim mail As New MailMessage()
            'Dim fn As System.Net.Mime.ContentDisposition
            Dim SmtpServer As New SmtpClient("mail.elaw.my", 25)
            mail.From = New MailAddress("subscribe@elaw.my", "The Digital Library-Elaw")
            mail.[To].Add(toAddress)
            mail.Subject = "User Account Expiry Date "
            mail.IsBodyHtml = True
            mail.Body = GetBody
            SmtpServer.EnableSsl = False
            SmtpServer.UseDefaultCredentials = True
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpServer.Credentials = New System.Net.NetworkCredential("subscribe@elaw.my", "Subscribe123#")
            SmtpServer.Send(mail)
            result = True
        Catch ex As Exception
            result = False
        End Try
        Return result
    End Function
    ''' <summary>
    ''' Read html file to get all content for the emial 
    ''' </summary>
    ''' <param name="Fn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function PerpareBodyMessage(ByVal Fn As String) As String
        Dim line As String
        Dim FileEmail = Server.MapPath("~\EmailMessage\expiredTrial.html")
        Using reader As StreamReader = New StreamReader(FileEmail)
            ' Read one line from file
            line = reader.ReadToEnd
        End Using
        line = line.Replace("{{Name}}", Fn)
        line = line.Replace("{{Date}}", ExpiryDate.ToString("D"))
        Return line
    End Function
End Class
