using System;

namespace Server.Exceptions.Register
{
    public class UnknownReasonException : Exception
    {
        public UnknownReasonException()
        {
            
        }

        public UnknownReasonException(string message) :
            base(string.Format($"Unknown reason {message}"))
        {
            
        }

        public UnknownReasonException(string message, Exception inner) :
            base(message, inner)
        {
            
        }
    }
}