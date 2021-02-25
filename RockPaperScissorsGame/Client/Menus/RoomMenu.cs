using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Client.Menus.Interfaces;
using Client.Models;
using Client.Models.Interfaces;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;

namespace Client.Menus
{
   /* internal class RoomMenu : IRoomMenu
    {
        private readonly string _sessionId;
        private readonly string _baseAddress;
        private IRoom _room;
        private readonly IRequestPerformer _performer;

        public RoomMenu(
            string sessionId,
            string baseAddress,
            IRoom room,
            IRequestPerformer performer)
        {
            _sessionId = sessionId;
            _baseAddress = baseAddress;
            _room = room;
            _performer = performer;
        }


        public async Task JoinPublicRoom()
        {
            Console.WriteLine("Trying to connect to random public room");
            
            var options = new RequestOptions
            {
                Address = _baseAddress + $"room/join/{_sessionId}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Get,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (!string.IsNullOrEmpty(reachedResponse.Content) && reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
                Console.WriteLine($"Found room!\nRoom id: {_room.RoomId}\n Entering room lobby");
                await ChangePlayerStatus();
                await StartRoomMenu();
            }
            else
            {
                Console.WriteLine("Error occured. Probably there is either no rooms or all are full");
            }
            
        }
        public async Task JoinPrivateRoom()
        {
            Console.Write("Please enter room token: ");
            var roomId = Console.ReadLine().Trim().ToLower();
            if (string.IsNullOrEmpty(roomId))
            {
                Console.WriteLine("Invalid input!");
            }
            
            var options = new RequestOptions
            {
                Address = _baseAddress + $"room/join/{_sessionId}&{roomId}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (!string.IsNullOrEmpty(reachedResponse.Content) && reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Room Founded! Redirecting to room menu...",ConsoleColor.Green);
                await ChangePlayerStatus();
                await StartRoomMenu();
            }
            else
            {
                Console.WriteLine("Error occured. Probably there is either no room or it is already full");
            }
        }

        public async Task CreateRoom()
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
                Address = _baseAddress + $"room/create/{_sessionId}&{isPrivate}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Post,
                Name = "Creating Room"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

                        
            if (reachedResponse.Code == (int) HttpStatusCode.OK)
            {
                _room = JsonConvert.DeserializeObject<Room>(reachedResponse.Content);
                ColorTextWriterService.PrintMessageWithSpecialColor($"Room created. Room id: ", ConsoleColor.Green);
                ColorTextWriterService.PrintMessageWithSpecialColor($" {_room.RoomId} ; ",ConsoleColor.Red);
                ColorTextWriterService.PrintMessageWithSpecialColor($" Is room private : {isPrivate}",ConsoleColor.Green);
                
                if (_room == null) return;
                
            }
            await ChangePlayerStatus();
            //ColorTextWriterService.PrintLineMessageWithSpecialColor(reachedResponse.Content, ConsoleColor.Red);
        }

        public async Task ChangePlayerStatus()
        {
            bool readyToStart;
                
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

                readyToStart = startListInput switch
                {
                    1 => true,
                    2 => false,
                    _ => false
                };
                if (readyToStart)
                    break;
            }
            var options = new RequestOptions
            {
                Address = _baseAddress + $"room/updateState/{_sessionId}&{readyToStart}",
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
        }

        public async Task StartRoomMenu()
        {
            Console.WriteLine($"Your room id: {_room.RoomId}\n");
            Console.WriteLine("Players in room: ");
             _room.Players.ToList().ForEach(x =>
            {
                var (key, value) = x;
                Console.WriteLine("Id: "+ key + "; Is ready: " + value);
            });
            await RecurrentlyUpdateRoom();


            await RecurrentlyUpdateRoom();

            if (_room.IsReady)
            { //todo: add redirection;
                Console.WriteLine("Opponent has joined and is ready.");
                Console.WriteLine("Redirecting to round game:");
               // await _roun
            }
        }

        
        public async Task RecurrentlyUpdateRoom()
        {
            var oneTimeShowed = true;
            while (!_room.IsReady)
            {
                await Task.Run(async () =>
                {
                    var options = new RequestOptions
                    {
                        Body = "",
                        Address = _baseAddress + $"room/updateState/{_room.RoomId}",
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
                            Console.WriteLine("Id: "+ key + "; Is ready: " + value);
                        });
                        oneTimeShowed = false;
                    }
                    
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }*/
}