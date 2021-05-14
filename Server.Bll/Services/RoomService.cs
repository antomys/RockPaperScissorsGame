using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Exceptions.LogIn;
using Server.GameLogic.Exceptions;
using Server.GameLogic.LogicServices.Interfaces;
using Server.GameLogic.Models;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.GameLogic.LogicServices
{
    public class RoomCoordinator : IRoomCoordinator
    {
        private readonly IAccountService _accountService;

        private readonly IRoundCoordinator _roundCoordinator;

        private static readonly Random Random = new();

        private Timer _timer;

        public ConcurrentDictionary<string, Room> ActiveRooms { get; }

        public RoomCoordinator(
            IAccountService accountService,
            IRoundCoordinator roundCoordinator)
        {
            _accountService = accountService;
            _roundCoordinator = roundCoordinator;
            ActiveRooms = new ConcurrentDictionary<string, Room>();
            _timer = new Timer(CheckRoomDate, null, 0, 10000);
        }

        public async Task<Room> CreateRoom(string sessionId, bool isPrivate)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountBySessionId(sessionId);

                if (ActiveRooms.Any(x => x.Value.Players.Any(p => p.Key.Equals(account.Id))))
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

                //_timer = new Timer(tm, null, 0, 10000); //todo: implement
                return newRoom;
            });
            return await tasks;
        }

        public async Task<Room> JoinPublicRoom(string sessionId)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var thisRoom = ActiveRooms
                    .FirstOrDefault(x =>
                        x.Value.IsPrivate == false
                        && x.Value.Players.Count < 2)
                    .Value;

                var thisAccount = GetAccountBySessionId(sessionId);

                thisRoom.Players.TryAdd(thisAccount.Id, false);

                return UpdateRoom(thisRoom).Result;
            });
            return await tasks;
        }

        public async Task<Room> CreateTrainingRoom(string sessionId)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountBySessionId(sessionId);

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

        public async Task<Room> JoinPrivateRoom(string sessionId, string roomId)
        {
            var tasks = Task.Run(() =>
            {
                if (!ActiveRooms.TryGetValue(roomId, out var thisRoom))
                    return null; //todo:exception;

                if (thisRoom.Players.Count == 2)
                    return null;

                var newRoom = thisRoom;
                var thisAccount = GetAccountBySessionId(sessionId);
                newRoom.Players.TryAdd(thisAccount.Id, false);

                if (newRoom.Players.Count > 1)
                    newRoom.IsFull = true;

                return ActiveRooms.TryUpdate(roomId,
                    newRoom, thisRoom)
                    ? newRoom
                    : null; //todo: change to exception;
            });

            return await tasks;
        }

        private async void CheckRoomDate(object state)
        {
            var threads = Task.Factory.StartNew(() =>
            {
                if (ActiveRooms.IsEmpty) return;
                foreach (var room in ActiveRooms)
                {
                    if (room.Value.CreationTime.AddMinutes(5) < DateTime.Now && room.Value.CurrentRoundId == null)
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
                    updated, room)
                    ? room
                    : null;
            });
            return await thread;
        }
        public async Task<Room> UpdatePlayerStatus(string sessionId, bool isReady)
        {
            var account = GetAccountBySessionId(sessionId);

            var room = ActiveRooms.Values
                .FirstOrDefault(x => x.Players.Keys
                    .Any(p => p
                        .Equals(account.Id)));

            var thisRoom = GetRoomByRoomId(room?.RoomId);

            if (thisRoom == null)
                return null; //Never performs


            var (key, oldValue) =
                thisRoom.Players.FirstOrDefault(x => x.Key == account.Id);


            thisRoom.Players.TryUpdate(key, isReady, oldValue);

            if (thisRoom.Players.Values.All(x => x) && thisRoom.Players.Count == 2)
            {
                thisRoom.IsReady = true;

                thisRoom.IsFull = true;

                if (thisRoom.CurrentRoundId != null)
                    return thisRoom;

                var round = new Round
                {
                    Id = Guid.NewGuid()
                        .ToString(),
                    IsFinished = false,
                    PlayerMoves = new ConcurrentDictionary<string, RequiredGameMove>(),
                    TimeFinished = DateTime.Now,
                    WinnerId = null,
                    LoserId = null,
                };

                foreach (var value in thisRoom.Players.Keys.ToList())
                {
                    round.PlayerMoves.TryAdd(value, RequiredGameMove.Default);
                }

                thisRoom.CurrentRoundId = round.Id;

                _roundCoordinator.ActiveRounds.TryAdd(thisRoom.RoomId, round);

            }

            return await UpdateRoom(thisRoom);
        }
        public async Task<Room> UpdateRoom(string roomId)
        {
            try
            {
                var room = GetRoomByRoomId(roomId);

                var thisRound = _roundCoordinator.ActiveRounds.FirstOrDefault(x => x.Key.Equals(room.RoomId));

                if (thisRound.Value != null && thisRound.Value.IsFinished)
                {
                    room.IsReady = false;
                    room.IsRoundEnded = false;
                    room.CurrentRoundId = null;
                    foreach (var (key, value) in room.Players)
                    {
                        if (key.Equals("Bot"))
                            room.Players.TryUpdate(key, true, value);
                        else
                        {
                            room.Players.TryUpdate(key, false, value);
                        }
                    }
                    _roundCoordinator.ActiveRounds.TryRemove(thisRound);

                    await UpdateRoom(room);
                }
                
                return await UpdateRoom(room);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        #region PrivateMethods
        private static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private Account GetAccountBySessionId(string sessionId)
        {
            _accountService.AccountsActive.TryGetValue(sessionId, out var account);
            if (account != null) 
                return account;
            throw new UserNotFoundException(nameof(account));

        }
        private Room GetRoomByRoomId(string roomId)
        {
            return ActiveRooms.TryGetValue(roomId, out var thisRoom)
                ? thisRoom
                : throw new UserNotFoundException();
        }

        #endregion
        
    }
}