Imports Microsoft.VisualBasic

Public Class WordHighlight

    Private Stext As String
    Private Swords As String
    Private FinalText As String
    Private ArrSword As New ArrayList
    Dim indexSsentence As Integer
    Dim StrHighlight As String
    Dim SByWords As String
    Sub New(ByVal StrText As String, ByVal StrWords As String, ByVal StrBreakWords As String)
        Stext = StrText
        Swords = StrWords
        SByWords = StrBreakWords
    End Sub
    Private Sub CleanText()
        FinalText = Regex.Replace(Stext.ToString, "<.*?>", " ")
        FinalText = FinalText.ToLower
    End Sub
    Private Sub WordProcess()
        Swords = Swords.Replace(",", "").Replace(";", "").Replace(".", "")
        Swords = Swords.Replace("""", "").Replace("+", "").Replace("-", "").Replace("\", "")
    End Sub
    Public Function GetHighlight() As String
        CleanText()
        WordProcess()
        Dim StrRetrun As String = ""
        Try
            StrRetrun = ExtactPhrase()
            If StrRetrun <> "" Then
                Return StrRetrun
            Else
                RemoveNosyWords()
                StrRetrun = WordByWord()
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return StrRetrun
    End Function
    Private Function ExtactPhrase() As String
        indexSsentence = FinalText.IndexOf(Swords, StringComparison.OrdinalIgnoreCase)
        If indexSsentence <> -1 Then
            If FinalText.Substring(indexSsentence).Length < 150 Then
                StrHighlight = FinalText.Substring(indexSsentence - 150)
                StrHighlight = StrHighlight.Substring(StrHighlight.IndexOf(" "))
            ElseIf indexSsentence > 20 Then
                StrHighlight = FinalText.Substring(indexSsentence - 20, 150)
                StrHighlight = StrHighlight.Substring(StrHighlight.IndexOf(" "))
            Else
                StrHighlight = FinalText.Substring(0, 150)
            End If
            StrHighlight = "..." & StrHighlight & "..."
            StrHighlight = Regex.Replace(StrHighlight, Swords, "<span class='hWord'><b>" & Swords & "</b></span>", RegexOptions.IgnoreCase)
            Return StrHighlight
        Else
            Return ""
        End If
    End Function
    Private Function WordByWord() As String
        Dim FirstWord, LastWord, Pattern, StrFound As String
        Dim indexFirstWord, indexLastWord, IndexWord As Integer
        Dim CollectHighlight As New StringBuilder
        Dim CollectStart As String = ""
        For i = 0 To ArrSword.Count - 1
            If i = 0 Then
                FirstWord = ArrSword.Item(0)
            Else
                LastWord = ArrSword.Item(i)
            End If
        Next
        '========================================================================================

        If ArrSword.Count > 1 Then
            indexFirstWord = FinalText.IndexOf(FirstWord, StringComparison.OrdinalIgnoreCase)
            If indexFirstWord > 1 Then
                indexLastWord = FinalText.IndexOf(LastWord, indexFirstWord, StringComparison.OrdinalIgnoreCase)
            End If

            '==================Multi Words================
            If indexLastWord - indexFirstWord <= 100 AndAlso indexLastWord <> -1 AndAlso indexFirstWord <> -1 Then

                If FinalText.Substring(indexFirstWord).Length < 150 Then
                    StrHighlight = FinalText.Substring(indexFirstWord - 150)
                    StrHighlight = StrHighlight.Substring(StrHighlight.IndexOf(" "))
                ElseIf indexFirstWord > 20 Then
                    StrHighlight = FinalText.Substring(indexFirstWord - 20, 150)
                    StrHighlight = StrHighlight.Substring(StrHighlight.IndexOf(" "))
                Else
                    StrHighlight = FinalText.Substring(0, 150)
                End If
                For i = 0 To ArrSword.Count - 1
                    StrHighlight = StrHighlight.Replace(Trim(ArrSword.Item(i)), "<span class='hWord'><b>" & ArrSword.Item(i) & "</b></span>")
                Next
            Else
                For i = 0 To ArrSword.Count - 1
                    If ArrSword.Item(i) <> " " Then
                        Pattern = Trim(ArrSword.Item(i))
                        IndexWord = FinalText.IndexOf(Pattern, StringComparison.OrdinalIgnoreCase)
                        If IndexWord <> -1 And IndexWord > 30 Then
                            StrFound = FinalText.Substring(IndexWord - 30, 100)
                            'StrFound = StrFound.Substring(StrFound.IndexOf(" "))
                            'StrFound = StrFound.Substring(0, StrFound.LastIndexOf(" "))
                            StrFound = " ..." & StrFound & "..."
                            CollectHighlight.Append(StrFound)
                        ElseIf IndexWord <> -1 And IndexWord < 20 Then
                            StrFound = FinalText.Substring(0, 100)
                            StrFound = " ...." & StrFound & "..."
                            CollectStart &= CollectStart & StrFound  '.Replace(Pattern, "<span size=4 style='color:red;background-color:yellow'><b>" & ArrSword.Item(i) & "</b></span>")
                        End If
                    End If
                Next
                StrHighlight = CollectStart & CollectHighlight.ToString()
                For Iloop = 0 To ArrSword.Count - 1
                    Pattern = Trim(ArrSword.Item(Iloop))
                    Pattern = " " & Pattern '& " "
                    StrHighlight = StrHighlight.Replace(Pattern, "<span class='hWord'><b>" & Pattern & "</b></span>")
                Next
            End If

        End If
        Return StrHighlight
        '========================================================================================
    End Function

    Private Sub RemoveNosyWords()
        Dim stopWords As String() = New String() {"a", "about", "above", "after", "again", "against", _
    "all", "am", "an", "and", "any", "are", _
    "aren't", "as", "at", "be", "because", "been", _
    "before", "being", "below", "between", "both", "but", _
    "by", "can't", "cannot", "could", "couldn't", "did", _
    "didn't", "do", "does", "doesn't", "doing", "don't", _
    "down", "during", "each", "few", "for", "from", _
    "further", "had", "hadn't", "has", "hasn't", "have", _
    "haven't", "having", "he", "he'd", "he'll", "he's", _
    "her", "here", "here's", "hers", "herself", "him", _
    "himself", "his", "how", "how's", "i", "i'd", _
    "i'll", "i'm", "i've", "if", "in", "into", _
    "is", "isn't", "it", "it's", "its", "itself", _
    "let's", "me", "more", "most", "mustn't", "my", _
    "myself", "no", "nor", "not", "of", "off", _
    "on", "once", "only", "or", "other", "ought", _
    "our", "ours ", " ourselves", "out", "over", "own", _
    "same", "shan't", "she", "she'd", "she'll", "she's", _
    "should", "shouldn't", "so", "some", "such", "than", _
    "that", "that's", "the", "their", "theirs", "them", _
    "themselves", "then", "there", "there's", "these", "they", _
    "they'd", "they'll", "they're", "they've", "this", "those", _
    "through", "to", "too", "under", "until", "up", _
    "very", "was", "wasn't", "we", "we'd", "we'll", _
    "we're", "we've", "were", "weren't", "what", "what's", _
    "when", "when's", "where", "where's", "which", "while", _
    "who", "who's", "whom", "why", "why's", "with", _
    "won't", "would", "wouldn't", "you", "you'd", "you'll", _
    "you're", "you've", "your", "yours", "yourself", "yourselves", "(", ")", "."}

        Dim SearchPattern As String
        'For Each word As String In stopWords
        '    SearchPattern = "\b" & word & "\b"
        '    Swords = Regex.Replace(Swords, SearchPattern, "", RegexOptions.IgnoreCase)
        'Next
        ArrSword.AddRange(Split(SByWords, " "))
        ' Remove Empty Index 
        For iLoop = 0 To ArrSword.Count - 1
            ArrSword.Remove("")
        Next
        SearchPattern = ""
    End Sub
End Class
