namespace Server.Bll.Exceptions;

public static class ExceptionTemplates
{
    // GENERAL EXCEPTION MESSAGES
    public const string Unknown = "Unknown error occured. Please try again";
    public const string NotAllowed = "You are not allowed to do this.";
        
    // ROOM EXCEPTIONS
    public const string TwinkRoom = "Failed to create one more game when you are sitting in another room.";
    public const string RoomNotExists = "Room does not exist.";
    public const string RoomFull = "This room is full.";
    public const string AlreadyInRoom = "You are already in room.";
    public const string RoomNotFull = "Room is not full.";
    public const string NoAvailableRooms = "Sorry, there are no public rooms available right now.";
        
    // ROUND EXCEPTIONS
    public const string RoundAlreadyCreated = "Round is already creaded.";
    public const string RoundNotFound = "Round with id \"{0}\" is not found";
}