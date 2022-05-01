using System;
using Microsoft.AspNetCore.Http;

namespace Server.Authentication.Exceptions;

public readonly struct UserException
{
    public int Code { get; }
    public string Message { get; }
        
    public UserException(string message)
    {
        Code = StatusCodes.Status400BadRequest;
        Message = message;
    }

    public UserException(string invalidCredentialsForUser, string loginName)
    {
        Code = StatusCodes.Status400BadRequest;
        Message = string.Format(invalidCredentialsForUser, loginName);
    }
}