using Client.Statistics.Enums;

namespace Client.Statistics.EnumExtensions;

internal static class EnumExtensions
{
    internal static string GetDisplayName(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Back => "Back",
            MenuTypes.All => "Top 10 Users",
            MenuTypes.Personal => "Personal statistics",
            _ => "Unknown",
        };
    }
    
    internal static MenuTypes TryGetMenuType(this string? stringInput)
    {
        return stringInput switch
        {
            "3" => MenuTypes.Back,
            "2" => MenuTypes.All,
            "1" => MenuTypes.Personal,
            _ => MenuTypes.Unknown,
        };
    }
    
    internal static int GetValue(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Back => 3,
            MenuTypes.All => 2,
            MenuTypes.Personal => 1,
            _ => 0,
        };
    }
}