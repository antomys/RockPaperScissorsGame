using RockPaperScissors.Models;
using RockPaperScissors.Services;
using RockPaperScissors.Validations;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RockPaperScissors
{
    public class ClientAppEmulator
    {
        static HttpClient client = new HttpClient();
        public ClientAppEmulator()
        {
            client.BaseAddress = new Uri("https://localhost:5001/");
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
                await StartMenu();
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
        private async Task StartMenu()
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
                        var registrationResponse = await Registration();
                        if (registrationResponse.Equals((int)HttpStatusCode.BadRequest))
                        {
                            Console.WriteLine("Unable to create account. Already exists?");
                        }
                        else
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor("Account successfully created", ConsoleColor.Green);
                            ColorTextWriterService.PrintLineMessageWithSpecialColor(_playerAccountDto.ToString(), ConsoleColor.Green); 
                        }
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        var loginResponse = await LogIn();
                        if (loginResponse != 404) //todo: change to HttpStatusCode
                        {
                            Console.WriteLine($"Successfully signed in!\nHello,{_playerAccountDto.Login}!");
                            //Transfer to another menu
                        }
                        else
                        {
                            Console.WriteLine("Unable to sign in. Please check login or password");
                        }
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
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
        private async Task<int> Registration()
        {
            ColorTextWriterService.PrintLineMessageWithSpecialColor("\nWe are glad to welcome you in the registration form!\n" +
                "Please enter the required details\n" +
                "to register an account on the platform", ConsoleColor.Magenta);
            _playerAccountDto = new AccountDto
            {
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password")
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(_playerAccountDto), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("user/create", stringContent);  //TODO: Cancellation token

            return (int)responseMessage.StatusCode;
        }

        private async Task<int> LogIn() //For now Int. Dunno what to make
        {
            var inputAccount = new AccountDto
            {
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password")
            };
            
            //var stringContent = new StringContent(JsonConvert.SerializeObject(inputAccount), Encoding.UTF8, "application/json");
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(client.BaseAddress+"user/login"),
                Content = new StringContent(JsonConvert.SerializeObject(inputAccount), Encoding.UTF8, "application/json")
            };
            
            var response = await client.SendAsync(request).ConfigureAwait(false); //TODO: Cancellation token
            if (!response.IsSuccessStatusCode) return 404;
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _playerAccountDto = JsonConvert.DeserializeObject<AccountDto>(responseBody);
            return 200; //Maybe another code?
            
        }
    }
}
