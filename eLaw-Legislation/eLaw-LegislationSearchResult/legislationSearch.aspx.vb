'/**********************************************************************/
'/*	Developer 	    :  Modified  by Mohd Elsayed & Ali  */
'/*	Company     	: The Digital Library Sdn. Bhd.		    					   */
'/* Date Modified	: 23/10/2013        					*/  
'/*	Description		: Legislation Search page                        */
'/*	Version			: 1.0											   */
'/**********************************************************************/
Imports System.Data
Namespace membersarea

Partial Class legislationSearch
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
        Dim QuerySubject As String ' subject Index for legislation 
        Dim UserName As String
        Dim obj_ph As New clsCasesSearch()
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            UserName = CType(Session("username"), String)
            If UserName = "" Then
               ' Response.Redirect("login.aspx")
			    Dim returnUrl = Server.UrlEncode(Request.Url.AbsoluteUri)
                Response.Redirect("~/login.aspx?returnUrl=" & returnUrl)
            End If
            fillYearDropList()
            bill()
            Fill_subject()
            ddlSubject.SelectedIndex = 0
            If IsPostBack = False Then
                Fill_subject()
                ddlSubject.SelectedIndex = 0
            End If

        End Sub

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

            Dim URL As String
            Dim obj As New clsSearch
            Dim SearchNotlegislation As String = ""
            Dim S_SreachType As String = "" ' type of search within title, section title, full docuement, proxmity etc..
            Dim searchFTS As String = ""
            Dim searchLegislation As String = ""
            Dim searchActs As String = ""
            Dim searchActNumber As String = ""
            Dim iProximityWithin As String = ""
            Dim iProximityLevel As String = ""
            Dim iOption As String = " "
            If Len(Trim(txtFTS.Text)) = 0 And Len(Trim(txtActNo.Text)) = 0 Then
                lblMsg.Text = "*At least one of the red fields must be filled"
                txtFTS.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                txtActNo.BackColor = Drawing.Color.FromArgb(255, 220, 220)
                Exit Sub
            End If
            SearchNotlegislation = Trim(txtNotCases.Text)
            searchFTS = Trim(txtFTS.Text)
            searchActNumber = Trim(txtActNo.Text)
            ' check for proxmity search 
            '===========================================
            Dim count1 As Integer = CountWords(searchFTS)
            '========================================================
            Dim S_dd_selected As Integer
            S_dd_selected = ddlsrch.SelectedIndex
            Select Case S_dd_selected
                Case 0
                    'full legislation
                    S_SreachType = 1
                Case 1
                    ' act title
                    S_SreachType = 2
                Case 2
                    ' section title
                    S_SreachType = 3
                Case 3 And count1 >= 2
                    'within 3 words
                    iProximityWithin = 3
                    iProximityLevel = 4
                    S_SreachType = 1
                Case 4 And count1 >= 2
                    ' within 4 words
                    iProximityWithin = 4
                    iProximityLevel = 4
                    S_SreachType = 1
                Case 5 And count1 >= 2
                    ' within 5 words
                    iProximityWithin = 5
                    iProximityLevel = 4
                    S_SreachType = 1
                Case 6 And count1 >= 2
                    'winth in sentence
                    iProximityLevel = 1
                    S_SreachType = 1
                Case 7 And count1 >= 2
                    'within paragraph
                    iProximityLevel = 2
                    S_SreachType = 1
                Case 8 And count1 >= 2
                    'within page
                    iProximityLevel = 3
                    S_SreachType = 1
                Case Else
                    S_SreachType = 1
                    iProximityLevel = 0
            End Select

            '==========================================
            ' Encrypt Query before submit
            Dim UrlEncrption As String = "ft=" & searchFTS & "&nc=" & SearchNotlegislation & "&st=" & S_SreachType & "&an=" & searchActNumber & "&prxOpt=" & iProximityLevel & "&prxOptWit=" & iProximityWithin
            Dim UrlLink As New Dba_UrlEncrption(UrlEncrption, True)
            Dim link As String = UrlLink.UrlEncrypt

            URL = "legislationsearchresult.aspx?info=" & link

            URL = Server.UrlPathEncode(URL)
            Response.Redirect(URL)
        End Sub
        Public Function CountWords(ByVal value As String) As Integer
            ' Count matches.
            Dim collection As MatchCollection = Regex.Matches(value, "\S+")
            Return collection.Count
        End Function
        Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
            txtFTS.Text = ""
            txtActNo.Text = ""
        End Sub
        Private Sub fillYearDropList()
            'update: generate current year and add it to drop list 
            '14/3/2012
            Dim alphabetList As New ArrayList
            Dim x As Integer
            Dim _year As Integer = 1952
            x = _year
            Dim z As Integer = DateTime.Now.Date.Year
            For x = _year To z
                alphabetList.Add(x)
            Next
            ddl_year.DataSource = alphabetList
            ddl_year.DataBind()
        End Sub

        Private Sub bill()
            ltr_bill.Text = ""
            Dim x As Integer
            Dim z As Integer = DateTime.Now.Date.Year
            Dim bill As Integer = 2000
            For x = 2000 To z
                ltr_bill.Text = ltr_bill.Text & "<a onclick='showbill(" & x & " )' href='#'>" & x & "</a> "
            Next
        End Sub
        Private Sub Fill_subject()
            QuerySubject = "SELECT DISTINCT LOWER([SUBJECT]) as Leg FROM LEGISLATION where SUBJECT IS NOT NULL order by leg asc"
            ddlSubject.DataSource = obj_ph.ExecuteMyQuery(QuerySubject) 'obj.getLawDictionarySortBy(CharSortBy)
            ddlSubject.DataTextField = "leg"
            ddlSubject.DataValueField = "leg"
            ddlSubject.DataBind()
        End Sub
        'Auto complete display suggesstion to user while searching 
        <System.Web.Services.WebMethod()> _
        Public Shared Function Getautocomplate(ByVal auto As String) As ArrayList
            Dim suggestions As New ArrayList
            'SELECT   ExtactPhrase,sortby FROM SearchEngine_Legislation where ExtactPhrase like '" & Trim(auto) & "%' order by case when SortBy like 'ACT%' then 1  end desc
            Dim query As String = "SELECT top 10 ExtactPhrase,sortby FROM SearchEngine_Legislation where ExtactPhrase like '" & Trim(auto) & "%' order by case when SortBy like 'ACT%' then 1  end desc"
            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            'Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            For i = 0 To dt.Rows.Count - 1
                suggestions.Add(dt.Rows(i).Item(0).ToString.Trim())
            Next
            Return suggestions
        End Function
        'auto complete for act number 
        <System.Web.Services.WebMethod()> _
        Public Shared Function GetautocomplateAct(ByVal auto As String) As ArrayList
            Dim suggestions As New ArrayList
            Dim query As String = " select DISTINCT top 10 NUMBER  from  LEGISLATION where   substring(number, PatIndex('%[0-9]%', number), len(number)) like '%" & Trim(auto) & "%' order by 1"
            Dim obj As New clsCasesSearch
            Dim dt As New DataTable
            'Dim al As New ArrayList
            dt = obj.ExecuteMyQuery(query)
            For i = 0 To dt.Rows.Count - 1
                suggestions.Add(dt.Rows(i).Item(0).ToString.Trim())
            Next
            Return suggestions
        End Function
        'display legislation browse 
        <System.Web.Services.WebMethod()> _
        Public Shared Function getResult(ByVal type As String, ByVal range As String, ByVal subj As String, ByVal title As String, ByVal page As String) As ArrayList
            Dim a As ArrayList = New ArrayList
            Dim b As ArrayList = New ArrayList
            Dim dt As New DataTable
            Dim titleCond As String = ""
            Dim subjectCond As String = ""
            Dim numRange As String = ""
            Dim optCond As String = ""
            Dim curPage As Integer = Integer.Parse(page)
            Dim minRes As Integer = (curPage * 10) - 10
            'minRes *= 10
            Dim maxRes As Integer = minRes + 10

            If type = "1" Then
                type = "ACTS"
            Else
                type = "AMEN"
            End If
            If range <> "" Then
                Select Case range
                    Case 1
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))<= 100 and substring(number, PatIndex('%[0-9]%', number), len(number))>0 "
                    Case 2
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))<= 200 and substring(number, PatIndex('%[0-9]%', number), len(number))>100 "
                    Case 3
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))<= 300 and substring(number, PatIndex('%[0-9]%', number), len(number))>200 "
                    Case 4
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))<= 400 and substring(number, PatIndex('%[0-9]%', number), len(number))>300 "
                    Case 5
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))<= 500 and substring(number, PatIndex('%[0-9]%', number), len(number))>400 "
                    Case 6
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))<= 600 and substring(number, PatIndex('%[0-9]%', number), len(number))>500 "
                    Case 7
                        numRange = " and substring(number, PatIndex('%[0-9]%', number), len(number))>= 600 "
                    Case Else

                End Select
            End If
            If title <> "" Then
                titleCond = " and title like '" & title & "%' "
            End If
            If subj <> "" Then
                subjectCond = " and subject like '%" & subj.Replace("_", " ") & "%' "
            End If

            'Dim sql As String = "select top 10 DATAFILENAME,TITLE,PREAMBLE, NUMBER,SUBJECT ,INFORCEFROM from Legislation where datafilename like '%" & type & "%' " & numRange & titleCond & subjectCond
            Dim sql As String = "select DATAFILENAME from Legislation where datafilename like '%" & type & "%' " & numRange & titleCond & subjectCond & " order by title asc"
            Dim obj As New clsCasesSearch
            dt = obj.ExecuteMyQuery(sql)
            If (maxRes > dt.Rows.Count - 1) Then
                maxRes = dt.Rows.Count - 1
            End If
            b.Add(dt.Rows.Count)
            For i = minRes To maxRes
                If i = minRes Then
                    optCond &= "where "
                End If
                optCond &= " DATAFILENAME like '%" & dt.Rows(i).Item(0).trim() & "%' "
                If i <> maxRes Then
                    optCond &= "or "
                End If
            Next
            If optCond <> "" Then
                sql = "select DATAFILENAME,TITLE,PREAMBLE, NUMBER,SUBJECT,INFORCEFROM from Legislation " & optCond & "order by title asc"
            Else
                'b.Add(sql)
                Return b
            End If
            dt = obj.ExecuteMyQuery(sql)
            'a.Add(sql)
            'b.Add(a)
            For i = 0 To dt.Rows.Count - 1
                Dim IdLink As String = "id=" & (dt.Rows(i).Item(0)).ToString.Replace(".xml", "").Trim() & "&sw="
                Dim UrlFIleID As New Dba_UrlEncrption(IdLink, True)
                Dim linkFileId As String = UrlFIleID.UrlEncrypt

                a.Add(linkFileId.Trim())
                a.Add(dt.Rows(i).Item(1).trim())
                a.Add(dt.Rows(i).Item(2).trim())
                a.Add(dt.Rows(i).Item(3).trim())
                a.Add(dt.Rows(i).Item(4).trim())
                a.Add(dt.Rows(i).Item(5).trim())
                b.Add(a)
                a = New ArrayList
            Next

            Return b
        End Function

        

        
    End Class
    
End Namespace
