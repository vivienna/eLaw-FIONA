'/**********************************************************************/
'/*	Developer 	    : Mohamed I. Elsayed 								   */
'/*	Company     	: The Digital Library Sdn. Bhd.		    					   */
'/* Date Modified	: 7 Jul  2013 			    			    */  
'/*	Description		: Changing user password                            */
'/*	Version			: 3.0											   */
'/**********************************************************************/

Imports System.Data
Namespace membersarea

Partial Class changePassword
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
        Dim obj As New clsUsers
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            UserName = CType(Session("UserName"), String)
           

        If UserName = "" Then
            '''/*Set the status of user to OFFLINE*/
            'Server.Transfer("login.aspx")
			 Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            Else
                getUserAccountType(UserName)
            End If
            'lblUsername.Text = "<b>" & UserName & "</b>"


    End Sub


    Private Sub btnChangePassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePassword.Click
            Dim OldPwd As String = Trim(txtOldPassword.Text)
            Dim NewPwd1 As String = Trim(txtNewPassword.Text)
            Dim NewPwd2 As String = Trim(txtReTypePassword.Text)
        Dim ObjCS As New clsCasesSearch
        Dim Query As String
        Dim flg As Boolean
        Dim PwdMatched As Boolean
            Dim OldPwdMatched As Boolean
            Dim LongEnough As Boolean
            lblMsg.Text = ""
            If Len(OldPwd) <= 5 Then
                'lblMsg.Text = "Please Insert Old Password"
                txtOldPassword.BackColor = Drawing.Color.FromArgb(255, 250, 220)
                lblMsg.Text = "<img src=""img/error.png""/> Yellow fields are too short (6 minimum)"
            Else
                txtOldPassword.BackColor = Drawing.Color.FromArgb(255, 255, 255)
            End If


            If (Len(NewPwd1) <= 5) And (Len(NewPwd2) <= 5) Then
                'lblMsg.Text = "Please Insert Passwords"
                LongEnough = False
                txtNewPassword.BackColor = Drawing.Color.FromArgb(255, 250, 220)
                txtReTypePassword.BackColor = Drawing.Color.FromArgb(255, 250, 220)
                lblMsg.Text = "<img src=""img/error.png""/> Yellow fields are too short (6 minimum)"
            Else
                LongEnough = True
                txtNewPassword.BackColor = Drawing.Color.FromArgb(255, 255, 255)
                txtReTypePassword.BackColor = Drawing.Color.FromArgb(255, 255, 255)
            End If


            If (NewPwd1 = NewPwd2 And LongEnough) Then
                PwdMatched = True
                'txtNewPassword.BackColor = Drawing.Color.FromArgb(255, 255, 255)
                'txtReTypePassword.BackColor = Drawing.Color.FromArgb(255, 255, 255)
            ElseIf (LongEnough) Then
                PwdMatched = False
                'lblMsg.Text &= " New Passwords are not matched"
                txtNewPassword.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                txtReTypePassword.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                lblMsg.Text = "<img src=""img/error.png""/> New passwords don't match"
            End If

            If (OldPasswordChk(OldPwd) = True) Then
                OldPwdMatched = True
                'txtOldPassword.BackColor = Drawing.Color.FromArgb(255, 255, 255)
            ElseIf (Len(OldPwd) > 5) Then
                OldPwdMatched = False
                'lblMsg.Text &= " Old Password is not correct"
                txtOldPassword.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                lblMsg.Text = "<img src=""img/error.png""/> Wrong old password"
            End If

        If (PwdMatched = True) And (OldPwdMatched = True) Then
                Query = "Update master_users set password='" & NewPwd1 & "' where user_name='" & UserName & "'"
            flg = ObjCS.UpdateRecord(Query)

            If flg = True Then
                    lblMsg.Text = "<img src=""img/error.png""/> Password is updated.."
                    txtOldPassword.Text = ""
                    txtNewPassword.Text = ""
                    txtReTypePassword.Text = ""
            Else
                    lblMsg.Text = "<img src=""img/error.png""/> Password is not updated due to some error, please send email to mylawbox admin.."

            End If


        End If

    End Sub

    Private Function PasswordMatching(ByVal pwd1 As String, ByVal pwd2 As String) As Boolean
        Dim Matchd As Boolean = False
        If pwd1 = pwd2 Then
            Matchd = True
        End If
        Return Matchd
    End Function

    Private Function OldPasswordChk(ByVal OldPwd As String) As Boolean
        Dim ObjUser As New clsUsers
        Dim Authenticated As Boolean
        Authenticated = ObjUser.getLoginAuthenticated(UserName, OldPwd)
        ObjUser = Nothing
        Return Authenticated
    End Function
        Private Function getUserAccountType(ByVal UserName As String) As String
            Dim DT As New DataTable
            Dim Query = "select First_Name from master_users where user_name='" & UserName & "'"

            Dim AccountType As String
            'dim count as in
            DT = obj.FetchDataSet(Query)
            AccountType = DT.Rows(0).Item(0)
            ' AccountType &= " " & DT.Rows(0).Item(1)
            fullName.Text = AccountType
            DT = Nothing
            Query = ""
            obj = Nothing


            Return AccountType
        End Function

End Class

End Namespace
