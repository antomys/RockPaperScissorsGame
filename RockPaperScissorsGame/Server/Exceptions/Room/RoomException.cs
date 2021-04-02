using System;

namespace Server.Exceptions.Room
{
    public class RoomException : Exception
    {
        public RoomException()
        {
            
        }

        public RoomException(string message) :
            base(message)
        {
            
        }

        public RoomException(string message, Exception inner) :
            base(message, inner)
        {
            
        }
    }
}