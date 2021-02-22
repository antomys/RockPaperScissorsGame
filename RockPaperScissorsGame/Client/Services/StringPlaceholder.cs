using System;
using System.Collections.Generic;
using System.Text;
using Client.Models;
using Client.Validations;

namespace Client.Services
{
    class StringPlaceholder
    {
        private StringDestination destination;
        public StringPlaceholder()
        {
            destination = StringDestination.Login;
        }
        public StringPlaceholder(StringDestination destination)
        {
            this.destination = destination;
        }
        public string BuildNewSpecialDestinationString(string msg)
        {
            string output;
            while (true)
            {
                bool passwordNotConfirmed = true;
                if(destination == StringDestination.PassportType || destination == StringDestination.Email)
                    ColorTextWriterService.PrintLineMessageWithSpecialColor($"What is your {msg}?", ConsoleColor.Yellow);
                else
                    ColorTextWriterService.PrintLineMessageWithSpecialColor($"Try to сome up with { msg}?", ConsoleColor.Yellow);
                Console.Write($"{msg}--> ");
                output = Console.ReadLine()
                    ?.Trim()
                    .Replace(" ", "");
                if (String.IsNullOrEmpty(output))
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Wrong data!", ConsoleColor.Red);
                    continue;
                }
                if(destination == StringDestination.Password && output.Length < 6)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Wrong password length!", ConsoleColor.Red);
                    continue;
                }
                if (destination == StringDestination.Email && !StringValidator.IsEmailValid(output))
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("This email is not valid!", ConsoleColor.Red);
                    continue;
                }
                if (destination == StringDestination.Password)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("You need to confirm password!", ConsoleColor.Yellow);
                    string confirmationPassword;
                    do
                    {
                        Console.Write("Confirmation--> ");
                        confirmationPassword = Console.ReadLine()
                            ?.Trim()
                            .Replace(" ", "");
                        if (String.IsNullOrEmpty(output))
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor("Wrong data!", ConsoleColor.Red);
                            continue;
                        }
                        if (output == confirmationPassword)
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor("Password confirmed", ConsoleColor.Green);
                            passwordNotConfirmed = false;
                        }
                        else
                            ColorTextWriterService.PrintLineMessageWithSpecialColor("Passwords dont match!",ConsoleColor.Red);
                    } while (passwordNotConfirmed);
                }
                if (destination == StringDestination.PassportType && StringValidator.IsStringContainsDigits(output))
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("You cannot enter nameType with digits!", ConsoleColor.Red);
                    continue;
                }
                break;
            }
            return output;
        }
    }
}
