using System;

namespace Server.Bll.Exceptions
{
    public class LoginErrorException : Exception
    {
        protected LoginErrorException()
        {
            
        }

        protected LoginErrorException(string message) :
            base(message)
        {
            
        }

        public LoginErrorException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}