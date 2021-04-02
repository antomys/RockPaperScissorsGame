using System;

namespace Server.Exceptions.Room
{
    public class InvalidRoomIdException : RoomException
    {
        public InvalidRoomIdException()
        {
            
        }

        public InvalidRoomIdException(string message) : base(string.Format($"Invalid room id: {message}!"))
        {
            
        }
    }
}