Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports membersarea


Partial Class ViewHistory
    Inherits System.Web.UI.Page
    Protected Shared ConnectionString As String
    Dim S_RecordCount As Int32
    Dim PageCurrent As Int16
    Dim RowCount As Integer
    Public Sub New()

        ConnectionString = clsConfigs.sGlobalConnectionString
        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If

    End Sub

    Dim UserName As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UserName = CType(Session("UserName"), String)
        'Session.Remove("CaseRelateResult")
        Dim r As DataRow = Nothing
        If UserName = "" Then
            'Response.Redirect("~/login.aspx")
            Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
            Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
        End If
        Dim PageSize As Integer = 10
        Dim PageIndex As Integer = 1
        If Request.QueryString("page") = "" Then
            PageCurrent = 1
            RowCount = 0
        Else
            PageCurrent = CInt(Request.QueryString("page"))
            RowCount = PageCurrent - 1
        End If
        myBindData()

    End Sub


    Private Sub myBindData()
        Dim PageFirst As Int16
        Dim RecordCount As Int32 'totalRec or MaxRec
        Dim PageSize As Byte  'showRec
        Dim PageCount As Int16
        Dim LastPage As Int16
        Dim CurrentRecord As Int32
        Dim Counter As Int16
        Dim I, J As Integer
        Dim StartTime, EndTime, ResultTime As Int32
        Dim CurRecForFetch As Int32
        Dim DT As New DataTable
        Dim TopNavigation As New System.Text.StringBuilder
        Dim Records As New System.Text.StringBuilder
        Dim ObjCS As New clsCasesSearch
        Dim ObjInt As New clsIntelligence
        Dim StrSQL As String
        Dim URLpage As String


        PageSize = 10 ' this ammount of rec to show

        If CurrentRecord = 0 Then
            CurrentRecord = 1
            'ElseIf CurrentRecord > 1 And PageCurrent > 1 Then
        End If
        If PageCurrent > 1 Then
            CurrentRecord = PageSize * (PageCurrent - 1) ' means pg2= 20 * (2-1) ,pg3=  30 * (3-1)
            CurrentRecord = CurrentRecord + 1 ' because 30 = 30+1 i.e CurRec = lastRec +1
        End If

        StartTime = System.DateTime.Now.Millisecond
        StrSQL = "select count from CasesViewHistory where UserName='" & UserName & "'"
        RecordCount = S_RecordCount
        If RecordCount = 0 Then
            DT = ObjCS.ExecuteMyQuery(StrSQL)
            RecordCount = DT.Rows.Count()
            S_RecordCount = RecordCount
        End If

        EndTime = System.DateTime.Now.Millisecond
        ResultTime = (EndTime - StartTime)
        If ResultTime > 0 Then
            ' lblSec.Text = ResultTime / 1000
        Else
            ' lblSec.Text = "0.09"
        End If

        If RecordCount <= 0 Then

            lblTbl.Text = "<center><span class='tTextNorm2' style='padding:30px;'>Currently, your case view history is empty.</span></center>"
            lblBottomNavigation.Text = ""
            Exit Sub
        End If
        PageCount = CInt(Math.Ceiling(CDbl(RecordCount) / PageSize)) 'recordCount to double because ceiling requies it
        If PageCount > 100 Then
            LastPage = 100 ' because of business logic
        Else
            LastPage = PageCount
        End If

        ' Lesser than firstPage than first page
        If PageCurrent < 1 Then
            PageCurrent = 1
        End If
        If PageCurrent >= PageCount Then
            PageCurrent = LastPage 'PageCount
        End If





        StrSQL = "Select * from CasesViewHistory where UserName='" & UserName & "' order by LastViewDate desc,NumberViewTimes desc"

        CurRecForFetch = 10 * (PageCurrent - 1)
        DT = ObjCS.ExecuteMyQuery(StrSQL, "CasesViewHistory", CurRecForFetch)

        URLpage = "<a href=ViewHistory.aspx?pageid="

        If PageCount > 0 Then

            lblTbl.Text = ""


            If PageCurrent >= 1 Then

                Records.Append("<table width='95%' border='0' cellpadding='0' cellspacing='2' style='margin:20px;'>")
                Records.Append("<tr>")
                Records.Append("<td height='35' bgcolor='#fcb87e'><table width='100%' border='0' cellpadding='0' cellspacing='0'>")
                Records.Append("<tr>")
                Records.Append("<td width='4%' align='center' class='tTextNorm3'><input type='checkbox' name='check' id='selectall'value='all'></td>")
                Records.Append("<td width='44%' class='tTextNorm3' style='border-left:1px #C60 solid;'>Title</td>")
                Records.Append("<td width='15%' class='tTextNorm3' style='border-left:1px #C60 solid;'>Viewed Date</td>")
                Records.Append("<td width='18%' class='tTextNorm3' style='border-left:1px #C60 solid;'>Viewed By</td>")
                Records.Append("<td width='12%' class='tTextNorm3' style='border-left:1px #C60 solid;'>View Cases</td>")
                Records.Append("<td width='11%' class='tTextNorm3' style='border-left:1px #C60 solid;'>Viewed Times</td>")

                Records.Append("</tr>")
                Records.Append("</table></td>")
                Records.Append("</tr>")

                J = 0
                Dim RC As Int16 = DT.Rows.Count - 1
                For Counter = 0 To RC
                    RowCount = RowCount + 1
                    If Counter = RecordCount Then
                        GoTo allRecDisplayed
                    End If
                    'CaseID,DateFileName,UserName,LastViewDate,NumberViewTimes,CaseTitle 
                    Dim CaseID As String = DT.Rows(Counter).Item(0)
                    Dim DateFileName As String = DT.Rows(Counter).Item(1) ' TYP 
                    Dim UserName = DT.Rows(Counter).Item(2)
                    Dim LastViewDate As Date = DT.Rows(Counter).Item(3)
                    Dim dateOnly As Date = LastViewDate.Date.ToString("d")

                    Dim NumberViewTimes As Integer = DT.Rows(Counter).Item(4) 'DT.Rows(Counter).Item(5)
                    Dim CaseTitle As String = DT.Rows(Counter).Item(5).ToString()
                    '================================
                    Dim IdLink As String = Trim(DateFileName)
                    Dim UrlLink As New Dba_UrlEncrption(IdLink, True)
                    Dim link As String = UrlLink.UrlEncrypt

                    '================================
                    Records.Append("<tr>")
                    Records.Append("<td class='menuoff' onmouseover='className='menuon';' onmouseout='className='menuoff';' ><table width='100%' border='0' cellpadding='0' cellspacing='0'>")
                    Records.Append("<tr>")
                    Records.Append("<td width='4%' align='center' class='tTextNorm3'><input type='checkbox' name='check' class='case' value=" & CaseID & " ></td>")
                    Records.Append("<td width='44%' class='tTextNorm3' style='border-left:1px #C60 solid;'><a href='showcase.aspx?id=" & link & "'>" & CaseTitle & "</a></td>")
                    Records.Append("<td width='15%' class='tTextNorm3' style='border-left:1px #C60 solid;'>" & dateOnly & "</td>")
                    Records.Append("<td width='18%' class='tTextNorm3' style='border-left:1px #C60 solid;'><a href='#'>" & UserName & "</a></td>")
                    Records.Append("<td width='12%' class='tTextNorm3' style='border-left:1px #C60 solid;'><a href='showcase.aspx?id=" & link & "'>View</a></td>")
                    Records.Append("<td width='7%' class='tTextNorm3' style='border-left:1px #C60 solid;'><a href='#'>" & NumberViewTimes & "</a></td>")
                    Records.Append(" </tr>")
                    Records.Append("</table></td>")
                    Records.Append("</tr>")




                Next Counter
                Records.Append("</table>")

allRecDisplayed:
                CurrentRecord = Counter ' i.e 60 or 90 because we loop through it
                lblTbl.Text = Records.ToString
            End If
        End If
        lblBottomNavigation.Text = ""
        Dim AT As String
        AT = "<a  href="

        URLpage = "ViewHistory.aspx?page="
            If PageCount > 1 Then

            If PageCurrent > 1 Then

                ' If PageCurrent >= 10 Then
                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & "1") & Chr(34) & "><img border='0' style='padding-top:2px;' align='top' src='../img/leftArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='../img/leftArrow.png'/></a>&nbsp;&nbsp;")
                ' End If
                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & (PageCurrent - 1)) & Chr(34) & "> <img border='0' style='padding-top:2px;' align='top' src='../img/leftArrow.png'/></a> &nbsp;")

            End If
            If ((PageCurrent - (PageCurrent Mod 10)) + 10) > PageCount Then
                For I = PageCurrent - (PageCurrent Mod 10) To PageCount
                    If Not I = 0 Then
                        If I = PageCurrent Then
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " class='currentPage'>" & I & " </a>")
                        Else
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " >" & I & " </a>")
                        End If
                    End If
                Next I
            Else
                For I = PageCurrent - (PageCurrent Mod 10) To (PageCurrent - (PageCurrent Mod 10)) + 10
                    If Not I = 0 Then
                        If I = PageCurrent Then
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " class='currentPage'>" & I & " </a>")
                        Else
                            TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & I) & Chr(34) & " >" & I & " </a>")
                        End If

                    End If

                Next I
                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & PageCurrent + 1) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='../img/rightArrow.png'/></a>&nbsp;&nbsp;")

                TopNavigation.Append(AT & Chr(34) & Server.UrlPathEncode(URLpage & PageCount) & Chr(34) & " ><img border='0' style='padding-top:2px;' align='top' src='../img/rightArrow.png'/><img border='0' style='padding-top:2px;' align='top' src='../img/rightArrow.png'/></a>")

            End If


        End If
        lblBottomNavigation.Text = TopNavigation.ToString
        DT = Nothing
        ObjCS = Nothing
        ObjInt = Nothing
        Records = Nothing
        TopNavigation = Nothing

        PageFirst = 0
        RecordCount = 0
        PageSize = 0
        PageCount = 0
        PageCurrent = 0
        CurrentRecord = 0
        Counter = 0
        I = 0
        J = 0
        StartTime = 0
        EndTime = 0
        ResultTime = 0

        StrSQL = ""

        Title = ""
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        Response.ContentType = "Application/pdf"
        Response.AppendHeader("Content-Disposition", "attachment; filename=Test_PDF.pdf")

        Response.[End]()
    End Sub
    
    
End Class
