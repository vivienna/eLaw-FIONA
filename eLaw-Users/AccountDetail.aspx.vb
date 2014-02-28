'/********************************************************************/
'/*	Developer 	    : Mohamed I. Elsayed  								*/
'/*	Company     	: The Digital Library Sdn. Bhd.		    					*/
'/* Date Modified	: April 2013									*/
'/*	Description		: User account details in memebers area         */
'/*	Version			: 3.0											*/
'/********************************************************************/
Imports System.Data
Imports System

Namespace membersarea

Partial Class AccountDetail1
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

    Private UserName As String
    Dim UserId As Long
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim obj As New clsUsers
        Dim DT As New DataTable
            UserName = CType(Session("UserName"), String)
            'Label1.Text = UserName
        If UserName = "" Then
		 Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            'Server.Transfer("login.aspx")
        End If
            'DT = obj.FetchDataSet("select user_id_no from master_users where user_name='" & UserName & "'")
        'UserId = DT.Rows(0).Item(0)
        UserId = obj.getUserId(UserName)

            'lblUserDetails.Text = UserDetails()
            'lblUserDetails.Text &= UserAdvanceDetails()
            UserAdvanceDetails()


    End Sub

        'Private Function UserDetails() As String
        '    Dim sbUserDetail As New System.Text.StringBuilder
        '    Dim DT As New DataTable
        '    Dim objClsUser As New clsUsers
        '    Try
        '        DT = objClsUser.getSingleUserBasicDetails(UserId)

        '            'city,state,postal_code,Country,Phone_number,Fax_number,Email_ID 
        '            sbUserDetail.Append("<table id='table-design'><thead><th colspan='4'>Personal Details</th></thead><tbody>")

        '            sbUserDetail.Append("<tr><td>User Name</td><td>" & DT.Rows(0).Item(3) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>First Name</td><td>" & DT.Rows(0).Item(0) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Last Name</td><td>" & DT.Rows(0).Item(1) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Firm Name</td><td>" & DT.Rows(0).Item(2) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Registration Date</td><td>" & DT.Rows(0).Item(4) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Allocated Size</td><td>" & DT.Rows(0).Item(5) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Address 1 </td><td>" & DT.Rows(0).Item(6) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Address 2 </td><td>" & DT.Rows(0).Item(7) & "</td></tr>")

        '            sbUserDetail.Append("<tr><td>City</td><td>" & DT.Rows(0).Item(8) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>State</td><td>" & DT.Rows(0).Item(9) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Postal/ZIP Code</td><td>" & DT.Rows(0).Item(10) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Country</td><td>" & DT.Rows(0).Item(11) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Phone Number</td><td>" & DT.Rows(0).Item(12) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Fax Number </td><td>" & DT.Rows(0).Item(13) & "</td></tr>")
        '            sbUserDetail.Append("<tr><td>Email_ID </td><td>" & DT.Rows(0).Item(14) & "</td></tr>")
        '            sbUserDetail.Append("</tbody></table><br>")

        '    Catch Err As Exception
        '            'lblMsg.Text = Err.Message
        '    Finally


        '        DT = Nothing
        '        objClsUser = Nothing
        '    End Try



        '    Return sbUserDetail.ToString
        'End Function

        Private Sub UserAdvanceDetails()
            Dim sbUserDetail As New System.Text.StringBuilder
            Dim DT As New DataTable
            'Dim Count As Int16
            'Dim i As Int16
            Dim objClsUser As New clsUsers '45678910 11 12 13 14
            DT = objClsUser.getSingleUserBasicDetails(UserId)
            fullName.Text = DT.Rows(0).Item(0).ToString().Trim() & " " & DT.Rows(0).Item(1).ToString().Trim()
            userNameID.Text = DT.Rows(0).Item(3).ToString().Trim()

            firstName.Text = DT.Rows(0).Item(0).ToString().Trim()
            lastName.Text = DT.Rows(0).Item(1).ToString().Trim()
            firmName.Text = DT.Rows(0).Item(2).ToString().Trim()
            regDate.Text = If(DT.Rows(0).Item(4).ToString() <> "", CType(DT.Rows(0).Item(4), Date).ToString("dd/MM/yyyy").Trim(), "")
            addr1.Text = DT.Rows(0).Item(6).ToString().Trim()
            addr2.Text = DT.Rows(0).Item(7).ToString().Trim()
            city.Text = DT.Rows(0).Item(8).ToString().Trim()
            state.Text = DT.Rows(0).Item(9).ToString().Trim()
            PCode.Text = DT.Rows(0).Item(10).ToString().Trim()
            country.Text = DT.Rows(0).Item(11).ToString().Trim()
            phone.Text = DT.Rows(0).Item(12).ToString().Trim()
            fax.Text = DT.Rows(0).Item(13).ToString().Trim()
            email.Text = DT.Rows(0).Item(14).ToString().Trim()
            subsc.Text = DT.Rows(0).Item(15).ToString().Trim()

            DT = objClsUser.getSingleUserAdvanceDetails(UserId)
            confDate.Text = If(DT.Rows(0).Item(0).ToString() <> "", CType(DT.Rows(0).Item(0), Date).ToString("dd/MM/yyyy").Trim(), "")
            expDate.Text = If(DT.Rows(0).Item(1).ToString() <> "", CType(DT.Rows(0).Item(1), Date).ToString("dd/MM/yyyy").Trim(), "")
            payMeth.Text = DT.Rows(0).Item(2).ToString().Trim()
            CardTyp.Text = DT.Rows(0).Item(3).ToString().Trim()
            chqDate.Text = If(DT.Rows(0).Item(11).ToString() <> "", CType(DT.Rows(0).Item(11), Date).ToString("dd/MM/yyyy").Trim(), "")
            ttlAmount.Text = DT.Rows(0).Item(12).ToString().Trim()
            pdAmount.Text = DT.Rows(0).Item(13).ToString().Trim()

            'Try


            '        'Count = DT.Rows.Count
            '        ''Confirmation_Date, Acct_Expiry, Mode_Of_Payment,Card_Type,Name_On_Card, Credit_Card_No, CC_Expiry_Date, Credit_Bank_Name, Cheque_Number, Cheque_Bank_Name,Cheque_Holder_Name, Cheque_Date, Total_Ammount,Ammount_Paid, Comments
            '        '    sbUserDetail.Append("<table id='table-design'><thead><th colspan='4'>Subscription Details</th></thead><tbody>")
            '        '    For i = 0 To Count - 1
            '        '        sbUserDetail.Append("<tr><td>Subscription</td><td>" & i + 1 & "</td></tr>")
            '        '        sbUserDetail.Append("<tr><td>Confirmation Date</td><td>" & DT.Rows(i).Item(0) & "</td></tr>")
            '        '        sbUserDetail.Append("<tr><td>Account Expiry Date</td><td>" & DT.Rows(i).Item(1) & "</td></tr>")
            '        '        sbUserDetail.Append("<tr><td>Mode Of Payment</td><td>" & DT.Rows(i).Item(2) & "</td></tr>")
            '        '        sbUserDetail.Append("<tr><td>Card Type</td><td>" & DT.Rows(i).Item(3) & "</td></tr>")
            '        '        'sbUserDetail.Append("<b>Name On Card: </b>" & DT.Rows(i).Item(4) & "</br>")
            '        '        'sbUserDetail.Append("<b>Credit Card No: </b>" & DT.Rows(i).Item(5) & "</br>")
            '        '        'sbUserDetail.Append("<b>CC Expiry Date: </b>" & DT.Rows(i).Item(6) & "</br>")
            '        '        'sbUserDetail.Append("<b>Credit Bank Name: </b>" & DT.Rows(i).Item(7) & "</br>")
            '        '        'sbUserDetail.Append("<b>Cheque Number: </b>" & DT.Rows(i).Item(8) & "</br>")
            '        '        'sbUserDetail.Append("<b>Cheque Bank Name: </b>" & DT.Rows(i).Item(9) & "</br>")
            '        '        'sbUserDetail.Append("<b>Cheque Holder Name: </b>" & DT.Rows(i).Item(10) & "</br>")
            '        '        sbUserDetail.Append("<tr><td>Cheque Date</td><td>" & DT.Rows(i).Item(11) & "</td></tr>")
            '        '        sbUserDetail.Append("<tr><td>Total Amount</td><td>" & DT.Rows(i).Item(12) & "</td></tr>")
            '        '        sbUserDetail.Append("<tr><td>Amount Paid </td><td>" & DT.Rows(i).Item(13) & "</td></tr>")
            '        '        'sbUserDetail.Append("<b>Comments: </b>" & DT.Rows(i).Item(14) & "</br>")
            '        '        sbUserDetail.Append("</tbody></table><br>")
            '        '    Next

            'Catch Err As Exception
            '        'lblMsg.Text = Err.Message
            'Finally
            '    DT = Nothing
            '        'Count = 0
            '        'i = 0
            '    objClsUser = Nothing
            'End Try
            'Return sbUserDetail.ToString

        End Sub



End Class

End Namespace
