Imports System.Data
Imports System.Data.SqlClient
Imports membersarea

Partial Class ElawSubscribe
    Inherits System.Web.UI.Page

    Private ConnectionString As String
    Private Shared CnString As String
    Public Sub New()
        ConnectionString = clsConfigs.sGlobalConnectionString
        CnString = ConnectionString
        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If
    End Sub
    Private SelectedAmount As Int32
    Dim AllotedSize As Long
    Dim TotalAmmount As Int32
    Dim objUsers As New clsUsers
    Dim PaidAmmount As Int32
    Dim UserName As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        SelectedAmount = GetPrice("elaw", "paid single")
        lblTotalAmmount.Text = SelectedAmount
        lbl_site_name.Text = "ELAW"
        If IsPostBack = False Then
            fill_Profession()
            'fill_Country()

            fill_AccountType()
            fill_SiteName()
            fill_Industry()
        End If
        AllotedSize = 8000000
    End Sub
    Private Function GetPrice(ByVal SiteName As String, ByVal AccountType As String) As Int32
        Dim Query As String
        Dim DT As New DataTable
        Dim price As Int32
        Dim objDb As New clsDB

        If (Len(Trim(SiteName)) > 0 And Len(Trim(AccountType)) > 0) Then
            'Query = "select price from tblSites where sitename='" & SiteName & "' and accounttype= '" & AccountType & "'"
            Query = "select price from tblSites where sitename like '" & SiteName & "' and accounttype= 'PAID SINGLE'"
            Try
                DT = objDb.ExecuteMyQuery(Query)
                price = CInt(DT.Rows(0).Item(0))

            Catch e As Exception
                lblErrMsg.Text = "Pricing problem." & "Debug SiteName= " & SiteName

            Finally
                Query = ""
                DT = Nothing
                objDb = Nothing
            End Try
        End If


        Return price
    End Function
    Private Sub fill_Profession()
        Dim ProfessionList As New clsInfo
        ddlProfession.DataSource = ProfessionList.GetProfessions
        ddlProfession.DataBind()

    End Sub
    
    Private Sub fill_AccountType()
        Dim AccountList As New ArrayList

        AccountList.Add(" ")
        AccountList.Add("1 Year")
        AccountList.Add("2 Year")
        'AccountList.Add("Six Monthly")
        'For i = 1 To 12
        '    AccountList.Add(i & "")
        'Next

        ddlAccountType.DataSource = AccountList
        ddlAccountType.DataBind()

    End Sub
    Private Sub fill_SiteName()
        Dim obj As New clsInfo
        ddlSiteName.DataSource = obj.GetSiteNames
        ddlSiteName.DataBind()

    End Sub
    Private Sub fill_Industry()
        Dim ProfessionList As New clsInfo
        ddlIndustry.DataSource = ProfessionList.GetIndustry
        ddlIndustry.DataBind()

    End Sub
    <System.Web.Services.WebMethod()> _
    Public Shared Function CheckUserName(ByVal userName As String) As String
        Dim flg As Boolean
        Dim returnValue As String = String.Empty
        Try
            If userName.Length >= 8 Then
                flg = clsDB.isUserExist(userName)
                If flg = True Then
                    returnValue = "0"
                Else
                    returnValue = "1"
                End If
            Else
                returnValue = "0"
            End If
        Catch
            returnValue = "error"
        End Try
        Return returnValue
    End Function


    <System.Web.Services.WebMethod()> _
    Public Shared Function CheckUserEmail(ByVal U_Email As String) As String
        Dim flg As Boolean
        Dim returnValue As String = String.Empty
        Dim emailCheck As Boolean = IsEmail(U_Email)
        Try

            If emailCheck = True Then
                flg = clsDB.isUserEmailExist(U_Email, "master_users", "Email_ID")

                If flg = True Then
                    returnValue = "0"
                Else
                    returnValue = "1"
                End If
            Else
                returnValue = "0"
            End If

        Catch
            returnValue = "error"
        End Try
        Return returnValue
    End Function
    Public Shared Function IsEmail(ByVal email As String) As Boolean
        Static emailExpression As New Regex("^[\w\d][\w\d\-\._]+[\w\d]@[\w\d][\w_\-\.]*[\w\d]\.[\w]+") '//64 'New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Return emailExpression.IsMatch(email)
    End Function
    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        Dim objDB As New clsDB
        Dim objUser As New clsUsers
        Dim Query As String
        Dim flg As Boolean
        Dim Sib_UserName As String = Trim(sub_txtUserName.Text)
        Dim ConfirmationDate1 As Date = System.DateTime.Today.ToString("yyyy/MM/dd")
        Dim PaymentMode As String = ddlAccountType.SelectedItem.Text
        Dim OnlineStatus As String = "OFFLINE"
        Dim AccountType As String = "Single" ' ddlLoginType.SelectedItem.Text '"PAID SINGLE"  '"PAID SINGLE / corporate"
        'Dim AllotedSize As Long '= Val(txtAllotedData.Text) '8192000
        Dim noOfUsers As Int16 = 1
        'Dim Status As String = ddlStatus.SelectedItem.Text
        Dim user_id As Long
        Dim AccountExpiry As Date
        Dim IntervalType As DateInterval
        Dim months As Double
        Dim SiteName As String = lbl_site_name.Text
        Dim BillNo As Long
        Dim BalanceAmmount As Long
        Dim ConfirmationDate As Date = CType(Year(ConfirmationDate1) & "-" & Month(ConfirmationDate1) & "-" & Day(ConfirmationDate1), DateTime)
        TotalAmmount = lblTotalAmmount.Text
        Dim profession As String = ddlProfession.SelectedItem.Text


        If Sib_UserName = "" Or txtEmail.Text = "" Or txtFirstName.Text = "" Or txtLastName.Text = "" Or txtTelephone.Text = "" Then
            lblErrMsg.Visible = True
            Me.lblErrMsg.Text = "Please Fill In All Required Files!"
            Exit Sub
        Else
            lblErrMsg.Visible = False
            Me.lblErrMsg.Text = ""
        End If
        ConfirmationDate = Format$(Now(), "yyyy-MM-yy")
        BalanceAmmount = CInt(Trim(lblTotalAmmount.Text))
        IntervalType = DateInterval.Month
        '======================= Expiry date for 2 years before 31 Oct , after that 1 year 
        If Date.Today <= "10/31/2013" Then
            AccountExpiry = "10/31/2015"
        Else
            months = 12
            AccountExpiry = DateAdd(IntervalType, months, ConfirmationDate)
        End If
        Dim MaxCustomerId As Integer = objUsers.getmax_customer_id()
        flg = clsDB.isUserExist(Sib_UserName)
        Dim txtSalesRepresentative As String = ""
        If txtSalesRepresentative = "" Then
            txtSalesRepresentative = txtFirstName.Text & " " & txtLastName.Text
        End If
        If Sib_UserName.Length < 8 Then
            '' this means user exists
            lblErrMsg.Visible = True
            Me.lblErrMsg.Text = "User Name Should be more than 8 charters."
            sub_txtUserName.Focus()
            Exit Sub
        Else
            lblErrMsg.Visible = False
        End If
        If flg = True Then
            '' this means user exists
            Me.lblErrMsg.Text = "User with the same username already exists, please choose another username."
            sub_txtUserName.Focus()
            Exit Sub
        Else
            lblErrMsg.Visible = False
        End If
        flg = clsDB.isUserEmailExist(txtEmail.Text, "master_users", "Email_ID")
        If flg = True Then
            '' this means user exists
            lblErrMsg.Visible = True
            Me.lblErrMsg.Text = "User with the same email address already exists, please choose another email address."
            txtEmail.Focus()
            Exit Sub
        Else
            lblErrMsg.Visible = False
        End If
        If CheckBox1.Checked = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Message", "<script> alert('Please tick the checkbox to agree to terms of using eLaw.');</script>", False)
            Exit Sub
        End If
        Try


            flg = objUsers.InsertFreeUser(txtFirmName.Text, txtAddress1.Text, txtAddress2.Text, txtCity.Text, txtState.Text, txtPostCode.Text, "Malaysia", txtTelephone.Text, _
                                            txtFax.Text, "Inactive", txtFirstName.Text, txtLastName.Text, profession.ToString(), txtTelephone.Text, txtEmail.Text, txtSalesRepresentative, _
                                            AllotedSize, Trim(AccountType), _
                                            Sib_UserName, Trim(sub_txtPassword.Text), MaxCustomerId, MaxCustomerId, MaxCustomerId, MaxCustomerId, AccountExpiry, "OnlineReg", "", SiteName)

            Dim userIDno As Integer = objUsers.GetMaxIDmaster_users()
            Query = "insert into  master1 ([user_id_no],[FirstName],[LastName],User_Name,Password,Profession,Industry,Firm_Name,Site_Name,Add_1,Add_2,city,State,PosCode,Country,Work_Phone,Fax_Number,Email_ID,Rep,Registration_date,AllotedSize,User_No, age, sex, Confirmation_date,Acct_Expiry,Payment_Period,Acct_type,ModeofPayment,Total_Amount, Amount_Paid,ProductName)"
            Query &= " values (" & userIDno & " , '" & Trim(txtFirstName.Text) & "' , '" & Trim(txtLastName.Text) & "', '" & Trim(Sib_UserName) & "',"
            Query &= "'" & Trim(sub_txtPassword.Text) & "' , '" & ddlProfession.SelectedItem.Text & "' , '" & ddlIndustry.SelectedItem.Text & "' , '" & Trim(txtFirmName.Text) & "',"
            Query &= "'" & SiteName & "' , '" & Trim(txtAddress1.Text) & "', '" & Trim(txtAddress2.Text) & "', '" & Trim(txtCity.Text) & "', '" & Trim(txtState.Text) & "', '" & Trim(txtPostCode.Text) & "',"
            Query &= "'Malaysia', '" & Trim(txtTelephone.Text) & "', '" & Trim(txtFax.Text) & "', '" & Trim(txtEmail.Text) & "', '" & Trim(txtSalesRepresentative) & "', '" & Format$(Now(), "yyyy-MM-yy") & "' , " & AllotedSize & ","
            Query &= "" & noOfUsers & " , '00', 'NoN', '" & Format$(Now(), "yyyy-MM-yy") & "','" & AccountExpiry & "'  ,'" & Trim(PaymentMode) & "', '" & Trim(AccountType) & "',  'Credit Card' ,"
            Query &= " " & TotalAmmount & " ,  " & PaidAmmount & " , '" & lbl_site_name.Text & "')"
            flg = objUser.AddRecord(Query)
            Query = "insert into  tblOnlineUsers (User_Name,OnlineStatus) values ('" & Sib_UserName & "','" & OnlineStatus & "' )"
            flg = objUser.AddRecord(Query)
            BillNo = objUser.getLastBillNoForCreditCard() 'this value will be use for retrieving values from table on the confirmation page
        Catch err As Exception
            'lblErrMsg.Text &= err.Message
        End Try

        objUser = Nothing
        objDB = Nothing
        Query = ""
        PaymentMode = ""
        OnlineStatus = ""
        AccountType = ""
        AllotedSize = 0
        noOfUsers = 0
        user_id = 0
        months = 0.0
        SiteName = ""
        UserName = ""
        '   BillNo = 0
        BalanceAmmount = 0
        flg = True
        If flg = True Then
            lblErrMsg.Text = "Successful"
            Response.Redirect("ShowUserDetailsForConfirmation.aspx?id=" & BillNo)
        End If

    End Sub

End Class
