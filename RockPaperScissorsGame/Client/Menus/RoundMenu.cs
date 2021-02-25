using Client.Menus.Interfaces;
using Client.Models;
using Client.Models.Interfaces;
using Client.Services;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Menus
{
    internal class RoundMenu : IRoundMenu
    {
        private readonly string _sessionId;
        private readonly string _baseAddress;
        private  IRoom _room;
        private  IAccount _playerAccount;
        private  IRequestPerformer _performer;
        public RoundMenu(string sessionId,
            string baseAddress,
            IRoom room,
            IAccount playerAccount,
            IRequestPerformer performer)
        {
            _sessionId = sessionId;
            _baseAddress = baseAddress;
            _room = room;
            _playerAccount = playerAccount;
            _performer = performer;
        }
        public async Task MakeYourMove()
        {
            int move = 0;
            ColorTextWriterService.PrintLineMessageWithSpecialColor("Now, try to coop with your move", ConsoleColor.Cyan);
            while (true)
            {
                ColorTextWriterService.PrintLineMessageWithSpecialColor("Available moves:" +
                    "1.\tRock\n" +
                    "2.\tPaper\n" +
                    "3.\tScissors\n", ConsoleColor.Magenta);
                ColorTextWriterService.PrintLineMessageWithSpecialColor("You should choose only a item from list!", ConsoleColor.DarkYellow);
                Console.Write("Select--> ");
                var input = int.TryParse(Console.ReadLine()?.Trim(), out var choosse);
                if (!input)
                {
                    ColorTextWriterService.PrintLineMessageWithSpecialColor("Bad input", ConsoleColor.Red);
                    continue;
                }
                if (choosse != 1 || choosse != 2 || choosse != 3)
                    continue;
                else 
                {
                    move = choosse;
                    break;
                }
            }
            var options = new RequestOptions //ToDo: Recycle
            {
                Address = _baseAddress + $"round/move/{_room.RoomId}&{_sessionId}&{move}",
                IsValid = true,
                Body = _sessionId,
                Method = Services.RequestModels.RequestMethod.Put,
                Name = "Change Player Status"
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            var round = JsonConvert.DeserializeObject<Round>(reachedResponse.Content);
        }
        public async Task UpdateRoundResultAsync()
        {
            var roundEnded = new Round();
            do
            {
                await Task.Run(async () =>
                {
                    var options = new RequestOptions
                    {
                        Body = "",
                        Address = _baseAddress + $"get/update/{_room.RoomId}",
                        IsValid = true,
                        Method = Services.RequestModels.RequestMethod.Get,
                        Name = "Updating round satus"
                    };
                    var reachedResponse = await _performer.PerformRequestAsync(options);
                    roundEnded = JsonConvert.DeserializeObject<Round>(reachedResponse.Content);
                });
                await Task.Delay(TimeSpan.FromSeconds(1));
            } while (roundEnded.IsFinished == true);
        }            
    }
}