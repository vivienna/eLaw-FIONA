
'/******************************************************************************/
'/*	Developer 	    : Updated & Modify by Ali,Abdo          								   */
'/*	Company     	: Elaw Sdn Bhd		    	    				        */
'/* Date Modified	: 2/ 1/ 2014       		        		    		     */  
'/*	Description		: This library has functions related to cases like fetching case citator etc      */
'/*	Version			: 1.0											            */
'/*******************************************************************************/


Imports System.Data.SqlClient
Imports System.Exception
Imports System.Data


Namespace membersarea


Public Class clsCasesSearch
    Inherits clsSearch

    'Private ConnectionString As String

    Public Sub New()

        '   ConnectionString = ConfigurationSettings.AppSettings("ConnectionString")

        If ConnectionString = "" Then
            Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
        End If

    End Sub

    Public Function CaseCitatorFetcher(ByVal DataFileName As String) As DataTable
        Dim DT As New DataTable() ' DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_getCaseCitator"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("@DATAFILENAME", SqlDbType.VarChar)).Value = " '" & DataFileName & "'"
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)
            conn.Close()
        Catch err As Exception
            Dim msg1 As String = err.Message
            '            Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return DT
    End Function

    'sp_CheckActNoterUpFiles
    Public Function isAmendingActOFPrincipalAct(ByVal ActNo As String) As Boolean
        'Dim Query As String
        'Dim count As Boolean
        'Query = "Select "

        Dim Found As Boolean = False
            Dim Query As String = " "
        Dim conn As New SqlConnection(ConnectionString)
            'Dim Reader As SqlDataReader
        Dim DT As New DataTable()
            'Dim recCount As Int32
            Dim Data As String = " "
        Dim cmd = New SqlCommand(Query, conn)
            'Dim msg As String

        Dim myParameter As SqlParameter


        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_CheckSubsidaryActOfMainAct"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("@DATAFILENAME", SqlDbType.VarChar)).Value = "'" & ActNo & "'"
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)

            If IsDBNull(DT.Rows(0).Item(0)) = True Then

                '    If IsDBNull(DT.Rows(0).Item(0)) = True Then
                Data = DT.Rows(0).Item(0)
            End If

            If Data <> "" Then
                Found = True
            End If

        Catch err As Exception
            Dim msg1 As String = err.Message
            'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            myParameter = Nothing
            DT = Nothing
        End Try


        Return Found

    End Function
    Public Function ActNoterUpFetcher(ByVal LegislationFile As String) As DataTable
        ' this is for fetching all cases refered in legislation
        Dim DT As New DataTable() ' DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_getActNoterUpFiles"
            cmd.CommandType = CommandType.StoredProcedure

            'cmd.Parameters.Add(New SqlParameter("@DATAFILENAME", SqlDbType.VarChar)).Value = "'%" & LegislationFile & "[^0-9]%'"
            cmd.Parameters.Add(New SqlParameter("@DATAFILENAME", SqlDbType.VarChar)).Value = "'%" & LegislationFile & "%'"
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)
            conn.Close()
        Catch err As Exception
            Dim msg1 As String = err.Message
            Throw New Exception("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
        End Try

        Return DT
    End Function

    Public Function CaseCitatorFileInfoFetcher(ByVal DataFileName As String) As DataTable
        Dim DT As New DataTable() ' DataSet()
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand()

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_getCaseCitatorFilesInfo"
            cmd.CommandType = CommandType.StoredProcedure
            'sp_getCaseCitatorFilesInfo

            cmd.Parameters.Add(New SqlParameter("@DATAFILENAME", SqlDbType.VarChar)).Value = "'" & DataFileName & "'"
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)
            conn.Close()
        Catch err As Exception
            'Dim msg1 As String = err.Message
            Throw New Exception("Error in CaseCitatorFileInfoFetcher function  " & err.Message)
        Finally
            conn.Close()
            
        End Try

        Return DT
    End Function

        Public Function CheckCaseCitator(ByVal DataFileName As String) As String

            Dim Found As Integer = 0
            Dim Query As String
            Dim RowCounts As Int32
            Dim DT As New DataTable()
            Dim msg1 As String = ""
            Dim SearchWord As String
            Dim ChFoundRefd As Integer
            Dim ChFoundFoll As Integer
            Dim ChFoundDist As Integer
            Dim lst As New List(Of String)
            Try
                Query = "select count(distinct(FileLinkTO))  as CountcitationRefd from refcases where RootCitation = '" & DataFileName & "'  and  TYPE='refd'  " & _
                        "UNION all " & _
                         " select count(distinct(FileLinkTO))  as Countcitationfoll from refcases where  RootCitation = '" & DataFileName & "' and TYPE='foll' " & _
                        " UNION all  " & _
                        " select count(distinct(FileLinkTO))  as Countcitationdist from refcases where RootCitation = '" & DataFileName & "'  and (TYPE='dist' or TYPE='ovrld' or TYPE='not foll') "
                DT = Me.ExecuteMyQuery(Query)
                RowCounts = DT.Rows.Count
                If RowCounts > 0 Then
                    ChFoundRefd = CInt(DT.Rows(0).Item(0))
                    ChFoundFoll = CInt(DT.Rows(1).Item(0))
                    ChFoundDist = CInt(DT.Rows(2).Item(0))
                    If ChFoundFoll > 0 Then
                        lst.Add("<span style='color:#00b300'>(" & ChFoundFoll & ")</span>")
                    End If
                    If ChFoundRefd > 0 Then
                        lst.Add("<span  style='color:#f77c00'>(" & ChFoundRefd & ")</span>")
                    End If

                    If ChFoundDist > 0 Then
                        lst.Add("<span style='color:#ff0040'>(" & ChFoundDist & ")</span>")
                    End If
                    msg1 = String.Join(",", lst.ToArray)
                    Found = RowCounts
                Else
                    Found = 0
                    msg1 = ""
                End If

            Catch err As Exception
                '  msg1 = err.Message
                Throw New Exception("Error   " & err.Message)

            Finally
                lst.Clear()
                RowCounts = 0
                ChFoundRefd = 0
                ChFoundFoll = 0
                ChFoundDist = 0
                DT = Nothing
                SearchWord = ""
                Query = ""
            End Try
            Return msg1
        End Function
           Public Function CheckLegislationCitator(ByVal DataFileName As String) As Integer
            ' // Note:
            ' //This function is for checking if the current case has case citator or not

            Dim Found As Boolean = False
            Dim Query As String
            Dim RowCounts As Int32
            Dim DT As New DataTable()
            Dim Count As Integer

            Try
                Query = "SELECT distinct(ReferredCitaion) as LegCite FROM REF_LEG_TB WHERE root_citation = '" & DataFileName & "'"
                DT = Me.ExecuteMyQuery(Query)
                RowCounts = DT.Rows.Count
                If RowCounts > 0 Then
                    Found = True
                    Count = RowCounts
                Else
                    Found = False
                    Count = 0
                End If

            Catch err As Exception
                Return Count = 0
                Throw New Exception("Error   " & err.Message)

            Finally
                RowCounts = 0
                DT = Nothing
                Query = ""
            End Try
            Return Count
        End Function
    Public Function CheckSubsidiaryActForMainLegislation(ByVal ActNo As String) As Boolean
        'sp_CheckCaseCitator

        Dim Found As Boolean = False

        Dim conn As New SqlConnection(ConnectionString)
            'Dim Reader As SqlDataReader
        Dim DT As New DataTable()
            'Dim recCount As Int32
        Dim Data As Int16
        Dim cmd = New SqlCommand()
            'Dim msg As String
        'Query = "%headnoteresult.asp?" & DataFileName & ";%"

        Dim myParameter As SqlParameter

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_CheckSubsidiaryAct"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("@ACTNO", SqlDbType.VarChar)).Value = " " & ActNo & " "  'ActNo
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)
            If IsDBNull(DT.Rows(0).Item(0)) = False Then
                Data = CInt(DT.Rows(0).Item(0))
            End If

            'If Data <> "" Then
            If Data >= 1 Then
                Found = True
            Else
                Found = False
            End If

        Catch err As Exception
            Dim msg1 As String = err.Message
            'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            myParameter = Nothing
            DT = Nothing
        End Try


        Return Found

    End Function
    ''###################################################################################
    '
    '///Description:
    '///      This method deprecated the previous versions and it won't returns false result for 
    '///    multiple countries. False result is in return from the same act no of different coutries.
    '
    ''###################################################################################

        Public Function CheckSubsidiaryActForMainLegislation1(ByVal ActNo As String, ByVal SelCountry As String) As Integer
            Dim Found As Boolean
            Dim Query As String

            Query = "select count(*) datafilename from  Legislation where  (datafilename "
            Query &= " LIKE '%MY_PUBS%' or datafilename "
            Query &= " LIKE '%MY_PUAS%') and PRINCIPALACTNO  = 'ACT 000' and Country='MALAYSIA'"
            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim recCount As Int32
            Dim cmd = New SqlCommand(Query, conn)
            Dim msg As String

            Try
                conn.Open()
                Reader = cmd.ExecuteReader()
                While Reader.Read()
                    recCount = Reader(0)
                End While
                If recCount > 0 Then
                    Found = True
                Else
                    Found = False
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                conn.Close()
                conn = Nothing
                Reader = Nothing
                cmd = Nothing
            End Try

            Return recCount

        End Function

    ''###################################################################################
    '
    '///Description:
    '///      This method is now deprecated as it returns false result for multiple countries. As there is
    '///  a good possiblity of similar act no. in multiple countries legistation. 
    '
    ''###################################################################################
    Public Function CheckSubsidiaryActForMainLegislation1(ByVal ActNo As String) As Boolean
        Dim Found As Boolean
        Dim Query As String


        'Query = "select count(*) datafilename from  Legislation where LEGISLATIONTYPE NOT LIKE '%FS_EXP%' AND LEGISLATIONTYPE LIKE '%FS_PUA%' and PRINCIPALACTNO  LIKE '" & ActNo & "' "
        Query = "select count(*) datafilename from  Legislation where LEGISLATIONTYPE NOT LIKE '%FS_EXP%' AND LEGISLATIONTYPE LIKE '%FS_PUA%' and PRINCIPALACTNO  = '" & ActNo & "' "

        Dim conn As New SqlConnection(ConnectionString)
        Dim Reader As SqlDataReader
        Dim recCount As Int32
        Dim cmd = New SqlCommand(Query, conn)
        Dim msg As String

        Try
            conn.Open()
            Reader = cmd.ExecuteReader()
            While Reader.Read()
                recCount = Reader(0)
            End While
            If recCount > 0 Then
                Found = True
            Else
                Found = False
            End If

        Catch err As Exception
            msg = err.Message

        Finally
            conn.Close()
            conn = Nothing
            Reader = Nothing
            cmd = Nothing
        End Try

        Return Found

    End Function

    ''###################################################################################
    '
    '///Description:
    '///      This method deprecated the previous versions and it won't returns false result for 
    '///    multiple countries.  
    '
    ''###################################################################################

        Public Function CheckAmendingActForMainLegislation1(ByVal ActNo As String, ByVal SelCountry As String) As Integer
            Dim Found As Boolean
            Dim Query As String

            Query = "select count(*) datafilename from  Legislation where  datafilename LIKE '%MY_AMEN%' and PRINCIPALACTNO  LIKE '" & ActNo & "' and country='" & SelCountry & "'"
            Dim conn As New SqlConnection(ConnectionString)
            Dim Reader As SqlDataReader
            Dim recCount As Int32
            Dim cmd = New SqlCommand(Query, conn)
            Dim msg As String

            Try
                conn.Open()
                Reader = cmd.ExecuteReader()
                While Reader.Read()
                    recCount = Reader(0)
                End While
                If recCount > 0 Then
                    Found = True
                Else
                    Found = False
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                conn.Close()
                conn = Nothing
                Reader = Nothing
                cmd = Nothing
            End Try

            Return recCount

        End Function
    ''###################################################################################
    '
    '///Description:
    '///      This method is now deprecated as it returns false result for multiple countries. As there is
    '///  a good possiblity of similar act no. in multiple countries legistation. 
    '
    ''###################################################################################
    Public Function CheckAmendingActForMainLegislation1(ByVal ActNo As String) As Boolean
        Dim Found As Boolean
        Dim Query As String

        Query = "select count(*) datafilename from  Legislation where LEGISLATIONTYPE NOT LIKE '%FS_EXP%' AND LEGISLATIONTYPE LIKE '%FS_AME%' and PRINCIPALACTNO  LIKE '" & ActNo & "' "
        Dim conn As New SqlConnection(ConnectionString)
        Dim Reader As SqlDataReader
        Dim recCount As Int32
        Dim cmd = New SqlCommand(Query, conn)
        Dim msg As String

        Try
            conn.Open()
            Reader = cmd.ExecuteReader()
            While Reader.Read()
                recCount = Reader(0)
            End While
            If recCount > 0 Then
                Found = True
            Else
                Found = False
            End If

        Catch err As Exception
            msg = err.Message

        Finally
            conn.Close()
            conn = Nothing
            Reader = Nothing
            cmd = Nothing
        End Try

        Return Found

    End Function

    Public Function CheckAmendingActForMainLegislation(ByVal ActNo As String) As Boolean
        'sp_CheckCaseCitator

        Dim Found As Boolean = False

        Dim conn As New SqlConnection(ConnectionString)
            'Dim Reader As SqlDataReader
        Dim DT As New DataTable
            'Dim recCount As Int32
        Dim Data As Double
        Dim cmd = New SqlCommand
            ' Dim msg As String

        Dim Query As String = "'" & ActNo & "'"

        Dim myParameter As SqlParameter

        Try
            cmd.Connection = conn
            cmd.CommandText = "sp_CheckAmendingAct"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("@ACTNO", SqlDbType.VarChar)).Value = Query
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim adapter As New SqlDataAdapter(cmd)

            adapter.Fill(DT)
            'If IsDBNull(DT.Rows(0).Item(0)) = True Then
            'If DT.Rows(0).Item(0) <> "" Then
            Data = CDbl(DT.Rows(0).Item(0))
            'End If

            'If Data <> "" Then
            If Data >= 1 Then
                Found = True
            Else
                Found = False
            End If

        Catch err As Exception
            Dim msg1 As String = err.Message

            'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
        Finally
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            conn = Nothing
            myParameter = Nothing
            DT = Nothing
        End Try


        Return Found

    End Function


        Public Function CheckActNoterUp(ByVal DataFileName As String) As Boolean
            ' // Note:
            '// This is for checking if the current section of legislation has Act Noterup or not.

            Dim Found As Boolean = False
            Dim Query As String
            Dim DT As New DataTable
            Dim RowCounts As Int32
            Dim SearchWord As String
            Dim msg1 As String
            For i = 0 To 3
                Query = "SELECT c_id FROM ref_leg_tb where [ReferredCitaion]= '" & DataFileName & "'"
                DT = Me.ExecuteMyQuery(Query)
                RowCounts = DT.Rows.Count
                If RowCounts > 0 Then
                    Return True
                Else
                    If i = 1 Then
                        DataFileName = DataFileName & "."
                    ElseIf i = 2 Then
                        DataFileName = DataFileName & ";"
                    End If
                End If
            Next
            Try
                If RowCounts > 0 Then
                    Found = True
                Else
                    Found = False
                End If

            Catch err As Exception

                'Throw New Exception("Error   " & err.Message)
            Finally
                msg1 = ""
                SearchWord = ""
                Query = ""
                RowCounts = 0
                DT = Nothing
            End Try
            Return Found
        End Function

    Public Function getCasesInfo(ByVal DataFileName As String) As DataTable
        
        Dim Query As String
        Dim DT As New DataTable
            Dim SearchWord As String
            If DataFileName.Contains(".xml") = False Then
                DataFileName = DataFileName & ".xml"
            End If
            SearchWord = DataFileName & "'"
            Query = "Select TITLE,CASENUMBER,COUNTRY,JUDGE,COURT,JUDGEMENTDATE,substring(HEADNOTES,1,75), CITATION from CasesIndustrialCourt where datafilename ='" & SearchWord

        Try
            DT = Me.ExecuteMyQuery(Query)

        Catch err As Exception

            Throw New Exception("Error   " & err.Message)
        Finally
            SearchWord = ""
            Query = ""
        End Try
        Return DT
        End Function
        Public Function getLegislationInfo(ByVal DataFileName As String, ByVal SecNo As String) As DataTable

            Dim Query As String
            Dim DT As New DataTable
            Query = "Select sec_title from secion_tb1 where LegFile ='" & DataFileName & "' and replace( substring(sec_no, PatIndex('%[0-9]%', sec_no), len(sec_no)),'.','')=rtrim(ltrim(REPLACE('" & SecNo & "','.',''))) "
            Try
                DT = Me.ExecuteMyQuery(Query)
            Catch err As Exception

                Throw New Exception("Error   " & err.Message)
            Finally

            End Try
            Return DT
        End Function

    Public Overloads Function getActNoterUpCases(ByVal DataFileName As String, ByVal NewSearch As String, ByVal SortBy As String) As DataTable
        ' // Note:
        '// This function will return cases search for casecitator and act noterup.


        Dim Query As String

        Dim DT As New DataTable
        Dim SearchWord As String


        'SearchWord = "'%MY_FS_ACT_1967_177;26%'" & "[^0-9]%'"
        'SearchWord = "'%MY_FS_ACT_1967_177;26%'"
        SearchWord = "'%" & DataFileName & "%'"

            Query = "SELECT datafilename FROM cases_links WHERE ATTRIBUTEVALUE like " & SearchWord & " " & NewSearch & " " & SortBy
        Try
            DT = Me.ExecuteMyQuery(Query)

        Catch err As Exception

            Throw New Exception("Error   " & err.Message)
        Finally
            SearchWord = ""
            Query = ""
        End Try
        Return DT
    End Function

        Public Overloads Function getActNoterUpCases(ByVal DataFileName As String, ByVal SortBy As String) As DataTable

            Dim Query As String
            Dim DT As New DataTable
            Dim SearchWord As String
            'Query = "SELECT distinct root_citation FROM ref_leg_tb WHERE ReferredCitaion like '%" & DataFileName & "%'"
            If DataFileName.ToUpper.Contains("MY") Then
                For i = 0 To 3
                    Query = "SELECT root_citation FROM ref_leg_tb where [ReferredCitaion] like '%" & DataFileName & "%' group by root_citation,ReferredCitaion"
                    DT = Me.ExecuteMyQuery(Query)
                    If DT.Rows.Count > 0 Then
                        Return DT
                    Else
                        If i = 1 Then
                            DataFileName = DataFileName & "."
                        ElseIf i = 2 Then
                            DataFileName = DataFileName & ";"
                        End If
                    End If
                Next

            Else
                Query = "SELECT  distinct(ReferredCitaion),RefferredTitle FROM ref_leg_tb WHERE root_citation ='" & DataFileName & "'"  ' 
                DT = Me.ExecuteMyQuery(Query)
            End If
            Try
            Catch err As Exception
                Throw New Exception("Error   " & err.Message)
            Finally
                SearchWord = ""
                Query = ""
            End Try
            Return DT
        End Function

    Public Overloads Function getCaseCitatorCases(ByVal DataFileName As String, ByVal SortBy As String) As DataTable
        ' // Note:
        '// This function will return cases citator .


        Dim Query As String
        Dim DT As New DataTable
        Dim SearchWord As String
        SearchWord = "'%" & DataFileName & "%'"
            Query = "SELECT FileLinkTO FROM refcases WHERE RootCitation = '" & DataFileName & "'"
            ' Query = "SELECT datafilename  FROM cases WHERE datafilename not like " & SearchWord & " AND  datafilename IN (  SELECT datafilename FROM cases_links WHERE ATTRIBUTEVALUE like " & SearchWord & " )" & " " & SortBy

        Try
            DT = Me.ExecuteMyQuery(Query)

        Catch err As Exception

            Throw New Exception("Error   " & err.Message)
        Finally
            SearchWord = ""
            Query = ""
        End Try
        Return DT
    End Function

    Public Overloads Function getCaseCitatorCases(ByVal DataFileName As String, ByVal NewSearch As String, ByVal SortBy As String) As DataTable
        ' // Note:
        '// This function will return cases citator .


        Dim Query As String
        Dim DT As New DataTable
        Dim SearchWord As String
            SearchWord = "'%" & DataFileName.TrimEnd & "%'"

        'Query = "SELECT datafilename FROM caselinks WHERE ATTRIBUTEVALUE like " & SearchWord

            Query = "SELECT datafilename  FROM cases WHERE datafilename not like " & SearchWord & " AND  datafilename IN (  SELECT datafilename FROM cases_links WHERE ATTRIBUTEVALUE like " & SearchWord & " ) " & NewSearch & " " & SortBy

        Try
            DT = Me.ExecuteMyQuery(Query)

        Catch err As Exception

            Throw New Exception("Error   " & err.Message)
        Finally
            SearchWord = ""
            Query = ""
        End Try
        Return DT
    End Function
        Public Overloads Function getCaseCitatorRef(ByVal DataFileName As String, ByVal SortBy As String, ByVal citatorType As Integer) As DataTable
            ' // Note:
            '// This function will return cases citator .
            Dim Query As String
            Dim DT As New DataTable
            If citatorType = 0 Then
                Query = "SELECT FileLinkTO FROM refcases WHERE RootCitation = '" & DataFileName & "'"
            ElseIf citatorType = 1 Then
                Query = "SELECT  RootCitation FROM refcases WHERE FileLinkTO = '" & DataFileName & "'"
            End If

            Try
                DT = Me.ExecuteMyQuery(Query)
            Catch err As Exception
                Throw New Exception("Error   " & err.Message)
            Finally
                Query = ""
            End Try
            Return DT
        End Function
    Public Function isActNoterUp(ByVal LegislationSection As String) As Boolean
        Dim Query As String
        Dim Found As Boolean

        'Query = "select count(*) from caselinks where ATTRIBUTEVALUE LIKE '%MY_FS_ACT_1999_593;3[^0-9]%'"
            Query = "select count(*) from cases_links where ATTRIBUTEVALUE LIKE '%" & LegislationSection & "[^0-9]%'"

        Dim conn As New SqlConnection(ConnectionString)
        Dim Reader As SqlDataReader
        Dim recCount As Int32
        Dim cmd = New SqlCommand(Query, conn)
        Dim msg As String

        Try
            conn.Open()
            Reader = cmd.ExecuteReader()
            While Reader.Read()
                recCount = Reader(0)
            End While
            If recCount > 0 Then
                Found = True
            Else
                Found = False
            End If

        Catch err As Exception
            msg = err.Message

        Finally
            conn.Close()
            conn = Nothing
            Reader = Nothing
            cmd = Nothing
        End Try

        Return Found
    End Function

    Public Function FileNameExtractor(ByVal filenames As String) As Collections.Hashtable

        Dim XMLFileNames As New Collections.Hashtable
        Dim ResultArr() As String

        Dim DT As New DataTable
        Dim i As Int16
        Dim TMP() As String
        Dim URL As String

        DT = Me.CaseCitatorFetcher(filenames)
        URL = DT.Rows(0).Item(0)

        ResultArr = Split(URL, "#")
        For i = 0 To ResultArr.Length - 1
            If Left(ResultArr(i), 18) = "headnoteresult.asp" Then
                TMP = Split(ResultArr(i), "?")
                TMP(1) = Replace(TMP(1), ";", ".xml")
                XMLFileNames.Add(i, TMP(1))

            End If

        Next

        DT = Nothing
        URL = ""
        TMP = Nothing
        i = 0
        ResultArr = Nothing
        Return XMLFileNames
    End Function

    Public Function CheckCaseExistInFileName(ByVal filenames As String) As Boolean

        Dim XMLFileNames As New Collections.Hashtable
        Dim DT As New DataTable
        Dim Count As Int16
        Dim FileSearch As String
        Dim URL As String
        Dim Exist As Boolean = False

        FileSearch = "headnoteresult.asp"
        ''Testing -- 24 aug 2004
        '        filenames = FileSearch & filenames

        DT = Me.CaseCitatorFetcher(filenames)
        URL = DT.Rows(0).Item(0)
        Count = InStr(URL, FileSearch)

        If Count > 0 Then
            Exist = True
        End If


        DT = Nothing
        URL = ""
        Count = 0

        Return Exist
    End Function


    Public Function CheckCaseHeadNotes(ByVal DataFileName As String) As Boolean
        Dim Found As Boolean
        Dim Query As String

            Query = "select count(*) from cases where datafilename ='" & DataFileName & "'  "
        Dim conn As New SqlConnection(ConnectionString)
        Dim Reader As SqlDataReader
        Dim recCount As Int32
        Dim cmd = New SqlCommand(Query, conn)
        Dim msg As String

        Try
            conn.Open()
            Reader = cmd.ExecuteReader()
            While Reader.Read()
                recCount = Reader(0)
            End While
            If recCount > 0 Then
                Found = True
            End If
        Catch err As Exception
            msg = err.Message

        Finally
            conn.Close()
            conn = Nothing
            Reader = Nothing
            cmd = Nothing
        End Try

        Return Found

    End Function

    Public Overloads Function AddRecord(ByVal Query As String) As Boolean
        'Dim Query As String = "insert into tblUserPrimary "
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

    'Public Shared Function ExecuteMyQuery(ByVal Query As String, ByVal tblName As String) As DataSet
    Public Function ExecuteMyQuery(ByVal Query As String, ByVal tblName As String) As DataSet
        Dim DS As New DataSet
        Dim conn As New SqlConnection(ConnectionString)
        Try

            Dim cmd As New SqlCommand(Query, conn)
            Dim adapter As New SqlDataAdapter(cmd)


            adapter.Fill(DS, tblName)
        Catch err As DataException
            'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            myErrorHandler(err)
        Finally
            conn.Close()
        End Try

        Return DS
    End Function

    'Public Shared Function ExecuteMyQuery(ByVal Query As String) As DataTable
    Public Function ExecuteMyQuery(ByVal Query As String) As DataTable
        Dim DT As New DataTable
        Dim conn As New SqlConnection(ConnectionString)
        Dim MSG As String

        Try

            Dim cmd As New SqlCommand(Query, conn)
            Dim adapter As New SqlDataAdapter(cmd)


            adapter.Fill(DT)
        Catch err As Exception
            'Throw New ApplicationException("Error in connecting to DB  " & err.Message)
            MSG = err.Message
        Finally
            conn.Close()
        End Try
        conn = Nothing
        Return DT
    End Function

    'Public Shared Function ExecuteMyQuery(ByVal Query As String, ByVal tblName As String, ByVal CurRec As Int16) As DataSet
    Public Function ExecuteMyQuery(ByVal Query As String, ByVal tblName As String, ByVal CurRec As Int16) As DataTable
        Dim DS As New DataSet
        Dim DT As New DataTable
        Dim conn As New SqlConnection(ConnectionString)
        Dim MSG As String
        Try

            'Dim cmd As New SqlCommand(Query, conn)
            'Dim adapter As New SqlDataAdapter(cmd)
            Dim adapter As New SqlDataAdapter(Query, conn)


            adapter.Fill(DS, CurRec, 10, tblName)
        Catch err As Exception
                'Throw New ApplicationException("Error in connecting to DB  " & err.Message)

            MSG = err.Message
        Finally
            conn.Close()
        End Try
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0) '.Add()
            End If
            '//Destructing the values
            DS = Nothing
            MSG = ""
            conn = Nothing ' have to test if this will effect other function
            Return DT
        End Function

    'Public Shared Function ExecuteMyReader(ByVal Query As String) As Int32
    Public Function ExecuteMyReader(ByVal Query As String) As Int32
        ''/*  This is for counting records fetched   */
        '' err in this function

            '        Dim Query As String = "select max(user_id_no) from master_users"
        Dim conn As New SqlConnection(ConnectionString)
        Dim Reader As SqlDataReader
        Dim recCount As Int32
        Dim cmd = New SqlCommand(Query, conn)
        Dim msg As String
        Try
            conn.Open()
            Reader = cmd.ExecuteReader()

            While Reader.Read()
                recCount = Reader(0)
            End While

        Catch err As Exception
            msg = err.Message
            'myErrorHandler(err)
        Finally
            conn.Close()
            'conn.Dispose()
            conn = Nothing
            Reader = Nothing
            cmd = Nothing
        End Try

        'ExecuteMyReader = recCount
        Return recCount
    End Function

    'Public Shared Function getCountriesForSearching() As DataSet
    Public Function getCountriesForSearching() As DataSet

        Dim ds As New DataSet
            'Dim myParameter As SqlParameter
        Dim conn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand

        Try

            cmd.Connection = conn
            cmd.CommandText = "sp_getAllCountriesForCasesSearch"
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
            'conn.Dispose()
            'cmd.Dispose()
        End Try

        Return ds

    End Function


    'Public Function RecordFound(ByVal Query As String) As Boolean
    '    '/* Note
    '    ' This function is for searching record in table. specially if there 
    '    'is some repeatetion in the table so we can eliminate redundancy while
    '    'inserting record in table 
    '    ' */
    '    Dim Reader1 As Data.SqlClient.SqlDataReader
    '    Dim Conn As New SqlClient.SqlConnection(ConnectionString)
    '    Dim Cmd As New SqlClient.SqlCommand(Query, Conn)


    '    'Dim Adapter As New SqlDataAdapter(Cmd)
    '    'Dim DS As New Data.DataSet()

    '    Dim RecordFoundCount As Int16
    '    RecordFoundCount = 0
    '    Try
    '        Conn.Open()
    '        Reader1 = Cmd.ExecuteReader '.ExecuteScalar '.ExecuteReader
    '        '   Adapter.Fill(DS)
    '        Do While Reader1.Read
    '            RecordFoundCount = RecordFoundCount + 1

    '        Loop
    '        Reader1.Close()
    '        If RecordFoundCount >= 1 Then
    '            RecordFound = True
    '        Else
    '            RecordFound = False

    '        End If
    '    Catch Err As Exception
    '        Dim lblErr As New Label()
    '        lblErr.Text = "This is the Err Raised.." & Err.Message
    '        Throw New Exception("Err on searching..")

    '    Finally

    '        Conn.Close()

    '        'Conn.Dispose()
    '        'Cmd.Dispose()
    '        RecordFoundCount = 0
    '    End Try

    '    Return RecordFound
    'End Function


    Public Function UpdateRecord(ByVal Query As String) As Boolean
        'Dim Query As String = "insert into tblUserPrimary "
        UpdateRecord = False
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

            UpdateRecord = True
        Catch err As Exception
                Throw New Exception("Error in inserting to DB  " & err.Message)
            UpdateRecord = False

        Finally
            conn.Close()
            cmd = Nothing
        End Try
        Return UpdateRecord

        End Function
        Public Function UpdateRecord2(ByVal Query As String) As Boolean '//////////64
            Dim updated As Boolean = False
            Dim DT As New DataTable
            Dim conn As New SqlConnection(ConnectionString)
            Dim MSG As String
            Try
                Dim cmd As New SqlCommand(Query, conn)
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(DT)
                updated = True
            Catch err As Exception
                MSG = err.Message
                updated = False
            Finally
                conn.Close()
            End Try
            conn = Nothing
            Return updated
        End Function


    Private Shared Sub myErrorHandler(ByVal Err As Exception)
        'Throw New ApplicationException("Error in connecting to DB  " & Err.Message)
        Throw New ApplicationException("Error in connecting to DB  " & Err.StackTrace)
        End Sub
        Public Function ExactPhraseAutoComplete(ByVal query As String, ByVal TableName As String, ByVal flag As Integer) As Boolean
            Dim CheckExtactPhrase As Boolean = False
            Dim SaveQuerySE As String = query
            Dim Exactphrase As String
            Dim objUtil As New clsMyUtility
            Dim conn As New SqlConnection(ConnectionString)
            Dim count As Integer
            Dim CoulumName As String = ""
            Dim InsertQuery As String = ""
            SaveQuerySE = objUtil.RefineSentence(SaveQuerySE) ' Save Extact phrase for autocomplete 
            If flag = 0 Then
                CoulumName = "Exact Phrase"
                InsertQuery = "INSERT INTO " & TableName & " (Qualifier, Phrase, flag) VALUES ('" & CoulumName & "', '" & SaveQuerySE & "', " & flag & ")"
                Exactphrase = "Select count(*) from  " & TableName & " where Phrase like '" & SaveQuerySE & "%' and flag=" & flag & ""
            Else
                CoulumName = "ExtactPhrase"
                InsertQuery = "INSERT INTO " & TableName & " (ExtactPhrase) VALUES ( '" & SaveQuerySE & "')"
                Exactphrase = "Select count(*) from  " & TableName & " where ExtactPhrase like '" & SaveQuerySE & "%'"
            End If
            Dim cmd As New SqlCommand(Exactphrase, conn)
            Try
                conn.Open()
                count = cmd.ExecuteScalar()
            Catch ex As Exception
            Finally
                cmd = Nothing
                conn.Close()
            End Try
            If count = 0 Then
                CheckExtactPhrase = AddRecord(InsertQuery)
            End If
            Return CheckExtactPhrase
        End Function

        Public Function GetDocCount() As Integer
            Dim TotalCount As Integer
            Dim Query As String = "select ( select count(*) from CasesIndustrialCourt ) + ( select count(*) from LEGISLATION )  as total_rows"
            Dim DT As New DataTable
            Try
                DT = ExecuteMyQuery(Query)
                If DT.Rows.Count > 0 Then
                    'return total count 
                    TotalCount = CInt(DT.Rows(0).Item(0))
                End If
            Catch ex As Exception
                'do not return empty result if any error ocure, 
                TotalCount = 56321
            End Try
            Return TotalCount
        End Function
End Class

End Namespace
