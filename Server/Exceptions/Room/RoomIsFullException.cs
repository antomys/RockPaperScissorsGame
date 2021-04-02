namespace Server.Exceptions.Room
{
    public class RoomIsFullException : RoomException
    {
        public RoomIsFullException()
        {
            
        }

        public RoomIsFullException(string message) : base(string.Format($"This room is full {message}!"))
        {
            
        }
    }
}