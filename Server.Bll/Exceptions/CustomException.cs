using Microsoft.AspNetCore.Http;

namespace Server.Bll.Exceptions;

public sealed class CustomException
{
    public int Code { get;}
    public string Message { get; }

    public CustomException(string template, int code = StatusCodes.Status400BadRequest)
    {
        Message = template;
        Code = code;
    }
}