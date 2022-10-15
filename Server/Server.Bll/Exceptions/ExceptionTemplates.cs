namespace Server.Bll.Exceptions;

internal static class ExceptionTemplates
{
    // GENERAL EXCEPTION MESSAGES
    internal const string Unknown = "Unknown error occured. Please try again";
    internal const string NotAllowed = "You are not allowed to do this.";
    internal static string NotExists(string entity) => $"{entity} does not exist.";


    // ROOM EXCEPTIONS
    internal const string TwinkRoom = "Failed to create one more game when you are sitting in another room.";
    internal const string RoomFull = "This room is full.";
    internal const string AlreadyInRoom = "You are already in room.";
    internal const string RoomNotFull = "Room is not full.";
    internal const string NoAvailableRooms = "Sorry, there are no public rooms available right now.";
        
    // ROUND EXCEPTIONS
    internal const string RoundAlreadyCreated = "Round is already creaded.";
    internal static string RoundNotFound(int roundId) => $"Round with id \"{roundId}\" is not found";
}