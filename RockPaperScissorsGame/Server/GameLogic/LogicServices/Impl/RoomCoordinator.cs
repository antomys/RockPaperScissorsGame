using Server.Exceptions.LogIn;
using Server.GameLogic.Exceptions;
using Server.GameLogic.Models.Impl;
using Server.Models;
using Server.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.GameLogic.LogicServices.Impl
{
    public class RoomCoordinator : IRoomCoordinator
    {
        public RoomCoordinator(IAccountManager accountManager)
        {
            _accountManager = accountManager;
            ActiveRooms = new ConcurrentDictionary<string, Room>();
            //_timer = new Timer(tm, null, 0, 2000); //todo: implement
            tm = CheckRoomDate;
        }
        public ConcurrentDictionary<string, Room> ActiveRooms { get; }
        public async Task<Room> CreateRoom(string sessionId, bool isPrivate)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);

                if(ActiveRooms.Any(x=> x.Value.Players.Any(p=> p.Key.Equals(account.Id))))
                    throw new TwinkGameRoomCreationException();

                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new ConcurrentDictionary<string, bool>(),
                    IsPrivate = isPrivate,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false,
                    CreationTime = DateTime.Now
                };

                if (newRoom.Players.TryAdd(account.Id, false) &&
                   newRoom.IsPrivate)
                {
                    ActiveRooms.TryAdd(newRoom.RoomId, newRoom);
                }

                _timer = new Timer(tm, null, 0, 10000); //todo: implement
                return newRoom;
            });
            return await tasks;
        }
        public async Task<Room> CreateTrainingRoom(string sessionId)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);

                if (ActiveRooms.Any(x => x.Value.Players.Any(p => p.Key.Equals(account.Id))))
                    throw new TwinkGameRoomCreationException();

                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new ConcurrentDictionary<string, bool>(),
                    IsPrivate = true,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false,
                    CreationTime = DateTime.Now
                };

                newRoom.Players.TryAdd(account.Id, false);
                newRoom.Players.TryAdd("Bot", true);
                newRoom.IsFull = true;
                ActiveRooms.TryAdd(newRoom.RoomId, newRoom);

                return newRoom;
            });
            return await tasks;

        }
        public Task<bool> DeleteRoom(string roomId)
        {
            throw new NotImplementedException();
        }
        public Task<Room> UpdateRoom(string roomId)
        {
            throw new NotImplementedException();
        }
        private async void CheckRoomDate(object state)
        {
            var threads = Task.Factory.StartNew(() =>
            {
                foreach (var room in ActiveRooms)
                {
                    if (room.Value.CreationTime.AddMinutes(1) < DateTime.Now && room.Value.CurrentRoundId == null)
                        ActiveRooms.TryRemove(room);
                }
            });
            await Task.WhenAll(threads);
        }
        private static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private Account GetAccountById(string sessionId)
        {
            _accountManager.AccountsActive.TryGetValue(sessionId, out var account);
            if (account != null) 
                return account;
            throw new UserNotFoundException(nameof(account));

        }
        private readonly IAccountManager _accountManager;
        private static readonly Random Random = new();
        private Timer _timer;
        private readonly TimerCallback tm;
    }
}
