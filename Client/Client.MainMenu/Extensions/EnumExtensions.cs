using Client.MainMenu.Enums;

namespace Client.MainMenu.Extensions;

internal static class EnumExtensions
{
    internal static string GetDisplayName(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.PersonalScoreboard => "Personal Statistics",
            MenuTypes.CreateRoom => "Create room",
            MenuTypes.Logout => "Logout",
            MenuTypes.Exit => "Exit",
            _ => "Unknown",
        };
    }
    
    internal static MenuTypes TryGetMenuType(this string? stringInput)
    {
        return stringInput switch
        {
            "4" => MenuTypes.Exit,
            "3" => MenuTypes.Logout,
            "2" => MenuTypes.CreateRoom,
            "1" => MenuTypes.PersonalScoreboard,
            _ => MenuTypes.Unknown,
        };
    }
    
    internal static int GetValue(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Exit => 4,
            MenuTypes.Logout => 3,
            MenuTypes.CreateRoom => 2,
            MenuTypes.PersonalScoreboard => 1,
            _ => 0,
        };
    }
}