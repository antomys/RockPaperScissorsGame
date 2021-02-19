using RockPaperScissors.Models;
using RockPaperScissors.Services;
using RockPaperScissors.Validations;
using System;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    public class ClientAppEmulator
    {
        public ClientAppEmulator()
        {
        }
        //For currently player on the platform //developing
        private AccountDto _playerAccountDto;
        public async Task<int> StartAsync()
        {
            try
            {
                Greeting();
                ColorTextWriterService.PrintLineMessageWithSpecialColor("\n\nPress any key to show start up menu list!"
                    ,ConsoleColor.Green);
                Console.ReadKey();
                Console.Clear();
                //Here we ` ll try to connect with server on the background and show smth to the user
                //smth like await TaskFactory (Connection + Menu StartUP)
                StartMenu();
                return 1;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private void Greeting()
        {
            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                    "Welcome to the world best game ----> Rock-Paper-Scissors!\n" +
                    "You are given the opportunity to compete with other users in this wonderful game,\n" +
                    "or if you don’t have anyone to play, don’t worry,\n" +
                    "you can find a random player or just try your skill with a bot.", ConsoleColor.Yellow);
            ColorTextWriterService.PrintLineMessageWithSpecialColor("(c)Ihor Volokhovych & Michael Terekhov", ConsoleColor.Cyan);
        }
        private void StartMenu()
        {
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("" +
                "Please auth to proceed:\n" +
                "1.\tSign up\n" +
                "2.\tLog in\n" +
                "3.\tSee Leaderboard\n" +       //This part will be available after we figure out the statistics
                "4.\tExit", ConsoleColor.DarkYellow);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("\nPlease select an item from the list", ConsoleColor.Green);

                Console.Write("Select -> ");
                var passed = int.TryParse(Console.ReadLine(), out int startMenuInput);
                if (!passed)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input",ConsoleColor.Red);
                    continue;
                }
                switch (startMenuInput)
                {
                    case 1:
                        Registration();
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("Account succesfully created", ConsoleColor.Green);
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(_playerAccountDto.ToString(), ConsoleColor.Green);
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("\n\nPress any key to back to the start up menu list!"
                    , ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        return;
                    default:
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                        continue;
                }
            
            }

        }
        private void Registration()
        {
            ColorTextWriterService.PrintLineMessageWithSpecialColor("\nWe are glad to welcome you in the registration form!\n" +
                "Please enter the required details\n" +
                "to register an account on the platform", ConsoleColor.Magenta);
            /*_playerAccountDto = new AccountDto(
                new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                new StringPlaceholder(StringDestination.Email).BuildNewSpecialDestinationString("Email"),
                new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password"),
                new StringPlaceholder(StringDestination.PassportType).BuildNewSpecialDestinationString("FirstName"),
                new StringPlaceholder(StringDestination.PassportType).BuildNewSpecialDestinationString("LastName"));*/
            Console.Clear();
        }
    }
}
