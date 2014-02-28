


Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports System.IO
Imports System.Data


Namespace membersarea


    Public Class clsMyUtility


        Public Function refineExactSentence(ByVal sentence As String) As String

            sentence = Trim(LCase((sentence)))
            If Len(sentence) <= 5 Then
                sentence = Me.RefineSentence(sentence)
                '            GoTo otherChars
            End If


            Return sentence
        End Function

        Public Function RefineSmallWords(ByVal sentence As String) As String
            If Len(sentence) <= 5 Then
                sentence = Replace(sentence, "about", " ")
                sentence = Replace(sentence, "shall", " ")
                sentence = Replace(sentence, "there", " ")
                sentence = Replace(sentence, "their", " ")
                sentence = Replace(sentence, "those", " ")
                sentence = Replace(sentence, "shall", " ")

                RefineSentence(sentence)
            End If

            If Len(sentence) = 4 Then
                sentence = Replace(sentence, "from", " ")
                sentence = Replace(sentence, "xnor", " ")
                sentence = Replace(sentence, "does", " ")
                sentence = Replace(sentence, "were", " ")
                sentence = Replace(sentence, "they", " ")
                sentence = Replace(sentence, "them", " ")
                sentence = Replace(sentence, "that", " ")

            End If

            If Len(sentence) = 3 Then
                sentence = Replace(sentence, "for", " ")
                sentence = Replace(sentence, "his", " ")
                sentence = Replace(sentence, "the", " ")
                sentence = Replace(sentence, "all", " ")
                sentence = Replace(sentence, "the", " ")
                sentence = Replace(sentence, "any", " ")
                sentence = Replace(sentence, "and", " ")
                sentence = Replace(sentence, "not", " ")
                sentence = Replace(sentence, "xor", " ")
                sentence = Replace(sentence, "did", " ")
                sentence = Replace(sentence, "are", " ")
                sentence = Replace(sentence, "was", " ")
                sentence = Replace(sentence, "his", " ")
                sentence = Replace(sentence, "him", " ")
                sentence = Replace(sentence, "had", " ")
                sentence = Replace(sentence, "she", " ")
                sentence = Replace(sentence, "had", " ")

            End If


            If Len(sentence) = 2 Then
                sentence = Replace(sentence, "of", " ")
                sentence = Replace(sentence, "by", " ")
                sentence = Replace(sentence, "be", " ")
                sentence = Replace(sentence, "in", " ")
                sentence = Replace(sentence, "is", " ")
                sentence = Replace(sentence, "an", " ")
                sentence = Replace(sentence, "or", " ")
                sentence = Replace(sentence, "as", " ")
                sentence = Replace(sentence, "am", " ")
                sentence = Replace(sentence, "to", " ")
                sentence = Replace(sentence, "do", " ")
                sentence = Replace(sentence, "we", " ")
                sentence = Replace(sentence, "he", " ")
                sentence = Replace(sentence, "me", " ")
                sentence = Replace(sentence, "we", " ")
                sentence = Replace(sentence, "us", " ")
            End If

            If Len(sentence) <= 1 Then
                sentence = Replace(sentence, "a", " ")
                sentence = Replace(sentence, "I", " ")
            End If


            Return sentence
        End Function

        'Public Function Refine_XML_File(ByVal sentence As String) As String

        '    sentence = Trim(LCase((sentence)))

        '    sentence = Replace(sentence, "'", " ")
        '    sentence = Replace(sentence, Chr(34), " ")


        '    '        Label1.Text = sentence
        '    Return sentence
        'End Function

        'Public Function Refine_XML_Tags(ByVal sentence As String) As String

        '    sentence = Trim(LCase((sentence)))

        '    sentence = Replace(sentence, "<p>", " ")
        '    sentence = Replace(sentence, "</p>", " ")
        '    sentence = Replace(sentence, "<br>", " ")
        '    sentence = Replace(sentence, "</br>", " ")
        '    sentence = Replace(sentence, "<p>", " ")
        '    sentence = Replace(sentence, "</p>", " ")
        '    sentence = Replace(sentence, "<i>", " ")
        '    sentence = Replace(sentence, "</i>", " ")
        '    sentence = Replace(sentence, "<b>", " ")
        '    sentence = Replace(sentence, "</b>", " ")
        '    sentence = Replace(sentence, "<CATCHWORDS>", " ")
        '    sentence = Replace(sentence, "</CATCHWORDS>", " ")
        '    sentence = Replace(sentence, "<SUBJECT>", " ")
        '    sentence = Replace(sentence, "</SUBJECT>", " ")

        '    'SUBJECT


        '    '        Label1.Text = sentence
        '    Return sentence
        'End Function

        Public Function Refine_XML_File(ByVal sentence As String) As String

            sentence = Trim(LCase((sentence)))

            sentence = Replace(sentence, "'", " ")
            sentence = Replace(sentence, Chr(34), " ")


            '        Label1.Text = sentence
            Return sentence
        End Function

        Public Function Refine_XML_Tags(ByVal sentence As String) As String

            Dim Pattern As String

            sentence = Trim(LCase((sentence)))
            If sentence = "" Or Len(sentence) = 0 Then
                ' Exit Function
                Return " "
            End If

            sentence = Replace(sentence, "<p>", " ")
            sentence = Replace(sentence, "</p>", " ")
            sentence = Replace(sentence, "<br>", " ")
            sentence = Replace(sentence, "</br>", " ")
            sentence = Replace(sentence, "<p>", " ")
            sentence = Replace(sentence, "</p>", " ")
            sentence = Replace(sentence, "<i>", " ")
            sentence = Replace(sentence, "</i>", " ")
            sentence = Replace(sentence, "<b>", " ")
            sentence = Replace(sentence, "</b>", " ")

            Pattern = "\b<LINK\w*\s*</LINK>\b"
            sentence = Regex.Replace(sentence, Pattern, " ", RegexOptions.IgnoreCase)

            sentence = Regex.Replace(sentence, "\b<CATCHWORDS>\b", " ", RegexOptions.IgnoreCase)
            sentence = Regex.Replace(sentence, "\b</CATCHWORDS>\b", " ", RegexOptions.IgnoreCase)

            sentence = Regex.Replace(sentence, "\b<COUNSEL>\b", " ", RegexOptions.IgnoreCase)
            sentence = Regex.Replace(sentence, "\b</COUNSEL>\b", " ", RegexOptions.IgnoreCase)

            sentence = Regex.Replace(sentence, "\b<VERDICT>\b", " ", RegexOptions.IgnoreCase)
            sentence = Regex.Replace(sentence, "\b</VERDICT>\b", " ", RegexOptions.IgnoreCase)

            sentence = Regex.Replace(sentence, "\b<LEGISLATIONREFERREDTO>\b", " ", RegexOptions.IgnoreCase)
            sentence = Regex.Replace(sentence, "\b</LEGISLATIONREFERREDTO>\b", " ", RegexOptions.IgnoreCase)

            sentence = Regex.Replace(sentence, "\b<CASESREFERREDTO>\b", " ", RegexOptions.IgnoreCase)
            sentence = Regex.Replace(sentence, "\b</CASESREFERREDTO>\b", " ", RegexOptions.IgnoreCase)

            sentence = Regex.Replace(sentence, "\b<OTHERSREFERREDTO>\b", " ", RegexOptions.IgnoreCase)
            sentence = Regex.Replace(sentence, "\b</OTHERSREFERREDTO>\b", " ", RegexOptions.IgnoreCase)


            Return sentence
        End Function

        Public Function RefineSentence(ByVal Sentence As String) As String

            Dim i As Int16
            Dim ReplacePattern As String = ""
            Dim ReplacePattern1 As String = " "
            Dim SearchPattern As String
            Dim sbSentence As New System.Text.StringBuilder()
            Sentence = Trim(Sentence)

            ' for stoping XSS ATTACK
            SearchPattern = "\b<script>\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b</script>\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            For i = 65 To 90 'include all English alphabets [A-Z,a-z] as its ignore case
                SearchPattern = "\b" & Chr(i) & "\b"
                Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            Next

            For i = 48 To 57 'include all English alphabets [A-Z,a-z] as its ignore case
                SearchPattern = "\b" & Chr(i) & "\b"
                Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            Next


            SearchPattern = "\b+\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b-\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b/\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b*\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b$\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "^"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            SearchPattern = "&"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "@"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "#"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "!"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "~"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "'"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "%"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            'SearchPattern = "\b.\b"
            'Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b,\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            'SearchPattern = "?"
            'Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\b\\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "|"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "="
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)
            ''/* Imp
            ''  i have to chk where to put replacepattern1 by testing
            ''  i came to know about - and _ so i m putting because anti-money is becoming
            ''  antimoney
            ''*/
            SearchPattern = "-"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "_"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = ";"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = ":"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "'"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = " & chr(34) & "
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            SearchPattern = "\banother\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bafter\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bof\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bby\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            SearchPattern = "\babout\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bis\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            SearchPattern = "\bin\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\ban\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bam\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bas\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bout\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\ball\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bfrom\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bfor\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bto\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\band\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bany\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bor\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bnot\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bxor\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bxnor\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bdid\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bdoes\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bdo\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bshall\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bare\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwas\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwere\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthere\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\btheir\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bshe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bit\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhad\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhas\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhave\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bto\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthey\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthose\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthem\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthen\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthat\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthis\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthat\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bme\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmy\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bus\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwill\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bshall\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhis\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhim\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhad\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmy\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmine\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bnot\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhave\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)


            SearchPattern = "\bwho\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwith\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwhat\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)



            '===================New================

            SearchPattern = "\bother\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\banother\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bafter\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbecause\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbeen\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbefore\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbeing\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbetween\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bboth\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbut\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bby\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bcame\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bcome\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bcan\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bcould\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbefore\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\beach\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bfor\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bfrom\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bget\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bgot\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhere\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhimself\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhow\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bhis\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bif\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bin\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\binto\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bis\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bit\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\blike\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmake\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmany\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bme\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmight\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bbefore\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmore\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmost\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmuch\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bmy\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bnever\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bnow\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bof\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bon\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bonly\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bother\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bour\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bout\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bover\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsaid\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsame\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsee\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsince\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bshould\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsince\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsome\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bstill\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bsuch\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\btake\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthen\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthat\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\btheir\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthem\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthen\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthere\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthese\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthey\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthis\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthose\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bthrough\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bto\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\btoo\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bunder\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bup\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bvery\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwas\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bway\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwe\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwell\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwere\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwhere\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwhich\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwhile\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwho\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwith\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bwould\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\byou\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\byour\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            SearchPattern = "\bno\b"
            Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            '=====================================
            'SearchPattern = "\bfrom\b"
            'Sentence = Regex.Replace(Sentence, SearchPattern, ReplacePattern, RegexOptions.IgnoreCase)

            Sentence = Replace(Sentence, """", "")
            Sentence = Replace(Sentence, ")", " ")
            Sentence = Replace(Sentence, "[", " ")
            Sentence = Replace(Sentence, "]", " ")

            Sentence = Replace(Sentence, "{", " ")
            Sentence = Replace(Sentence, "}", " ")

            Sentence = Replace(Sentence, "(", " ")
            Sentence = Replace(Sentence, ")", " ")

            Sentence = Replace(Sentence, "?", " ")

            Sentence = Replace(Sentence, ".", " ")
            Sentence = Replace(Sentence, ",", " ")
            Sentence = Replace(Sentence, ";", " ")
            Sentence = Replace(Sentence, ":", " ")
            Sentence = Replace(Sentence, "/", " ")
            Sentence = Replace(Sentence, "\", " ")
            Sentence = Replace(Sentence, "|", " ")
            Sentence = Replace(Sentence, "-", " ")
            Sentence = Replace(Sentence, "_", " ")
            Sentence = Replace(Sentence, "=", " ")
            Sentence = Replace(Sentence, "+", " ")
            Sentence = Replace(Sentence, "*", " ")

            'sentence = Replace(sentence, " will ", " ") '' check according requirement


            Return Sentence

        End Function

        Public Function myParser(ByVal sentence As String, ByVal FIELD As String, ByVal LogicOperator As String) As String
            Dim col As New Collections.ArrayList()
            Dim i As Integer
            Dim Result As String

            sentence = Me.RefineSentence(sentence)

            Result = ""
            col = Tokenizer(sentence, " ")


            For i = 0 To col.Count - 1
                If i = 0 Then
                    'Result = "'" & col.Item(i) & "%'"
                    Result = " ( " & FIELD & " like '%" & col.Item(Trim(i)) & "%'" & " )"
                    GoTo againLoop
                End If
                If (col.Item(i) <> "" And col.Item(i) <> " ") Then
                    ' this condition is for stoping search for space or null in collection
                    Result &= " " & LogicOperator & " ( " & FIELD & " like '%" & col.Item(Trim(i)) & "%' " & " ) "
                End If

againLoop:
            Next

            Return Result
        End Function


        Public Function myParser1(ByVal sentence As String, ByVal FIELD As String, ByVal LogicOperator As String) As String
            Dim col As New Collections.ArrayList()
            Dim i As Integer
            Dim Result As String

            'sentence = WordExtractor(TextBox1.Text)
            Result = ""
            col = Tokenizer(sentence, " ")
            '


            For i = 0 To col.Count - 1
                If i = 0 Then
                    'Result = "'" & col.Item(i) & "%'"
                    Result = "" & col.Item(i) & "*"
                    GoTo againLoop
                End If
                '           Result &= " " & LogicOperator & " " &  contains FIELD & " like %" & col.Item(i) & "% "
againLoop:
            Next

            If LogicOperator = "and" And i > 0 Then

            End If

            Return Result
        End Function

        Public Overloads Function Tokenizer(ByVal Statement As String, ByVal Identifier As String) As Collections.ArrayList

            Dim ST As String = " "
            Dim Length As Integer
            Dim i As Integer
            Dim COLL As New Collections.ArrayList()

            Length = Len(Statement)

            Dim starr(Length) As String

            Statement = Trim(Statement)

            For i = 1 To Length

                starr(i) = Mid(Statement, i, 1)

                If starr(i) = Identifier Then

                    COLL.Add(Trim(ST))
                    ST = ""

                End If

                If starr(i) <> Identifier Then
                    ST = ST & starr(i)
                End If


            Next i
            COLL.Add(ST)

            'Tokenizer = COLL
            Return COLL
        End Function

        Public Overloads Function Tokenizer1(ByVal Statement As String, ByVal Identifier As String) As String()
            Dim Result() As String

            Result = Split(Statement, Identifier)

            Return Result
        End Function

        Public Overloads Function Tokenizer(ByVal Statement As String) As Collections.ArrayList

            Dim ST As String = " "
            Dim Length As Integer
            Dim i As Integer
            Dim COLL As New Collections.ArrayList()

            Length = Len(Statement)

            Dim starr(Length) As String

            Statement = Trim(Statement)

            For i = 1 To Length

                starr(i) = Mid(Statement, i, 1)

                If starr(i) = " " Or starr(i) = "," Or starr(i) = ";" Or starr(i) = "" Then

                    COLL.Add(ST)
                    ST = ""

                End If

                If starr(i) <> " " Or starr(i) <> "," Or starr(i) <> ";" Or starr(i) = "" Then
                    ST = ST & starr(i)
                End If


            Next i
            COLL.Add(ST)

            Tokenizer = COLL

        End Function

        Public Overloads Function Tokenizer(ByVal Statement As String, ByVal Identifier As Char) As String

            Dim ST As String = " "
            Dim Length As Integer
            Dim i As Integer
            'Dim COLL As New Collections.ArrayList()
            Dim Result As String = " "

            Length = Len(Statement)

            Dim starr(Length) As String

            Statement = Trim(Statement)

            For i = 1 To Length

                starr(i) = Mid(Statement, i, 1)

                If starr(i) = ";" Then

                    'COLL.Add(ST)
                    Result = ST
                    ST = ""
                    Exit For

                End If

                If starr(i) <> ";" Then
                    ST = ST & starr(i)
                End If


            Next i
            'COLL.Add(ST)
            'ST = 'COLL.Item(0)
            'Tokenizer = COLL

            Return Result

        End Function


        Public Function CountryParser(ByVal sentence As String, ByVal FIELD As String, ByVal LogicOperator As String) As String
            Dim col As New Collections.ArrayList()
            Dim i As Integer
            Dim Result As String
            '//Note: The database field is united kingdom which we can use for searching but they want to show house of lords.
            Dim UK As String = "UNITED KINGDOM"
            Dim UK1 As String = "UK-House of lords"
            Dim Country As String


            'sentence = WordExtractor(TextBox1.Text)
            Result = ""
            col = Tokenizer(sentence, ",")


            For i = 0 To col.Count - 1
                Country = CStr(col.Item(i))
                If i = 0 Then


                    If (String.Compare(Country, UK1, True) = 0) Then ' //dec152005 
                        Result = " ( " & FIELD & " = '" & UK & "' "
                    Else
                        Result = " ( " & FIELD & " = '" & Country & "' "
                    End If

                    'Result = " ( " & FIELD & " = '" & col.Item(i) & "' "

                    GoTo againLoop
                End If
                If (col.Item(i) <> "" And col.Item(i) <> " ") Then
                    ' this condition is for stoping search for space or null in collection

                    If (String.Compare(Country, UK1, True) = 0) Then  ' //dec152005 
                        Result &= " " & LogicOperator & "  " & FIELD & " = '" & UK & "' " ' //dec152005 
                    Else
                        Result &= " " & LogicOperator & "  " & FIELD & " = '" & Country & "' "  ' 
                    End If

                    '  Result &= " " & LogicOperator & "  " & FIELD & " = '" & col.Item(i) & "' " ' 

                End If

againLoop:
            Next
            Result &= " ) "

            col = Nothing
            i = 0
            UK = ""
            UK1 = ""
            Country = ""

            Return Result
        End Function

        Public Function CourtForCountries(ByVal Countries As String, ByVal CourtLevel As Byte) As String
            Dim col As New Collections.ArrayList()
            Dim i As Integer
            Dim Result As String
            Dim Court As String
            Dim obj As New clsIntelligence()

            Result = ""
            col = Tokenizer(Countries, ",")

            For i = 0 To col.Count - 1
                Court = obj.GetRequiredCourt(CourtLevel, col.Item(i))
                If i = 0 And Court <> "" Then
                    Result &= " ( "
                    Result &= Court & " and country= '" & Trim(col.Item(i)) & "' "
                    Result &= " )"
                    GoTo againLoop
                End If
                If (i > 0 And col.Item(i) <> "" And col.Item(i) <> " " And Court <> "") Then

                    Result &= " or ( "
                    Result &= Court & " and country= '" & Trim(col.Item(i)) & "' "
                    Result &= " )"


                End If

againLoop:
            Next
            'Result &= " ) "
            obj = Nothing
            Return Result
        End Function

        Public Function getCountriesListCases() As String
            Dim objInt As New clsIntelligence()
            Dim Country As New System.Text.StringBuilder()
            Dim CountryList As New ArrayList()
            Dim i As Byte
            Dim Count As Int16
            CountryList = objInt.CountriesForCases1
            Count = CountryList.Count
            For i = 0 To Count - 1
                Country.Append(CountryList.Item(i) & ", ")

            Next

            Return Country.ToString

        End Function

        Public Function Contains_Parser_notDetailed(ByVal sentence As String, ByVal FIELD As String, ByVal LogicOperator As String) As String
            'this means that contains will not have % 


            If sentence.Length < 3 Then
                Return ""

            End If
            Dim strlevel1 As String()
            Dim finallevel1, finallevel2 As String

            Dim strlevel2 As String()



            If FIELD = "level1" Then
                strlevel1 = sentence.Split(",")


                For zz = 0 To strlevel1.Length - 1
                    strlevel1(zz) = " (level1 = '" & strlevel1(zz) & "' and level2 is null )"
                Next
                finallevel1 = String.Join(" or ", strlevel1)



                'sentence = "subj_level1 = '" & sentence.Replace(",", "' or  subj_level1 =  ")


                'for

                Return finallevel1


            ElseIf FIELD = "level2" Then

                strlevel2 = sentence.Split(",")



                For zz = 0 To strlevel2.Length - 1
                    strlevel2(zz) = "(level1 = '" & strlevel2(zz).Split("-")(1) & "' and level2 = '" & strlevel2(zz).Split("-")(0) & "')"
                Next
                finallevel2 = String.Join(" or ", strlevel2)


                Return finallevel2

            End If





            Dim col As New Collections.ArrayList()

            Dim i As Integer
            'Dim ArraySize As Int16
            Dim Count As Int16
            Dim FinalResult As String
            Dim Result As New System.Text.StringBuilder()

            sentence = Me.RefineSentence(sentence)
            sentence = Trim(sentence)
            If sentence = "" Then
                Return " "
            End If
            'Result = Nothing
            col = Tokenizer(sentence, " ")
            'StringArr = Tokenizer1(sentence, " ")
            'ArraySize = StringArr.Length
            Count = col.Count
            For i = 0 To Count - 1
                'For i = 0 To ArraySize - 1
                If (i = 0 And col.Item(i) <> " ") Then
                    Result.Append("( Contains( " & FIELD & " , ' " & Chr(34) & col.Item(i) & Chr(34) & " ")
                    '    Result.Append("( Contains( " & FIELD & " , ' " & Chr(34) & StringArr(i) & Chr(34) & " ")
                    GoTo againLoop
                End If
                If (col.Item(i) <> "" And col.Item(i) <> " ") Then

                    Result.Append(" " & LogicOperator & "  " & Chr(34) & col.Item(i) & Chr(34) & " ")

                End If


againLoop:
            Next
            '' /*Equavalent to murder*/
            'Dim objExtraChk As New clsIntelligence()

            'If objExtraChk.ChkingMurder(col) = True Then
            '    Result &= " OR " & Chr(34) & "murder" & Chr(34) & " "
            'End If


            '        Result &= "' ) )"
            Result.Append("' ) )")
            'objExtraChk = Nothing
            col = Nothing
            i = 0
            FinalResult = Result.ToString
            Result = Nothing
            Return FinalResult
        End Function
        '// proximity searching options
        '1. normal
        '2. with synonyms
        '3. 

        ' function description:
        '               this function is to provide FTS sql query creation for proimity searching 
        '
        ' parameter description:
        '   @sSentence1 = First word or phrase
        '   @sSentence2 = Second word or phrase
        '   @iDistance = distance between sSentence1 and sSentence2
        '   @Field = field to be use for searching eg. booleanText       

        '   return
        '   function will return the part of the query for proximity searching
        Public Function PerpareTokenizer(ByVal Sentence As String) As String
            'System.Windows.Forms.MessageBox.Show(Sentence)
            Dim ResultSen As New System.Text.StringBuilder()

            Dim sPhrase As String = " "
            Dim col As New Collections.ArrayList()
            Dim count As Integer
            col = Tokenizer(Sentence, " ")
            count = col.Count

            For i = 0 To count - 1
                If (i = 0 And col.Item(i) <> " ") Or (col.Item(i) <> "" And col.Item(i) <> " ") Then
                    If (i = count - 1) Then
                        ResultSen.Append(col.Item(i))
                    Else
                        ResultSen.Append(col.Item(i) & ",")
                    End If
                    'GoTo againLoop

                End If


                'againLoop:
            Next
            'System.Windows.Forms.MessageBox.Show(ResultSen.ToString())
            'If (ResultSen.Length > 1) Then
            sPhrase = ResultSen.ToString
            'End If
            Return sPhrase
        End Function
        Public Function Contains_Parser_proximity(ByVal sSentence1 As String, ByVal iDistance As Integer, ByVal FIELD As String) As String
            Dim Result As New System.Text.StringBuilder()
            Dim sPhrase As String


            ''/// if sentence1 or sentence 2 are empty or if there is no distance mentioned then return space
            If (sSentence1 = "") Or (iDistance <= 0) Then
                Return " "
            End If
            Dim words As String() = sSentence1.Split(New Char() {","c})
            If words.Length <= 1 Then
                Return " "
            End If



            sPhrase = "Near((" & sSentence1 & ") ," & iDistance & ", TRUE " & ")"

            'eg. Contains( BOOLEANTEXT , 'NEAR((gleeson, crennan), 50, TRUE)' )   

            Result.Append("( Contains( " & FIELD & " , '" & sPhrase & "' ) )")

            Return Result.ToString
        End Function


        ' function description:
        '               this function is to provide FTS sql query creation for searching with Inflection i.e. murder, murdering...
        '
        ' parameter description:
        '   @sWord = word for inflection
        '   @Field = field to be use for searching eg. booleanText       

        '   return
        '   function will return the part of the query for proximity searching

        Public Function Contains_Parser_Proximity_Inflection(ByVal sWord As String, ByVal FIELD As String) As String
            Dim Result As New System.Text.StringBuilder()
            Dim sPhrase As String
            If (sWord = "") Then
                Return " "
            End If

            sPhrase = "FORMSOF(INFLECTIONAL, " & sWord & ")"

            'eg. Contains( BOOLEANTEXT , 'NEAR((gleeson, crennan), 50, TRUE)' )   

            Result.Append("( Contains( " & FIELD & " , '" & sPhrase & "' ) )")

            Return Result.ToString
        End Function




        ' function description:
        '               this function is to provide FTS sql query creation for searching with Inflection i.e. murder, murdering...
        '
        ' parameter description:
        '   @sWord = word for inflection
        '   @Field = field to be use for searching eg. booleanText       

        '   return
        '   function will return the part of the query for proximity searching

        Public Function Contains_Parser_Proximity_Thesaurus(ByVal sWord As String, ByVal FIELD As String) As String
            Dim Result As New System.Text.StringBuilder()
            Dim sPhrase As String
            '/// if sentence1 or sentence 2 are empty or if there is no distance mentioned then return space
            If (sWord = "") Then
                Return " "
            End If

            sPhrase = "FORMSOF(THESAURUS, " & sWord & ")"

            'eg. Contains( BOOLEANTEXT , 'NEAR((gleeson, crennan), 50, TRUE)' )   

            Result.Append("( Contains( " & FIELD & " , '" & sPhrase & "' ) )")

            Return Result.ToString
        End Function


        Public Function Contains_Parser_operatorSearch(ByVal sentence As String, ByVal FIELD As String) As String

            Dim i As Integer
            Dim FinalResult As String
            Dim Result As New System.Text.StringBuilder()
            Dim sOprAnd As String = "And" '#
            Dim sOprOr As String = "Or" '*
            Dim sOprNot As String = "and Not" '$
            'sentence = Me.RefineSentenceOperatorSearch(sentence)
            sentence = sentence.Replace("+", " + # + ").Replace("-", " - * - ").Replace("\", " \ $ \ ")
            'process and replace before use replace
            'example law-act
            '=============================================
            'sentence = sentence.Replace(".", "^^^")
            sentence = sentence.Replace("'", "''")
            sentence = sentence.Replace("__", "&")
            sentence = sentence.Replace("""", " ")
            '=============================================
            sentence = Trim(sentence)
            If sentence = "" Then
                Return " "
            End If
            sentence = sentence.ToLower
            Dim query As String()
            query = Regex.Split(sentence, "[\\+-]")
            Dim currentword As String
            Result.Append("( Contains( " & FIELD & " , '")
            For i = 0 To query.Count - 1
                If (query(i) <> " " And query(i) <> "") Then
                    currentword = query(i).ToString()
                    currentword = Trim(currentword)
                    '// 1. words
                    If currentword = "#" Then
                        Result.Append(" " & sOprAnd & "  ")
                    ElseIf currentword = "*" Then
                        Result.Append(" " & sOprNot & " ")
                    ElseIf currentword = "$" Then
                        Result.Append(" " & sOprOr & " ")
                    Else
                        Result.Append(" " & Chr(34) & Trim(query(i)) & Chr(34) & " ")
                    End If
                End If
            Next
            Result.Append(" '  ) )    ")
            i = 0
            FinalResult = Result.ToString
            Result = Nothing
            Return FinalResult
        End Function


        Public Function Contains_Parser_notDetailed(ByVal sentence As String, ByVal FIELD As String) As String
            'Dim col As New Collections.ArrayList()
            Dim i As Integer
            Dim ChkSentence As String
            Dim Result As String

            Result = ""
            sentence = sentence.Replace(",", "")
            sentence = sentence.Replace("""", "")
            sentence = sentence.Replace("'", "")

            ChkSentence = Trim(Me.RefineSentence(sentence))


            If ChkSentence = "" Then
                Return " "
            End If

            '/**
            'firstly i m chking if the sentence got all noise words, if it has then exit otherwise
            'the next thing was to chk if first word is noise. if i go with the above only method then
            ' i got 4 records only which means it only chks "jurisdiction court" as "of" is extracted. 
            ' and if i go with with only below method it gives 329 records, which means it chks
            ' "jurisdiction of court" ignoring anything in between jurisdiction and court.
            '*/
            ' sentence = Trim(Me.refineExactSentence(sentence))

            If sentence <> "" Then
                sentence = Chr(34) & sentence & Chr(34)
                Result = " Contains( " & FIELD & " , '" & sentence & "' )"

            End If


            'col = Nothing
            i = 0
            ChkSentence = ""
            sentence = ""

            Return Result
        End Function

        Public Function Contains_Parser_Detailed(ByVal sentence As String, ByVal FIELD As String, ByVal LogicOperator As String) As String
            'this means that contains will have % for more results
            Dim col As New Collections.ArrayList()
            Dim i As Integer
            Dim Result As String

            sentence = Me.RefineSentence(sentence)
            Result = ""
            col = Tokenizer(sentence, " ")


            For i = 0 To col.Count - 1
                If i = 0 Then
                    'Result = "'" & col.Item(i) & "%'"
                    Result = " Contains( " & FIELD & " , '*" & col.Item(i) & "*'" & ")"
                    GoTo againLoop
                End If
                If (col.Item(i) <> "" And col.Item(i) <> " ") Then
                    ' this condition is for stoping search for space or null in collection
                    'EG. contains(field,"data") and contains(field,"data")
                    Result &= " " & LogicOperator & " Contains(" & FIELD & " , '*" & col.Item(i) & "*'" & " )"

                End If

againLoop:
            Next

            Return Result
        End Function

        'Public Function SentenceCase(ByVal ST As String) As String
        '    Dim STLen As Int16
        '    Dim I As Int16
        '    Dim Sentence As String
        '    STLen = Len(ST)
        '    Dim Temp(STLen) As String
        '    Sentence = ST
        '    For I = 1 To STLen - 1
        '        If I = 1 Then
        '            Temp = UCase(Mid(ST, 1, 1))
        '            I = I + 1
        '        End If
        '        If I > 1 Then
        '            Sentence = LCase(Mid(Sentence, 1, I))
        '        End If
        '    Next I


        '    Return Sentence
        'End Function

        Public Shared Function SentenceCase(ByVal ST As String) As String
            'Dim STLen As Int16
            'Dim I As Int16
            Dim Sentence As String
            Dim UChar As String
            'STLen = Len(ST)
            'Dim Temp(STLen) As String
            Sentence = Trim(ST)
            If Len(Sentence) = 0 Then
                Return " "
            End If
            'For I = 0 To Sentence.Length
            'if sentence.IndexOf(sentence,0, 

            '        Next

            UChar = Left(Sentence, 1)
            Sentence = LCase(Right(Sentence, Sentence.Length - 1))
            UChar = UCase(UChar) 'Right(Sentence, 1))
            Sentence = UChar & Sentence

            Return Sentence
        End Function

        Public Function TitleCase(ByVal Sentence As String) As String
            Dim myDelegate As New MatchEvaluator(AddressOf MatchHandler)
            'dim sb as New s
            Dim Pattern As String = "\b(\w)(\w+)?\b"
            Dim RE As New Regex(Pattern, RegexOptions.IgnoreCase Or RegexOptions.Multiline)
            Dim Result As String
            Result = RE.Replace(Sentence, myDelegate)
            Return Result

        End Function
        Private Function MatchHandler(ByVal m As Match) As String
            Return m.Groups(1).Value.ToUpper & m.Groups(2).Value
        End Function

        Public Function getXmlFileSize(ByVal FileName As String) As Long
            Dim FileSize As Long
            Try


                FileOpen(1, FileName, OpenMode.Input) ' Open file.
                FileSize = LOF(1)   ' Get length of file.

            Catch Exp As Exception
                Dim msg As String
                msg = Exp.Message
                msg = ""
            Finally

                FileClose(1)   ' Close file.
            End Try
            Return FileSize
        End Function
        Function SafeSqlLiteral(strValue, intLevel) As String



            ' intLevel represent how thorough the value will be checked for dangerous code
            ' intLevel (1) - Do just the basic. This level will already counter most of the SQL injection attacks
            ' intLevel (2) - &nbsp; (non breaking space) will be added to most words used in SQL queries to prevent unauthorized access to the database. Safe to be printed back into HTML code. Don't use for usernames or passwords

            If Not IsDBNull(strValue) Then
                If intLevel > 0 Then
                    strValue = Replace(strValue, "'", "''") ' Most important one! This line alone can prevent most injection attacks
                    strValue = Replace(strValue, "--", "")
                    strValue = Replace(strValue, "[", "[[]")
                    strValue = Replace(strValue, "%", "[%]")
                    strValue = Replace(strValue, "*", "")
                End If

                If intLevel > 1 Then
                    Dim myArray As Array
                    myArray = Split("xp_ ;update ;insert ;select ;drop ;alter ;create ;rename ;delete ;replace ", ";")
                    Dim i, i2, intLenghtLeft As Integer
                    For i = LBound(myArray) To UBound(myArray)
                        Dim rx As New Regex(myArray(i), RegexOptions.Compiled Or RegexOptions.IgnoreCase)
                        Dim matches As MatchCollection = rx.Matches(strValue)
                        i2 = 0
                        For Each match As Match In matches
                            Dim groups As GroupCollection = match.Groups
                            intLenghtLeft = groups.Item(0).Index + Len(myArray(i)) + i2
                            strValue = Left(strValue, intLenghtLeft - 1) & "&nbsp;" & Right(strValue, Len(strValue) - intLenghtLeft)
                            i2 += 5
                        Next
                    Next
                End If

                'strValue = replace(strValue, ";", ";&nbsp;")
                'strValue = replace(strValue, "_", "[_]")

                Return strValue
            Else
                Return strValue
            End If

        End Function

        Function RefineProxmitysentence(ByVal Sentence As String) As String

            Return Sentence
        End Function



        '///////////////////////64

        Public Shared Function EmailText(ByVal email As String, ByVal subject As String, ByVal message As String) As Boolean
            Dim RetunrMessage As Boolean = False

            Dim CountCases As Integer = 0
            Dim CollectCasetitles As String = ""
            Dim filePath As String = ""

            Try
                Dim mail As New System.Net.Mail.MailMessage()
                Dim SmtpServer As New System.Net.Mail.SmtpClient("mail.elaw.my", 25)
                mail.From = New System.Net.Mail.MailAddress("subscribe@elaw.my", "The Digtal Library-Elaw")
                'rahim@elaw.my and ridhwan@elaw.my instead.
                mail.[To].Add(email)
                mail.[To].Add("ridhwan@elaw.my")
                mail.Bcc.Add("rahim@elaw.my")
                mail.Subject = subject

                mail.IsBodyHtml = True
                mail.Body = message

                SmtpServer.EnableSsl = False
                SmtpServer.UseDefaultCredentials = True
                SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
                SmtpServer.Credentials = New System.Net.NetworkCredential("subscribe@elaw.my", "Subscribe123#")
                SmtpServer.Send(mail)
                RetunrMessage = True

            Catch ex As Exception
                RetunrMessage = False
            End Try

            Return RetunrMessage
        End Function

        Public Shared Function updateQuota(ByVal fn As String, ByVal user As String, ByVal local As String, ByVal remote As String, ByVal table As Short, ByVal sz As Integer) As Boolean
            Dim obj As New clsCasesSearch
            Dim dt As New Data.DataTable
            Dim RemoteAddress As String = remote
            Dim LocalAddress As String = local
            Dim uid As String = ""
            Dim fs As Integer = 0
            Dim ts As Long = 0
            Dim tab As String = ""
            Dim fcol As String = ""
            Dim Dquota As Long = 0
            Dim Mquota As Long = 300000
            Dim message As String = ""
            Dim at As Boolean = False
            Dim hash As Boolean = False
            Dim MailQuery As String = ""
            Dim sql As String = "select user_id_no,AllotedSize from master_users where user_name='" & user & "'"
            If fn(0) = "@" Then
                at = True
                fn = fn.Substring(1)
            ElseIf fn(0) = "#" Then
                hash = True
                fn = fn.Substring(1)
            End If
            dt = obj.ExecuteMyQuery(sql)
            uid = dt.Rows(0).Item(0)
            Mquota = Long.Parse(dt.Rows(0).Item(1)) * 1048576

            Dquota = Mquota / 30
            ' ext = true : xml  , false : pdf ,,,,,, table = 1 : cases, 2: legislation
            If table = 1 Then
                tab = "cases"
            ElseIf table = 2 Then
                tab = "LEGISLATION"
            End If
            If sz = 0 Then
                fcol = "FileSize"
                'Else
                '    fcol = "PDFSize"


                sql = "select " & fcol & " from " & tab & " where DATAFILENAME like '%" & fn.Trim() & "%'"
                dt = obj.ExecuteMyQuery(sql)
                If dt.Rows.Count > 0 Then
                    fs = Integer.Parse(obj.ExecuteMyQuery(sql).Rows(0).Item(0).ToString())
                End If

            Else
                fs = sz
            End If
            If Not Mquota = 0 Then
                sql = "select FileSize from UserLog where User_Name = '" & user & "' and cast(CurrentDateTime as date) = cast(GETDATE() as DATE)"
                dt = obj.ExecuteMyQuery(sql)
                For i = 0 To dt.Rows.Count - 1
                    ts += Long.Parse(dt.Rows(i).Item(0).ToString())
                Next
                If ts <= Dquota And (ts + fs) > Dquota Then
                    ' Get user's Information 
                    MailQuery = GetUserInforEmailQuta("SELECT [Firm_Name],[First_Name],[Last_Name],[Acct_Expiry],[Access_Type],[Phone_Number],[Email_ID],[Group_Name] FROM master_users where [user_name]='" & user & "'")
                    message = "User [" & user & "] has exceeded his/her daily quota by " & Date.Now & "<br/><br/>Quota Info:<br/> Monthly: " & Math.Round(Mquota / 1048576, 2) & " MB <br/> Daily: " & Math.Round(Dquota / 1048576, 2) & " MB<br/>Current: " & Math.Round((ts + fs) / 1048576, 2) & " MB <br>" & MailQuery & " <br/><br/>Thanks,<br/>elaw Administration"
                    clsMyUtility.EmailText("rahim@malaysianlawreview.com", "Exceeded Daily Quota [" & user & "]", message)
                End If
            End If

            sql = "insert into UserLog(User_Id_no, User_Name, Site_Name, DataFilename, FileSize, CurrentDateTime, Local_Addr, RemoteAddr) values('" & uid & "', '" & user & "', 'Elaw', '" & If(at, "@" & fn, If(hash, "#" & fn, fn)) & "', " & fs & ", GETDATE() , '" & LocalAddress & "', '" & RemoteAddress & "')"
            Return obj.UpdateRecord2(sql)

        End Function
        Public Shared Function GetUserInforEmailQuta(ByVal Q As String) As String
            Dim DT As New DataTable
            Dim obj As New clsCasesSearch
            Dim UserInfo As String
            DT = obj.ExecuteMyQuery(Q)
            Try
                UserInfo = "<table style='border-collapse:collapse;border:3px solid black;padding:10px;'>"
                UserInfo &= "<tr><th style='background-color:#eee' colspan='2'>User Information For Exceeing Daily Quota</th></tr>"
                UserInfo &= "<tr><td>Firm Name :</td><td>" & DT.Rows(0).Item("Firm_Name") & "</td></tr>"
                UserInfo &= "<tr><td>First Name :</td><td>" & DT.Rows(0).Item("First_Name") & "</td></tr>"
                UserInfo &= "<tr><td>Last Name :</td><td>" & DT.Rows(0).Item("Last_Name") & "</td></tr>"
                UserInfo &= "<tr><td>Expiry Date :</td><td>" & DT.Rows(0).Item("Acct_Expiry") & "</td></tr>"
                UserInfo &= "<tr><td>Account Type :</td><td>" & DT.Rows(0).Item("Access_Type") & "</td></tr>"
                UserInfo &= "<tr><td>Phone No :</td><td>" & DT.Rows(0).Item("Phone_Number") & "</td></tr>"
                UserInfo &= "<tr><td>Email  :</td><td>" & DT.Rows(0).Item("Email_ID") & "</td></tr>"
                If Not IsDBNull(DT.Rows(0).Item("Group_Name")) Then
                    UserInfo &= "<tr><td>Group Name  :</td>" & DT.Rows(0).Item("Group_Name") & "<td></tr>"
                End If
                UserInfo &= "</table>"

            Catch ex As Exception
                Return String.Empty
            Finally
                DT.Clear()
                obj = Nothing
            End Try
            Return UserInfo
        End Function


        Public Function SplitCitation(ByVal fn As String) As String
            Dim citation, CaseType, JudgementYear, VOL, Storepage As String
            fn = fn.Replace(".xml", "")
            Dim SplitDBfn() As String = fn.Split(New Char() {"_"c})
            If SplitDBfn.Count = 3 Then
                CaseType = SplitDBfn(0)
                JudgementYear = SplitDBfn(1)
                VOL = SplitDBfn(2)
                citation = "[" & JudgementYear & "] " & " " & CaseType & " " & VOL
            Else
                CaseType = SplitDBfn(0)
                JudgementYear = SplitDBfn(1)
                VOL = SplitDBfn(2)
                Storepage = SplitDBfn(3)
                citation = "[" & JudgementYear & "] " & " " & VOL & " " & CaseType & " " & Storepage
            End If

            Return citation
        End Function
        Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
            Return value.Count(Function(c As Char) c = ch)
        End Function
        Public Shared Function RemoveFirstIndex(ByVal value As String, ByVal ch As String) As String
            Dim Sentence As String = value
            Dim singleChar As Char
            Dim parts() = ch.Split(","c)
            For i = 0 To Sentence.Length
                singleChar = Sentence.Chars(0)
                For Each Part In parts
                    If Part = singleChar Then
                        Sentence = Sentence.Remove(0, 1)
                        Exit For
                    End If
                Next
            Next

            Return Sentence
        End Function
#Region " Search Operator w/p, w/s, w/n"
        Public Function GetSearchWithin(ByVal ExPh As String) As Integer
            Dim Found As Boolean = False
            Dim Distance As Integer
            Dim Lst As New List(Of String)
            Dim match As Match
            Lst = ExPh.Split(" ").ToList()
            For i = 0 To Lst.Count - 1
                match = Regex.Match(Lst.Item(i).ToLower(), "w/([0-9\-]+)$")
                If match.Success Then
                    Distance = match.Groups(1).Value
                ElseIf Lst.Item(i).ToLower = "w/p" Then
                    Distance = 2 ' distance varialbe for paragraph 
                ElseIf Lst.Item(i).ToLower = "w/s" Then
                    Distance = 1  ' distance varialbe for Sentence 
                End If
            Next
            Return Distance
        End Function
        Public Function CheckReaptedOp(ByVal ExPh As String) As String
            Dim processExph As String = ExPh.ToLower()
            Dim Lst As New List(Of String)
            Dim Indexcol As New ArrayList
            Dim NumberOfTrues As Integer = Regex.Matches(processExph, "w/").Count
            Lst = processExph.Split(" ").ToList()
            Dim match As Match
            If NumberOfTrues > 1 Then
                For i = 0 To Lst.Count - 1
                    match = Regex.Match(Lst.Item(i).ToLower(), "w/([0-9\-]+)$")
                    If match.Success Then
                        Indexcol.Add("w/" & match.Groups(1).Value)
                    ElseIf Lst.Item(i).ToLower = "w/p" Then
                        Indexcol.Add("w/p")
                    ElseIf Lst.Item(i).ToLower = "w/s" Then
                        Indexcol.Add("w/s")
                    End If
                Next
                For rr = 1 To Indexcol.Count - 1
                    processExph = processExph.Replace(Indexcol.Item(rr), "")
                Next
            End If

            Return processExph
        End Function
        Public Function ClearOprator(ByVal ExPh As String) As String
            Dim processExph As String = ExPh.ToLower()
            Dim Indexcol As New ArrayList
            Dim Lst As New List(Of String)
            Lst = processExph.Split(" ").ToList()
            Dim match As Match
            For i = 0 To Lst.Count - 1
                match = Regex.Match(Lst.Item(i).ToLower(), "w/([0-9\-]+)$")
                If match.Success Then
                    Indexcol.Add("w/" & match.Groups(1).Value)
                ElseIf Lst.Item(i).ToLower = "w/p" Then
                    Indexcol.Add("w/p")
                ElseIf Lst.Item(i).ToLower = "w/s" Then
                    Indexcol.Add("w/s")
                End If
            Next
            For rr = 0 To Indexcol.Count - 1
                processExph = processExph.Replace(Indexcol.Item(rr), "")
            Next
            Return processExph
        End Function


#End Region


    End Class

End Namespace
