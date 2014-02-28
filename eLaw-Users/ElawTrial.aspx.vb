Imports membersarea
Imports System.IO
Imports System.Net.Mail
Imports System.Data

Partial Class ElawTrial
    Inherits System.Web.UI.Page
    Private ConnectionString As String
    Private Shared CnString As String
    Public Sub New()
        ConnectionString = clsConfigs.sGlobalConnectionString
        CnString = ConnectionString
        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file")
        End If
    End Sub
    Dim link As String
    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim objDB As New clsDB
        Dim objUser As New clsUsers
        Dim flg As Boolean
        Dim txtSalesRepresentative As String = FirstName.Text
        Dim profession As String = pro.SelectedItem.Text
        Dim MaxCustomerId As Integer = objUser.getmax_customer_id()
        Dim usname As String = Trim(us.Text)
        Dim AccountExpiry As Date = Date.Now.AddDays(3)
        Dim PaidAmmount As Int32
        Dim TotalAmmount As Int32 = 1800
        '================Validation section =======================
        flg = clsDB.isUserExist(usname)
        If usname.Length < 8 Then
            '' this means user exists
            lblErrMsg.Visible = True
            Me.lblErrMsg.Text = "User Name should be more than 8 characters"
            us.Focus()
            Exit Sub
        End If
        If flg = True Then
            '' this means user exists
            lblErrMsg.Visible = True
            Me.lblErrMsg.Text = "User with the same username already exists, please choose another username"
            us.Focus()
            Exit Sub
        End If

        flg = clsDB.isUserEmailExist(txtEmail.Text, "ElawTrialUser", "UserEmail")
        If flg = True Or isfakedomain(txtEmail.Text) Then
            lblErrMsg.Visible = True
            Me.lblErrMsg.Text = "User with the same email address already exists, please choose another email address"
            txtEmail.Focus()
            Exit Sub
        End If
        flg = objUser.InsertFreeUser(org.Text, "None", "None", "None", "None", "00000", "Malaysia", Trim(ContactNo.Text), _
                                            "", "Inactive", FirstName.Text, LastName.Text, profession.ToString(), Trim(ContactNo.Text), txtEmail.Text, txtSalesRepresentative, _
                                            180, "Free", _
                                            usname, Trim(password.Text), MaxCustomerId, MaxCustomerId, MaxCustomerId, MaxCustomerId, AccountExpiry, "OnlineReg", "", "Elaw")

        Dim Query As String
        Dim userIDno As Integer = objUser.GetMaxIDmaster_users()
        Query = "insert into  master1 ([user_id_no],[FirstName],[LastName],User_Name,Password,Profession,Industry,Firm_Name,Site_Name,Add_1,Add_2,city,State,PosCode,Country,Work_Phone,Fax_Number,Email_ID,Rep,Registration_date,AllotedSize,User_No, age, sex, Confirmation_date,Acct_Expiry,Payment_Period,Acct_type,ModeofPayment,Total_Amount, Amount_Paid,ProductName)"
        Query &= " values (" & userIDno & " , '" & Trim(FirstName.Text) & "' , '" & Trim(LastName.Text) & "', '" & Trim(usname) & "',"
        Query &= "'" & Trim(password.Text) & "' , '" & profession.ToString() & "' , '" & ddlIndustry.SelectedItem.Text & "' , '" & Trim(org.Text) & "',"
        Query &= "'Elaw' , 'None', 'None', 'None', 'None', '00000',"
        Query &= "'Malaysia', '" & ContactNo.Text & "', '000000000000', '" & Trim(txtEmail.Text) & "', '" & Trim(txtSalesRepresentative) & "', '" & Format$(Now(), "yyyy-MM-yy") & "' , " & 300 & ","
        Query &= "1 , '00', 'None', '" & Format$(Now(), "yyyy-MM-yy") & "','" & AccountExpiry & "'  ,'', 'Free',  'Credit Card' ,"
        Query &= " " & TotalAmmount & " ,  " & PaidAmmount & " , 'Elaw')"

        flg = objUser.AddRecord(Query)
        Query = "insert into  tblOnlineUsers (User_Name,OnlineStatus) values ('" & usname & "','Offline' )"
        flg = objUser.AddRecord(Query)
        flg = objUser.AddTrialAccount(FirstName.Text, LastName.Text, password.Text, txtEmail.Text, 0, 3, usname, Server.MachineName.ToString(), Request.UserHostAddress.ToString())
        If flg = True Then
            Dim UrlEncrption As String = txtEmail.Text & "#" & usname & "#" & MaxCustomerId & "#" & userIDno
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            link = UrlLink.UrlEncrypt
            SendEmail(txtEmail.Text)
        End If
        objUser = Nothing
        objDB = Nothing
        If flg = True Then
            'main.Visible = False
            'lblErrMsg.Visible = True
            Response.Redirect("~/ThanksTrialAccount.html")
            'lblErrMsg.Text = "<br><br><br>Congratulation, Your Account has been successfully Created, Please Check your Email To activate Your Trial Account <br><br><br>"
        End If

    End Sub
    Function isfakedomain(ByVal domain As String) As Boolean
        Try

            domain = domain.Replace(" ", "") 'ignore sql exception 
            If domain.Contains("@") Then
                domain = domain.Split("@")(1)
            End If

            Dim query As String = "SELECT domain from fakedomain where domain = '" & domain & "'"
            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            dt = obj.ExecuteMyQuery(query)
            If dt.Rows.Count > 0 Then
                Return True
            End If
            Return False

        Catch ex As Exception
            Return True
        End Try
    End Function
    Protected Function SendEmail(ByVal toAddress As String) As Boolean
        Dim result As Boolean = False
        Dim GetBody As String = PerpareBodyMessage(FirstName.Text)
        Try
            Dim mail As New MailMessage()
            'Dim fn As System.Net.Mime.ContentDisposition
            Dim SmtpServer As New SmtpClient("mail.elaw.my", 25)
            mail.From = New MailAddress("subscribe@elaw.my", "The Digital Library")
            mail.[To].Add(toAddress)
            mail.Bcc.Add("subscribe@elaw.my")
            mail.Subject = "User Details For The Digital Library Trial Account"
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


    Function PerpareBodyMessage(ByVal Fn As String) As String
        Dim line As String
        Dim activateCode As String = "<a href='http://www.elaw.my/login.aspx?code=" & link & "'>Click here to activate your account and experience the difference now</a>"
        Dim FileEmail = Server.MapPath("~\EmailMessage\activatecode.html")
        Using reader As StreamReader = New StreamReader(FileEmail)
            ' Read one line from file
            line = reader.ReadToEnd
        End Using
        'Dim Email_date As String = DateTime.Now.Day & "/" & DateTime.Now.Month & "/" & DateTime.Now.Year
        line = line.Replace("{{Name}}", Fn)
        line = line.Replace("{{link}}", activateCode)

        Return line
    End Function

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        fill_Industry()
        fill_Profession()
    End Sub
    Private Sub fill_Profession()
        Dim ProfessionList As New clsInfo
        pro.DataSource = ProfessionList.GetProfessions
        pro.DataBind()

    End Sub
    Private Sub fill_Industry()
        Dim ProfessionList As New clsInfo
        ddlIndustry.DataSource = ProfessionList.GetIndustry
        ddlIndustry.DataBind()

    End Sub
End Class