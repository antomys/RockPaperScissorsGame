using Client.Account.Enums;

namespace Client.Account.Extensions;

internal static class EnumExtensions
{
    internal static string GetDisplayName(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Back => "Back",
            MenuTypes.Login => "Login",
            MenuTypes.SignUp => "Sign Up",
            _ => "Unknown",
        };
    }
    
    internal static MenuTypes TryGetMenuType(this string? stringInput)
    {
        return stringInput switch
        {
            "3" => MenuTypes.Back,
            "2" => MenuTypes.Login,
            "1" => MenuTypes.SignUp,
            _ => MenuTypes.Unknown,
        };
    }
    
    internal static int GetValue(this MenuTypes menuTypes)
    {
        return menuTypes switch
        {
            MenuTypes.Back => 3,
            MenuTypes.Login => 2,
            MenuTypes.SignUp => 1,
            _ => 0,
        };
    }
}