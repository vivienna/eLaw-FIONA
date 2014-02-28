


Imports System.Data.SqlClient
Imports System.Exception
Imports System.Data


Namespace membersarea

    Public Enum ElawPlatforms
        pc = 1
        mobile = 2
    End Enum

    Public Class clsInfo

        Private ConnectionString As String
        Public Sub New()
            ConnectionString = clsConfigs.sGlobalConnectionString
            If ConnectionString = "" Then
                Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
            End If
        End Sub

        Public Function LoadAllCountries() As DataTable
            Dim DT As New DataTable()
            ' Dim myParameter As SqlParameter
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            'Dim TotalResult As Int16
            'Dim temp As String

            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_getCountries"
                cmd.CommandType = CommandType.StoredProcedure
                conn.Open()
                cmd.ExecuteNonQuery()

                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
            End Try

            Return DT
        End Function

        Public Function getMembersCountries() As DataTable

            Dim DT As New DataTable()
            'Dim myParameter As SqlParameter
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            'Dim TotalResult As Int16
            'Dim temp As String

            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_getMemberCountries"
                cmd.CommandType = CommandType.StoredProcedure
                conn.Open()
                cmd.ExecuteNonQuery()

                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
            End Try

            Return DT
        End Function

        Public Function getMemberSiteNames() As DataTable
            'Description: Deprecated

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct Site_Name from master_users"

            Try
                DT = Obj.FetchDataSet(Query)


            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getMemberStatesByCountries(ByVal Country As String) As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct state from master_users where country='" & Country & "' "

            Try
                DT = Obj.FetchDataSet(Query)


            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getSalesRepresentative() As DataTable

            Dim DT As New DataTable()
            ' Dim myParameter As SqlParameter
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            'Dim TotalResult As Int16
            ' Dim temp As String

            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_getSalesRepName"
                cmd.CommandType = CommandType.StoredProcedure
                conn.Open()
                cmd.ExecuteNonQuery()

                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
            End Try

            Return DT
        End Function

        Public Function GetSiteNames() As ArrayList
            Dim SiteNames As New ArrayList()
            SiteNames.Add("ELAW")
            Return SiteNames
        End Function

        Public Function GetProfessions() As ArrayList
            Dim ProfessionList As New ArrayList()
            ProfessionList.Add("Lawyer")
            ProfessionList.Add("Legal Counsel")
            ProfessionList.Add("Judge")
            ProfessionList.Add("Consultant")
            ProfessionList.Add("Admin/Secretary")
            ProfessionList.Add("Executive/Managerial")
            ProfessionList.Add("IT")
            ProfessionList.Add("Student")
            ProfessionList.Add("Others")


            Return ProfessionList
        End Function

        Public Function getCasesUploadedCountriesList() As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct country from cases"

            Try
                DT = Obj.FetchDataSet(Query)

            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getLegislationUploadedCountriesList() As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct country from Legislation"

            Try
                DT = Obj.FetchDataSet(Query)

            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getArticleUploadedCountriesList() As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct country from Article"

            Try
                DT = Obj.FetchDataSet(Query)

            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getPracticeNotesUploadedCountriesList() As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct country from reference_PRACTICENOTES"

            Try
                DT = Obj.FetchDataSet(Query)

            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getPrecedentsUploadedCountriesList() As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct country from reference_PRECEDENTS"

            Try
                DT = Obj.FetchDataSet(Query)

            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getFormsUploadedCountriesList() As DataTable

            Dim DT As New DataTable()
            Dim Query As String
            Dim Obj As New clsUsers()
            Query = "Select distinct country from LEGALFORM"

            Try
                DT = Obj.FetchDataSet(Query)

            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                Obj = Nothing
            End Try

            Return DT
        End Function

        Public Function getCurrentMonthRevenueAllSites() As Double
            Dim Query As String
            Dim CurrentDate As Date
            Dim Month As Int16
            Dim Year As Int16
            Dim obj As New clsUsers()
            Dim DT As New DataTable()
            Dim Result As Double
            CurrentDate = System.DateTime.Today
            Month = CurrentDate.Month()
            Year = CurrentDate.Year()


            Query = "select sum(tbl2.Total_ammount) from master_users as TBL1, Admin_Users as TBL2,invoiceMembers as TBL3 where (TBL2.Mode_Of_Payment='cheque' or TBL2.Mode_Of_Payment='credit card') and (TBL1.user_id_no = TBL2.user_id_no AND TBL2.user_id_no = TBL3.user_id_no )  AND month(TBL2.Confirmation_Date)=" & Month & " and year(TBL2.Confirmation_Date)= " & Year & "   "
            DT = obj.FetchDataSet(Query)
            Result = DT.Rows(0).Item(0)

            Query = ""
            Month = 0
            Year = 0
            obj = Nothing
            DT = Nothing

            Return Result
        End Function

        Public Function getCurrentMonthPaymentAllSites() As Double
            Dim Query As String
            Dim CurrentDate As Date
            Dim Month As Int16
            Dim Year As Int16
            Dim obj As New clsUsers()
            Dim DT As New DataTable()
            Dim Result As Double
            CurrentDate = System.DateTime.Today
            Month = CurrentDate.Month()
            Year = CurrentDate.Year()


            Query = "select sum(tbl2.ammount_paid) from master_users as TBL1, Admin_Users as TBL2,invoiceMembers as TBL3 where (TBL2.Mode_Of_Payment='cheque' or TBL2.Mode_Of_Payment='credit card') and (TBL1.user_id_no = TBL2.user_id_no AND TBL2.user_id_no = TBL3.user_id_no )  AND month(TBL2.Confirmation_Date)=" & Month & " and year(TBL2.Confirmation_Date)= " & Year & "   "
            DT = obj.FetchDataSet(Query)
            Result = DT.Rows(0).Item(0)

            Query = ""
            Month = 0
            Year = 0
            obj = Nothing
            DT = Nothing

            Return Result
        End Function



        Public Function getCurrentMonthInvoicesAllSites() As Long
            Dim Query As String
            Dim CurrentDate As Date
            Dim Month As Int16
            Dim Year As Int16
            Dim obj As New clsUsers()
            Dim DT As New DataTable()
            Dim Result As Long
            CurrentDate = System.DateTime.Today
            Month = CurrentDate.Month()
            Year = CurrentDate.Year()

            Query = "select count(TBL3.invoice_no) from master_users as TBL1, Admin_Users as TBL2,invoiceMembers as TBL3 where (TBL2.Mode_Of_Payment='cheque' or TBL2.Mode_Of_Payment='credit card') and (TBL1.user_id_no = TBL2.user_id_no AND TBL2.user_id_no = TBL3.user_id_no )  AND month(TBL2.Confirmation_Date)=" & Month & " and year(TBL2.Confirmation_Date)= " & Year & " "
            DT = obj.FetchDataSet(Query)
            Result = DT.Rows(0).Item(0)

            Query = ""
            Month = 0
            Year = 0
            obj = Nothing
            DT = Nothing

            Return Result
        End Function
        'Query = "select TBL3.invoice_no, TBL1.user_id_no, TBL1.First_Name, TBL1.Last_Name, TBL1.Firm_Name, TBL1.User_Name, TBL1.Site_Name , TBL2.Payment_Mode , TBL2.Mode_of_Payment , TBL2.Credit_Bank_Name , TBL2.Cheque_Bank_Name , TBL1.Group_Name ,  TBL2.Confirmation_Date , TBL2.Ammount_Paid, TBL2.Total_Ammount,TBL3.Invoice_Date, tbl3.Invoice_Ammount from master_users as TBL1, Admin_Users as TBL2,invoiceMembers as TBL3 where (TBL2.Mode_Of_Payment='cheque' or TBL2.Mode_Of_Payment='credit card') and (TBL1.user_id_no = TBL2.user_id_no AND TBL2.user_id_no = TBL3.user_id_no )  AND month(TBL2.Confirmation_Date)=" & Month & " and year(TBL2.Confirmation_Date)= " & Year & " and TBL1.Site_Name like '%" & SiteName & "%'  and TBL3.Invoice_Status='Cancel' "

        Public Function getCurrentMonthCancelInvoicesAllSites() As Long
            Dim Query As String
            Dim CurrentDate As Date
            Dim Month As Int16
            Dim Year As Int16
            Dim obj As New clsUsers()
            Dim DT As New DataTable()
            Dim Result As Long
            CurrentDate = System.DateTime.Today
            Month = CurrentDate.Month()
            Year = CurrentDate.Year()

            Query = "select count(TBL3.invoice_no) from master_users as TBL1, Admin_Users as TBL2,invoiceMembers as TBL3 where (TBL2.Mode_Of_Payment='cheque' or TBL2.Mode_Of_Payment='credit card') and (TBL1.user_id_no = TBL2.user_id_no AND TBL2.user_id_no = TBL3.user_id_no )  AND month(TBL2.Confirmation_Date)=" & Month & " and year(TBL2.Confirmation_Date)= " & Year & " and TBL3.Invoice_Status='Cancel' "
            DT = obj.FetchDataSet(Query)
            Result = DT.Rows(0).Item(0)

            Query = ""
            Month = 0
            Year = 0
            obj = Nothing
            DT = Nothing

            Return Result
        End Function


        Public Function GetIndustry() As ArrayList
            Dim IndustryList As New ArrayList()

            IndustryList.Add("Banking/Finance /Real Estate")
            IndustryList.Add("Computer Related (IS/MIS/DP, Internet")
            IndustryList.Add("Computer Related - Hardware")
            IndustryList.Add("Computer Related (Software)")
            IndustryList.Add("Education/Research")
            IndustryList.Add("Engineering/Construction")
            IndustryList.Add("Manufacturing/Distribution")
            IndustryList.Add("Business Supplies or Services")
            IndustryList.Add("Medical/Health Services")
            IndustryList.Add("Entertainment/Media/publishing")
            IndustryList.Add("Hospitality / Travel Accommodation")
            IndustryList.Add("Consumer Retail/Wholesale")
            IndustryList.Add("Non-profit/membership organisations")
            IndustryList.Add("Government")
            IndustryList.Add("Legal Services")
            IndustryList.Add("Other")


            Return IndustryList
        End Function

        Public Function GetCreditCardTypes() As ArrayList
            Dim CreditCardList As New ArrayList()
            CreditCardList.Add("master_users Card")
            CreditCardList.Add("Visa")
            CreditCardList.Add("American Express")
            Return CreditCardList
        End Function

        Public Function Years() As ArrayList
            Dim YearList As New ArrayList()
            Dim i As Int16

            For i = 2004 To 2010
                YearList.Add(i & "")

            Next

            Return YearList
        End Function

        Public Function Months() As ArrayList
            Dim List As New ArrayList()
            List.Add("January")
            List.Add("February")
            List.Add("March")
            List.Add("April")
            List.Add("May")
            List.Add("June")
            List.Add("July")
            List.Add("August")
            List.Add("September")
            List.Add("October")
            List.Add("November")
            List.Add("December")


            Return List

        End Function

        Public Function PaymentStatus() As ArrayList
            Dim List As New ArrayList()
            List.Add("Confirm")
            List.Add("Payment Pending")

            'List.Add("Waiting")

            Return List

        End Function


        Public Function UserStatus() As ArrayList
            Dim List As New ArrayList()
            List.Add("Confirm")
            List.Add("Payment Pending")
            List.Add("Suspend")
            List.Add("Terminate")
            Return List

        End Function

        Public Function InvoiceStatus() As ArrayList
            Dim List As New ArrayList()
            List.Add("Confirm")

            List.Add("Cancel")

            Return List

        End Function

        Public Function MonthConversion(ByVal sMonth As String) As Byte
            Dim Month As Byte
            If sMonth = "January" Then
                Month = 1
            ElseIf sMonth = "February" Then
                Month = 2

            ElseIf sMonth = "March" Then
                Month = 3

            ElseIf sMonth = "April" Then
                Month = 4

            ElseIf sMonth = "May" Then
                Month = 5

            ElseIf sMonth = "June" Then
                Month = 6

            ElseIf sMonth = "July" Then
                Month = 7

            ElseIf sMonth = "August" Then
                Month = 8

            ElseIf sMonth = "September" Then
                Month = 9

            ElseIf sMonth = "October" Then
                Month = 10

            ElseIf sMonth = "November" Then
                Month = 11

            ElseIf sMonth = "December" Then
                Month = 12
            End If

            Return Month

        End Function


    End Class
End Namespace
