using System;
using Client.Models;
using Client.Validations;

namespace Client.Services
{
    internal class StringPlaceholder
    {
        private readonly StringDestination _destination;
        public StringPlaceholder()
        {
            _destination = StringDestination.Login;
        }
        public StringPlaceholder(StringDestination destination)
        {
            _destination = destination;
        }
        public string BuildString(string msg, bool isNeedConfirmation = false)
        {
            string output;
            while (true)
            {
                var passwordNotConfirmed = true;
                TextWrite.Print(
                    _destination is StringDestination.PassportType or StringDestination.Email
                        ? $"What is your {msg}?"
                        : $"Try to come up with {msg}?", ConsoleColor.Yellow);
                Console.Write($"{msg}--> ");
                
                output = Console.ReadLine()
                    ?.Trim()
                    .Replace(" ", "");
                if (string.IsNullOrEmpty(output))
                {
                    TextWrite.Print("Wrong data!", ConsoleColor.Red);
                    continue;
                }
                switch (_destination)
                {
                    case StringDestination.Password when output.Length < 6:
                        TextWrite.Print("Wrong password length!", ConsoleColor.Red);
                        continue;
                    case StringDestination.Email when !StringValidator.IsEmailValid(output):
                        TextWrite.Print("This email is not valid!", ConsoleColor.Red);
                        continue;
                }

                if (_destination == StringDestination.Password)
                {
                    if (isNeedConfirmation)
                        break;
                    TextWrite.Print("You need to confirm password!", ConsoleColor.Yellow);
                    do
                    {
                        Console.Write("Confirmation--> ");
                        var confirmationPassword = Console.ReadLine()
                            ?.Trim()
                            .Replace(" ", "");
                        if (string.IsNullOrEmpty(output))
                        {
                            TextWrite.Print("Wrong data!", ConsoleColor.Red);
                            continue;
                        }
                        if (output == confirmationPassword)
                        {
                            TextWrite.Print("Password confirmed", ConsoleColor.Green);
                            passwordNotConfirmed = false;
                        }
                        else
                            TextWrite.Print("Passwords dont match!",ConsoleColor.Red);
                    } while (passwordNotConfirmed);
                }
                if (_destination == StringDestination.PassportType && StringValidator.IsStringContainsDigits(output))
                {
                    TextWrite.Print("You cannot enter nameType with digits!", ConsoleColor.Red);
                    continue;
                }
                break;
            }
            return output;
        }
    }
}
