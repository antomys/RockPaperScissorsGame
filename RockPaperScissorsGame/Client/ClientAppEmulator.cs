using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Client.Models;
using Client.Models.Interfaces;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;
using NLog;

namespace Client
{
    public class ClientAppEmulator
    {
        private readonly string _sessionId;
        private const string baseAddress = "http://localhost:5000/";
        
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClientAppEmulator(IRequestPerformer performer)
        {
            _performer = performer;
            _sessionId = Guid.NewGuid().ToString();
        }
        private readonly IRequestPerformer _performer;
        
        private Account _playerAccount;
        
        private Room _room;
        
        private Round _round;
        public async Task<int> StartAsync()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            try
            {
                Greeting();
                ColorTextWriterService.PrintLineMessageWithSpecialColor("\n\nPress any key to show start up menu list!"
                    , ConsoleColor.Green);
                Console.ReadKey();
                Console.Clear();
                //todo: trying to connect to the server
                await StartMenu(token);
                return 1;
            }
            catch (Exception exception)
            {
                logger.Error("Catched exception in StartAsync method", exception.Message);
                throw;
                
            }
        }
        private void Greeting()
        {
            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                    "Welcome to the world best game ----> Rock-Paper-Scissors!\n" +
                    "You are given the opportunity to compete with other users in this wonderful game,\n" +
                    "or if you don’t have anyone to play, don’t worry,\n" +
                    "you can find a random player or just try your skill with a bot.", ConsoleColor.White);
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
                    logger.Trace($"CancellationTokenRequest from Start menu");
                    return;
                }
                var passed = int.TryParse(Console.ReadLine(), out var startMenuInput);
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
                        if (status == 0)
                        {
                            logger.Trace($"Successfull login Code: {status}");
                            Console.Clear();
                            await PlayerMenu();
                        }
                        else
                        {
                            logger.Trace($"Login status code Code: {status}");
                            ColorTextWriterService.PrintLineMessageWithSpecialColor(
                                "\n\nPress any key to back to the start up menu list!", ConsoleColor.Cyan);
                            Console.ReadKey();
                            Console.Clear();
                        }
                        break;
                    case 3:
                        var statistics = await OverallStatistics();
                        if(statistics == null)
                            Console.WriteLine("No statistics so far");
                        else
                        {
                            PrintStatistics(statistics); 
                        }
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
                    "4\tJoin Public room\n" +
                    "5\tShow Statistics\n" +
                    "6\tLog out", ConsoleColor.Yellow);

                ColorTextWriterService.PrintLineMessageWithSpecialColor("\nPlease select an item from the list", ConsoleColor.Green);

                Console.Write("Select -> ");
                var passed = int.TryParse(Console.ReadLine(), out int playersMenuInput);
                if (!passed)
                {
                    logger.Trace($"Not passed argument to player Menu");
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                    continue;
                }
                switch (playersMenuInput)
                {
                    case 1:
                        Console.Clear();
                        await JoinRoomWithBot();
                        break;
                    case 2:
                        Console.Clear();
                        await CreationRoom();
                        break;
                    case 3:
                        Console.Clear();
                        await JoinPrivateRoom();
                        break;
                    case 4:
                        Console.Clear();
                        await JoinPublicRoom();
                        break;
                    case 5:
                        Console.Clear();
                        var statistics = await PersonalStatistics(_sessionId);
                        Console.WriteLine(statistics+"\n\nPress any key to go back.");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        await Logout();
                        return;
                    default:
                        ColorTextWriterService.PrintLineMessageWithSpecialColor("Unsupported input", ConsoleColor.Red);
                        continue;
                }
            }
        }
        private async Task JoinRoomWithBot()
        {
            logger.Trace("JoinRoomWithBot method");
            Console.WriteLine("Trying to connect to training room with bot");
            var options = new RequestOptions
            {
                Address = baseAddress + $"room/create/training/{_sessionId}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Get,
                Name = "Creating training Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (reachedResponse.Content != null)
            {
                logger.Trace($"Catched response from Join Room with Bot method. Code: {reachedResponse.Code}");
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
                Console.WriteLine("Room with bot created!");
                logger.Info("Room with bot created!");
                await ChangePlayerStatus();
                await StartRoomMenu();
            }
        }
        private async Task JoinPublicRoom()
        {
            Console.WriteLine("Trying to connect to random public room");
            
            var options = new RequestOptions
            {
                Address = baseAddress + $"room/join/{_sessionId}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Get,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (reachedResponse.Content != null && reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                logger.Trace($"Catched response from Join Public Room method Code: {reachedResponse.Code}");
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
                Console.WriteLine("Found room! Entering room lobby");
                logger.Info("Found room! Entering room lobby");
                await ChangePlayerStatus();
                await StartRoomMenu();
            }
            else
            {
                Console.WriteLine("Error occured. Probably there is either no room or all rooms are full");
                logger.Info("Error occured. Probably there is either no room or all rooms are full");
            }
        }
        
        private async Task JoinPrivateRoom()
        {
            Console.Write("Please enter room token: ");
            var roomId = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(roomId))
            {
                Console.WriteLine("Invalid input!");
                logger.Info("Invalid input in Join Private room method");
            }

            var options = new RequestOptions
            {
                Address = baseAddress + $"room/join/{_sessionId}&{roomId}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (!string.IsNullOrEmpty(reachedResponse.Content) && reachedResponse.Code == (int)HttpStatusCode.OK)
            {
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Room Founded! Redirecting to room menu...", ConsoleColor.Green);
                await ChangePlayerStatus();
                await StartRoomMenu();
                logger.Info("Room Founded! Redirecting to room menu...");
            }
            else
            {
                Console.WriteLine("Error occured. Probably there is either no room or it is already full");
                logger.Info("Error occured. Probably there is either no room or it is already full");
            }
        }
        /**/
        private async Task StartRoomMenu()
        {
            
            ColorTextWriterService.PrintLineMessageWithSpecialColor($"Your room id: {_room.RoomId}\n" +
                                                                    "Players in room: ", ConsoleColor.Yellow);
            await RecurrentlyUpdateRoom();
            
            if (_room.IsReady)
            { 
                Console.WriteLine("Opponent has joined and is ready.");
                Console.WriteLine("Redirecting to round game:");
                await MakeYourMove();
                await UpdateRoundResultAsync();                
                    Console.WriteLine("Do you want to continue playing in this room?");
                    Console.WriteLine("Write 0 to continue\n\nWrite anything to back to the players menu");
                    var j = "0";
                    string? inputed;
                    do
                    {
                        inputed = Console.ReadLine();
                        await UpdateRoom();
                        await ChangePlayerStatus();
                        await RecurrentlyUpdateRoom();
                        await MakeYourMove();
                        await UpdateRoundResultAsync();
                } while (inputed != j);
                   
                Console.WriteLine("---------------------------");
                Console.ReadKey();
            }
        }
        /**/
        private async Task MakeYourMove()
        {
            var move = 0;
            ColorTextWriterService.PrintLineMessageWithSpecialColor("Please make your move", ConsoleColor.White);
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Available moves:\n" +
                    "1.\tRock\n" +
                    "2.\tPaper\n" +
                    "3.\tScissors\n", ConsoleColor.Magenta);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("You should choose only a item from list!", ConsoleColor.DarkYellow);
                Console.Write("Select--> ");
                var input = int.TryParse(Console.ReadLine()?.Trim(), out var choose);
                if (!input)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Bad input", ConsoleColor.Red);
                    continue;
                }
                if (!(choose > 0 && choose <4))
                    continue;
                else
                {
                    move = choose;
                    break;
                }
            }
            var options = new RequestOptions //ToDo: Recycle
            {
                Address = baseAddress + $"round/move/{_room.RoomId}&{_sessionId}&{move}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Patch,
                Name = "Make your move"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            _round = JsonConvert.DeserializeObject<Round>(reachedResponse.Content);
            
            ColorTextWriterService.PrintLineMessageWithSpecialColor("Waiting for move of another player", ConsoleColor.Cyan);
        }
        private async Task UpdateRoundResultAsync()
        {
            try
            {
                while (!_round.IsFinished)
                {
                    await Task.Run(async () =>
                    {
                        var options = new RequestOptions
                        {
                            Body = "",
                            Address = baseAddress + $"round/get/update/{_room.RoomId}",
                            IsValid = true,
                            Method = Services.RequestModels.RequestMethod.Get,
                            Name = "Updating Round"
                        };
                        var reachedResponse = await _performer.PerformRequestAsync(options);

                        _round = JsonConvert.DeserializeObject<Round>(reachedResponse.Content);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                if (_round.WinnerId != null && _round.LoserId != null)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor($"Winner: {_round.WinnerId}", ConsoleColor.Green);
                    ColorTextWriterService.PrintLineMessageWithSpecialColor($"Loser: {_round.LoserId}", ConsoleColor.Red);
                }
                else Console.WriteLine("Round draw");
            }
            catch (NullReferenceException ex)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("This round was finished with [DRAW]", ConsoleColor.Cyan);
                await StartRoomMenu();
            }
        }
        private async Task UpdateRoom()
        {
            await Task.Run(async () =>
            {
                var options = new RequestOptions
                {
                    Body = "",
                    Address = baseAddress + $"room/updateState/{_room.RoomId}",
                    IsValid = true,
                    Method = Services.RequestModels.RequestMethod.Get,
                    Name = "Updating Room"
                };

                var reachedResponse = await _performer.PerformRequestAsync(options);
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
            });
        }
        private async Task RecurrentlyUpdateRoom()
        {
            try
            {
                var oneTimeShowed = true;
                while (!_room.IsReady)
                {
                    await Task.Run(async () =>
                    {
                        var options = new RequestOptions
                        {
                            Body = "",
                            Address = baseAddress + $"room/updateState/{_room.RoomId}",
                            IsValid = true,
                            Method = Services.RequestModels.RequestMethod.Get,
                            Name = "Updating Room"
                        };

                        var reachedResponse = await _performer.PerformRequestAsync(options);

                        _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);

                    //**************************************
                    //This is just to show once message when someone has joined to your room
                    if (oneTimeShowed && _room.Players.Count > 1)
                        {
                            Console.WriteLine("Updated player list: ");
                            _room.Players.ToList().ForEach(x =>
                            {
                                var (key, value) = x;
                                Console.WriteLine("Id: " + key + "; Is ready: " + value);
                            });
                            oneTimeShowed = false;
                        }

                    });
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            catch(NullReferenceException ex)
            {
                await PlayerMenu();
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
                Address = baseAddress + $"room/create/{_sessionId}&{isPrivate}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

             _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content); //todo: remove           
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                logger.Info($"Thred http from Creation room method. Code: {reachedResponse.Code}");
                
                ColorTextWriterService.PrintLineMessageWithSpecialColor($"Room created. Room id: {_room.RoomId};" +
                                                                        $"Private flag : {isPrivate}", ConsoleColor.Green);
                
                if (_room == null) return;
                
            }
            await ChangePlayerStatus();
        }
        private async Task ChangePlayerStatus()
        {
            bool readyToStart;
                
            ColorTextWriterService.PrintLineMessageWithSpecialColor("Ready to start game?", ConsoleColor.Cyan);

            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(
                    "1.\tReady\n" +
                    "2.\tChanged my mind, doesnt want to play!", ConsoleColor.Magenta);
                Console.Write("Select--> ");
                var input = int.TryParse(Console.ReadLine()?.Trim(), out var startListInput);
                if (!input)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Bad input", ConsoleColor.Red);
                    continue;
                }
                switch (startListInput)
                {
                    case 1:
                        readyToStart = true;
                        break;
                    case 2:
                        await DeleteRoom();
                        await PlayerMenu();
                        return;
                    default:
                        continue;
                }
                if (readyToStart)
                    break;
            }
            var options = new RequestOptions
            {
                Address = baseAddress + $"room/updateState/{_sessionId}&{readyToStart}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Put,
                Name = "Change Player Status"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
                
            _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);

            Console.WriteLine($"Your status changed to {readyToStart}");

            if (readyToStart)
            {
                await StartRoomMenu();
            }
            //Console.WriteLine("Here to spam update until round is created");
        }
        private async Task ChangePlayerStatus(bool readyToStart)
        {
            var options = new RequestOptions
            {
                Address = baseAddress + $"room/updateState/{_sessionId}&{readyToStart}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Put,
                Name = "Change Player Status"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
                
            _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
            
        }
        private async Task DeleteRoom()
        
        {
            var options = new RequestOptions
            {
                ContentType = $"{_sessionId}",
                Body = _sessionId,
                Address = baseAddress + $"room/delete/{_room.RoomId}",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Delete,
                Name = "Delete room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);           
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
            
            var options = new RequestOptions
            {
                ContentType = "application/json",
                Body = JsonConvert.SerializeObject(_playerAccount),
                Address = baseAddress + "user/create",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.Created)
            {
                
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                return 1;
            }

            logger.Info($"Threw http from Registration method. Code: {reachedResponse.Code}");
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
                Address = baseAddress + "user/login",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Registration"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Green);
                _playerAccount = inputAccount;
                return 0; //todo: change to 0
            }

            ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
            return -1;
        }
        private async Task Logout()
        {
            var options = new RequestOptions
            {
                Address = baseAddress + $"user/logout/{_sessionId}",
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
        private async Task<IEnumerable<StatisticsDto>> OverallStatistics() 
        {
            var options = new RequestOptions
            {
                Address = baseAddress + "statistics/overallStatistics",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            if (reachedResponse.Code == 404)
                return null;
            return JsonConvert.DeserializeObject<IEnumerable<StatisticsDto>>(reachedResponse.Content);
            
        }

        private async Task<Statistics> PersonalStatistics(string sessionId)
        {
            var options = new RequestOptions
            {
                Address = baseAddress + $"statistics/personalStatistics/{sessionId}",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);
            
            return JsonConvert.DeserializeObject<Statistics>(reachedResponse.Content);
            
        }
        private static void PrintStatistics(IEnumerable<StatisticsDto> statisticsEnumerable)//Refurbish
        {
            Console.WriteLine(statisticsEnumerable.Select(x=> x.ToString()).ToArray());
        }
    }
}

        