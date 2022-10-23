using Client.StartMenu.Enums;

namespace Client.StartMenu.Extensions;

internal static class EnumExtensions
{
    internal static string GetDisplayName(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Account => "Account",
            MenuTypes.Leaderboard => "Leaderboard",
            MenuTypes.Exit => "Exit",
            _ => "Unknown",
        };
    }
    
    internal static MenuTypes TryGetMenuType(this string? stringInput)
    {
        return stringInput switch
        {
            "3" => MenuTypes.Exit,
            "2" => MenuTypes.Leaderboard,
            "1" => MenuTypes.Account,
            _ => MenuTypes.Unknown,
        };
    }
    
    internal static int GetValue(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Exit => 3,
            MenuTypes.Leaderboard => 2,
            MenuTypes.Account => 1,
            _ => 0,
        };
    }
}