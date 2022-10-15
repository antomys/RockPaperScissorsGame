using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Server.Host.Extensions;

internal sealed class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
        
    public LoggingMiddleware(
        RequestDelegate next,
        ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
    }
        
    public async Task Invoke(HttpContext context)
    {
        var requestInformation = await BuildLogAsync(context);

        _logger.LogWarning(requestInformation);

        var originalResponseBody = context.Response.Body;
        await using var responseBody = new MemoryStream();
            
        context.Response.Body = responseBody;
        await _next(context);

        var status = GetStatusCode(context);
        var level = GetLogLevel(status);

        _logger.Log(level, "Response body: LogLevel: {Enum}; Code: {Status}\n Body: {Body}",
            Enum.GetName(GetLogLevel(status)), status, await ObtainResponseBodyAsync(context));

        await responseBody.CopyToAsync(originalResponseBody);

    }

    private static async Task<string> ObtainRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        var encoding = GetEncodingFromContentType(request.ContentType);
        string bodyStr;

        using (var reader = new StreamReader(request.Body, encoding, true, 1024, true))
        {
            bodyStr = await reader.ReadToEndAsync().ConfigureAwait(false);
        }
        
        request.Body.Seek(0, SeekOrigin.Begin);
        return bodyStr;
    }
    
    private static async Task<string> ObtainResponseBodyAsync(HttpContext context)
    {
        var response = context.Response;
        response.Body.Seek(0, SeekOrigin.Begin);
        
        var encoding = GetEncodingFromContentType(response.ContentType);
        
        using var reader = new StreamReader(response.Body, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
        
        var text = await reader.ReadToEndAsync().ConfigureAwait(false);
        response.Body.Seek(0, SeekOrigin.Begin);
        
        return text;
    }
    
    private static Encoding GetEncodingFromContentType(string? contentTypeStr)
    {
        if (string.IsNullOrEmpty(contentTypeStr))
        {
            return Encoding.UTF8;
        }
        ContentType contentType;
        
        try
        {
            contentType = new ContentType(contentTypeStr);
        }
        catch (FormatException)
        {
            return Encoding.UTF8;
        }
        
        return string.IsNullOrEmpty(contentType.CharSet) 
            ? Encoding.UTF8 
            : Encoding.GetEncoding(contentType.CharSet, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
    }
    private static LogLevel GetLogLevel(int? statusCode)
    {
        return statusCode > 399 ? LogLevel.Error : LogLevel.Information;
    }
    private static int? GetStatusCode(HttpContext context)
    {
        return context.Response.StatusCode;
    }

    private static async Task<string> BuildLogAsync(HttpContext context)
    {
        var requestBody = await ObtainRequestBodyAsync(context.Request);
        
        var length = 89 +
                     context.Request.Scheme.Length +
                     context.Request.ContentType?.Length +
                     context.Request.Host.Host.Length +
                     (context.Request.Path.HasValue ? context.Request.Path.Value.Length : 0) +
                     (context.Request.QueryString.HasValue ? context.Request.QueryString.Value!.Length : 0) +
                     requestBody.Length ?? 0;

        if (length <= 88)
        {
            return string.Empty;
        }

        return string.Create(length, (context, requestBody), (span, tuple) =>
        {
            var index = 0;

            var (thisContext, thisRequestBody) = tuple;
            
            var tempString = "Request information:\n";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            tempString = "Schema:";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            thisContext.Request.Scheme.CopyTo(span[index..]);
            index += thisContext.Request.Scheme.Length;

            "\n".CopyTo(span[index++..]);

            tempString = "Content-Type:";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            thisContext.Request.ContentType?.CopyTo(span[index..]);
            index += thisContext.Request.ContentType?.Length ?? 0;

            "\n".CopyTo(span[index++..]);

            tempString = "Host:";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            thisContext.Request.Host.Host.CopyTo(span[index..]);
            index += thisContext.Request.Host.Host.Length;

            "\n".CopyTo(span[index++..]);

            tempString = "Path:";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            thisContext.Request.Path.Value?.CopyTo(span[index..]);
            index += thisContext.Request.Path.Value?.Length ?? 0;

            "\n".CopyTo(span[index++..]);

            tempString = "Query String:";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            thisContext.Request.QueryString.Value?.CopyTo(span[index..]);
            index += thisContext.Request.QueryString.Value?.Length ?? 0;

            "\n".CopyTo(span[index++..]);

            tempString = "Request Body:";
            tempString.CopyTo(span[index..]);
            index += tempString.Length;

            thisRequestBody.CopyTo(span[index..]);
            index += thisRequestBody.Length;

            "\n".CopyTo(span[index..]);
        });
    }
}