using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Exceptions.LogIn;
using Server.Game.Models;
using Server.Models;
using Server.Models.Interfaces;
using Server.Services.Interfaces;

namespace Server.Game.Services
{
    public class RoomManager : IRoomManager  // this class to Singleton
    {
      
        private readonly IAccountManager _accountManager;
        
        private static readonly Random Random = new();

        private Timer _timer;
        private readonly TimerCallback tm;

        public RoomManager(IAccountManager accountManager)
        {
            _accountManager = accountManager;
            ActiveRooms = new ConcurrentDictionary<string, Room>();
            tm = CheckRoomDate;
        }

        private async void CheckRoomDate(object state)
        {
            var threads = Task.Factory.StartNew(() =>
            {
                foreach (var room in ActiveRooms)
                {
                    if (room.Value.CreationTime.AddMinutes(5) < DateTime.Now && room.Value.CurrentRound == null)
                        ActiveRooms.TryRemove(room);
                }
            });
            await Task.WhenAll(threads);
        }

        public ConcurrentDictionary<string, Room> ActiveRooms { get; }


        public async Task<Room> CreateRoom(string sessionId, bool isPrivate)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);
                
                if (ActiveRooms.Any(x =>
                    x.Value.Players.Any(p => p.Key.Item2.Equals(account.Login))))
                {
                    return null; //Zaglushka;
                }
                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new ConcurrentDictionary<Tuple<string,string>, bool>(),  //Where Item1 = SessionId and Item2 = Login
                    CurrentRound = null,
                    IsPrivate = isPrivate,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false,
                    CreationTime = DateTime.Now
                };

                if(newRoom.Players.TryAdd(new Tuple<string, string>(sessionId, account.Login), false) &&
                   newRoom.IsPrivate)
                {
                    ActiveRooms.TryAdd(newRoom.RoomId, newRoom);
                }

                _timer = new Timer(tm, null, 0, 2000);
                return newRoom;
            });

            return await tasks;
        }
        
        public async Task<Room> CreateTrainingRoom(string sessionId)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);
                
                if (ActiveRooms.Any(x =>
                    x.Value.Players.Any(p => p.Key.Item2.Equals(account.Login))))
                {
                    return null; //Zaglushka;
                }
                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new ConcurrentDictionary<Tuple<string,string>, bool>(),  //Where Item1 = SessionId and Item2 = Login
                    CurrentRound = null,
                    IsPrivate = true,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false,
                    CreationTime = DateTime.Now
                };

                if(newRoom.Players.TryAdd(new Tuple<string, string>(sessionId, account.Login), false) &&
                   newRoom.IsPrivate)
                {
                    newRoom.Players.TryAdd(new Tuple<string, string>("null", "bot"), true);
                    newRoom.IsFull = true;
                    ActiveRooms.TryAdd(newRoom.RoomId, newRoom);
                }
                _timer = new Timer(tm, null, 2000, 2000);
                return newRoom;
            });

            return await tasks;
        }
        
        public async Task<Room> JoinPrivateRoom(string sessionId, string roomId) //Ideally client would send a room
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);
                
                if (!ActiveRooms.TryGetValue(roomId, out var thisRoom)) return null; //todo: exception;

                var newRoom = thisRoom;  //bullshit
                
                if(newRoom.Players.TryAdd(new Tuple<string, string>(sessionId, account.Login), false))
                    newRoom.IsFull = true;
                
                //UpdateRoom(room);

                return ActiveRooms.TryUpdate(roomId, newRoom, thisRoom) ? newRoom : null; //todo: redo;
            });

            return await tasks;
        }

        public async Task<Room> UpdatePlayerState(string sessionId, bool state) //Ideally client would send a room
        {
            var tasks = Task.Factory.StartNew( () =>
            {
                var account = GetAccountById(sessionId);
                var room = GetRoomBySessionId(sessionId);
                if (!ActiveRooms.TryGetValue(room.RoomId, out var thisRoom)) return null; //todo: exception;
                var newRoom = thisRoom;

                var (key, oldValue) = newRoom.Players.FirstOrDefault(x => x.Key.Item2 == account.Login);

                newRoom.Players.TryUpdate(key,state,oldValue);

                if (newRoom.Players.Count != 2 || !newRoom.Players.Values.All(x => x))
                    return null; //todo: redo
                
                newRoom.IsReady = true;
                newRoom.CurrentRound = new Round
                {
                    SessionIdNextMove = newRoom.Players.Keys.FirstOrDefault(x=>x.Item2 == account.Login)?.Item1,
                    IsFinished = false,
                    RoundId = Guid.NewGuid().ToString()
                };
                
                return newRoom; //todo: redo
            });

            return await tasks;
        }

        public async Task<bool> DeleteRoom(string roomId)
        {
            var tasks = Task.Factory.StartNew(() => 
                ActiveRooms.TryRemove(roomId,out _));

            return await tasks;
        }

        public async Task<Room> UpdateRoom(string sessionId)
        {
            var thread = Task.Factory.StartNew(() =>
            {
                var room = GetRoomBySessionId(sessionId);

                if (room == null)
                {
                    return null;
                }
                return ActiveRooms.TryUpdate(room.RoomId,
                    room,
                    ActiveRooms.FirstOrDefault(x => x.Key == room.RoomId).Value) ? room : null;
            });

            return await thread;
        }

        private bool IsPlayerExists(IAccount account)
        {
            return ActiveRooms.Values
                .Any(x => x.Players.Keys
                    .Any(p => p.Item2
                        .Equals(account.Login)));
        }

        private Room GetRoomByAccount(IAccount account)
        {
            
            return ActiveRooms.Values
                .FirstOrDefault(x => x.Players.Keys
                    .Any(p => p.Item2
                        .Equals(account.Login)));
        }
        private Room GetRoomBySessionId(string sessionId)
        {
            return ActiveRooms.Values
                .FirstOrDefault(x => x.Players.Keys
                    .Any(p => p.Item1
                        .Equals(sessionId)));
        }

        private Account GetAccountById(string sessionId)
        {
            _accountManager.AccountsActive.TryGetValue(sessionId, out var account);

            if (account != null) return account;
            throw new UserNotFoundException(nameof(account));

        }

        private KeyValuePair<string, Room> GetPairFromActiveRoomsByKey(string key)
        {
            return ActiveRooms.FirstOrDefault(x => x.Key.Equals(key));
        }
        
        private static string RandomString()
        {
            
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)       //Made length equal 6. Can be bigger.
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}