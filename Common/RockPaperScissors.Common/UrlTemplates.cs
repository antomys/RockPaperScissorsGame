namespace RockPaperScissors.Common;

public static class UrlTemplates
{
    // Account-related
    public const string RegisterUrl = "api/account/register";
    public const string LoginUrl = "api/account/login";
    public const string LogoutUrl = "api/account/logout";
    
    // Statistics-related
    public const string AllStatistics = "api/statistics/all";
    public const string PersonalStatistics = "api/statistics/personal";

    // Room-related
    public const string CreateRoom = "api/room/create";
    public const string JoinPublicRoom = "api/room/public/join";
    public const string JoinPrivateRoom = "api/room/private/{roomCode}/join";
    public const string UpdateRoom = "api/room/update";
    public const string DeleteRoom = "api/room/delete";
    public const string CheckRoomUpdateTicks = "api/room/{roomId}/status";
    
    // Round-related
    public const string CreateRound = "api/round/create";
    public const string UpdateRound = "api/round/update";
    public const string CheckRoundUpdateTicks = "api/round/{roundId}/status";
}