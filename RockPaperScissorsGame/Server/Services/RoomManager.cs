/*using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class RoomManager : IRoomManager
    {
        private readonly ConcurrentDictionary<string,Room> _activeRooms = new ();
        private readonly IAccountManager _accountManager;
        //private readonly IStatisticsManager _statisticsManager;
        //private readonly 
        

        public RoomManager(IAccountManager accountManager)
        {
            _accountManager = accountManager;
            //_statisticsManager = statisticsManager;
        }

        public async Task<Room> CreateRoom(Account account)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new List<Account>()
                };
                newRoom.Players.Add(account);

                if (_activeRooms.ContainsKey(newRoom.RoomId))
                    return null; //throw exception;
                _activeRooms.TryAdd(newRoom.RoomId, newRoom);
                return newRoom;
            });

            return await tasks;
        }

        public async Task<Room> JoinRoom(string roomId, Account account)
        {
            var tasks = Task.Factory.StartNew(() =>  //task run of task factory?
            {
                if (!_activeRooms.TryGetValue(roomId, out var thisRoom)) return null;
                thisRoom.Players.Add(account);
                if (thisRoom.Players.Count == 2)
                {
                    thisRoom.IsFull = true;
                }

                return _activeRooms.TryUpdate(thisRoom.RoomId, thisRoom,
                    (_activeRooms.FirstOrDefault(x => x.Key == roomId).Value)) ? thisRoom : null;
                //Todo: redo and place into other bag if private.
            });

            return await tasks;

        }
        
        private static string RandomString()
        {
            var random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}*/