using System;
using Microsoft.AspNetCore.Http;

namespace Server.Authentication.Exceptions
{
    public readonly struct UserException
    {
        public int Code { get; }
        public string Message { get; }
        
        public UserException(string messageType, string message, DateTimeOffset dateTimeOffset)
        {
            Code = StatusCodes.Status400BadRequest;
            Message = string.Format(messageType, message, dateTimeOffset.ToString("f"));
        }

        public UserException(string invalidCredentialsForUser, string loginName)
        {
            Code = StatusCodes.Status400BadRequest;
            Message = string.Format(invalidCredentialsForUser, loginName);
        }
    }
}