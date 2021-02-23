using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Server.Game.Models;
using Server.Models;
using Server.Models.Interfaces;
using Server.Services.Interfaces;

namespace Server.Game.Services
{
    public class RoomManager : IRoomManager  // this class to Singleton
    {
        private readonly IDeserializedObject<Round> _deserializedRounds;

        private readonly ConcurrentDictionary<string, Room> _activeRooms = new();
        
        
        private static readonly Random _random = new();

        public RoomManager(IDeserializedObject<Round> deserializedRounds)
        {
            _deserializedRounds = deserializedRounds;
        }

        public async Task<Room> CreateRoom(IAccount account, string sessionId, bool isPrivate)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                if (_activeRooms.Any(x =>
                    x.Value.Players.Any(x => x.Key.Item2.Equals(account.Login))))
                {
                    return null; //Zaglushka;
                }
                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new ConcurrentDictionary<Tuple<string,string>, object>(),  //Where Item1 = SessionId and Item2 = Login
                    CurrentRound = null,
                    IsPrivate = isPrivate,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false
                };

                if(newRoom.Players.TryAdd(new Tuple<string, string>(sessionId, account.Login), "false") &&
                   newRoom.IsPrivate)
                {
                    _activeRooms.TryAdd(newRoom.RoomId, newRoom);
                }

                return newRoom;
            });

            return await tasks;
        }
        
        public async Task<Room> JoinPrivateRoom(IAccount account, string sessionId, string roomId) //Ideally client would send a room
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                if (!_activeRooms.TryGetValue(roomId, out var thisRoom)) return null; //todo: exception;

                var newRoom = thisRoom;  //bullshit
                
                newRoom.Players.TryAdd(new Tuple<string, string>(sessionId, account.Login), "false");
                
                newRoom.IsFull = true;
                
                //UpdateRoom(room);

                return _activeRooms.TryUpdate(roomId, newRoom, thisRoom) ? newRoom : null; //todo: redo;
            });

            return await tasks;
        }

        public async Task<Room> UpdatePlayerState(IAccount account, bool state) //Ideally client would send a room
        {
            var tasks = Task.Factory.StartNew( () =>
            {
                var room = GetRoomByAccount(account);
                if (!_activeRooms.TryGetValue(room.RoomId, out var thisRoom)) return null; //todo: exception;
                var newRoom = thisRoom;

                var (key, oldValue) = newRoom.Players.FirstOrDefault(x => x.Key.Item2 == account.Login);

                newRoom.Players.TryUpdate(key,state.ToString().ToLower(),oldValue);

                if (newRoom.Players.Values.Contains("false"))
                    return _activeRooms.TryUpdate(room.RoomId, newRoom, thisRoom) ? newRoom : null; //todo: redo
                {
                    newRoom.IsReady = true;
                    newRoom.CurrentRound = new Round
                    {
                        IsFinished = false,
                        RoundId = Guid.NewGuid().ToString()
                    };
                    newRoom.NextMove = newRoom.Players.Keys.FirstOrDefault(x=>x.Item2 == account.Login)?.Item1;
                }

                //UpdateRoom(room); //Maybe todo await???

                return _activeRooms.TryUpdate(room.RoomId, newRoom, thisRoom) ? newRoom : null; //todo: redo
            });

            return await tasks;
        }

        public async Task<Room> UpdateRoom(string login)
        {
            var thread = Task.Factory.StartNew(() =>
            {
                var room = GetRoomByAccount(login);
                if (room == null)
                {
                    return null;
                }
                return _activeRooms.TryUpdate(room.RoomId,
                    room,
                    _activeRooms.FirstOrDefault(x => x.Key == room.RoomId).Value) ? room : null;
            });

            return await thread;
        }

        private bool IsPlayerExists(IAccount account)
        {
            return _activeRooms.Values
                .Any(x => x.Players.Keys
                    .Any(p => p.Item2
                        .Equals(account.Login)));
        }

        private Room GetRoomByAccount(IAccount account)
        {
            return _activeRooms.Values
                .FirstOrDefault(x => x.Players.Keys
                    .Any(p => p.Item2
                        .Equals(account.Login)));
        }
        private Room GetRoomByAccount(string login)
        {
            return _activeRooms.Values
                .FirstOrDefault(x => x.Players.Keys
                    .Any(p => p.Item2
                        .Equals(login)));
        }
        
        private static string RandomString()
        {
            
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)       //Made length equal 6. Can be bigger.
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}