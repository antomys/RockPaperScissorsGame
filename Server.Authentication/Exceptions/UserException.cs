using System;
using System.Net;

namespace Server.Authentication.Exceptions
{
    public readonly struct UserException
    {
        public int Code { get; }
        public string Message { get; }
        
        public UserException(string messageType, string message, DateTimeOffset dateTimeOffset)
        {
            Code = (int) HttpStatusCode.BadRequest;
            Message = string.Format(messageType, message, dateTimeOffset);
        }

        public UserException(string invalidCredentialsForUser, string loginName)
        {
            Code = (int) HttpStatusCode.BadRequest;
            Message = string.Format(invalidCredentialsForUser, loginName);
        }
    }
}