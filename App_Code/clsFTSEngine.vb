Imports Microsoft.VisualBasic
Imports System.Data


Namespace membersarea



    Public Class clsFTSEngine

       

        ' // StrSearchCondition changed to integer bSearchCondition for performance "and == 1 . or == 2"
        ' // bCourtLevel = 0 will represent all courts, if >1 and < 5 means some courts
        ' // bPlatformType = 1 for PC and 2 for Mobile application
        ' // function will "" if after refination nothing left.

        Public Shared Function FTS_Circuits(ByVal sStrSearchExactWords As String, ByVal sStrSearchExactWords1 As String, _
                                      ByVal bSearchCondition As Byte, ByVal sStrSearchWords As String, _
                                      ByVal sStrSearchAnyWords As String, ByVal sStrSearchNotWords As String, _
                                      ByVal bSearchType As Byte, ByVal sSearchNew As String, _
                                      ByVal sSearchJudge As String, ByVal sSearchCaseNumber As String, _
                                      ByVal sSearchCounsel As String, ByVal sSearchTitle As String, _
                                      ByVal bPlatformType As Byte, ByVal bCourtLevel As Byte, _
                                      ByVal bSearchIndustrialCourt As Byte, ByVal sYear1 As String, _
                                      ByVal sYear2 As String, ByVal sSortBy As String) As String

            Dim objUtil As New membersarea.clsMyUtility
            Dim Sql As String = ""
            'Dim StrSearchExactWords As String
            'Dim StrSearchExactWords1 As String
            'Dim strsearchcondition As String
            'Dim StrSearchWords As String
            'Dim StrSearchAnyWords As String
            'Dim StrSearchNotWords As String
            'Dim StrNewSearch As String
            Dim newSearch As String
            Dim SearchTitle As String
            Dim SearchCourt As String
            Dim Judge As String
            Dim CaseNumber As String
            Dim Counsel As String
            'Dim srchCountry As String
            Dim SearchField As String

            'StrSearchExactWords = s_searchExactCases
            'StrSearchExactWords1 = s_searchExactCases_1
            'strsearchcondition = s_searchcondition
            'StrSearchWords = s_searchAllCases
            'StrSearchAnyWords = s_searchAnyCases
            'StrSearchNotWords = s_searchNotCases
            'StrNewSearch = s_searchNew

            SearchTitle = sSearchTitle
            Judge = sSearchJudge
            CaseNumber = sSearchCaseNumber
            Counsel = sSearchCounsel


            If bSearchType = 1 Then
                SearchField = "HEADNOTES"
            ElseIf bSearchType = 2 Then
                SearchField = "BOOLEANTEXT"
            ElseIf bSearchType = 3 Then
                SearchField = "BOOLEANTEXT" 'SearchField = "HEADNOTES"
            End If

            ''/// todo: is there any possibility of reducing if conditions and making it more faster? 20110517

            If sStrSearchWords <> "" Then

                sStrSearchWords = objUtil.Contains_Parser_notDetailed(sStrSearchWords, SearchField, "and")
            End If

            If sStrSearchAnyWords <> "" Then

                sStrSearchAnyWords = objUtil.Contains_Parser_notDetailed(sStrSearchAnyWords, SearchField, "or")
            End If

            If sStrSearchAnyWords <> "" Then

                sStrSearchAnyWords = objUtil.Contains_Parser_notDetailed(sStrSearchAnyWords, SearchField, "or") '' for not

            End If

            If sStrSearchExactWords <> "" Then

                sStrSearchExactWords = objUtil.Contains_Parser_notDetailed(sStrSearchExactWords, SearchField)

            End If

            If sStrSearchExactWords1 <> "" Then

                sStrSearchExactWords1 = objUtil.Contains_Parser_notDetailed(sStrSearchExactWords1, SearchField)

            End If

            'If strsearchcondition <> "" Then

            '    strsearchcondition = objUtil.Contains_Parser_notDetailed(strsearchcondition, SearchField)

            'End If





            ' exact=y + all=y + any= y + none=y
            'If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchNotWords <> "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " ) and  ( " & sStrSearchExactWords1 & " ) and ( " & sStrSearchWords & " ) and (" & sStrSearchAnyWords & ") and NOT(" & sStrSearchNotWords & " )"

            End If


            '            If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords <> "" And sStrSearchExactWords <> "" And sStrSearchAnyWords = "" And sStrSearchNotWords <> "" Then


                Sql = " ( " & sStrSearchExactWords & " ) and ( " & sStrSearchWords & " ) and  ( " & sStrSearchAnyWords & " ) "

            End If


            'If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " )  and ( " & sStrSearchWords & ") and NOT(" & sStrSearchAnyWords & " )"

            End If


            'If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " ) and  ( " & sStrSearchExactWords1 & " ) and ( " & sStrSearchWords & " ) "

            End If

            'If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " ) and  ( " & sStrSearchExactWords1 & " ) and ( " & sStrSearchAnyWords & " ) and NOT(" & sStrSearchAnyWords & " )"

            End If

            'If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " )  and ( " & sStrSearchExactWords1 & " )  and ( " & sStrSearchAnyWords & " ) "

            End If





            'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
            If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" Then

                'Sql = " ( " & sStrSearchExactWords & " ) and ( " & sStrSearchExactWords1 & " )and NOT(" & sStrSearchAnyWords & " )"
                Sql = " ( " & sStrSearchExactWords & " ) and NOT(" & sStrSearchAnyWords & " )"

            End If





            'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 = "" Then
            If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " ) and NOT(" & sStrSearchAnyWords & " )"

            End If




            'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords1 <> "" And sStrSearchExactWords = "" Then
            'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords = "" Then

            '    Sql = " ( " & sStrSearchExactWords1 & " ) and NOT(" & sStrSearchAnyWords & " )"

            'End If

            'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 = "" Then
            If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" Then

                Sql = " ( " & sStrSearchExactWords & " ) "

            End If


            'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords = "" And sStrSearchExactWords1 <> "" Then


            'Sql = " ( " & sStrSearchExactWords1 & " ) "

            'End If

            If bSearchCondition = 1 Then ' // and

                'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
                If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" Then

                    'Sql = " ( " & sStrSearchExactWords & " ) and ( " & sStrSearchExactWords1 & " )"
                    Sql = " ( " & sStrSearchExactWords & " ) " ' and ( " & sStrSearchExactWords1 & " )"

                End If

            End If

            If bSearchCondition = 2 Then ' // or

                'If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 <> "" Then
                If sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" Then

                    'Sql = " ( " & sStrSearchExactWords & " ) or ( " & sStrSearchExactWords1 & " )"
                    Sql = " ( " & sStrSearchExactWords & " ) " 'or ( " & sStrSearchExactWords1 & " )"

                End If

            End If



            ''----
            ''----


            If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords = "" Then

                Sql = " (" & sStrSearchWords & ") and (" & sStrSearchAnyWords & ") and NOT(" & sStrSearchAnyWords & " )"

            End If


            If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords = "" Then

                Sql = " (" & sStrSearchWords & " ) and  ( " & sStrSearchAnyWords & " ) "

            End If


            If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords = "" Then

                Sql = " ( " & sStrSearchWords & ") and NOT(" & sStrSearchAnyWords & " )"

            End If

            If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords = "" Then

                Sql = sStrSearchWords

            End If

            If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords = "" Then

                Sql = " ( " & sStrSearchAnyWords & " ) and NOT(" & sStrSearchAnyWords & " )"

            End If

            If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords = "" Then

                Sql = sStrSearchAnyWords

            End If

            If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 = "" Then

                Sql = " ( " & sStrSearchExactWords & " ) and (" & sStrSearchWords & ") and (" & sStrSearchAnyWords & ") and NOT(" & sStrSearchAnyWords & " )"

            End If


            If sStrSearchWords <> "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 = "" Then

                Sql = " ( " & sStrSearchExactWords & " ) and (" & sStrSearchWords & " ) and  ( " & sStrSearchAnyWords & " ) "

            End If


            If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 = "" Then

                Sql = " ( " & sStrSearchWords & ") and ( " & sStrSearchExactWords & " )and NOT(" & sStrSearchAnyWords & " )"

            End If

            If sStrSearchWords <> "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "" And sStrSearchExactWords = "" And sStrSearchExactWords1 = "" Then

                Sql = sStrSearchWords

            End If

            If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords <> "" And sStrSearchExactWords <> "" And sStrSearchExactWords1 = "" Then

                Sql = " ( " & sStrSearchAnyWords & " ) and ( " & sStrSearchExactWords & " )and NOT(" & sStrSearchAnyWords & " )"

            End If

            If sStrSearchWords = "" And sStrSearchAnyWords <> "" And sStrSearchAnyWords = "" And sStrSearchExactWords = "" And sStrSearchExactWords1 = "" Then

                Sql = sStrSearchAnyWords

            End If





            '====================================

            If sSearchNew <> "" Then
                newSearch = objUtil.Contains_Parser_notDetailed(Trim(sSearchNew), "BOOLEANTEXT", "and")


                If Sql = "" Then
                    Sql &= " ( " & newSearch & " )"
                Else
                    Sql &= " and ( " & newSearch & " )"
                End If


            End If



            ''------------------------------------




            If Trim(SearchTitle) <> "" Then
                SearchTitle = Trim(objUtil.RefineSentence(SearchTitle))

                If SearchTitle = "" Then GoTo skipTitle

                SearchTitle = objUtil.myParser(SearchTitle, "TITLE", "and")

            End If
skipTitle:


            If (SearchTitle <> "" And (Judge <> "") And (CaseNumber <> "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then

                Sql = " ( " & SearchTitle & " ) "
                Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                Sql &= Judge & CaseNumber

            End If

            If (SearchTitle <> "" And (Judge <> "") And (CaseNumber = "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then

                Sql = " ( " & SearchTitle & " ) "
                Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                Sql &= Judge
            End If

            If (SearchTitle <> "" And (Judge = "") And (CaseNumber <> "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then

                Sql = " ( " & SearchTitle & " ) "
                CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                Sql &= CaseNumber
            End If

            If (SearchTitle <> "" And (Judge = "") And (CaseNumber = "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then
                Sql = " ( " & SearchTitle & " ) "

            End If

            If (SearchTitle = "" And (Judge <> "") And (CaseNumber <> "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then

                Judge = " ( judge like '%" & sSearchJudge & "%' )"
                CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "

                Sql = Judge & CaseNumber
            End If

            If (SearchTitle = "" And (Judge <> "") And (CaseNumber = "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then
                Judge = " ( judge like '%" & sSearchJudge & "%' )"
                Sql = Judge

            End If

            If (SearchTitle = "" And (Judge = "") And (CaseNumber <> "")) And (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords = "") Then

                CaseNumber = " ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                Sql &= CaseNumber


            End If

            '// combination of all fields

            If (SearchTitle <> "" And (Judge <> "") And (CaseNumber <> "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then


                If Sql = "" Then
                    Sql &= "  ( " & SearchTitle & " ) "
                    Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= Judge & CaseNumber

                Else
                    Sql &= " and ( " & SearchTitle & " ) "
                    Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= Judge & CaseNumber

                End If


            End If

            If (SearchTitle <> "" And (Judge <> "") And (CaseNumber = "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then

                If Sql = "" Then
                    Sql &= " ( " & SearchTitle & " ) "
                    Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                    Sql &= Judge

                Else
                    Sql &= " and ( " & SearchTitle & " ) "
                    Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                    Sql &= Judge

                End If


            End If

            If (SearchTitle <> "" And (Judge = "") And (CaseNumber <> "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then


                If Sql = "" Then
                    Sql &= " ( " & SearchTitle & " ) "
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= CaseNumber

                Else
                    Sql &= " and ( " & SearchTitle & " ) "
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= CaseNumber

                End If



            End If

            If (SearchTitle <> "" And (Judge = "") And (CaseNumber = "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then

                If Sql = "" Then
                    Sql &= " ( " & SearchTitle & " ) "
                Else
                    Sql &= " and ( " & SearchTitle & " ) "
                End If

            End If

            If (SearchTitle = "" And (Judge <> "") And (CaseNumber <> "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then

                If Sql = "" Then
                    Judge = " ( judge like '%" & sSearchJudge & "%' )"
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= Judge & CaseNumber

                Else
                    Judge = " and ( judge like '%" & sSearchJudge & "%' )"
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= Judge & CaseNumber

                End If


            End If

            If (SearchTitle = "" And (Judge <> "") And (CaseNumber = "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then

                Judge = " ( judge like '%" & sSearchJudge & "%' )"


                If Sql = "" Then
                    Sql &= Judge
                Else
                    Sql &= " and " & Judge
                End If

            End If

            If (SearchTitle = "" And (Judge = "") And (CaseNumber <> "")) And (sStrSearchExactWords <> "" Or sStrSearchExactWords1 <> "" Or sStrSearchWords <> "" Or sStrSearchAnyWords <> "" Or sStrSearchAnyWords <> "") Then


                If Sql = "" Then
                    CaseNumber = " ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= Judge & CaseNumber

                Else
                    CaseNumber = " and ( CASENUMBER like '%" & sSearchCaseNumber & "%' ) "
                    Sql &= Judge & CaseNumber

                End If


            End If

            If (Counsel <> "") Then
                ''(SearchTitle <> "" or Judge <> "" or CaseNumber <> "" ) And (sStrSearchExactWords  <> "" Or sStrSearchWords <> "" Or StrSearchAnyWords <> "" Or sStrSearchAnyWords <> "")


                If Sql = "" Then
                    ''Note: This means that only counsel is going search as no data is given for other field.
                    Counsel = "  ( Counsel like '%" & Counsel & "%' )"
                    Sql = Counsel

                Else
                    ''Note: This means that counsel is going to be search with other data is given.
                    Counsel = " and ( Counsel like '%" & Counsel & "%' )"
                    Sql &= Counsel

                End If


            End If



            '' this is a condition when non of the words is only filled but any of the misc 
            '' option is filled which means that title: red box , non of the words: agreed by letters of offer dated 
            If (sStrSearchExactWords = "" And sStrSearchExactWords1 = "" And sStrSearchWords = "" And sStrSearchAnyWords = "" And sStrSearchAnyWords <> "") And ((SearchTitle <> "") Or (Judge <> "") Or (CaseNumber <> "")) Then
                Sql &= " and NOT(" & sStrSearchAnyWords & " )"
            End If

            '// if after refination nothing lefts then don't proceed for search 
            If Sql = "" Then
                ' // IMP: need to handle in the calling function


                Return Sql
            End If



            '// usman - 2011apr08. I have disabled this part due to going into this if while having null value
            'If (S_SearchIndustrialCourt <> 4) And S_SearchIndustrialCourt <> 5 And S_SearchCountry <> "All" Then  'And CountryChk <> 10 Then
            '    Dim IndustrialCourt As Byte = 4
            '    SearchCourt = "and not( " & objUtil.CourtForCountries(S_SearchCountry, IndustrialCourt) & " )"

            '    Sql &= SearchCourt

            'End If


            ' If (S_SearchIndustrialCourt <> 4) And S_SearchIndustrialCourt <> 5 And S_SearchCountry = "All" Then  'And CountryChk <> 10 Then
            If (bSearchIndustrialCourt <> 4) And bSearchIndustrialCourt <> 5 Then
                Dim IndustrialCourt As Byte = 4

                SearchCourt = " and not(court like '%Industrial Court%')"

                Sql &= SearchCourt

            End If






            'If (S_SearchCourt > 0 And S_SearchCourt < 5) And S_SearchCountry <> "All" Then
            If (bCourtLevel > 0 And bCourtLevel < 5) Then

                SearchCourt = "and ( " & objUtil.CourtForCountries("Malaysia", bCourtLevel) & " )"

                Sql &= SearchCourt

            End If

            ' //*** bCourtLevel = 6 represents all the courts so no need to put any extension to the query


            '// This is for multiple countries
            'If (S_SearchCourt > 0 And S_SearchCourt < 5) And S_SearchCountry = "All" Then
            '    Dim AllCountryList As String
            '    AllCountryList = objUtil.getCountriesListCases
            '    SearchCourt = "and ( " & objUtil.CourtForCountries(AllCountryList, S_SearchCourt) & " )"

            '    Sql &= SearchCourt

            'End If






            '/// I think no need to include language 20110516
            'Dim Language As String
            'Language = "and (language='English')"
            'Sql &= Language

            ' // which means the years are mentioned
            If (sYear1 <> "" And sYear2 <> "") Then
                Dim year As String
                Dim StYear, EndYear As Int16
                Dim obj As New membersarea.clsIntelligence

                StYear = obj.CasesStartingYear
                EndYear = obj.CasesEndingYear

                If (StYear = sYear1) And (EndYear = sYear2) Then
                    year = " "
                Else
                    year = "and (judgementyear >= " & sYear1 & " and judgementyear<= " & sYear2 & " )"

                End If


                Sql &= year
                year = ""
            End If

            'If S_SearchCountry = "All" Then
            '    ''This Condition means that all countries or fetch all type of countries
            'Else

            '    srchCountry = " and  " & objUtil.CountryParser(S_SearchCountry, "Country", "or")

            '    Sql &= srchCountry
            'End If

            'If sSortBy <> "" And Not (bSearchType = 3) Then
            If sSortBy <> "" Then
                Sql &= sSortBy
            End If

            ''/*  Explicitly Destructing the values.  */
            objUtil = Nothing



            Return Sql
        End Function


    End Class

End Namespace
