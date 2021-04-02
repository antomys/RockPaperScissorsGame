namespace Server.Exceptions.Room
{
    public class RoomNotFoundException : RoomException
    {
        public RoomNotFoundException()
        {
            
        }

        public RoomNotFoundException(string message) : base(string.Format($"Room is not found {message} !"))
        {
            
        }
    }
}