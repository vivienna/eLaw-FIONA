
'/******************************************************************************/
'/*	Project     	: elaw          		    	    				        */
'/* Date Modified	: 13 jun 2012       		        		    		     */  
'/*	Description		: This library has functions related to information used in the project like years of the cases, 
'*/ court level and countries. the infomation can be put into database but operation required is very fast which cannot 
'*/ be achieved from database and I test it even by implementing stored procedures but performa */
'/*	Version			: 1.0											            */
'/*******************************************************************************/


Imports System.Data
Namespace membersarea

Public Class clsIntelligence
    Public CasesStartingYear As Int16 = 1894
        Public CasesEndingYear As Int16 = DateTime.Now.Year '// to get current year  

    '// This is for Browse
    Public ArticlesStartingYear As Int16 = 1981
        Public ArticlesEndingYear As Int16 = CasesEndingYear

    Public TreatiesStartingYear As Int16 = 1910
        Public TreatiesEndingYear As Int16 = CasesEndingYear

    Public PracticeNotesStartingYear As Int16 = 1946
        Public PracticeNotesEndingYear As Int16 = CasesEndingYear

    'Public PrecedentsStartingYear As Int16 = 2002
    'Public PrecedentsEndingYear As Int16 = 2002


    ''/*I m putting these values due to performace, i did try to retrieve values from db 
    'but it takes time. so by this way i can reduce steps of accessing data.    
    '
    ''*/

    '    Public murder() As String = {"kill", "slay", "assassisanate", "execute", "slaughter", "massacre", "snuff", "slang"}

    Public Function ChkingMurder(ByVal Col As Collections.ArrayList) As Boolean
        Dim i As Int16
        Dim Found As Boolean = False

        For i = 0 To Col.Count - 1

            If Col.Item(i) = "slay" Or Col.Item(i) = "assassisanate" Or Col.Item(i) = "execute" Or Col.Item(i) = "slaughter" Or Col.Item(i) = "massacre" Or Col.Item(i) = "snuff" Or Col.Item(i) = "slang" Then
                'col.Item(i) = "put to death" or 
                Found = True

            End If
        Next

        Return Found

    End Function
        ' // todo: do I really need to align with the courts now? concern about speed // 20110403
        Public Function GetRequiredCourt(ByVal courtLevel As Byte, ByVal country As String) As String


            ''/*
            '   i have hard coded Courts because there are countries which had different names in 
            '  past. and other main reason was speed / performance. although i did use stored 
            '  procedure but the performance was not as optimise as required.
            ''*/

            Dim court As String = ""
            Dim Result As String = ""


            ' court level 1= supreme court
            ' court level 2= high court
            ' court level 3= session court
            ' court level 4= industrial court

            country = Trim(country)
            ''//Highest level of court.
            If courtLevel = 1 Then
                '            If country = "Australia" Then
                'If (String.Compare(country, "Australia", True) = 0) Then
                '    court = " court like '%high court%' "
                'ElseIf (String.Compare(country, "Brunei", True) = 0) Then
                '    court = " court like '%Court of Appeal%' "
                'ElseIf (String.Compare(country, "Hong Kong", True) = 0) Then
                '    court = " court like '%Court of Final Appeal%' "
                'ElseIf (String.Compare(country, "India", True) = 0) Then
                '    court = " court like '%Supreme Court%' "
                'ElseIf (String.Compare(country, "Singapore", True) = 0) Then
                '    court = " court like '%Court of Appeal%' "
                'ElseIf (String.Compare(country, "Sri Lanka", True) = 0) Then
                '    court = " court like '%Supreme Court%' "
                If (String.Compare(country, "Malaysia", True) = 0) Then
                    court = " court like '%Supreme Court%' " ' need to reconfirm
                    'ElseIf (String.Compare(country, "Pakistan", True) = 0) Then
                    '    court = " court like '%Supreme Court%' "
                    'ElseIf (country.Compare(country, "New Zealand", True) = 0) Then
                    '    court = " court like '%Court of Appeal%' "
                    'ElseIf (country.Compare(country, "South Africa", True) = 0) Then
                    '    court = " court like '%CONSTITUTIONAL COURT%' or court like '%COURT OF APPEAL%' or court like '%SUPREME COURT OF AFRICA%'"
                    '    ''''ElseIf (country.Compare(country, "HOUSE OF LORDS", True) = 0) Then
                    ''''    'ElseIf country = "United Kingdom" Then
                    ''''    court = " court like '%HOUSE OF LORDS%'"
                    ''''End If



                    'ElseIf (country.Compare(country, "UNITED KINGDOM", True) = 0) Then
                    '    'ElseIf country = "United Kingdom" Then
                    '    court = " court like '%HOUSE OF LORDS%'"
                End If

            End If

            ''//2nd Highest level of court.
            If courtLevel = 2 Then

                'If (country.Compare(country, "Australia", True) = 0) Then
                '    court = " court like '%Fedral court%' "
                'ElseIf (country.Compare(country, "Brunei", True) = 0) Then
                '    court = " court like '%High Court%' "
                'ElseIf (country.Compare(country, "Hong Kong", True) = 0) Then
                '    court = " court like '%Court of Appeal of the high court%' "
                'ElseIf (country.Compare(country, "India", True) = 0) Then
                '    court = " court like '%High Court%' "
                'ElseIf (country.Compare(country, "Singapore", True) = 0) Then
                '    court = " court like '%high Court%' "
                'ElseIf (country.Compare(country, "Singapore", True) = 0) Then
                '    court = " court like '%Court of Appeal%' "
                If (String.Compare(country, "Malaysia", True) = 0) Then
                    court = " court like '%Court of Appeal%' "
                    'ElseIf (country.Compare(country, "Pakistan", True) = 0) Then
                    '    court = " court like '%High Court%' "
                    'ElseIf (country.Compare(country, "New Zealand", True) = 0) Then
                    '    court = " court like '%High Court%'"
                    'ElseIf (country.Compare(country, "New Zealand", True) = 0) Then
                    '    court = " court like '%JURISDICTION OF LAND CLAIMS COURT%' or court like '%JURISDICTION OF LAND CLAIMS COURT%'"
                    'ElseIf country = "United Kingdom" Then
                    ''''ElseIf (country.Compare(country, "HOUSE OF LORDS", True) = 0) Then
                    ''''    court = " court like '%THE LORDS OF APPEAL%'"
                    ''''End If



                    'ElseIf (String.Compare(country, "UNITED KINGDOM", True) = 0) Then
                    '    court = " court like '%THE LORDS OF APPEAL%'"
                End If



            End If
            If courtLevel = 3 Then

                'If (country.Compare(country, "Australia", True) = 0) Then
                '    court = " court like '%Lower court%' "
                'ElseIf (country.Compare(country, "Brunei", True) = 0) Then
                '    court = " court like '%Subordinate Court%' "

                'ElseIf (country.Compare(country, "Hong Kong", True) = 0) Then
                '    court = " court like '%Court of First Instant%' " ' "Court of First Instant high court"
                'ElseIf (country.Compare(country, "India", True) = 0) Then
                '    court = " court like '%session Court%' "
                'ElseIf (country.Compare(country, "Singapore", True) = 0) Then
                '    court = " court like '%high Court%' "
                'ElseIf (country.Compare(country, "Singapore", True) = 0) Then
                '    court = " court like '%High Court%' "

                If (String.Compare(country, "Malaysia", True) = 0) Then
                    court = "court like '%High Court%'"
                    'ElseIf (country.Compare(country, "Pakistan", True) = 0)Then
                    '    court = " court like '%session Court%' "
                    'ElseIf (country.Compare(country, "New Zealand", True) = 0) Then
                    '    court = " court like '%District Court%' "

                    'ElseIf (country.Compare(country, "New Zealand", True) = 0)Then
                    '    court = " court like '%High Court%' "
                End If


            End If
            'Imp Note : We must have to give industrial court for every country. 
            '        as when it searches for normal court it chks if there is not(industrial court) case
            If courtLevel = 4 Then
                ''                    court = " court like '%Industrial Court%' "
                'If (country.Compare(country, "Australia", True) = 0) Then
                '    court = " court like '%Industrial Court%' "
                'ElseIf (country.Compare(country, "Brunei", True) = 0) Then
                '    court = " court like '%Industrial Court%' "

                'ElseIf (country.Compare(country, "Hong Kong", True) = 0) Then
                '    court = " court like '%Industrial Court%' "
                'ElseIf (country.Compare(country, "India", True) = 0) Then
                '    court = " court like '%Industrial Court%' "
                'ElseIf (country.Compare(country, "Singapore", True) = 0) Then
                '    court = " court like '%Industrial Court%' "

                If (String.Compare(country, "Malaysia", True) = 0) Then
                    court = " court like '%Industrial Court%' "
                    'ElseIf (country.Compare(country, "Pakistan", True) = 0) Then
                    '    court = " court like '%Industrial Court%' "
                    'ElseIf (country.Compare(country, "New Zealand", True) = 0) Then
                    '    court = " court like '%Industrial Court%' "
                    'ElseIf (country.Compare(country, "Sri Lanka", True) = 0) Then
                    '    court = " court like '%Industrial Court%' "

                    'ElseIf (country.Compare(country, "canada", True) = 0) Then
                    '    court = " court like '%Industrial Court%' "

                    'ElseIf (country.Compare(country, "South Africa", True) = 0) Then
                    '    court = " court like '%Industrial Court%' "
                    '    ''ElseIf (country.Compare(country, "HOUSE OF LORDS", True) = 0) Then
                    '    ''    court = " court like '%Industrial Court%'"
                    '    ''End If

                    'ElseIf (country.Compare(country, "UK-House of Lords", True) = 0) Then
                    '    court = " court like '%Industrial Court%'"
                End If

            End If
            If courtLevel = 5 Then

                If (String.Compare(country, "Malaysia", True) = 0) Then
                    court = " court like '%Federal%' "
                    
                End If

            End If
            Result = court

            Return Result




        End Function

    Public Function FileTypeChecker(ByVal filename As String) As Boolean
        Dim IsOk As Boolean
        Dim Type As String
        IsOk = False
        Type = Left(Trim(filename), 3)


        If Type = "CLJ" Then
            IsOk = True
        ElseIf Type = "ILR" Then
            IsOk = True
        ElseIf Type = "LNS" Then
            IsOk = True
        End If


        Return IsOk
    End Function

    Public Function CountriesForLegislation() As Collections.ArrayList
        Dim TypeList As New ArrayList()
        TypeList.Add("All")
        TypeList.Add("Malaysia")
        '        TypeList.Add("Sri Lanka")

 
        Return TypeList

    End Function

    Public Function CountriesForCases() As Collections.ArrayList
        ''/********
        ''Note: This list have all and countries.
        ''*********/
        Dim TypeList As New ArrayList()

        TypeList.Add("All") '1
        TypeList.Add("Australia") '2
        TypeList.Add("Brunei") '3
        TypeList.Add("Canada") '4
        TypeList.Add("Hong Kong") '5
        TypeList.Add("Malaysia") '6
        TypeList.Add("New Zealand") '7
        TypeList.Add("India") '8
        'TypeList.Add("Singapore") '9
        TypeList.Add("Pakistan") '9
        TypeList.Add("Sri Lanka") '10
        TypeList.Add("South Africa") '11
        TypeList.Add("UK-House of Lords")



        Return TypeList

    End Function


    Public Function CountriesForCases1() As Collections.ArrayList
        ''/********
        ''Note: This list doesn't have all word.
        ''*********/
        Dim TypeList As New ArrayList()


        TypeList.Add("Australia")
        TypeList.Add("Brunei")
        TypeList.Add("Canada")
        TypeList.Add("Hong Kong")
        TypeList.Add("Malaysia")
        TypeList.Add("New Zealand")
        TypeList.Add("India")
        'TypeList.Add("Singapore")
        TypeList.Add("Pakistan")
        TypeList.Add("Sri Lanka")
        TypeList.Add("South Africa")

        Return TypeList

    End Function

    Public Function CountriesForArticles() As Collections.ArrayList
        Dim TypeList As New ArrayList()

        TypeList.Add("All")
        TypeList.Add("Africa")
        TypeList.Add("Australia")
        TypeList.Add("Canada")
        TypeList.Add("Hong Kong")
        TypeList.Add("India")
        TypeList.Add("International")
        TypeList.Add("Malaysia")
        TypeList.Add("New Zealand")
        TypeList.Add("Nigeria")
        TypeList.Add("Singapore")
        TypeList.Add("Sri Lanka")
        TypeList.Add("South Africa")
        TypeList.Add("United Kingdom")
        TypeList.Add("USA")

        Return TypeList

    End Function

    Public Function CountriesForPrecedents() As Collections.ArrayList
        Dim TypeList As New ArrayList()

        'TypeList.Add("All")        
        TypeList.Add("Malaysia")

        Return TypeList

    End Function

    Public Function CountriesForPracticeNotes() As Collections.ArrayList
        Dim TypeList As New ArrayList()

        'TypeList.Add("All")        
        TypeList.Add("Malaysia")

        Return TypeList

    End Function

    Public Function CountriesForLegalForms() As Collections.ArrayList
        Dim TypeList As New ArrayList()

        'TypeList.Add("All")        
        TypeList.Add("Malaysia")

        Return TypeList

    End Function

    Public Function CountriesForTreaties() As Collections.ArrayList
        Dim TypeList As New ArrayList()

        'TypeList.Add("All")        
        TypeList.Add("International")

        Return TypeList

    End Function


    Public Function isValidFileForHeadnotes(ByVal filename As String) As Boolean
        '' this is for chking if the case has our headnotes & if not don't show it
        Dim i As Integer
        Dim FileType As String
        Dim Valid As Boolean = False
        Dim FileList = ValidFileName()


            FileType = Left(UCase(Trim(filename)), 4)

        For i = 0 To FileList.Count - 1
                If FileType = FileList.Item(i) Or Left(FileType, 2) = "PC" Then
                    Valid = True
                    Exit For
                End If
        Next
        FileList = Nothing
        Return Valid
    End Function

    Public Function ValidFileName() As Collections.ArrayList
        Dim TypeList As New ArrayList()

            TypeList.Add("MLRH")
            TypeList.Add("MLRA")
            TypeList.Add("MELR")
            TypeList.Add("MLRHU")
            TypeList.Add("MLRAU")
            TypeList.Add("MELRU")
        Return TypeList
    End Function


    Public Function GetCountryColor(ByVal Country As String) As String
        '' This Function is for returning the required color for specific country
            'Dim i As Integer
        Dim Color As String

        Dim CountriesList As New Hashtable()
        CountriesList = ColorsForCountries()
        Country = UCase(Country) ' Because hastable key is case sensitive

        If CountriesList.ContainsKey(Country) Then

            Color = CountriesList.Item(Country)
        Else
            Color = "#000000" 'Default: if no color match then assign this color for background

        End If

        CountriesList = Nothing
        Return Color
    End Function


    Private Function ColorsForCountries() As Collections.Hashtable
        Dim ColorList As New Hashtable()

        ColorList.Add("AUSTRALIA", "#660099")
        ColorList.Add("BRUNEI", "#990066")
        ColorList.Add("HONG KONG", "#990000")
        ColorList.Add("INDIA", "#006666")
        ColorList.Add("MALAYSIA", "#3366CC")
        ColorList.Add("SINGAPORE", "#ED9CAD")
        ColorList.Add("SRI LANKA", "#CC9900")
        ColorList.Add("CANADA", "#3399CC")
        ColorList.Add("NEW ZEALAND", "#DD0000")
        ColorList.Add("PAKISTAN", "#009900")
        ColorList.Add("UNITED KINGDOM", "#003366")
        ColorList.Add("SOUTH AFRICA", "#FF6600")

        Return ColorList
    End Function


    Public Function GetCountryIndex(ByVal Country As String) As String
        '' This Function is for returning the required index for cases country dropdownlist
        Dim i As Integer
        Dim CountryIndex As Byte

        Dim CountriesList As New Hashtable()
        CountriesList = IndexForCountries()
        Country = UCase(Country) ' Because hastable key is case sensitive
        Country = Replace(Country, ",", "")

        If CountriesList.ContainsKey(Country) Then
            'If CountriesList.ContainsValue(Country) Then

            CountryIndex = CInt(CountriesList.Item(Country))
        Else
            CountryIndex = 0 'Default: if no Country Found then All countries
        End If


        CountriesList = Nothing
        i = 0

        Return CountryIndex
    End Function

    'Description : This function will return the index of countries for matching for showing the list in the country
    '             list on casessearchresult page. it is case sensitive for matching for indexing.
    Private Function IndexForCountries() As Collections.Hashtable
        Dim IndexList As New Hashtable()

            IndexList.Add("ALL", "0")
        IndexList.Add("AUSTRALIA", "1")
        IndexList.Add("BRUNEI", "2")
        IndexList.Add("CANADA", "3")
        IndexList.Add("HONG KONG", "4")
        IndexList.Add("MALAYSIA", "5")
        IndexList.Add("NEW ZEALAND", "6")
        IndexList.Add("INDIA", "7")
        IndexList.Add("SINGAPORE", "8")
        IndexList.Add("SRI LANKA", "9")
        IndexList.Add("SOUTH AFRICA", "10")

        'IndexList.Add("PAKISTAN", "11")
        'IndexList.Add("BANGLADESH", "12")

        Return IndexList
    End Function

    Public Function GetLegislationCountryIndex(ByVal Country As String) As String

            'Dim i As Integer
        Dim CountryIndex As Byte

        Dim CountriesList As New Hashtable()
        CountriesList = IndexLegislationForCountries()
        Country = UCase(Country) ' Because hastable key is case sensitive
        Country = Replace(Country, ",", "")

        If CountriesList.ContainsKey(Country) Then

            CountryIndex = CInt(CountriesList.Item(Country))
        Else
            CountryIndex = 0 'Default: if no Country Found then All countries
        End If

        CountriesList = Nothing
        Return CountryIndex
    End Function

    Public Function GetArticleCountryIndex(ByVal Country As String) As String

            ' Dim i As Integer
        Dim CountryIndex As Byte

        Dim CountriesList As New Hashtable()
        CountriesList = IndexArticleForCountries()
        Country = UCase(Country) ' Because hastable key is case sensitive
        Country = Replace(Country, ",", "")

        If CountriesList.ContainsKey(Country) Then

            CountryIndex = CInt(CountriesList.Item(Country))
        Else
            CountryIndex = 0 'Default: if no Country Found then All countries
        End If

        CountriesList = Nothing
        Return CountryIndex
    End Function


    Public Function GetCourtIndex(ByVal Court As String) As String
        '' This Function is for returning the required color for specific country
            ' Dim i As Integer
        Dim CourtIndex As Byte
        Dim CourtList As New Hashtable()

        CourtList = IndexForCourts()
        Court = UCase(Court) ' Because hastable key is case sensitive
        'Court = Replace(Court, ",", "")

        If CourtList.ContainsKey(Court) Then

            CourtIndex = CStr(CourtList.Item(Court))
        Else
            CourtIndex = 4 'Default: if no Court Found then All courts
        End If

        CourtList = Nothing
        Return CourtIndex
    End Function



    Private Function IndexLegislationForCountries() As Collections.Hashtable
        Dim IndexList As New Hashtable()

        IndexList.Add("ALL", "0")
        IndexList.Add("MALAYSIA", "1")
        IndexList.Add("SRI LANKA", "2")

        Return IndexList
    End Function

    Private Function IndexArticleForCountries() As Collections.Hashtable
        Dim IndexList As New Hashtable()

        IndexList.Add("ALL", "0")
        IndexList.Add("Africa", "1")
        IndexList.Add("Australia", "2")
        IndexList.Add("Canada", "3")
        IndexList.Add("Hong Kong", "4")
        IndexList.Add("India", "5")
        IndexList.Add("International", "6")
        IndexList.Add("New Zealand", "7")
        IndexList.Add("Nigeria", "8")
        IndexList.Add("Singapore", "9")
        IndexList.Add("Sri Lanka", "10")
        IndexList.Add("South Africa", "11")
        IndexList.Add("United Kingdom", "12")
        IndexList.Add("USA", "13")

        Return IndexList
    End Function

    Private Function IndexForCourts() As Collections.Hashtable
        Dim IndexList As New Hashtable()



        IndexList.Add("1", "0") 'Superior Court
        IndexList.Add("2", "1") 'Appeal Court
        IndexList.Add("3", "2") 'Lower Court
        IndexList.Add("4", "3") 'Industrial Court
        IndexList.Add("6", "4") 'All Courts


        Return IndexList
    End Function


End Class

End Namespace
