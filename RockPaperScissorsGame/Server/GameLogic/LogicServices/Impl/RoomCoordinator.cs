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
        private readonly IAccountManager _accountManager;

        private IRoundCoordinator _roundCoordinator;
        
        private static readonly Random Random = new();
        
        private Timer _timer;

        private readonly TimerCallback tm;
        
        public ConcurrentDictionary<string, Room> ActiveRooms { get; }
        
        public RoomCoordinator(
            IAccountManager accountManager, 
            IRoundCoordinator roundCoordinator)
        {
            _accountManager = accountManager;
            ActiveRooms = new ConcurrentDictionary<string, Room>();
            _roundCoordinator = roundCoordinator;
            tm = CheckRoomDate;
        }
       
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

                if (newRoom.Players.TryAdd(account.Id, false))
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
                    return null; //todo: change into exception;
                }
                return ActiveRooms.TryUpdate(room.RoomId,
                    updated, room) ? room : null;
            });
            return await thread;
        }
       
        public async Task<Room> UpdatePlayerStatus(string sessionId, bool isReady)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);
                var room = ActiveRooms.Values
                    .FirstOrDefault(x => x.Players.Keys
                        .Any(p => p
                            .Equals(account.Id))); //Here you was equaling to sessionId
                
                if (!ActiveRooms.TryGetValue(room.RoomId, out var thisRoom))
                    return null; //To fix
                var newRoom = thisRoom;
                var (key, oldValue) = newRoom.Players.FirstOrDefault(x => x.Key == account.Id); //Here you were equaling to Login
                
                newRoom.Players.TryUpdate(key, isReady, oldValue);
                
                return newRoom;
            });
            
            
            return await tasks;
        }
        
        public async Task<Room> UpdateRoom(string roomId)
        {
            var thread = Task.Factory.StartNew(() =>
            {
                var room = GetRoomByRoomId(roomId);

                if (room == null)
                {
                    throw new Exception(); //todo: exception;
                }

                if (room.Players.Values.All(x => x))
                {
                    var round = new Round
                    {
                        Id = Guid.NewGuid()
                            .ToString(),
                        IsFinished = false,
                        PlayerMoves = new ConcurrentDictionary<string, int>(),
                        TimeFinished = default,
                        WinnerId = null,
                        LoserId = null,
                    };

                    var accounts = room.Players.Keys.ToList();
                    
                }
                    return ActiveRooms.TryUpdate(room.RoomId,
                    room,
                    ActiveRooms.FirstOrDefault(x => x.Key == room.RoomId).Value) ? room : null;
            });

            return await thread;
        }


        #region PrivateMethods
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

        private Room GetRoomByRoomId(string roomId)
        {
            if (ActiveRooms.TryGetValue(roomId, out var thisRoom))
                return thisRoom;
            throw new Exception(); //todo: implement exception;
        }
        
        #endregion
        
    }
}
