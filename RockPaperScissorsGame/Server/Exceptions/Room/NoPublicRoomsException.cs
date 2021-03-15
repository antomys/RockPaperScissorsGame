namespace Server.Exceptions.Room
{
    public class NoPublicRoomsException : RoomException
    {
        public NoPublicRoomsException()
        {
            
        }

        public NoPublicRoomsException(string message) : base(string.Format($"No public rooms {message}!"))
        {
            
        }
    }
}