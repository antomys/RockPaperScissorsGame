using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private Room _room;
        
        private Round _round;
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
                        var statistics = await OverallStatistics();
                        PrintStatistics(statistics);
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
        private async Task PlayerMenu()
        {
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor($"***\nHello, {_playerAccount.Login}\n" +
                    "Please choose option", ConsoleColor.Cyan);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("1.\tPlay with bot\n" +
                    "2\tCreate room\n" +
                    "3\tJoin Private room\n" +
                    "4\tJoin Public room" +
                    "5\tLog out", ConsoleColor.Yellow);

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
                        //todo: play with bot
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
        private async Task CreationRoom()
        {
            var isPrivate = true;
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Room builder\n" +
                    "Please choose room type\n" +
                    "1.\tOpen\n" +
                    "2.\tPrivate\n", ConsoleColor.Magenta);
                Console.Write("Select--> ");
                var input = int.TryParse(Console.ReadLine()?.Trim(), out var creationMenuInput);
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

             _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content); //todo: remove           
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                
                ColorTextWriterService.PrintLineMessageWithSpecialColor($"Room created. Room id: {_room.RoomId};" +
                                                                        $"Private flag : {isPrivate}", ConsoleColor.Green);
                
                if (_room == null) return;
                
            }

            await ChangePlayerStatus();
            ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
        }

        private async Task ChangePlayerStatus()
        {
            var readyToStart = false;
                
            ColorTextWriterService.PrintLineMessageWithSpecialColor("Ready to start game?", ConsoleColor.Cyan);
                
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(
                    "1.\tReady\n" +
                    "2.\tNot ready\n", ConsoleColor.Magenta);
                Console.Write("Select--> ");
                var input = int.TryParse(Console.ReadLine()?.Trim(), out var startListInput);
                if (!input)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Bad input", ConsoleColor.Red);
                    continue;
                }
                if (startListInput == 1)
                    readyToStart = true;
                break;
            }
            var options = new RequestOptions
            {
                Address = BaseAddress + $"room/updateState/{_sessionId}&{readyToStart}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Put,
                Name = "Change Player Status"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
                
            _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);

            Console.WriteLine($"Your status changed to {readyToStart}");

            Console.WriteLine("Here to spam update until round is created");
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

        private async Task<int> LogIn()
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
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                _playerAccount = inputAccount;
                return 1; //todo: change to 0
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
        private async Task<IEnumerable<Statistics>> OverallStatistics()
        {
            var options = new RequestOptions
            {
                Address = BaseAddress + "overallStatistics",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            
            return JsonConvert.DeserializeObject<IEnumerable<Statistics>>(reachedResponse.Content);
            
        }
        private static void PrintStatistics(IEnumerable<Statistics> statisticsEnumerable)
        {
            Console.WriteLine(statisticsEnumerable.Select(x=> x.ToString()).ToArray());
        }
    }
}

        

