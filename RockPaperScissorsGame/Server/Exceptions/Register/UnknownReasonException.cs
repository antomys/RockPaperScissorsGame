using System;

namespace Server.Exceptions.Register
{
    public class UnknownReasonException : Exception
    {
        public UnknownReasonException()
        {
            
        }

        public UnknownReasonException(string message) :
            base(message)
        {
            
        }

        public UnknownReasonException(string message, Exception inner) :
            base(message, inner)
        {
            
        }
    }
}