using Microsoft.AspNetCore.Http;

namespace Server.Bll.Exceptions;

public readonly struct CustomException
{
    public int Code { get;}
    public string Message { get; }

    public CustomException(string template, int code = StatusCodes.Status400BadRequest)
    {
        Message = template;
        Code = code;
    }

    public CustomException(string template, string customObject, int code = StatusCodes.Status400BadRequest)
    {
        Message = string.Format(template, customObject);
        Code = code;
    }
}