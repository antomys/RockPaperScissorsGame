using System;
using System.Linq;
using System.Threading.Tasks;
using Server.Database;
using Server.Exceptions.LogIn;
using Server.Exceptions.Room;
using Server.GameLogic.Exceptions;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class RoomManager : IRoomManager
    {
        private readonly ApplicationDbContext _applicationDbContext;
        
        private static readonly Random Random = new();

        //private Timer _timer;
        public RoomManager(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            //_timer = new Timer(CheckRoomDate, null, 0, 50000);
        }

        public async Task<Room> CreateRoom(string sessionId, bool isPrivate)
        { 
            var thisPlayer = await GetAccountBySessionId(sessionId);

            if(_applicationDbContext.Rooms
                .Any(x=> x.FirstPlayerId == thisPlayer.Id || x.SecondPlayerId == thisPlayer.Id))
                throw new TwinkGameRoomCreationException();

            var newRoom = new Room
            {
                Id = RandomString(),
                FirstPlayerId = thisPlayer.Id,
                IsReadyFirst = false,
                SecondPlayerId = null,
                IsReadySecond = false,
                IsPrivate = isPrivate,
                CreationTime = DateTime.Now,
            };
            
            //_timer = new Timer(tm, null, 0, 10000); //todo: implement
            await _applicationDbContext.Rooms.AddAsync(newRoom);

            await _applicationDbContext.SaveChangesAsync();
            
            return newRoom;
            }
        

        /*public async Task<Room> CreateTrainingRoom(string sessionId)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountBySessionId(sessionId);

                if (ActiveRooms.Any(x => x.Value.Players.Any(p => p.Key.Equals(account.Id))))
                    throw new TwinkGameRoomCreationException();

                var newRoom = new Room
                {
                    Id = RandomString(),
                    Players = new Dictionary<string, bool>(),
                    IsPrivate = true,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false,
                    CreationTime = DateTime.Now
                };

                newRoom.Players.TryAdd(account.Id, false);
                newRoom.Players.TryAdd("Bot", true);
                newRoom.IsFull = true;
                ActiveRooms.TryAdd(newRoom.Id, newRoom);

                return newRoom;
            });
            return await tasks;
        }*/

        public async Task<Room> JoinRoom(string sessionId, string roomType, string roomId = "")
        {
            if (string.IsNullOrEmpty(roomId) && roomType == "Private")
                throw new InvalidRoomIdException(roomId);
           
            if (roomType == "Public" || string.IsNullOrEmpty(roomId) && roomType != "Private")
            {
                var room = _applicationDbContext.Rooms.FirstOrDefault(x => x.IsPrivate == false);
                    if ( room== null)
                        throw new NoPublicRoomsException("Public");
                    return room;
            }
            //if (!_applicationDbContext.Rooms.Any(x => x.Id == roomId))
                

            var thisRoom = await _applicationDbContext.Rooms.FindAsync(roomId);
            
            if(thisRoom is null)
                throw new RoomNotFoundException(roomId);

            if (thisRoom.FirstPlayerId != null && thisRoom.SecondPlayerId != null)
                throw new RoomIsFullException(roomId);

            var thisAccount = await GetAccountBySessionId(sessionId);

            if (thisRoom.FirstPlayerId == thisAccount.Id || thisRoom.SecondPlayerId == thisAccount.Id)
                throw new RoomException("Already entered the room");
            thisRoom.SecondPlayerId = thisAccount.Id;
            _applicationDbContext.Update(thisRoom);

            await _applicationDbContext.SaveChangesAsync();
            return thisRoom;

        }

        /*private void CheckRoomDate(object state)
        { 
            //todo: probably redo
            if (!_applicationDbContext.Rooms.Any()) return; 
                foreach (var room in _applicationDbContext.Rooms)
                {
                    if (room.CreationTime.AddMinutes(5) < DateTime.Now && room.RoundId == null)
                        _applicationDbContext.Rooms.Remove(room);
                }
        }*/

        /*public async Task<bool> DeleteRoom(string roomId)
        {
            var tasks = Task.Factory.StartNew(() =>
                ActiveRooms.TryRemove(roomId, out _));
            return await tasks;
        }*/

        /*public async Task<Room> UpdateRoom(Room updated)
        {
            var thread = Task.Factory.StartNew(() =>
            {
                ActiveRooms.TryGetValue(updated.Id, out var room);
                if (room == null)
                {
                    return null; //todo: change into exception;
                }

                return ActiveRooms.TryUpdate(room.Id,
                    updated, room)
                    ? room
                    : null;
            });
            return await thread;
        }*/
        /*public async Task<Room> UpdatePlayerStatus(string sessionId, bool isReady)
        {
            var account = GetAccountBySessionId(sessionId);

            var room = ActiveRooms.Values
                .FirstOrDefault(x => x.Players.Keys
                    .Any(p => p
                        .Equals(account.Id)));

            var thisRoom = GetRoomByRoomId(room?.Id);

            if (thisRoom == null)
                return null; //Never performs


            var (key, oldValue) =
                thisRoom.Players.FirstOrDefault(x => x.Key == account.Id);


            //todo: change this
            //thisRoom.Players.TryUpdate(key, isReady, oldValue);

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

                _roundCoordinator.ActiveRounds.TryAdd(thisRoom.Id, round);

            }

            return await UpdateRoom(thisRoom);
        }*/
        /*public async Task<Room> UpdateRoom(string roomId)
        {
            try
            {
                var room = GetRoomByRoomId(roomId);

                var thisRound = _roundCoordinator.ActiveRounds.FirstOrDefault(x => x.Key.Equals(room.Id));

                //todo:and this
                /*if (thisRound.Value != null && thisRound.Value.IsFinished)
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
                }#1#
                
                return await UpdateRoom(room);
            }
            catch (Exception exception)
            {
                return null;
            }
            
        }*/

        #region PrivateMethods
        private static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private async  Task<Account> GetAccountBySessionId(string sessionId)
        {
            var activeAccountSession = await  _applicationDbContext.ActiveSessionsEnumerable.FindAsync(sessionId);
            if (activeAccountSession != null) 
                return await _applicationDbContext.Accounts.FindAsync(activeAccountSession.AccountId);
            throw new UserNotFoundException(nameof(sessionId));

        }
        /*private Room GetRoomByRoomId(string roomId)
        {
            return ActiveRooms.TryGetValue(roomId, out var thisRoom)
                ? thisRoom
                : throw new UserNotFoundException();
        }*/

        #endregion
        
    }
}