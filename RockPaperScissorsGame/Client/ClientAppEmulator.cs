using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Client.Services;
using Newtonsoft.Json;

namespace Client
{
    public class ClientAppEmulator
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly string _sessionId;

        //private Timer _updateTimer;
        //private bool _isAlive = true;
        
        public ClientAppEmulator()
        {
            Client.BaseAddress = new Uri("http://localhost:5000/");
            _sessionId = Guid.NewGuid().ToString();
        }

        

        //For currently player on the platform //developing
        private Account _playerAccount;
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
                        /*if (registrationResponse.Equals((int)HttpStatusCode.BadRequest))
                        {
                            Console.WriteLine("Unable to create account. Already exists?");
                        }
                        else
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor("Account successfully created", ConsoleColor.Green);
                            ColorTextWriterService.PrintLineMessageWithSpecialColor(_playerAccount.ToString(), ConsoleColor.Green); 
                        }*/
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        await LogIn();
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        var statistics = await OverallStatistics();
                        PrintStats(statistics);
                        //await Logout(); //todo: REMOVE
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
            var registrationAccount = new Account
            {
                SessionId = _sessionId,
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password"),
                LastRequest = DateTime.Now
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(_playerAccount), Encoding.UTF8, "application/json");
            var responseMessage = await Client.PostAsync("user/create", stringContent);  //TODO: Cancellation token
            
            
            if (responseMessage.IsSuccessStatusCode)
            {
                return (int)responseMessage.StatusCode;
            }

            Console.WriteLine("This account already exists!");
            return (int) responseMessage.StatusCode;

        }

        private async Task LogIn() //For now Int. Dunno what to make
        {
            var inputAccount = new Account
            {
                SessionId = _sessionId,
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password"),
                LastRequest = DateTime.Now
            };
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(inputAccount), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("user/login", stringContent);
            
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                _playerAccount = inputAccount;
                Console.WriteLine(responseBody);
                Console.WriteLine($"{_playerAccount.SessionId}");
                //SetUpTimer();
            }
            else
            {
                Console.WriteLine(responseBody);
            }
        }

        private async Task Logout()
        {
            if (_playerAccount == null) //todo: exception.
                return ;
            var response = await Client.GetAsync($"user/logout/{_sessionId}");//TODO: Cancellation token
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully signed out!\n");
                _playerAccount = null;
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        private async Task<IEnumerable<Statistics>> OverallStatistics()
        {
            var response = await Client.GetAsync($"overallStatistics");//TODO: Cancellation token
            if (!response.IsSuccessStatusCode)
                throw new Exception();
            
            var responseBody = await response.Content.ReadAsStringAsync();

            var deserialized = JsonConvert.DeserializeObject<List<Statistics>>(responseBody);

            return deserialized;
        }

        private void PrintStats(IEnumerable<Statistics> statisticsEnumerable)
        {
            foreach (var statistics in statisticsEnumerable)
            {
                Console.WriteLine(statistics);
            }
        }
    }
}
