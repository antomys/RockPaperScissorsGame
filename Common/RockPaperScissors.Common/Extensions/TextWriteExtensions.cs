using System.Text.RegularExpressions;
using RockPaperScissors.Common.Enums;

namespace RockPaperScissors.Common.Extensions;

public static class TextWriteExtensions
{
    private static readonly Regex EmailRegex =
        new("[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}", RegexOptions.Compiled);
    
    public static void Print(this string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    public static string BuildString(this string msg, StringDestination destination, bool isNeedConfirmation = false)
    {
        string output;
        while (true)
        {
            var passwordNotConfirmed = true;
            Print(
                destination is StringDestination.PassportType or StringDestination.Email
                    ? $"What is your {msg}?"
                    : $"Try to come up with {msg}?", ConsoleColor.Yellow);
            Console.Write($"{msg}--> ");
                
            output = Console.ReadLine()
                ?.Trim()
                ?.Replace(" ", "");
            if (string.IsNullOrEmpty(output))
            {
                Print("Wrong data!", ConsoleColor.Red);
                continue;
            }
            switch (destination)
            {
                case StringDestination.Password when output.Length < 6:
                    Print("Wrong password length!", ConsoleColor.Red);
                    continue;
                case StringDestination.Email when !IsEmailValid(output):
                    Print("This email is not valid!", ConsoleColor.Red);
                    continue;
            }

            if (destination == StringDestination.Password)
            {
                if (isNeedConfirmation)
                    break;
                Print("You need to confirm password!", ConsoleColor.Yellow);
                do
                {
                    Console.Write("Confirmation--> ");
                    var confirmationPassword = Console.ReadLine()
                        ?.Trim()
                        .Replace(" ", "");
                    if (string.IsNullOrEmpty(output))
                    {
                        Print("Wrong data!", ConsoleColor.Red);
                        continue;
                    }
                    if (output == confirmationPassword)
                    {
                        Print("Password confirmed", ConsoleColor.Green);
                        passwordNotConfirmed = false;
                    }
                    else
                        Print("Passwords dont match!",ConsoleColor.Red);
                } while (passwordNotConfirmed);
            }
            if (destination is StringDestination.PassportType && ContainsDigits(output))
            {
                Print("You cannot enter nameType with digits!", ConsoleColor.Red);
                continue;
            }
            break;
        }
        
        return output;
    }
    
    public static bool ContainsDigits(this string str)
    {
        var res = str.Any(character => char.IsDigit(character));
        
        return res;
    }
    
    public static bool IsEmailValid(this string email)
    {
        var match = EmailRegex.Match(email);
        
        return match.Success;
    }
}