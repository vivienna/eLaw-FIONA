'/******************************************************************************/
'/*	Developer 	    : updated By MOhammed Ali           								   */
'/*	Company     	: Elaw Sdn Bhd		    	    				        */
'/* Date Modified	: 30 july 2013  		        		    		     */  
'/*	Description		: This library has functions related to users like authentication etc      */
'/*	Version			: 1.0											            */
'/*******************************************************************************/

Imports System.Data.SqlClient
Imports System.Exception
Imports System.Configuration
Imports System.Data
Imports System.Net.Mail
Imports System.Net


Namespace membersarea


    Public Class clsUsers

        Protected Shared ConnectionString As String

        Public Sub New()

            ConnectionString = clsConfigs.sGlobalConnectionString
            If ConnectionString = "" Then
                Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
            End If

        End Sub




        Public Function getLoginAuthenticated(ByVal Username As String, ByVal Password As String) As Boolean

            Dim Query As String = "select USER_NAME,PASSWORD from master_users where User_Name ='" & Username & "'  "
            'Dim Query As String = "select count(*) from master_users where User_Name ='" & Username & "' and PASSWORD ='" & Password & "' "
            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim dbUserName As String = " "
            Dim dbPassword As String = " "
            Dim Found As Boolean = False
            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader
                While Reader.Read()
                    dbUserName = Reader("USER_NAME")
                    dbPassword = Reader("Password")
                End While
                If ((String.Compare(Password, dbPassword, False) = 0) And (String.Compare(Username, dbUserName, False) = 0)) Then
                    'If (String.Compare(Password, dbPassword, False) = 0) Then
                    Found = True
                End If


            Catch err As Exception
                Throw New ApplicationException("Error in connecting to DB " & err.Message)
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing
                'Result = ""
                Query = ""

            End Try

            Return Found
        End Function

        Public Function getUserStatusTrial(ByVal Username As String) As Byte

            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim Query As String = "select TrialStatuts from ElawTrialUser where UserName ='" & Username & "'  "
            Dim cmd As New SqlCommand(Query, conn)
            Dim Msg As Boolean
            Dim Status As Byte
            Try

                conn.Open()
                Reader = cmd.ExecuteReader
                While Reader.Read()
                    Msg = Reader("TrialStatuts")
                End While
                If Msg = False Then
                    Status = 0
                Else
                    Status = 2
                End If
            Catch err As Exception
                Throw New ApplicationException("Error in connecting to DB " & err.Message)
                Status = 1
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing
                Query = ""

            End Try

            Return Status
        End Function
        Public Function getUserStatus(ByVal Username As String) As Byte

            Dim Query As String = "select status from master_users where User_Name ='" & Username & "'  "

            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim Msg As String = " "
            Dim Status As Byte = 0

            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    Msg = Reader("Status")
                End While

                If String.Compare(Msg, "Inactive", False) = 0 Then
                    Status = 1
                End If

            Catch err As Exception
                Throw New ApplicationException("Error in connecting to DB " & err.Message)
                Status = 2
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing
                Query = ""

            End Try

            Return Status
        End Function

        'Description: Setting the user status.
        'Status= Confirm / Payment Pending / Suspend
        Public Function setUserStatus(ByVal Username As String, ByVal Status As String) As Boolean
            Dim Query As String
            Dim Flag As Boolean
            Try
                Query = "update master_users set status='" & Status & "' where user_name='" & Username & "'"
                Flag = Me.AddRecord(Query)
            Catch ex As Exception

            End Try

            Return Flag
        End Function

        Public Overloads Function getMemberSiteName(ByVal Username As String) As String

            Dim Query As String = "select Site_Name from master_users where User_Name ='" & Username & "'"

            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim SiteName As String = " "

            Dim Found As Boolean
            Found = False
            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    SiteName = Reader(0)

                End While



            Catch err As Exception
                Throw New ApplicationException("Error in connecting to DB " & err.Message)
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing
                'Result = ""
                Query = ""

            End Try

            Return SiteName
        End Function

        '''Description: This is for checking the user sitename with like operator as there can be more than 1 site subscribed by the user.
        Public Overloads Function getMemberSiteName(ByVal Username As String, ByVal Sitename As String) As Boolean

            Dim Query As String = "select count(user_name) from master_users where User_Name ='" & Username & "' and site_name like '%" & Sitename & "%'"
            Dim conn As New SqlConnection(ConnectionString)
            Dim isSiteSubscribed As Boolean
            Dim Reader As SqlDataReader
            Dim count As Int16
            Dim Found As Boolean
            Found = False
            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    count = CInt(Reader(0))
                End While

            Catch err As Exception
                'Throw New ApplicationException("Error in connecting to DB " & err.Message)
                count = 0
                isSiteSubscribed = False
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing

                Query = ""

            End Try
            If (count > 0) Then
                isSiteSubscribed = True
            Else
                isSiteSubscribed = False
            End If

            Return isSiteSubscribed
        End Function
        Public Function getUserOnlineStatus(ByVal Username As String) As String

            Dim Query As String = "select onlineStatus from tblOnlineUsers where User_Name ='" & Username & "' "
            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim Status As String = " "

            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    Status = Reader("onlineStatus")
                End While
                cmd = Nothing
            Catch err As Exception
                Throw New ApplicationException("Error in connecting to DB " & err.Message)
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing
                Query = ""
            End Try

            Return Status
        End Function


        Public Sub updateOnlineUserStatus(ByVal USERNAME As String, ByVal STATUS As String)

            Dim ds As New DataSet
            ' Dim myParameter As SqlParameter
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand

            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_OnlineStatus"
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Add(New SqlParameter("@USERNAME", SqlDbType.VarChar)).Value = USERNAME
                cmd.Parameters.Add(New SqlParameter("@STATUS", SqlDbType.VarChar)).Value = STATUS


                conn.Open()
                cmd.ExecuteNonQuery()

                Dim adapter As New SqlDataAdapter(cmd)

                adapter.Fill(ds)

            Catch err As DataException
                Throw New ApplicationException("Error .." & err.Message)
            Finally
                conn.Close()
                'cmd.Dispose()
                conn = Nothing
                cmd = Nothing
                ds = Nothing
            End Try

            '    Return ds
        End Sub


        Public Function getMemberInfoForAuthentication(ByVal Username As String) As Collections.Hashtable

            'Dim Query As String = "select user_id_no, Site_Name, AllotedSize,Acct_Expiry, Acct_Type, ip_address  from master_users where User_Name ='" & Username & "' "
            Dim Query As String = "EXECUTE sp_getmemberInfo '" & Username & "' "
            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim SiteName As String = " "
            Dim AllotedSize As String
            Dim AcctExpiry As DateTime
            Dim UserIdNo As Long
            Dim AcctType As String
            Dim IpAddress As String
            Dim ErrMsg As String
            Dim UserInfo As New Hashtable

            'Dim Result As String
            'Dim Found As Int16
            'Found = False
            Dim cmd As New SqlCommand(Query, conn)
            Try

                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    SiteName = Reader("Site_Name")
                    UserIdNo = Reader("user_id_no")
                    AllotedSize = Reader("AllotedSize")
                    AcctExpiry = Reader("Acct_Expiry")
                    AcctType = Reader("acct_type")
                    IpAddress = Reader("ipaddress")


                End While


                UserInfo.Add("SITENAME", SiteName)
                UserInfo.Add("USERIDNO", UserIdNo)
                UserInfo.Add("USEREXPIRED", ExpiryDateAuthentication(AcctExpiry))
                UserInfo.Add("SITENAME", SiteName)


            Catch err As Exception
                'Throw New ApplicationException("Error in connecting to DB " & err.Message)
                ErrMsg = err.Message
            Finally
                conn.Close()
                Reader.Close()
                Reader = Nothing
                conn = Nothing

                Query = ""

            End Try

            Return UserInfo
        End Function

        Public Overloads Function ExpiryDateAuthentication(ByVal ExpiryDate As DateTime) As Boolean
            Dim CurrentDate As DateTime
            Dim Expired As Boolean = False
            CurrentDate = DateTime.Today
            If DateTime.Compare(ExpiryDate, CurrentDate) <= 0 Then
                't1 must be equal or greater than 0 for expired users
                Expired = True
            End If
            Return Expired
        End Function



        Public Function GetUserEmailByUserName(ByVal us As String) As DataTable
            Dim Query As String = "SELECT First_Name,Email_ID FROM master_users where User_Name='" & us & "'"
            Dim DT As DataTable = FetchDataSet(Query)
            Return DT
        End Function
        Public Overloads Function ExpiryDateAuthentication(ByVal UserName As String) As ArrayList
            Dim CurrentDate As DateTime
            Dim ExpiryDate As DateTime
            Dim ExtendDate As Integer = 0
            Dim Expired As Byte = 0
            Dim AccountType As String = ""
            Dim DT As New DataTable
            Dim AR As New ArrayList
            Dim Query As String = "select  a.Acct_Expiry,a.Acc_Extend,a.Acct_Type from Account as a,Admin_Users as au  where au.UserName='" & UserName & "' and au.AccountID= a.AccountID"
            Try
                DT = FetchDataSet(Query)

                If DT.Rows.Count > 0 Then
                    ExpiryDate = CDate(DT.Rows(0).Item("Acct_Expiry"))
                    AccountType = DT.Rows(0).Item("Acct_Type")
                    If AccountType <> "Free" Then
                        If Not IsDBNull(DT.Rows(0).Item(1)) Then
                            ExtendDate = DT.Rows(0).Item(1)
                        Else
                            ExtendDate = 10
                        End If
                    End If

                End If
            Catch ERR As Exception
                Dim msg As String
                msg = ERR.Message
                AR.Add(2) ' which means there is some error occured.
            End Try
            CurrentDate = DateTime.Today
            If DateTime.Compare(ExpiryDate, CurrentDate) <= 0 Then
                AR.Add(1)
                AR.Add(ExtendDate)
                AR.Add(ExpiryDate)
            End If
            Return AR
        End Function

        Public Overloads Function GetUserAllotedData(ByVal UserName As String) As Int32
            'Public Overloads Function GetUserAllotedData(ByVal UserName As String) As String
            Dim CurrentDate As DateTime

            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader

            Dim CurrentDayData As Int32
            Dim AllotedData As Int32

            CurrentDate = System.DateTime.Today
            'Dim Query As String = "select sum(filesize) from UserLog where User_Name ='" & UserName & "' and currentdatetime='" & CurrentDate & "'  "
            'Dim Query As String = "select sum(filesize) from UserLog where User_Name ='" & UserName & "' and day(currentdatetime)=day(" & CurrentDate & ") and month(currentdatetime)=month(" & CurrentDate & ")and year(currentdatetime)=year(" & CurrentDate & ")"
            Dim Query As String = "select sum(filesize) from UserLog where User_Name ='" & UserName & "' and day(currentdatetime)=day('" & CurrentDate & "') and month(currentdatetime)=month('" & CurrentDate & "') and year(currentdatetime)=year('" & CurrentDate & "')"

            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    If IsDBNull(Reader(0)) = False Then
                        CurrentDayData = CInt(Reader(0))
                    End If

                End While

                ''If CurrentDayData = 0 Then
                conn.Close()
                Query = "select AllotedSize from master_users where User_Name ='" & UserName & "' "
                cmd = New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader

                While Reader.Read()
                    If IsDBNull(Reader(0)) = False Then
                        AllotedData = CInt(Reader(0))
                    End If

                End While

                ''End If
                cmd = Nothing
            Catch ERR As Exception
                Dim msg As String
                msg = ERR.Message
            Finally
                conn.Close()
                Query = ""

            End Try

            If AllotedData = -1000000 Then
                CurrentDayData = AllotedData
                GoTo SkipInfinteAccountFound
            End If

            If CurrentDayData > AllotedData Then
                '///Which means that the user did accessed the
                '///cases for current day and already exceed the limit.

                CurrentDayData = -1
            ElseIf CurrentDayData < AllotedData Then
                '///Which means that the user already open the cases 
                '///and current day data accessed is more than zero
                '///and less than AllotedData
                CurrentDayData = AllotedData - CurrentDayData

            ElseIf CurrentDayData = 0 Then
                '///Which means that the user didn't accessed the
                '///cases for current day
                CurrentDayData = AllotedData

            End If

SkipInfinteAccountFound:
            Return CurrentDayData
            '        Return Result
        End Function


        Private Function IsUserLogonToday(ByVal UserId As String, ByVal UserName As String) As Boolean
            ''I m using UserId because most often user do ask for changing their username so this is the main 
            ' PK for monitoring
            Dim CurrentDate As DateTime
            Dim Query As String = "select min(size) from UserLog where UserName='" & UserName & "' and DateTime='" & CurrentDate & "' "
            Dim Expired As Boolean = False


            Return Expired
        End Function



        Public Function getReader(ByVal Query As String) As SqlDataReader
            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim Result As String
            Dim ErrMSg As String
            Dim cmd As New SqlCommand(Query, conn)
            Try

                conn.Open()
                Reader = cmd.ExecuteReader


            Catch err As Exception
                ErrMSg = err.Message

            Finally
                conn.Close()
                Reader.Close()
                cmd = Nothing
                Reader = Nothing
                conn = Nothing
                Result = ""
                Query = ""

            End Try

            Return Reader
        End Function

        Public Function getSingleUserBasicDetails(ByVal UserIdNo As Long) As DataTable
            'Public Function getSingleUserBasicDetails(ByVal UserName As String) As DataTable

            Dim conn As New SqlConnection(ConnectionString)
            'Dim Query As String = "select First_Name,Last_Name,Firm_Name,User_Name,Registration_Date, AllotedSize, Address_1,Address_2,city,state,postal_code,Country,Phone_number,Fax_number,Email_ID, Site_Name from master_users where Access_Type= 'Paid Single' and User_Name= " & UserName & "  "
            Dim Query As String = "select First_Name,Last_Name,Firm_Name,User_Name,Registration_Date, AllotedSize, Address_1,Address_2,city,state,postal_code,Country,Phone_number,Fax_number,Email_ID, Site_Name from master_users where  User_id_no= " & UserIdNo & "  "

            Dim DT As DataTable = FetchDataSet(Query)

            Return DT

        End Function

        Public Overloads Function getSingleUserAdvanceDetails(ByVal UserIdNo As Long) As DataTable
            Dim Query As String = "select Confirmation_Date, Account_Expiry, Mode_Of_Payment,Card_Type,Name_On_Card, Credit_Card_No, CC_Expiry_Date, Credit_Bank_Name, Cheque_Number, Cheque_Bank_Name,Cheque_Holder_Name, Cheque_Date, Total_Ammount,Ammount_Paid, Comments  from Admin_Master where User_ID_no = '" & UserIdNo & "'  "
            Dim DT As DataTable = FetchDataSet(Query)
            Return DT
        End Function

        Public Function getUserId(ByVal UserName As String) As Long
            Dim UserId As Long
            Dim DT As DataTable = Me.FetchDataSet("select user_id_no from master_users where user_name='" & UserName & "'")
            UserId = DT.Rows(0).Item(0)
            Return UserId

        End Function

        Public Function getLastBillNoForCreditCard() As Long

            Dim BillNo As Long
            Dim DT As DataTable = Me.FetchDataSet("select bil from master1 order by bil desc")
            BillNo = DT.Rows(0).Item(0)
            Return BillNo

        End Function
        Public Function getBillNoForCreditCard(ByVal us As String) As Long
            Dim BillNo As Long
            Dim DT As DataTable = Me.FetchDataSet("select bil from master1 where user_name='" & us & "' order by bil desc")
            BillNo = DT.Rows(0).Item(0)
            Return BillNo

        End Function

        Public Function isUserCorporate(ByVal UserName As String) As Boolean

            Try


                Dim Query As String
                Dim DT As New DataTable
                Dim Count As Int16
                Dim Flag As Boolean = False
                Try


                    Query = "Select count(user_name) from master_users where user_name='" & UserName & "' and access_type='Group'"
                    DT = Me.FetchDataSet(Query)
                    Count = CInt(DT.Rows(0).Item(0))
                    If Count > 0 Then
                        Flag = True
                    End If
                Catch ex As Exception
                    Flag = False
                End Try
                isUserCorporate = Flag
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function getIPAddress(ByVal UserName As String) As String
            Dim Query As String
            Dim DT As New DataTable
            Dim count As Int16

            Dim IPAddress As String = ""

            Query = "Select IP_Address from master_users where user_name='" & UserName & "' and access_type='Group'"
            DT = Me.FetchDataSet(Query)
            count = DT.Rows.Count
            If count > 0 Then
                IPAddress = CStr(DT.Rows(0).Item(0) & "")
            End If

            'If IPAddress = "" Then

            'End If

            getIPAddress = IPAddress

        End Function

        'Public Function UserLog(ByVal UserName As String, ByVal SiteName As String, ByVal FileName As String, ByVal FileSize As Long, ByVal LocalAddr As String, ByVal RemoteAddr As String) As Boolean
        Public Sub UserLog(ByVal UserName As String, ByVal SiteName As String, ByVal FileName As String, ByVal FileSize As Long, ByVal LocalAddr As String, ByVal RemoteAddr As String)
            Dim DS As New DataSet
            Dim CurrentDateTime As DateTime = System.DateTime.Now
            'Dim myParameter As SqlParameter
            Dim conn As New SqlConnection(ConnectionString)
            Dim Query As String
            Dim UserID As Long = Me.getUserId(UserName)
            Dim cmd As New SqlCommand
            'Dim isInserted As Boolean
            'isInserted = False
            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_UserLog"
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Add(New SqlParameter("@USER_ID", SqlDbType.BigInt)).Value = UserID
                cmd.Parameters.Add(New SqlParameter("@USER_NAME", SqlDbType.VarChar)).Value = UserName
                cmd.Parameters.Add(New SqlParameter("@SITE_NAME", SqlDbType.VarChar)).Value = SiteName
                cmd.Parameters.Add(New SqlParameter("@DATAFILENAME", SqlDbType.VarChar)).Value = FileName
                cmd.Parameters.Add(New SqlParameter("@DATETIME1", SqlDbType.DateTime)).Value = CurrentDateTime
                cmd.Parameters.Add(New SqlParameter("@SIZE", SqlDbType.BigInt)).Value = FileSize
                cmd.Parameters.Add(New SqlParameter("@LOCAL_ADDRESS", SqlDbType.VarChar)).Value = LocalAddr
                cmd.Parameters.Add(New SqlParameter("@REMOTE_ADDRESS", SqlDbType.VarChar)).Value = RemoteAddr

                conn.Open()
                cmd.ExecuteNonQuery()

            Catch err As DataException
                Throw New DataException("Error .." & err.Message)
                'isInserted = False
            Finally
                conn.Close()
                Query = ""
                conn = Nothing
                cmd = Nothing
                DS = Nothing
            End Try

            'Return isInserted
        End Sub

        Public Overloads Function FetchDataSet(ByVal Query As String) As DataTable
            Dim DT As New DataTable
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand(Query, conn)
            Try
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
                cmd = Nothing

            End Try

            Return DT
        End Function

        Public Function AddRecord(ByVal Query As String) As Boolean

            AddRecord = False
            Dim Statement As String
            Statement = Query
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand
            Try
                cmd.Connection = conn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = Query
                conn.Open()
                cmd.ExecuteNonQuery()

                AddRecord = True
            Catch err As Exception
                AddRecord = False
                Throw New Exception("Error in inserting to DB  " & err.Message)

            Finally
                conn.Close()
                cmd = Nothing
            End Try
            Return AddRecord

        End Function


        Public Function InsertFreeUser(ByVal customerName As String, ByVal address As String, ByVal address2 As String, ByVal city As String, ByVal state As String, ByVal postalcode As String, ByVal country As String, ByVal phone_num As String, ByVal fax_num As String, ByVal status As String, ByVal first_name As String, ByVal last_name As String, ByVal position As String, ByVal tel_num As String, ByVal email As String, ByVal account As String, ByVal asize As String, ByVal account_type As String, ByVal user_name As String, ByVal password As String, ByVal customerID As String, ByVal CPID As String, ByVal accountID As String, ByVal userID As String, ByVal AccountExpiry As Date, ByVal adminName As String, ByVal comments As String, ByVal SiteName As String) As Boolean
            Dim InsertRecord As Boolean
            Dim conn As New SqlConnection(ConnectionString)

            Dim cmd As New SqlCommand()
            Try

                cmd.Connection = conn
                cmd.CommandText = "sp_insertFreeUser"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@customerName", SqlDbType.VarChar, 35).Value = customerName
                cmd.Parameters.Add("@address1", SqlDbType.VarChar, 35).Value = address
                cmd.Parameters.Add("@address2", SqlDbType.VarChar, 35).Value = address2
                cmd.Parameters.Add("@city", SqlDbType.VarChar, 35).Value = city
                cmd.Parameters.Add("@state", SqlDbType.VarChar, 35).Value = state
                cmd.Parameters.Add("@postalcode", SqlDbType.VarChar, 35).Value = postalcode
                cmd.Parameters.Add("@country", SqlDbType.VarChar, 35).Value = country
                cmd.Parameters.Add("@phonenum", SqlDbType.VarChar, 18).Value = phone_num
                cmd.Parameters.Add("@faxnum", SqlDbType.VarChar, 18).Value = fax_num
                cmd.Parameters.Add("@status", SqlDbType.VarChar, 10).Value = status
                cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 35).Value = first_name
                cmd.Parameters.Add("@lastname", SqlDbType.VarChar, 35).Value = last_name
                cmd.Parameters.Add("@position", SqlDbType.VarChar, 35).Value = position
                cmd.Parameters.Add("@telnum", SqlDbType.VarChar, 18).Value = tel_num
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 35).Value = email
                cmd.Parameters.Add("@account", SqlDbType.VarChar, 25).Value = account
                cmd.Parameters.Add("@asize", SqlDbType.VarChar, 25).Value = asize
                cmd.Parameters.Add("@accttype", SqlDbType.VarChar, 10).Value = account_type
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 20).Value = user_name
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 15).Value = password
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar, 10).Value = customerID
                cmd.Parameters.Add("@CPID", SqlDbType.VarChar, 10).Value = CPID
                cmd.Parameters.Add("@accountID", SqlDbType.VarChar, 10).Value = accountID
                cmd.Parameters.Add("@userID", SqlDbType.VarChar, 10).Value = userID
                cmd.Parameters.Add("@expiry_date", SqlDbType.DateTime).Value = AccountExpiry
                cmd.Parameters.Add("@admin", SqlDbType.VarChar, 10).Value = adminName
                cmd.Parameters.Add("@comments", SqlDbType.VarChar, 50).Value = comments
                cmd.Parameters.Add("@SiteName", SqlDbType.VarChar, 50).Value = SiteName
                conn.Open()

                cmd.ExecuteNonQuery() '.ExecuteReader() 

                conn.Close()
                InsertRecord = True
            Catch err As DataException
                Throw New ApplicationException("Error in inserting to DB  " & err.Message)
                InsertRecord = False
            Finally
                conn.Close()
                cmd.Dispose()
            End Try

            Return InsertRecord
        End Function
        Public Function getmax_customer_id() As Integer

            Dim conn As New SqlConnection(ConnectionString)

            Dim _increas_id As Integer

            Try
                conn.Open()
                Dim SqlQuery As String = "SELECT TOP 1 CustomerID,id FROM Customer order by id desc"

                Dim MYcmd = New SqlCommand(SqlQuery, conn)

                Dim cmd = New SqlCommand(SqlQuery, conn)
                Dim mysqlreader As SqlDataReader


                mysqlreader = cmd.ExecuteReader()
                While mysqlreader.Read
                    _increas_id = mysqlreader("id").ToString()
                End While
                _increas_id = _increas_id + 1

            Catch ex As Exception

            End Try
            conn.Close()
            Return _increas_id

        End Function
        Function GetMaxIDmaster_users() As Integer
            Dim conn As New SqlConnection(ConnectionString)
            Dim UserMaxID As Integer
            Try
                conn.Open()
                Dim SqlQuery As String = "SELECT max(user_id_no) as MaxID FROM master_users"

                Dim MYcmd = New SqlCommand(SqlQuery, conn)
                Dim mysqlreader As SqlDataReader
                mysqlreader = MYcmd.ExecuteReader()
                While mysqlreader.Read
                    UserMaxID = mysqlreader("MaxID")
                End While

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            conn.Close()
            Return UserMaxID
        End Function
        Public Function AddLogin(ByVal UserName As String, ByVal Acc_type As String, ByVal iP As String) As Boolean
            Dim QueryLogin As String
            QueryLogin = "insert into LoginReport ([LoginUserName] ,[LoginDate]  ,[LoginCount] ,[LoginAccountType] ,[LoginGroup], IPAddress) VALUES ('" & UserName & "' ,getdate(),1,'" & Acc_type & "','','" & iP & "') "
            Dim Flg As Boolean
            Try
                Flg = Me.AddRecord(QueryLogin)
            Catch ex As Exception
                Flg = False
            End Try
            Return Flg
        End Function
        Public Function InsertNewInvoice(ByVal customerID As String, ByVal Factor As Decimal, ByVal totalAmount As Decimal, ByVal status As String, ByVal product_num As String, ByVal quantity As Integer, ByVal PayType As String, ByVal PayStatus As String, ByVal PaymentMode As String, ByVal expiryDate As String, ByVal CPID As String, ByVal adminName As String, ByVal reason_dis As String, ByVal AllIds As String, ByVal AllIdsQuanatity As String) As Boolean
            Dim InsertRecord As Boolean
            Dim conn As New SqlConnection(ConnectionString)

            Dim cmd As New SqlCommand()
            Try

                cmd.Connection = conn
                cmd.CommandText = "sp_InsertNewInvoice"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@customerID", SqlDbType.VarChar, 10).Value = customerID
                Dim spm As SqlParameter = cmd.Parameters.Add("@dfactor", SqlDbType.Float)
                spm.Value = Factor
                'spm.Precision = 8
                'spm.Scale = 8
                cmd.Parameters.Add("@price", SqlDbType.Money).Value = totalAmount
                cmd.Parameters.Add("@status", SqlDbType.VarChar, 10).Value = status
                cmd.Parameters.Add("@productno", SqlDbType.VarChar, 10).Value = product_num
                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = quantity
                cmd.Parameters.Add("@paytype", SqlDbType.VarChar, 10).Value = PayType
                cmd.Parameters.Add("@paystatus", SqlDbType.VarChar, 10).Value = PayStatus
                cmd.Parameters.Add("@paymentmode", SqlDbType.VarChar, 11).Value = PaymentMode
                cmd.Parameters.Add("@ccexpirydate", SqlDbType.Date).Value = expiryDate
                cmd.Parameters.Add("@CPID", SqlDbType.VarChar, 10).Value = CPID
                cmd.Parameters.Add("@admin", SqlDbType.VarChar, 10).Value = adminName
                cmd.Parameters.Add("@reason_dis", SqlDbType.VarChar, 100).Value = reason_dis
                cmd.Parameters.Add("@allIds", SqlDbType.VarChar, 100).Value = AllIds
                cmd.Parameters.Add("@allIdsQ", SqlDbType.VarChar, 100).Value = AllIdsQuanatity
                conn.Open()

                cmd.ExecuteNonQuery() '.ExecuteReader() 

                conn.Close()
                InsertRecord = True
            Catch err As DataException
                Throw New ApplicationException("Error in inserting to DB  " & err.Message)
                InsertRecord = False
            Finally
                conn.Close()
                cmd.Dispose()
            End Try

            Return InsertRecord
        End Function
        Function PerpareInsertingNewInv(ByVal billid As Integer) As DataTable
            Dim DT As New DataTable
            Dim Query = "select MU.Total_Amount,AU.UserName,c.CustomerName,c.CustomerID,ac.CPID,m1.Site_Name,m1.Firm_Name,m1.FirstName,m1.LastName,"
            Query &= " m1.user_name,m1.Password,m1.Email_Id,au.UserID,mu.user_id_no ,m1.Acct_type, Mu.Acct_Expiry   from master_users MU "
            Query &= " inner join master1 M1 on m1.user_id_no=mu.user_id_no "
            Query &= " inner join Admin_Users AU on AU.UserNo=mu.user_id_no "
            Query &= " inner join Customer c on convert(VARCHAR(10),c.CustomerID)  =convert(VARCHAR(100), AU.UserID) "
            Query &= " inner join admin_ContactPerson ac on ac.CPID=c.CustomerID "
            Query &= "  where m1.Bil=" & billid & ""

            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand(Query, conn)
            cmd.Parameters.AddWithValue("@billid", billid)
            Try
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
            Catch err As DataException
                Throw New DataException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
                cmd = Nothing

            End Try

            Return DT

        End Function
        Function GetProductIdbyname(ByVal Pname As String) As Integer
            Dim productId As Integer
            Dim Query As String = "select ProductID from admin_product where PName=@pname "
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand(Query, conn)
            cmd.Parameters.AddWithValue("@pname", Pname)
            conn.Open()

            Try
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                While dr.Read
                    productId = dr("ProductID")
                End While

            Catch ex As Exception
            End Try
            conn.Close()
            Return productId
        End Function
        Function GetInvoiceNoByCustomerID(ByVal customerID As Integer) As Integer
            Dim InvoiceNo As Integer
            Dim Query As String = "select top 1  InvoiceNo from Invoice where CustomerID=@customerID  order by InvoiceNo desc"
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand(Query, conn)
            cmd.Parameters.AddWithValue("@customerID", customerID)
            conn.Open()

            Try
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                While dr.Read
                    InvoiceNo = dr("InvoiceNo")
                End While

            Catch ex As Exception
            End Try
            conn.Close()
            Return InvoiceNo
        End Function
        Public Overloads Function DisplayReceiptNo(ByVal InvoiceNo As String) As DataTable
            Dim DT As New DataTable
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            'Dim Reader1 As SqlClient.SqlDataReader
            Try
                cmd.CommandText = "sp_getReceiptNo"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Connection = conn
                cmd.Parameters.Add("@invoice_no", SqlDbType.VarChar, 10).Value = InvoiceNo
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)


            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
            End Try

            Return DT
        End Function
        Public Function UpdateReceiptDetails(ByVal InvoiceNo As String, ByVal ReceiptNo As String, ByVal amountPaid As Decimal, ByVal chequeNo As String, ByVal bank As String, ByVal status As String, ByVal payMode As String, ByVal ccdate As Date) As Boolean
            Dim UpdateRecord As Boolean
            Dim conn As New SqlConnection(ConnectionString)

            Dim cmd As New SqlCommand()
            Try

                cmd.Connection = conn
                cmd.CommandText = "sp_UpdateReceiptDetails"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@invoice_no", SqlDbType.VarChar, 10).Value = InvoiceNo
                cmd.Parameters.Add("@receipt_no", SqlDbType.VarChar, 10).Value = ReceiptNo
                cmd.Parameters.Add("@amount_paid", SqlDbType.Money).Value = amountPaid
                cmd.Parameters.Add("@cheque_no", SqlDbType.VarChar, 25).Value = chequeNo
                cmd.Parameters.Add("@bank", SqlDbType.VarChar, 50).Value = bank
                cmd.Parameters.Add("@status", SqlDbType.VarChar, 10).Value = status
                cmd.Parameters.Add("@paymode", SqlDbType.VarChar, 11).Value = payMode
                cmd.Parameters.Add("@ccdate", SqlDbType.Date).Value = ccdate
                conn.Open()

                cmd.ExecuteNonQuery() '.ExecuteReader() 

                conn.Close()
                UpdateRecord = True
            Catch err As DataException
                Throw New ApplicationException("Error in inserting to DB  " & err.Message)
                UpdateRecord = False
            Finally
                conn.Close()
                cmd.Dispose()
            End Try

            Return UpdateRecord
        End Function

        Function IsInvoiceExistByBillID(ByVal BillID As Integer) As DataTable
            Dim DT As New DataTable
            Dim conn As New SqlConnection(ConnectionString)
            Dim SqlQuery As String = "select      MAX(i.InvoiceNo) from master_users MU "
            SqlQuery &= " inner join master1 M1 on m1.user_id_no=mu.user_id_no "
            SqlQuery &= " inner join Admin_Users AU on AU.UserNo=mu.user_id_no "
            SqlQuery &= " inner join Customer c on convert(VARCHAR(10),c.CustomerID)  =convert(VARCHAR(100), AU.UserID) "
            SqlQuery &= " inner join Invoice i on i.CustomerID =c.CustomerID "
            SqlQuery &= "   where m1.Bil=@billID "
            Dim cmd = New SqlCommand(SqlQuery, conn)
            cmd.Parameters.AddWithValue("@billID", BillID)
            Try
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
            Catch err As Exception

            Finally
                conn.Close()
                conn = Nothing
                cmd = Nothing
            End Try
            Return DT
        End Function
        Function UpdateAccountTypeStatus(ByVal type As String, ByVal AMID As String, ACId As String, ByVal USID As Integer) As Boolean
            Dim flag As Boolean
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand
            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_UpdateFreeAccountOnlinePayment"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = type
                cmd.Parameters.Add("@AMID", SqlDbType.VarChar, 10).Value = AMID
                cmd.Parameters.Add("@acID", SqlDbType.VarChar, 10).Value = ACId
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = USID
                conn.Open()
                cmd.ExecuteNonQuery()
                flag = True
            Catch ex As Exception
                Throw New ApplicationException("Error in inserting to DB  " & ex.Message)
            Finally
                conn.Close()
                cmd.Dispose()
            End Try



            Return flag
        End Function
        Function CheckInvoiceStatus(ByVal InvoiceNo As Integer) As Boolean
            Dim Status As Boolean = False
            Dim StoreVar As String = ""
            Dim Query As String = "SELECT [Status]  FROM Admin_PaymentMaster WHERE InvoiceNo =@invoice"
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd = New SqlCommand(Query, conn)
            cmd.Parameters.AddWithValue("@invoice", InvoiceNo)
            Dim mysqlreader As SqlDataReader
            Try
                conn.Open()
                mysqlreader = cmd.ExecuteReader()
                While mysqlreader.Read
                    StoreVar = mysqlreader("Status").ToString()
                End While
                If StoreVar = "Paid" Or StoreVar = "paid" Then
                    Status = False
                Else
                    Status = True
                End If
            Catch ex As Exception
                Status = False
            Finally
                conn.Dispose()
                conn.Close()
                StoreVar = ""
            End Try
            Return Status
        End Function
        Public Function RenewAccount(ByVal UserID As String, ByVal ExpiryDate As Date, ByVal AccountType As String, ByVal PaymentMode As String, ByVal InvoiceNum As Integer, ByVal chequeDate As Date, ByVal comments As String) As Boolean
            Dim UpdateRecord As Boolean
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            Try
                cmd.Connection = conn
                cmd.CommandText = "sp_RenewAccount"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@userid", SqlDbType.VarChar, 10).Value = UserID
                cmd.Parameters.Add("@expiry_date", SqlDbType.Date).Value = ExpiryDate
                cmd.Parameters.Add("@acct_type", SqlDbType.VarChar, 10).Value = AccountType
                cmd.Parameters.Add("@paymode", SqlDbType.VarChar, 11).Value = PaymentMode
                cmd.Parameters.Add("@invoice_num", SqlDbType.Int).Value = InvoiceNum
                cmd.Parameters.Add("@chequedate", SqlDbType.Date).Value = chequeDate
                cmd.Parameters.Add("@comments", SqlDbType.VarChar, 50).Value = comments
                conn.Open()
                cmd.ExecuteNonQuery() '.ExecuteReader() 
                conn.Close()
                UpdateRecord = True
            Catch err As Exception
                Throw New ApplicationException("Error in inserting to DB  " & err.Message)
                UpdateRecord = False
            Finally
                conn.Close()
                cmd.Dispose()
            End Try

            Return UpdateRecord
        End Function
        ' this function not used for current time 8-10-2013
        Public Function UpdateExpiryAccount(ByVal UserID As String, ByVal ExpiryDate As Date) As Boolean
            Dim UpdateRecord As Boolean
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            Try
                cmd.Connection = conn
                cmd.CommandText = "Sp_updateExpiryDate"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@userno", SqlDbType.VarChar, 20).Value = UserID
                cmd.Parameters.Add("@expiry_date", SqlDbType.Date).Value = ExpiryDate
                conn.Open()
                cmd.ExecuteNonQuery() '.ExecuteReader() 
                conn.Close()
                UpdateRecord = True
            Catch err As Exception
                Throw New ApplicationException("Error in inserting to DB  " & err.Message)
                UpdateRecord = False
            Finally
                conn.Close()
                cmd.Dispose()
            End Try

            Return UpdateRecord
        End Function

        Public Function AddTrialAccount(ByVal FN As String, ByVal LN As String, ByVal PW As String, ByVal Email As String, ByVal Status As Byte, ByVal days As Integer, ByVal username As String, ByVal PcName As String, ByVal IP As String) As Boolean
            Dim InsertRecord As Boolean
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()
            Try

                cmd.Connection = conn
                cmd.CommandText = "sp_InsertTrialAccount"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@FN", SqlDbType.VarChar, 50).Value = FN
                cmd.Parameters.Add("@LN", SqlDbType.VarChar, 50).Value = LN
                cmd.Parameters.Add("@PW", SqlDbType.VarChar, 50).Value = PW
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = Status
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = Email
                cmd.Parameters.Add("@days", SqlDbType.Int).Value = days
                cmd.Parameters.Add("@usname", SqlDbType.VarChar, 50).Value = username
                cmd.Parameters.Add("@pcName", SqlDbType.VarChar, 50).Value = PcName
                cmd.Parameters.Add("@IP", SqlDbType.VarChar, 50).Value = IP


                conn.Open()

                cmd.ExecuteNonQuery() '.ExecuteReader() 

                conn.Close()
                InsertRecord = True
            Catch err As DataException
                Throw New ApplicationException("Error in inserting to DB  " & err.Message)
                InsertRecord = False
            Finally
                conn.Close()
                cmd.Dispose()
            End Try
            Return InsertRecord
        End Function
        ''' <summary>
        ''' Save Reminder After Sending Email to User to avoid sending multi emails when user's account going to expir
        ''' 
        ''' </summary>
        ''' <param name="Reminder"></param>
        ''' <param name="level" ></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveReminder(ByVal Reminder As ArrayList, ByVal level As String) As Boolean
            Dim R As Boolean
            Dim sql As String = "SELECT [User_Name] ,[User_Email] ,FirstReminder ,[DateFirstReminder] ,[SecondReminder] ,DateSecondReminder FROM EmailReminder where [User_Name]='" & Reminder.Item(0) & "'"
            Dim obj As New clsCasesSearch
            Dim dt = New DataTable
            Dim docDate As DateTime = DateTime.Now.ToString
            dt = obj.ExecuteMyQuery(sql)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("FirstReminder") = "0" And level = "R" Then
                    Dim updateFirstReminder As String = "update EmailReminder set DateFirstReminder='" & docDate & "',FirstReminder=1  where User_Name='" & Reminder.Item(0) & "'"
                    obj.UpdateRecord(updateFirstReminder)
                    R = True
                ElseIf dt.Rows(0).Item("SecondReminder") = "0" And level = "W" Then
                    Dim updateFirstReminder As String = "update EmailReminder set DateSecondReminder='" & docDate & "',SecondReminder=1  where User_Name='" & Reminder.Item(0) & "'"
                    obj.UpdateRecord(updateFirstReminder)
                    R = True
                Else
                    R = False
                End If
            Else
                Dim AddNewReminder As String = "INSERT INTO EmailReminder" & _
                  " ([User_Name] ,[User_Email] ,[FirstReminder], SecondReminder,[DateFirstReminder] ) " & _
                    "  VALUES('" & Reminder.Item(0) & "' ,'" & Reminder.Item(1) & "',0,1,'" & Date.Now & "')"
                obj.AddRecord(AddNewReminder)
                R = True
            End If
            Return R
        End Function

        ''' <summary>
        ''' Send Reminder To User Before account Expired 
        ''' </summary>
        ''' <param name="toAddress"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SendReminder(ByVal toAddress As ArrayList) As Boolean
            Dim result As Boolean = False

            Dim AccountExpiry As Date = toAddress.Item(0)
            Dim Email As String = toAddress.Item(1)
            Dim firstName As String = toAddress.Item(2)
            'Dim GetBody As String = PerpareBodyMessage()
            Try
                Dim mail As New MailMessage()
                'Dim fn As System.Net.Mime.ContentDisposition
                Dim SmtpServer As New SmtpClient("mail.elaw.my", 25)
                mail.From = New MailAddress("subscribe@elaw.my", " The Digital Library")
                mail.[To].Add(Email)
                mail.Bcc.Add("marketing@elaw.my")
                mail.Subject = "eLaw Account"
                mail.IsBodyHtml = True
                mail.Body = "<br/><br/> Dear " & firstName & ", <br/><br/> Thank you for subscribing to eLaw.my. <br/><br/>Your account will expire on <b>" & AccountExpiry.ToString("D") & "</b>. <br/><br/> Should you require further assistance, kindly contact us at <b>1300-88-3529 (eLaw)</b> or email us at <b>enquiries@elaw.my</b>.<br/>"
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


        Public Function AccExpiryOneDay(ByVal DaysLeft As Integer, ByVal AccExpiry As Date, ByVal Email As String, ByVal Name As String, ByVal ContactNo As String) As Boolean
            Dim flg As Boolean
            DaysLeft = DaysLeft + 1
            If DaysLeft = 1 Then
                Try
                    Dim mail As New MailMessage()
                    'Dim fn As System.Net.Mime.ContentDisposition
                    Dim SmtpServer As New SmtpClient("mail.elaw.my", 25)
                    mail.From = New MailAddress("subscribe@elaw.my", " The Digital Library")
                    mail.[To].Add("marketing@elaw.my")
                    'mail.Bcc.Add("marketing@elaw.my")
                    mail.Subject = "eLaw Account Expiry For: " & Name
                    mail.IsBodyHtml = True
                    mail.Body = "<br/><br/> Name :  " & Name & ", <br/><br/>Email : " & Email & "<br/><br/>Phone No : " & ContactNo & "<br/><br/>Account will expire on <b>" & AccExpiry.ToString("D") & "</b>. <br/><br/> Should you require further assistance, kindly contact us at <b>1300-88-3529 (eLaw)</b> or email us at <b>enquiries@elaw.my</b>.<br/>"
                    SmtpServer.EnableSsl = False
                    SmtpServer.UseDefaultCredentials = True
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network
                    SmtpServer.Credentials = New System.Net.NetworkCredential("subscribe@elaw.my", "Subscribe123#")
                    SmtpServer.Send(mail)

                Catch ex As Exception

                End Try

            End If
            Return flg

        End Function

#Region "Ip Range For Login Function  "

        Public Function CheckIpRange(ByVal UserName As String, ByVal Ip As String) As Boolean
            Dim flg As Boolean = False
            Dim DT As New DataTable
            Dim Query As String = "SELECT RangeFrom ,RangeTo FROM IP_Range where IpUserName ='" & UserName & "'"
            DT = FetchDataSet(Query)
            For i = 0 To DT.Rows.Count - 1
                flg = IsInRange(DT.Rows(i).Item(0).ToString, DT.Rows(i).Item(1).ToString, Ip)
                If flg = True Then
                    Exit For
                    Return True
                End If
            Next
            Return flg
        End Function
        Private Shared Function IsInRange(startIpAddr As String, endIpAddr As String, address As String) As Boolean
            Dim ipStart As Long = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0)
            Dim ipEnd As Long = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0)
            Dim ip As Long = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0)
            Return ip >= ipStart AndAlso ip <= ipEnd
        End Function
        Public Function IsExistInIpRange(ByVal Username As String) As Boolean
            Dim dt As New DataTable
            Dim Query As String = "select IpUserName from IP_Range where IpUserName ='" & Username & "'  "
            Dim Found As Boolean = False
            Try
                dt = FetchDataSet(Query)
                If dt.Rows.Count > 0 Then
                    Found = True
                Else
                    Found = False
                End If
            Catch err As Exception
                Throw New ApplicationException("Error in connecting to DB " & err.Message)
            Finally
            End Try
            Return Found
        End Function
#End Region

#Region " Multi Session Function "
        ''' <summary>
        ''' get no of times allow this user to login to eLaw , given by admin (0,3,5)
        ''' </summary>
        ''' <param name="UserName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMultiSessionGroup(ByVal UserName As String) As Integer
            Dim RetStatus As Integer
            Dim Query As String
            Dim DT As New DataTable
            Dim count As Int16
            Dim IPAddress As String = ""
            Query = "Select MultiSession from UserMltiSession where LoginUserName='" & UserName & "' "
            DT = Me.FetchDataSet(Query)
            count = DT.Rows.Count
            If count > 0 Then
                RetStatus = CInt(DT.Rows(0).Item(0))
            End If
            Return RetStatus
        End Function
        ''' <summary>
        ''' this function use for login , get user name , Current status, and times of login (n) so far
        ''' </summary>
        ''' <param name="UserName"></param>
        ''' <param name="Status"  ></param>
        ''' <param name="MultiLogin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
         Public Function ProcessMultiSession(ByVal UserName As String, ByVal Status As String, ByVal MultiLogin As Integer) As Integer
            Dim IsAllowToLogin As Integer
            Dim TempStatus As String = Status.ToLower()
            Dim ExccedLogin As String = ("online" & MultiLogin - 1).ToString()
            Dim TempLoginNo As String = ""
            '================= General Condition =====================
            'offline, online, excced status
            If String.Compare(TempStatus, "offline", True) = 0 Then
                updateOnlineUserStatus(UserName, "Online")
                IsAllowToLogin = 2
                Return IsAllowToLogin
            End If
            If String.Compare(TempStatus, "online", True) = 0 Then
                updateOnlineUserStatus(UserName, "Online1")
                IsAllowToLogin = 2
                Return IsAllowToLogin
            End If
            If String.Compare(TempStatus, ExccedLogin, True) = 0 Then
                IsAllowToLogin = 1
                Return IsAllowToLogin
            End If

            '========Now looping though all avaiable session for current user ========
            For i = MultiLogin - 1 To 1 Step -1
                TempLoginNo = ("online" & i).ToString()
                If String.Compare(TempStatus, TempLoginNo, True) = 0 Then
                    '=======Update user Status ==================
                    updateOnlineUserStatus(UserName, "online" & i + 1)
                    IsAllowToLogin = 2
                    Return IsAllowToLogin
                End If
            Next
            '========================================================================================
            Return IsAllowToLogin

        End Function
        ''' <summary>
        ''' Logout for account with multi session ,get currnt status , update status, and retur value 
        ''' </summary>
        ''' <param name="UserName"></param>
        ''' <param name="Status"></param>
        ''' <param name="MultiLogin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ProcessMultiSessionLogout(ByVal UserName As String, ByVal Status As String, ByVal MultiLogin As Integer) As Integer
            Dim IsAllowToLogin As Integer
            Dim TempStatus As String = Status.ToLower()
            Dim TempLoginNo As String = ""
            '=============================Process for mutli login Group Account =====================
            '================= General Condition =====================
            'offline, online, excced status
            If String.Compare(TempStatus, "online", True) = 0 Then
                updateOnlineUserStatus(UserName, "offline")
                IsAllowToLogin = 2
                Return IsAllowToLogin
            End If
            If String.Compare(TempStatus, "offline", True) = 0 Then
                IsAllowToLogin = 2
                Return IsAllowToLogin
            End If
            If String.Compare(TempStatus, "online1", True) = 0 Then
                updateOnlineUserStatus(UserName, "online")
                IsAllowToLogin = 2
                Return IsAllowToLogin
            End If
            '================
            For i = MultiLogin - 1 To 1 Step -1
                TempLoginNo = ("online" & i).ToString()
                If String.Compare(TempStatus, TempLoginNo, True) = 0 Then
                    '=======Update user Status ==================
                    updateOnlineUserStatus(UserName, "online" & i - 1)
                    IsAllowToLogin = 2
                    Return IsAllowToLogin
                End If
            Next

            '========================================================================================
            Return IsAllowToLogin

        End Function
#End Region
    End Class
End Namespace
