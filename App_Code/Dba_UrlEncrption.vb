Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Public Class Dba_UrlEncrption
    Public UrlQuery As String = ""
    Public UrlEncrypt As String = ""
    Public UrlDecrypt As String = ""
    Private EncrptKey As String = "1239;[pewGKG)abcdefghiklmnopqrstuvwxyz"
    Public Structure urlbreaker
        Dim parvalue As String
        Dim parvar As String
        Dim exist As Boolean
    End Structure
    Public Sub New(ByVal myurl As String, ByVal cryptiontype As Boolean)
        UrlQuery = myurl
        If cryptiontype = True Then
            UrlEncrypt = Encrypt_QS()
        Else
            UrlDecrypt = Decrypt_QS()

        End If
        
    End Sub
    Public Function Encrypt_QS() As String
        Dim str As String = UrlQuery
        Dim byKey As Byte() = {}
        Dim IV As Byte() = {18, 52, 86, 120, 144, 171, 205, 239}
        byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8))
        Dim des As New DESCryptoServiceProvider()
        Dim inputByteArray As Byte() = Encoding.UTF8.GetBytes(str)
        Dim ms As New MemoryStream()
        Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
        cs.Write(inputByteArray, 0, inputByteArray.Length)
        cs.FlushFinalBlock()
        Return Convert.ToBase64String(ms.ToArray())
    End Function
    Public Function Decrypt_QS() As String
        Dim str As String = UrlQuery
        str = str.Replace(" ", "+")
        Try
            Dim bykey As Byte() = {}
            Dim Iv As Byte() = {18, 52, 86, 120, 144, 171, 205, 239}
            Dim inputByteArray As Byte() = New Byte(str.Length - 1) {}
            bykey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(str)
            Dim MS As New MemoryStream
            Dim cs As New CryptoStream(MS, des.CreateDecryptor(bykey, Iv), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(MS.ToArray())
        Catch ex As Exception
            Return ""
        End Try

    End Function

    Public Function UrlDetails() As List(Of urlbreaker)
        Dim myvars As urlbreaker
        Dim myvardetials As New List(Of urlbreaker)
        Dim query As String()
        If UrlDecrypt <> "" Then
            query = Regex.Split(UrlDecrypt, "[&=]")
            For i As Integer = 0 To query.Length - 1 Step 2
                myvars.parvar = query(i)
                myvars.parvalue = query(i + 1)
                If myvars.parvalue <> "" Then
                    myvars.exist = True
                Else
                    myvars.exist = False
                End If
                myvardetials.Add(myvars)
            Next
        End If
        Return myvardetials
    End Function
End Class
