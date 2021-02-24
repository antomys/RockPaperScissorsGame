using Server.Exceptions.LogIn;
using Server.GameLogic.Exceptions;
using Server.GameLogic.Models.Impl;
using Server.Models;
using Server.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            tm = CheckRoomDate;
        }
        public ConcurrentDictionary<string, Room> ActiveRooms { get; }
        public async Task<Room> CreateRoom(string sessionId, bool isPrivate)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);

                if (ActiveRooms.Any(x => x.Value.Players.Any(p => p.Key.Item1.Equals(account.Login))))
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

                if (newRoom.Players.TryAdd(sessionId, false) &&
                   newRoom.IsPrivate)
                {
                    ActiveRooms.TryAdd(newRoom.RoomId, newRoom);
                }

                _timer = new Timer(tm, null, 0, 2000);
                return newRoom;
            });
            return await tasks;
        }
        public  Task<Room> CreateTrainingRoom(string sessionId)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteRoom(string roomId)
        {
            var tasks = Task.Factory.StartNew(() =>
                ActiveRooms.TryRemove(roomId, out _));
            return await tasks;
        }
        public async Task<Room> UpdateRoom(Room updated)
        {
            var thread = Task.Factory.StartNew(() =>
            {
                ActiveRooms.TryGetValue(updated.RoomId, out var room);
                if (room == null)
                {
                    return null;
                }
                return ActiveRooms.TryUpdate(room.RoomId,
                    updated,
                    ActiveRooms.FirstOrDefault(x => x.Key == room.RoomId).Value) ? room : null;
            });
            return await thread;
        } 
        //update client status
        
        private async void CheckRoomDate(object state)
        {
            var threads = Task.Factory.StartNew(() =>
            {
                foreach (var room in ActiveRooms)
                {
                    if (room.Value.CreationTime.AddMinutes(5) < DateTime.Now && room.Value.CurrentRoundId == null)
                        ActiveRooms.TryRemove(room);
                }
            });
            await Task.WhenAll(threads);
        }
        private static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private Account GetAccountById(string sessionId)
        {
            _accountManager.AccountsActive.TryGetValue(sessionId, out var account);
            if (account != null) 
                return account;
            else 
                throw new UserNotFoundException(nameof(account));

        }
        private readonly IAccountManager _accountManager;
        private static readonly Random Random = new();
        private Timer _timer;
        private readonly TimerCallback tm;
    }
}
