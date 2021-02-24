using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Models;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;

namespace Client
{
    public class ClientAppEmulator
    {
        private readonly string _sessionId;
        private const string BaseAddress = "http://localhost:5000/";

        public ClientAppEmulator(IRequestPerformer performer)
        {
            _performer = performer;
            _sessionId = Guid.NewGuid().ToString();
        }
        private IRequestPerformer _performer;


        //For currently player on the platform //developing
        private Account _playerAccount;
        public async Task<int> StartAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            try
            {
                Greeting();
                ColorTextWriterService.PrintLineMessageWithSpecialColor("\n\nPress any key to show start up menu list!"
                    , ConsoleColor.Green);
                Console.ReadKey();
                Console.Clear();
                //Here we ` ll try to connect with server on the background and show smth to the user
                //smth like await TaskFactory (Connection + Menu StartUP)
                /*
                                var task = new Task(async()=> await TryToConnectWithServer());
                                task.Start();
                                await StartMenu(token);

                                task.Wait();
                                if (task.IsCompleted)
                                {
                                    tokenSource.Cancel();
                                }*/
                await StartMenu(token);
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
        private async Task StartMenu(CancellationToken token)
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
                if (token.IsCancellationRequested)
                {
                    return;
                }
                var passed = int.TryParse(Console.ReadLine(), out int startMenuInput);
                if (!passed)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                switch (startMenuInput)
                {
                    case 1:
                        var registrationResponse = await Registration();
                        ColorTextWriterService.PrintLineMessageWithSpecialColor(
                            "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        var status = await LogIn();
                        if (status == 1)
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                               "\n\nPress any key to go to the players menu", ConsoleColor.Cyan);
                            Console.ReadKey();
                            Console.Clear();
                            await PlayerMenu();
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                        {
                            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                                "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                            Console.ReadKey();
                            Console.Clear();
                        }
                        break;
                    case 3:

                        /*  var statistics = await OverallStatistics();

                          /*  var statistics = await OverallStatistics();
                          PrintStats(statistics);*/
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
        private async Task PlayerMenu()
        {
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Welcome to the game menu\n" +
                    "What would you like to do?", ConsoleColor.Cyan);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("1.\tPlay with bot\n" +
                    "2\tPlay in public pool\n" +
                    "3\tPlay in private room\n" +
                    "4\tLogout", ConsoleColor.Yellow);

                ColorTextWriterService.PrintLineMessageWithSpecialColor("\nPlease select an item from the list", ConsoleColor.Green);

                Console.Write("Select -> ");
                var passed = int.TryParse(Console.ReadLine(), out int playersMenuInput);
                if (!passed)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                switch (playersMenuInput)
                {
                    case 1:
                        break;
                    case 2:
                        await CreationRoom();
                        Console.ReadKey();
                        break;
                    case 3:
                        break;
                    case 4:
                        await Logout();
                        return;
                    default:
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                        continue;
                }
            }
        }

        private async Task<int> CreationRoom()
        {
            bool isPrivate = true;
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Welcome to lobby builder!\n" +
                    "Which type of room would you like to create!\n" +
                    "1.\tOpen\n" +
                    "2.\tPrivate\n", ConsoleColor.Magenta);
                Console.Write("Select--> ");
                var input = int.TryParse(Console.ReadLine().Trim(), out int creationMenuInput);
                if (!input)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Bad input", ConsoleColor.Red);
                    continue;
                }
                if (creationMenuInput == 1)
                    isPrivate = false;
                break;
            }
            var options = new RequestOptions
            {
                Address = BaseAddress + $"room/create/{_sessionId}&{isPrivate}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

             var room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content); //todo: remove
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);               
                return 0;
            }
            else
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
                //await Logout();
                return -1;
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
            _playerAccount = registrationAccount;
            // var stringContent = new StringContent(JsonConvert.SerializeObject(_playerAccount), Encoding.UTF8, "application/json");
            //var responseMessage = await Client.PostAsync("user/create", stringContent);  //TODO: Cancellation token
            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(_playerAccount),
                Address = BaseAddress + "user/create",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                return 1;
            }

            ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
            return -1;
        }

        private async Task<int> LogIn() //For now Int. Dunno what to make
        {
            var inputAccount = new Account
            {
                SessionId = _sessionId,
                Login = new StringPlaceholder().BuildNewSpecialDestinationString("Login"),
                Password =
                    new StringPlaceholder(StringDestination.Password).BuildNewSpecialDestinationString("Password", true),
                LastRequest = DateTime.Now
            };
            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(inputAccount),
                Address = BaseAddress + "user/login",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == 200)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                _playerAccount = inputAccount;
                return 1;
            }
            else
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
                return -1;
            }
        }
        private async Task Logout()
        {
            var options = new RequestOptions
            {
                Address = BaseAddress + $"user/logout/{_sessionId}",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == 200)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Successfully signed out", ConsoleColor.Green);
                _playerAccount = null;
            }
            else
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
            }
        }
    }
}

        

