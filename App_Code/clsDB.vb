
'/******************************************************************************/
'/*	Developer 	    : Usman Sarwar ,Mohammed Ali       								   */
'/*	Company     	: Elaw Sdn Bhd		    	    				        */
'/* Date Modified	: 16-8-2013       		        		    		     */  
'/*	Description		: This library has functions related to database retrieval      */
'/*	Version			: 1.0											            */
'/*******************************************************************************/

Imports System.Data.SqlClient
Imports System.Exception
Imports System.Data
Imports System.Xml.Linq
Imports System.Xml

Namespace membersarea


    Public Class clsDB
        Private ConnectionString As String
        Private Shared CnString As String ' // this is for shared function
        Public Sub New()

            'ConnectionString = ConfigurationSettings.AppSettings("ConnectionString")
            '<add key="ConnectionString" value="Data Source=CLJ-LOCAL-SVR;initial catalog=mylawboxdb;uid=sa;pwd=divinecljlegal"/>
            ConnectionString = clsConfigs.sGlobalConnectionString

            CnString = ConnectionString
            If ConnectionString = "" Then
                Throw New ApplicationException("Missing ConnectionString variable in web.config file.")
            End If

        End Sub

        'Public Shared Function isFileExist(ByVal FileName As String) As Boolean
        Public Overloads Function isFileExist(ByVal FileName As String) As Boolean
            ''/*  
            'Description: Checking if the case exists
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from cases where datafilename = '" & FileName & "'"
            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isFileExist = False ' file not Exist
                Else
                    isFileExist = True   ' File Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try



            Return isFileExist
        End Function

        Public Overloads Function isArticleFileExist(ByVal FileName As String) As Boolean
            ''/*  
            'Description: Checking if the article exists 
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from Article where datafilename = '" & FileName & "'"
            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isArticleFileExist = False ' file not Exist
                Else
                    isArticleFileExist = True  ' File Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try



            Return isArticleFileExist
        End Function

        Public Overloads Function isPracticeNotesFileExist(ByVal FileName As String) As Boolean
            ''/*  
            'Description: Checking if the practice notes exists
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from reference_PRACTICENOTES where datafilename = '" & FileName & "'"
            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isPracticeNotesFileExist = False ' file not Exist
                Else
                    isPracticeNotesFileExist = True ' File Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try



            Return isPracticeNotesFileExist
        End Function

        Public Function isPrecedentExist(ByVal FileName As String) As Boolean
            ''/*  
            'Description: Checking if the practice notes exists
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from reference_PRACTICENOTES where datafilename = '" & FileName & "'"
            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isPrecedentExist = False ' file not Exist
                Else
                    isPrecedentExist = True ' File Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try



            Return isPrecedentExist
        End Function

        Public Function isTreatiesExist(ByVal FileName As String) As Boolean
            ''/*  
            'Description: Checking if the TREATIES exists
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from TREATIES where datafilename = '" & FileName & "'"
            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isTreatiesExist = False ' file not Exist
                Else
                    isTreatiesExist = True ' File Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try



            Return isTreatiesExist
        End Function

        Public Function isLegislationExist(ByVal FileName As String) As Boolean
            ''/*  
            'Description: Checking if the LEGISLATION exists
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from LEGISLATION where datafilename = '" & FileName & "'"
            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isLegislationExist = False ' file not Exist
                Else
                    isLegislationExist = True ' File Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try

            Return isLegislationExist
        End Function


        Public Overloads Function isFileExist(ByVal FileName As String, ByVal TableName As String) As Boolean
            ''/*  
            'Description: Checking if the case file exist or not
            ' 
            ''*/
            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            Dim ErrMsg As String
            Dim DT As New DataTable()
            Dim msg As String
            Dim Query As String = "select count(*) from " & TableName & " where datafilename = '" & FileName & "'"

            Try
                DT = Me.ExecuteMyQuery(Query)

                FileCount = CInt(DT.Rows(0).Item(0))

                If FileCount = 0 Then
                    isFileExist = False ' file not Exist
                Else
                    isFileExist = True   ' File Exist
                End If
            Catch err As Exception
                msg = err.Message

            Finally
                FileCount = 0
                ErrMsg = ""
                DT = Nothing

                Query = ""
            End Try


            Return isFileExist
        End Function

        Public Shared Function isUserExist(ByVal UserName As String) As Boolean

            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            'Dim ErrMsg As String
            Dim conn As New SqlConnection(CnString)
            Dim Reader As SqlDataReader
            Dim SqlQuery As String = "select count(*) from master_users where User_Name = '" & UserName & "'"
            Dim cmd = New SqlCommand(SqlQuery, conn)
            Dim msg As String

            Try
                conn.Open()
                Reader = cmd.ExecuteReader()
                While Reader.Read()
                    FileCount = Reader(0)
                End While

                If FileCount = 0 Then
                    isUserExist = False ' user not Exist
                Else
                    isUserExist = True  ' user Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                conn.Close()
                conn = Nothing
                Reader = Nothing
                cmd = Nothing
            End Try


            Return isUserExist
        End Function
        Public Shared Function isUserEmailExist(ByVal Email As String, ByVal table As String, ByVal col As String) As Boolean

            Dim FileCount As Int32 ' Because DataFile will always be 1 if it exist in table
            'Dim ErrMsg As String
            Dim conn As New SqlConnection(CnString)
            Dim Reader As SqlDataReader
            Dim SqlQuery As String = "select count(*) from " & table & " where  " & col & "= '" & Email & "'"
            Dim cmd = New SqlCommand(SqlQuery, conn)
            Dim msg As String

            Try
                conn.Open()
                Reader = cmd.ExecuteReader()
                While Reader.Read()
                    FileCount = Reader(0)
                End While

                If FileCount = 0 Then
                    isUserEmailExist = False ' user not Exist
                Else
                    isUserEmailExist = True  ' user Exist
                End If

            Catch err As Exception
                msg = err.Message

            Finally
                conn.Close()
                conn = Nothing
                Reader = Nothing
                cmd = Nothing
            End Try


            Return isUserEmailExist
        End Function
        Public Function getUserId(ByVal UserName As String) As Int64



            ' Dim ErrMsg As String
            Dim conn As New SqlConnection(CnString)
            Dim Reader As SqlDataReader
            Dim user_id_no As Int64
            Dim SqlQuery As String = "select user_id_no from master_users where USER_NAME = '" & UserName & "'"
            Dim cmd = New SqlCommand(SqlQuery, conn)
            Dim msg As String

            Try
                conn.Open()
                Reader = cmd.ExecuteReader()
                While Reader.Read()
                    user_id_no = Reader(0)
                End While


            Catch err As Exception
                msg = err.Message

            Finally
                conn.Close()
                conn = Nothing
                Reader = Nothing
                cmd = Nothing
            End Try


            Return user_id_no
        End Function

        Public Function ExecuteMyQuery(ByVal Query As String) As DataTable
            Dim DT As New DataTable()
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

            Return DT
        End Function

        Public Function IsFreeCase(ByVal ID As String) As Boolean
            Dim flg As Boolean
            Dim XmlDoc As XmlDocument = New XmlDocument

            'XmlDoc.Load(Server.MapPath("~\xmlfiles\cases"))

            For Each Attribute As XmlAttribute In XmlDoc.DocumentElement.Attributes

                Console.WriteLine("{0}: {1}", Attribute.Name, Attribute.Value)

            Next


            Return flg
        End Function
    End Class

End Namespace
