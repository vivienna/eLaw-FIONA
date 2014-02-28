
'/******************************************************************************/
'/*	Developer 	    : Usman Sarwar ,Mohammed Ali          								   */
'/*	Company     	: Elaw Sdn Bhd		    	    				        */
'/* Date Modified	: 8 Aug 2013       		        		    		     */  
'/*	Description		: This library has database search related functions        */
'/*	Version			: 1.0											            */
'/*******************************************************************************/

Imports System.Data.SqlClient
Imports System.Exception
Imports System.Configuration
Imports System.Data


Namespace membersarea



Public Class clsSearch
    'Protected Shared ConnectionString As String
    Public Shared ConnectionString As String

    Public Sub New()

            ConnectionString = clsConfigs.sGlobalConnectionString

        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If

    End Sub

    Public Function isFileExist(ByVal FileName As String) As Boolean
        ''/*  This is for counting records fetched   */


        'CnString = ConfigurationSettings.AppSettings("ConnectionString")
        Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            'Dim ErrMsg As String
        Dim conn As New SqlConnection(ConnectionString)
        Dim Reader As SqlDataReader
            'Dim recCount As Int32
            Dim SqlQuery As String = "select count(*) from cases where datafilename = '" & FileName & "'"
        Dim cmd = New SqlCommand(SqlQuery, conn)
        Dim msg As String

        Try
            conn.Open()
            Reader = cmd.ExecuteReader()
            While Reader.Read()
                FileCount = Reader(0)
            End While

            If FileCount = 0 Then
                isFileExist = False ' file not Exist
            Else
                isFileExist = True  ' File Exist
            End If

        Catch err As Exception
            msg = err.Message

        Finally
            conn.Close()
            conn = Nothing
            Reader = Nothing
            cmd = Nothing
        End Try


        Return isFileExist
    End Function

    Public Function AddRecord_Admin(ByVal Query As String) As Boolean
        'Dim Query As String = "insert into tblUserPrimary "
        AddRecord_Admin = False
        Dim Statement As String
        Statement = Query
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        Try
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = Query
            conn.Open()
            cmd.ExecuteNonQuery()

            AddRecord_Admin = True
        Catch err As Exception
            AddRecord_Admin = False
            Throw New Exception("Error in inserting to DB  " & err.Message)


        Finally
            conn.Close()
            cmd = Nothing
        End Try
        Return AddRecord_Admin

    End Function


    Public Overloads Function DeleteRecord_admin(ByVal Query As String) As Boolean

        DeleteRecord_admin = False
        Dim Statement As String
        Statement = Query
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        Try
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = Query
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            DeleteRecord_admin = True
        Catch err As Exception
            Throw New Exception("Error in inserting to DB  " & err.Message)
            DeleteRecord_admin = False

        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return DeleteRecord_admin

    End Function


    '   Public Function LoadMemberExpiryDate(ByVal UserName As String) As Int16
    Public Function LoadMemberExpiryDate(ByVal UserName As String) As Int16

        'sp_getMemberRemainingTime

        'Dim ds As New DataSet()
        'Dim myParameter As SqlParameter
        'Dim conn As New SqlConnection(ConnectionString)
        ''Dim conn As SqlConnection(ConnectionString)
        ''Dim cmd As New SqlCommand()
        'Dim cmd As SqlCommand()

        'Try

        '    cmd.Connection = conn
        '    cmd.CommandText = "sp_getmemberexpiry"
        '    cmd.CommandType = CommandType.StoredProcedure
        '    myParameter = cmd.CreateParameter()
        '    myParameter.ParameterName = "@username"
        '    myParameter.Direction = ParameterDirection.Input
        '    myParameter.SqlDbType = SqlDbType.VarChar

        '    myParameter.Value = UserName.ToString
        '    cmd.Parameters.Add(myParameter)

        '    conn.Open()
        '    cmd.ExecuteNonQuery()

        'reader = cmd.ExecuteReader

        Dim conn As New SqlConnection(ConnectionString)
            Dim reader As SqlDataReader ' = New SqlDataReader() {}
        Dim expDate As SqlDbType '   SqlDbType
        Dim expDate1 As Date
        Dim DaysLeft As Int16
        Dim strSql As String
        strSql = "EXECUTE sp_getmemberexpiry '" & UserName & "' "
        Dim cmd = New SqlCommand(strSql, conn)
        Try
            conn.Open()
            reader = cmd.ExecuteReader()
            '            reader.
            'expDate = SqlDbType.SmallDateTime

            expDate = SqlDbType.DateTime

            '            strSql = Convert.ToString(reader("acct_expiry"))   '.ToDateTime
            expDate1 = Convert.ToDateTime(reader("acct_expiry"))    '.ToDateTime

            'expDate = Convert.ToDateTime(strSql)

            'expDate = DateTime.Parse(reader("acct_expiry"))      '.ToDateTime

            'Bind to DataGrid
            '    dtgAuthors.DataSource = dataReader
            '   dtgAuthors.DataBind()
        Catch err As Exception
            strSql = err.Message
        Finally
            conn.Close()
            conn.Dispose()
        End Try




        'Dim adapter As New SqlDataAdapter(cmd)

        'adapter.Fill(ds)
        'conn.Close()

        'Catch err As DataException
        '   Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        'Finally
        '   conn.Close()
        '   conn.Dispose()
        '   cmd.Dispose()
        '   End Try



        DaysLeft = 10
        Return DaysLeft
        'Return expDate1
    End Function



    Public Function FetchPrecedentType() As DataSet
        'Dim Query As String = "select DISTINCT Type from Precedents order by Type"
            Dim Query As String = "select Type from reference_PRECEDENTS"
        'Dim DS As DataSet = FetchDataSet(Query, "Precedents")
        'sp_getPrecedentTypes 

        Dim ds As New DataSet()
            'Dim myParameter As SqlParameter
        Try
            Dim conn As New SqlConnection(ConnectionString)

            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            cmd.CommandText = "sp_getPrecedentTypes"
            cmd.CommandType = CommandType.StoredProcedure
            'myParameter = cmd.CreateParameter()
            'myParameter.ParameterName = "@ExactPhrase"
            'myParameter.Direction = ParameterDirection.Input
            'myParameter.SqlDbType = SqlDbType.VarChar '.SmallDateTime

            '   myParameter.Value = ExactPhrase.ToString '.ToShortDateString
            'cmd.Parameters.Add(myParameter)

            conn.Open()

            cmd.ExecuteNonQuery() '.ExecuteReader() 

            Dim adapter As New SqlDataAdapter(cmd)

                adapter.Fill(ds, "reference_PRACTICENOTES")
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally

        End Try

        Return ds

    End Function

    'Public Function getFormTypeResult(ByVal Type As String, ByVal SELCOUNTRY As String) As DataSet
    Public Overloads Function getForms(ByVal formNumber As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        formNumber = formNumber '& "'%'"
        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getFormDetails"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@FRMNUMBER"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = formNumber.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)


            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Function getForms1(ByVal formTitle As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getFormDetailsByTitle"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@FRMNUMBER"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = formTitle.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)


            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function



    Public Overloads Function getCasesByCaseNumber(ByVal Title As String, ByVal SeltdCountry As String, ByVal flg As Byte) As DataSet

        Dim ds As New DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_getCasesByCaseNumber"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = Title
            cmd.Parameters.Add(New SqlParameter("@COUNTRY", SqlDbType.VarChar)).Value = SeltdCountry
            cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.Int)).Value = flg

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function


    Public Function getCasesByHeadNotes(ByVal Title As String, ByVal SeltdCountry As String, ByVal flg As Byte) As DataSet

        Dim ds As New DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_getCasesByHeadNotes"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = Title
            cmd.Parameters.Add(New SqlParameter("@COUNTRY", SqlDbType.VarChar)).Value = SeltdCountry
            cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.Int)).Value = flg

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            '            conn.Dispose()
            '            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Function getCasesByTitle(ByVal Title As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getCasesByTitle"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@SEARCHTITLE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = Title.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Function getCasesByFullText(ByVal Title As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getCasesByFullText"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@SEARCHTITLE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = Title.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getLegislationByAct(ByVal Title As String, ByVal Act As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationByAct"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@SEARCHTITLE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = Title.ToString
            cmd.Parameters.Add(myParameter)


            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@SEARCHACT"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = Act.ToString
            cmd.Parameters.Add(myParameter)


            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getLegislationByTitle(ByVal Title As String, ByVal SeltdCountry As String, ByVal flg As Byte) As DataSet

        Dim ds As New DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationByTitle"

            cmd.CommandType = CommandType.StoredProcedure
            'myParameter = cmd.CreateParameter()
            'myParameter.ParameterName = "@SEARCHTITLE"
            'myParameter.Direction = ParameterDirection.Input
            'myParameter.SqlDbType = SqlDbType.VarChar
            'myParameter.Value = Title.ToString
            'cmd.Parameters.Add(myParameter)

            'myParameter = cmd.CreateParameter()
            'myParameter.ParameterName = "@COUNTRY"
            'myParameter.Direction = ParameterDirection.Input
            'myParameter.SqlDbType = SqlDbType.VarChar

            'myParameter.Value = SeltdCountry.ToString
            'cmd.Parameters.Add(myParameter)

            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = Title
            cmd.Parameters.Add(New SqlParameter("@COUNTRY", SqlDbType.VarChar)).Value = SeltdCountry
            cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.Int)).Value = flg

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            'conn.Dispose()
            'cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getLegislationByActNumber(ByVal Title As String, ByVal SeltdCountry As String, ByVal flg As Integer) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationByActNumber"

            cmd.CommandType = CommandType.StoredProcedure

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@SEARCHTITLE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = Title.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@FLAG"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.Int
            myParameter.Value = flg
            cmd.Parameters.Add(myParameter)

            'cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = Title
            'cmd.Parameters.Add(New SqlParameter("@COUNTRY", SqlDbType.VarChar)).Value = SeltdCountry
            'cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.TinyInt)).Value = flg

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            'conn.Dispose()
            'cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getLegislation_Subject_Index() As DataSet

        Dim ds As New DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationSubjectIndex"
            cmd.CommandType = CommandType.StoredProcedure
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getLawDictionaryMeaning(ByVal flg As Byte, ByVal SelectedWord As String) As String
        ' flg is only used because of overloaded function, both function differs in return type and another
        ' function was used mainly by retrieving data by stored procedure
            Dim Query As String = "select Description from Reference_Dictionary where Word = '" & SelectedWord & "' "
        Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim Result As String = " "

        Try
            Dim cmd As New SqlCommand(Query, conn)
            conn.Open()
            Reader = cmd.ExecuteReader
            Reader.Read()
            Result = Reader("Description")
        Catch err As DataException
            'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
 
                Reader.Close()


        End Try

        Return Result
        End Function
        Public Overloads Function TranslateWord(ByVal flg As String, ByVal SelectedWord As String) As String
            Dim Translate_Word As String = ""
            If flg = "English" Then
                Translate_Word = "Malay"
            ElseIf flg = "Malay" Then
                Translate_Word = "English"
            End If
            Dim Query As String = "select " & Translate_Word & " from reference_translation where " & flg & " = '" & SelectedWord & "' "
                Dim conn As New SqlConnection(ConnectionString)
                Dim Reader As SqlDataReader
                Dim Result As String = " "
            Try
                Dim cmd As New SqlCommand(Query, conn)
                conn.Open()
                Reader = cmd.ExecuteReader
                Reader.Read()
                Result = Reader(Translate_Word)
            Catch err As DataException
                'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
            End Try
            Return Result
        End Function

    Public Overloads Function getLawDictionaryMeaning(ByVal SelectedSubject As String) As DataSet

        Dim ds As New DataSet()
            ' Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLawDictionarybyMeaning"

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = SelectedSubject
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getGlossaryDefinations(ByVal Terms As String) As DataTable

        Dim DT As New DataTable()
            ' Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getGlossaryDefinations"

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = Terms
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return DT

    End Function


        Public Function getLawDictionarySortBy(ByVal SelectedSubject As String) As DataTable

            Dim ds As New DataTable
            ' Dim myParameter As SqlParameter
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand()

            Try

                cmd.Connection = conn
                cmd.CommandText = "sp_getLawDictionaryBySort"

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = SelectedSubject
                conn.Open()
                cmd.ExecuteNonQuery()

                Dim adapter As New SqlDataAdapter(cmd)

                adapter.Fill(ds)
                conn.Close()

            Catch err As DataException
                Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            Finally
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
            End Try

            Return ds

        End Function


    Public Function getGlossarySortBy(ByVal SelectedChr As String) As DataTable 'DataSet

        'Dim ds As New DataSet()
        Dim DT As New DataTable()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getGlossaryBySort"

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = SelectedChr
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            'adapter.Fill(ds)
            adapter.Fill(DT)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return DT

    End Function

    Public Function getLegislation_Subject_Index_Result(ByVal SelectedSubject As String) As DataSet

        Dim ds As New DataSet()
            ' Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationSubjectIndexResult"

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = SelectedSubject
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getLegislationBySubject(ByVal Title As String, ByVal SeltdCountry As String, ByVal flg As Byte) As DataSet

        Dim ds As New DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationBySubject"

            cmd.CommandType = CommandType.StoredProcedure
            'myParameter = cmd.CreateParameter()
            'myParameter.ParameterName = "@SEARCHTITLE"
            'myParameter.Direction = ParameterDirection.Input
            'myParameter.SqlDbType = SqlDbType.VarChar
            'myParameter.Value = Title.ToString
            'cmd.Parameters.Add(myParameter)

            'myParameter = cmd.CreateParameter()
            'myParameter.ParameterName = "@COUNTRY"
            'myParameter.Direction = ParameterDirection.Input
            'myParameter.SqlDbType = SqlDbType.VarChar
            'myParameter.Value = SeltdCountry.ToString
            'cmd.Parameters.Add(myParameter)

            cmd.Parameters.Add(New SqlParameter("@SEARCHTITLE", SqlDbType.VarChar)).Value = Title
            cmd.Parameters.Add(New SqlParameter("@COUNTRY", SqlDbType.VarChar)).Value = SeltdCountry
            cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.Int)).Value = flg

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function


    Public Overloads Function getLegislationResultCount(ByVal Title As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getLegislationByTitleCount"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@SEARCHTITLE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = Title.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)

            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function

    Public Overloads Function getForms(ByVal formNumber As String, ByVal formTitle As String, ByVal SeltdCountry As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        formNumber = formNumber '& "'%'"
        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getFormDetails"

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@FRMNUMBER"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = formNumber.ToString
            cmd.Parameters.Add(myParameter)

            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@FRMTITLE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar
            myParameter.Value = formNumber.ToString
            cmd.Parameters.Add(myParameter)

            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SeltdCountry.ToString
            cmd.Parameters.Add(myParameter)


            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds, "legalform")
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function



    Public Function getPrecedentTypeResult(ByVal Type As String, ByVal SELCOUNTRY As String) As DataSet

        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getprecedentsearchresult"
            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@TYPE"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = Type.ToString

            cmd.Parameters.Add(myParameter)
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@COUNTRY"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar

            myParameter.Value = SELCOUNTRY.ToString
            cmd.Parameters.Add(myParameter)


            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()



        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return ds

    End Function



    Public Function FindCase(ByVal ExactPhrase As String) As DataSet
        'this is for chking 
        'Dim Query As String = "select DataFileName,Country,CaseNumber,JudgementDate from caseslaw where judge='Abdul Hamid J.' "
            Dim Query As String = "select DataFileName,Country,CaseNumber,JudgementDate from cases where judge= '" & ExactPhrase & "' "
        'Dim DS As DataSet = FetchDataSet(Query, "caseslaw")
        Dim DS As DataSet = FetchDataSetWithSP(ExactPhrase)
        Return DS
    End Function


    Private Function FetchDataSetWithSP(ByVal ExactPhrase As String) As DataSet
        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Try
            Dim conn As New SqlConnection(ConnectionString)

            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            cmd.CommandText = "sp_getCases"
            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@ExactPhrase"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.VarChar '.SmallDateTime

            myParameter.Value = ExactPhrase.ToString '.ToShortDateString
            cmd.Parameters.Add(myParameter)


            conn.Open()

            cmd.ExecuteNonQuery() '.ExecuteReader() 

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally

        End Try

        Return ds
    End Function


    Public Overloads Function getUserDetails() As DataSet
        ' i have to change this one because i m not using it right now

        'Dim Query As String = "select * from tblUserInfo"
        '        Dim DS As DataSet = FetchDataSet()

            Dim Query As String = "select First_Name,Firm_Name,User_Name,Registration_Date,Acct_Expiry,Package_Type from master_users where Acct_Type<>'Free' "
            Dim DS As DataSet = FetchDataSet(Query, "master_users")
            Return DS
        End Function

        'Public Overloads Function getUserDetails(ByVal Query As String) As DataSet
        '    Dim DS As DataSet

        'End Function


        Public Function getUsersInvoices(ByVal invoiceDate As String) As DataSet
            

            Dim DS As DataSet = FetchDataSet1(invoiceDate)
            'Dim DS As DataSet = FetchDataSet1("4/14/2003")


            Return DS
        End Function

        Public Overloads Function getArticle(ByVal article As String) As DataSet
            Dim Query As String = "select DatafileName,Head,Author from Articles where HEAD like '" & "%" & article & "%" & "'  "
            Dim DS As DataSet = FetchDataSet(Query, "ARTICLE")
            Return DS
        End Function

        Public Overloads Function getMembersDetails() As DataSet
            Dim Query As String = "select First_Name,Firm_Name,User_Name,Registration_Date,Acct_Expiry,Package_Type from master_users where Acct_Type<>'Free' "
            Dim DS As DataSet = FetchDataSet(Query, "master_users")
            Return DS
        End Function

        Public Overloads Function getMembersDetails(ByVal sortByChar As String) As DataSet
            sortByChar = " First_Name LIKE  '" & sortByChar & "%" & "' "
            Dim Query As String = "select First_Name,Firm_Name,User_Name,Registration_Date,Acct_Expiry,Package_Type from master_users where Acct_Type<>'Free' and " & sortByChar & " "
            Dim DS As DataSet = FetchDataSet(Query, "master_users")
            Return DS
        End Function

        Public Function getLastBillNo() As DataSet
            ' buggy
            Dim Query As String = "select Bil from master1 where bil=max(bil) "
            'Dim Query As String = "select Bil from master_users where bil=max(bil) "
            Dim DS As DataSet = FetchDataSet(Query, "master1")
            'Dim DS As DataSet = FetchDataSet(Query, "master1")
            Return DS
        End Function

    Public Function RecordFound(ByVal Query As String) As Boolean
        '/* Note
        ' This function is for searching record in table. specially if there 
        'is some repeatetion in the table so we can eliminate redundancy while
        'inserting record in table 
        ' */
        Dim Reader1 As Data.SqlClient.SqlDataReader
        Dim Conn As New SqlClient.SqlConnection(ConnectionString)
        Dim Cmd As New SqlClient.SqlCommand(Query, Conn)


        'Dim Adapter As New SqlDataAdapter(Cmd)
        'Dim DS As New Data.DataSet()

        Dim RecordFoundCount As Int16
        RecordFoundCount = 0
        Try
            Conn.Open()
            Reader1 = Cmd.ExecuteReader '.ExecuteScalar '.ExecuteReader
            '   Adapter.Fill(DS)
            Do While Reader1.Read
                RecordFoundCount = RecordFoundCount + 1

            Loop
            Reader1.Close()
            If RecordFoundCount >= 1 Then
                RecordFound = True
            Else
                RecordFound = False

            End If
        Catch Err As Exception
            Dim lblErr As New Label()
            lblErr.Text = "This is the Err Raised.." & Err.Message
            Throw New Exception("Err on searching..")

        Finally

            Conn.Close()

            'Conn.Dispose()
            'Cmd.Dispose()
            RecordFoundCount = 0
        End Try

        Return RecordFound
    End Function

    Public Overloads Function AddRecord(ByVal UserID As Long, ByVal Username As String) As Boolean
        'Dim Query As String = "insert into tblUserPrimary "
        AddRecord = False
        Dim Query As String
        Dim pwd As String = "test"
        Query = "insert into  tblUserPrimary (UserNameID,UserName,Password) values ( " & UserID & " , '" & Username & "','" & pwd & "' )"
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        Try
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = Query
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            AddRecord = True
        Catch err As Exception
            Throw New Exception("Error in inserting to DB  " & err.Message)
            AddRecord = False

        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return AddRecord

    End Function

    Public Overloads Function AddRecord(ByVal Query As String) As Boolean
        'Dim Query As String = "insert into tblUserPrimary "
        AddRecord = False
        Dim Statement As String
        Statement = Query
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()
        Try
            cmd.Connection = conn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = Query
            conn.Open()
            cmd.ExecuteNonQuery()
            conn.Close()
            AddRecord = True
        Catch err As Exception
            Throw New Exception("Error in inserting to DB  " & err.Message)
            AddRecord = False

        Finally
            conn.Close()
            cmd.Dispose()
        End Try
        Return AddRecord

    End Function



    Private Function FetchDataSet(ByVal Query As String, ByVal tblName As String) As DataSet
        Dim ds As New DataSet()
        Try
            Dim conn As New SqlConnection(ConnectionString)
            Dim cmd As New SqlCommand(Query, conn)
            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds, tblName)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally

        End Try

        Return ds
    End Function


    Private Function FetchDataSet1(ByVal invoiceDate As String) As DataSet
        Dim ds As New DataSet()
        Dim myParameter As SqlParameter
        Dim invoice_date As DateTime

        'Dim datecnv As Date
        invoice_date = Convert.ChangeType(invoiceDate, TypeCode.DateTime)
        'invoiceDate = "'4/14/2003'"

        'invoice_date = invoiceDate   'datecnv

        Try
            Dim conn As New SqlConnection(ConnectionString)

            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            cmd.CommandText = "sp_getUserInvoices"
            cmd.CommandType = CommandType.StoredProcedure
            myParameter = cmd.CreateParameter()
            myParameter.ParameterName = "@invoicedate"
            myParameter.Direction = ParameterDirection.Input
            myParameter.SqlDbType = SqlDbType.SmallDateTime

            myParameter.Value = invoice_date.ToShortDateString
            cmd.Parameters.Add(myParameter)


            conn.Open()

            cmd.ExecuteNonQuery() '.ExecuteReader() 

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(ds)
            conn.Close()

        Catch err As DataException
            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally

        End Try

        Return ds
    End Function

End Class

End Namespace

