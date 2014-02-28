#Region "Using"

Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO.Compression
Imports System.Web.Caching
Imports System.Net

#End Region

Public Class JavaScriptHandler
    Implements IHttpHandler.IsReusable
    ''' <summary>
    ''' Enables processing of HTTP Web requests by a custom 
    ''' HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
    ''' </summary>
    ''' <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
    ''' references to the intrinsic server objects 
    ''' (for example, Request, Response, Session, and Server) used to service HTTP requests.
    ''' </param>
    Public Sub ProcessRequest(context As HttpContext)
        Dim path As String = context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.QueryString("path")
        Dim script As String = Nothing

        If Not String.IsNullOrEmpty(path) Then
            If context.Cache(path) Is Nothing Then
                script = RetrieveScript(path)
            End If
        End If

        script = DirectCast(context.Cache(path), String)
        If Not String.IsNullOrEmpty(script) Then
            context.Response.Write(script)
            SetHeaders(script.GetHashCode(), context)

            Compress(context)
        End If
    End Sub

    ''' <summary>
    ''' Retrieves the specified remote script using a WebClient.
    ''' </summary>
    ''' <param name="file">The remote URL</param>
    Private Shared Function RetrieveScript(file As String) As String
        Dim script As String = Nothing

        Try
            Dim url As New Uri(file, UriKind.Absolute)

            Using client As New WebClient()
                Using buffer As Stream = client.OpenRead(url)
                    Using reader As New StreamReader(buffer)
                        script = StripWhitespace(reader.ReadToEnd())
                        HttpContext.Current.Cache.Insert(file, script, Nothing, Cache.NoAbsoluteExpiration, New TimeSpan(7, 0, 0, 0))
                    End Using
                End Using
            End Using
            ' The remote site is currently down. Try again next time.
        Catch generatedExceptionName As System.Net.Sockets.SocketException
            ' Only valid absolute URLs are accepted
        Catch generatedExceptionName As UriFormatException
        End Try

        Return script
    End Function

    ''' <summary>
    ''' Strips the whitespace from any .js file.
    ''' </summary>
    Private Shared Function StripWhitespace(body As String) As String
        Dim lines As String() = body.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
        Dim sb As New StringBuilder()
        For Each line As String In lines
            Dim s As String = line.Trim()
            If s.Length > 0 AndAlso Not s.StartsWith("//") Then
                sb.AppendLine(s.Trim())
            End If
        Next

        body = sb.ToString()
        body = Regex.Replace(body, "^[\s]+|[ \f\r\t\v]+$", [String].Empty)
        body = Regex.Replace(body, "([+-])\n\1", "$1 $1")
        body = Regex.Replace(body, "([^+-][+-])\n", "$1")
        body = Regex.Replace(body, "([^+]) ?(\+)", "$1$2")
        body = Regex.Replace(body, "(\+) ?([^+])", "$1$2")
        body = Regex.Replace(body, "([^-]) ?(\-)", "$1$2")
        body = Regex.Replace(body, "(\-) ?([^-])", "$1$2")
        body = Regex.Replace(body, "\n([{}()[\],<>/*%&|^!~?:=.;+-])", "$1")
        body = Regex.Replace(body, "(\W(if|while|for)\([^{]*?\))\n", "$1")
        body = Regex.Replace(body, "(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n", "$1$3")
        body = Regex.Replace(body, "([;}]else)\n", "$1 ")
        body = Regex.Replace(body, "(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", [String].Empty)

        Return body
    End Function

    ''' <summary>
    ''' This will make the browser and server keep the output
    ''' in its cache and thereby improve performance.
    ''' </summary>
    Private Shared Sub SetHeaders(hash As Integer, context As HttpContext)
        context.Response.ContentType = "text/javascript"
        context.Response.Cache.VaryByHeaders("Accept-Encoding") = True

        context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(7))
        context.Response.Cache.SetCacheability(HttpCacheability.[Public])
        context.Response.Cache.SetMaxAge(New TimeSpan(7, 0, 0, 0))
        context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
        context.Response.Cache.SetETag("""" + hash.ToString() + """")
    End Sub

#Region "Compression"

    Private Const GZIP As String = "gzip"
    Private Const DEFLATE As String = "deflate"

    Private Shared Sub Compress(context As HttpContext)
        If context.Request.UserAgent IsNot Nothing AndAlso context.Request.UserAgent.Contains("MSIE 6") Then
            Return
        End If

        If IsEncodingAccepted(GZIP) Then
            context.Response.Filter = New GZipStream(context.Response.Filter, CompressionMode.Compress)
            SetEncoding(GZIP)
        ElseIf IsEncodingAccepted(DEFLATE) Then
            context.Response.Filter = New DeflateStream(context.Response.Filter, CompressionMode.Compress)
            SetEncoding(DEFLATE)
        End If
    End Sub

    ''' <summary>
    ''' Checks the request headers to see if the specified
    ''' encoding is accepted by the client.
    ''' </summary>
    Private Shared Function IsEncodingAccepted(encoding As String) As Boolean
        Return HttpContext.Current.Request.Headers("Accept-encoding") IsNot Nothing AndAlso HttpContext.Current.Request.Headers("Accept-encoding").Contains(encoding)
    End Function

    ''' <summary>
    ''' Adds the specified encoding to the response headers.
    ''' </summary>
    ''' <param name="encoding"></param>
    Private Shared Sub SetEncoding(encoding As String)
        HttpContext.Current.Response.AppendHeader("Content-encoding", encoding)
    End Sub

#End Region

    ''' <summary>
    ''' Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
    ''' </summary>
    ''' <value></value>
    ''' <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
    Public ReadOnly Property IsReusable() As Boolean
        Get
            Return False
        End Get
    End Property

End Class


''' <summary>
''' Removes whitespace from the webpage.
''' </summary>
Public Class CompressWebResource
    Implements IHttpModule

#Region "IHttpModule Members"

    Private Sub Dispose() Implements IHttpModule.Dispose
        ' Nothing to dispose; 
    End Sub

    Private Sub Init(context As HttpApplication) Implements IHttpModule.Init

        AddHandler context.PostRequestHandlerExecute, AddressOf context_BeginRequest
    End Sub

#End Region

    Private Sub context_BeginRequest(sender As Object, e As EventArgs)
        Dim app As HttpApplication = TryCast(sender, HttpApplication)
        If TypeOf app.Context.CurrentHandler Is Page Then
            app.Response.Filter = New WebResourceFilter(app.Response.Filter)
        End If
    End Sub

#Region "Stream filter"

    Private Class WebResourceFilter
        Inherits Stream

        Public Sub New(sink As Stream)
            _sink = sink
        End Sub

        Private _sink As Stream

#Region "Properites"

        Public Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides Sub Flush()
            _sink.Flush()
        End Sub

        Public Overrides ReadOnly Property Length() As Long
            Get
                Return 0
            End Get
        End Property

        Private _position As Long
        Public Overrides Property Position() As Long
            Get
                Return _position
            End Get
            Set(value As Long)
                _position = value
            End Set
        End Property

#End Region

#Region "Methods"

        Public Overrides Function Read(buffer As Byte(), offset As Integer, count As Integer) As Integer
            Return _sink.Read(buffer, offset, count)
        End Function

        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
            Return _sink.Seek(offset, origin)
        End Function

        Public Overrides Sub SetLength(value As Long)
            _sink.SetLength(value)
        End Sub

        Public Overrides Sub Close()
            _sink.Close()
        End Sub

        Public Overrides Sub Write(buffer__1 As Byte(), offset As Integer, count As Integer)
            Dim data As Byte() = New Byte(count - 1) {}
            Buffer.BlockCopy(buffer__1, offset, data, 0, count)
            Dim html As String = System.Text.Encoding.[Default].GetString(buffer__1)

            Dim regex As New Regex("<script\s*src=""((?=[^""]*webresource.axd)[^""]*)""\s*type=""text/javascript""[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase)
            For Each match As Match In regex.Matches(html)
                Dim relative As String = match.Groups(1).Value
                html = html.Replace(relative, "js.axd?path=" + HttpUtility.UrlEncode(relative))
            Next

            Dim outdata As Byte() = System.Text.Encoding.[Default].GetBytes(html)
            _sink.Write(outdata, 0, outdata.GetLength(0))
        End Sub

#End Region

    End Class

#End Region

End Class