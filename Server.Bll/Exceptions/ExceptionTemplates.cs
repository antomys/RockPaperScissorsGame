namespace Server.Bll.Exceptions
{
    public static class ExceptionTemplates
    {
        public const string Unknown = "Unknown error occured. Please try again";
        public const string TwinkRoom = "Failed to create one more game when you are sitting in another room.";
        public const string RoomNotExists = "Room does not exist.";
        public const string RoomFull = "This room is full.";
        public const string AlreadyInRoom = "You are already in room.";
    }
}