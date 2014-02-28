Imports System.Data
Imports System
Imports membersarea

Partial Class signout
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objUsers As New clsUsers
        Dim UserName As String
        Dim UserType As String
        Dim onlineStatus As String
        Dim MutliLoginAcc As Integer = 0
        Try
            'UserName = CType(Session("UserName"), String)
            UserName = CStr(Session.Item("UserName"))
            If UserName <> "" Then
                lblMsg.Text = "<b>" & Session("FirstName") & "</b>"
                UserType = getUserType(UserName)

                If (UserType = "Single" Or UserType = "Free") Then
                    LogOut(UserName)
                Else
                    onlineStatus = objUsers.getUserOnlineStatus(Trim(UserName))
                    MutliLoginAcc = objUsers.GetMultiSessionGroup(Trim(UserName))
                    If MutliLoginAcc > 0 Then
                        objUsers.ProcessMultiSessionLogout(Trim(UserName), onlineStatus, MutliLoginAcc)
                    End If
                End If
                lblMsg.Text &= " logged out on " & System.DateTime.Now
                Session("USERNAME") = Nothing
                Session("UserInfo") = Nothing
                Session.Abandon()
            End If


        Catch ex As Exception
            lblMsg.Text = ex.Message
            'Response.Redirect("index.aspx")

        End Try
        UserName = ""
        objUsers = Nothing
        MutliLoginAcc = 0
        onlineStatus = ""
    End Sub

    Private Sub LogOut(ByVal userName As String)
        Dim objUsers As New clsUsers
        Dim OnlineStatus As String
        Dim ResultStatus As String
        Dim sessionId As String

        OnlineStatus = getUserOnlineStatus(userName)
        'If OnlineStatus.Compare(OnlineStatus, "Online2", True) Then

        If String.Compare(OnlineStatus, "Online1", True) = 0 Then
            'If OnlineStatus = "Online1" Then
            ResultStatus = "Online"
        ElseIf String.Compare(OnlineStatus, "Online", True) = 0 Then
            'If OnlineStatus = "Online" Then
            ResultStatus = "Offline"
        Else
            ResultStatus = "Offline"
        End If
        If userName <> "" Then
            objUsers.updateOnlineUserStatus(userName, ResultStatus)
            Dim Query As String = "update  tblOnlineUsers set   SessionDate1=GETDATE()  where SessionID1='" & Session.SessionID & "'"
            objUsers.AddRecord(Query)
        End If
        Try
            'sessionId = Session.SessionID()
            Session("USERNAME") = Nothing
            Session("UserInfo") = Nothing
            Session.Abandon()
        Catch ex As Exception
        Finally
            objUsers = Nothing
            OnlineStatus = ""
            ResultStatus = ""
        End Try
    End Sub

    Private Function getUserOnlineStatus(ByVal UserName As String) As String
        Dim objUsers As New clsUsers
        Dim SqlQuery As String
        Dim DT As New DataTable
        Dim Status As String
        SqlQuery = "select OnlineStatus from tblonlineusers where user_name='" & UserName & "'"
        DT = objUsers.FetchDataSet(SqlQuery)
        Status = DT.Rows(0).Item(0)

        DT = Nothing
        objUsers = Nothing
        SqlQuery = ""

        Return Status
    End Function

    Private Function getUserType(ByVal UserName As String) As String
        Dim objUsers As New clsUsers
        Dim SqlQuery As String
        Dim DT As New DataTable
        Dim UserType As String

        SqlQuery = "select Access_type from master_users where user_name='" & UserName & "'"
        DT = objUsers.FetchDataSet(SqlQuery)
        UserType = DT.Rows(0).Item(0)

        DT = Nothing
        objUsers = Nothing
        SqlQuery = ""

        Return UserType
    End Function
End Class
