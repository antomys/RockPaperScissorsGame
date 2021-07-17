using System;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Dal.Context;
using Server.Dal.Entities;

namespace Server.Bll.Services
{
    public class RoomService : IRoomService, IHostedRoomService
    {
        private readonly DbSet<Room> _rooms;
        private readonly ServerContext _repository;

        public RoomService(ServerContext repository)
        {
            _repository = repository;
            _rooms = _repository.Rooms;
        }

        public async Task<OneOf<RoomModel, CustomException>> CreateRoom(int userId, bool isPrivate = false)
        {
            var doesRoomExist = await _repository.RoomPlayersEnumerable
                .FirstOrDefaultAsync(x=>x.FirstPlayerId == userId || x.SecondPlayerId == userId);
            if (doesRoomExist != null)
                return new CustomException(ExceptionTemplates.TwinkRoom);
            
            var room = new Room
            {
                IsPrivate = isPrivate,
                RoomCode = Guid.NewGuid().ToString("n")[..8],
                IsFull = false,
                CreationTimeTicks = DateTimeOffset.Now.Ticks,
            }; 
            await _rooms.AddAsync(room);
            await _repository.SaveChangesAsync();
            
            var newRoomPlayers = new RoomPlayers
            {
                RoomId = room.Id,
                FirstPlayerId = userId,
                PlayersCount = 1
            };
            
            await _repository.RoomPlayersEnumerable.AddAsync(newRoomPlayers);
            room.RoomPlayers = newRoomPlayers;
            _rooms.Update(room);
            
            await _repository.SaveChangesAsync();
            return room.Adapt<RoomModel>();
        }

        public async Task<OneOf<RoomModel, CustomException>> JoinRoom(int userId, bool isPrivate, string roomCode)
        {
            if (isPrivate)
            {
                var randomRoom = await _rooms
                    .Include(x => x.RoomPlayers)
                    .Where(x => !x.IsFull).FirstOrDefaultAsync();
                if (randomRoom is null)
                    return new CustomException(ExceptionTemplates.NoAvailableRooms);
                
                if (randomRoom.RoomPlayers.FirstPlayerId == userId 
                    || randomRoom.RoomPlayers.SecondPlayerId == userId)
                    return new CustomException(ExceptionTemplates.AlreadyInRoom);

                if (randomRoom.RoomPlayers.PlayersCount < 2)
                {
                    randomRoom.RoomPlayers.SecondPlayerId = userId;
                }
                
                randomRoom.IsFull = true;

                var round = new Round
                {
                    Id = 0,
                    RoomPlayersId = randomRoom.RoomPlayers.Id,
                    FirstPlayerMove = 0,
                    SecondPlayerMove = 0,
                    LastMoveTicks = DateTimeOffset.Now.Ticks,
                    IsFinished = false
                };
                
                await _repository.Rounds.AddAsync(round);

                randomRoom.RoundId = round.Id;

                _rooms.Update(randomRoom);
                await _repository.SaveChangesAsync();
                return randomRoom.Adapt<RoomModel>();
            }
            var foundRoom = await _rooms
                .Include(x=>x.RoomPlayers).
                FirstOrDefaultAsync(x => x.RoomCode == roomCode);

            if (foundRoom == null)
                return new CustomException(ExceptionTemplates.RoomNotExists);

            if (foundRoom.RoomPlayers.FirstPlayerId == userId || foundRoom.RoomPlayers.SecondPlayerId == userId)
                return new CustomException(ExceptionTemplates.AlreadyInRoom);
            if (foundRoom.RoomPlayers.FirstPlayerId != 0 && foundRoom.RoomPlayers.SecondPlayerId != 0)
                return new CustomException(ExceptionTemplates.RoomFull);

            if (foundRoom.RoomPlayers.FirstPlayerId != 0)
            {
                foundRoom.RoomPlayers.FirstPlayerId = userId;
                _rooms.Update(foundRoom);

                await _repository.SaveChangesAsync();
                return foundRoom.Adapt<RoomModel>();
            }

            if (foundRoom.RoomPlayers.SecondPlayerId == 0) 
                return new CustomException(ExceptionTemplates.Unknown);
            
            foundRoom.RoomPlayers.SecondPlayerId = userId;
            _rooms.Update(foundRoom);

            await _repository.SaveChangesAsync();
            return foundRoom.Adapt<RoomModel>();
        }

        public async Task<OneOf<RoomModel, CustomException>> GetRoom(int roomId)
        {
            var room = await _rooms.FindAsync(roomId);

            return room != null 
                ? room.Adapt<RoomModel>() 
                : new CustomException(ExceptionTemplates.RoomNotExists);
        }
        
        public async Task<int?> UpdateRoom(RoomModel room)
        {
            var thisRoom = await _rooms.FindAsync(room.Id);
            if ( thisRoom == null)
                return 400;
            var updatedRoom = room.Adapt<Room>();

            thisRoom.IsFull = updatedRoom.IsFull;
            thisRoom.IsPrivate = updatedRoom.IsPrivate;
            thisRoom.RoundId = updatedRoom.RoundId;

            if (!_repository.Entry(thisRoom).Properties.Any(x => x.IsModified)) return 400;
            _repository.Update(thisRoom);
            return 200;

        }

        public async Task<int?> DeleteRoom(int userId, int roomId)
        {
            var thisRoom = await _rooms
                .Include(x=>x.RoomPlayers)
                .FirstOrDefaultAsync(x=>x.Id == roomId);
            if (thisRoom == null)
                return 400;
            if (thisRoom.RoomPlayers.FirstPlayerId != userId)
                return 400;
            if (thisRoom.RoomPlayers.SecondPlayerId != userId)
                return 400;
            
            _rooms.Remove(thisRoom);

            await _repository.SaveChangesAsync();
            return 200;
        }

        /// <summary>
        /// Only to use with I HOSTED SERVICE!        
        /// </summary>
        /// <param name="roomOutDate"></param>
        /// <param name="roundOutDate"></param>
        /// <returns></returns>
        [Obsolete(message:"Should be carefully used")]
        public async Task<int> RemoveEntityRangeByDate(TimeSpan roomOutDate, TimeSpan roundOutDate)
        {
            var currentDate = DateTimeOffset.Now.Ticks;
            var rooms = await _rooms
                .Where(x => x.CreationTimeTicks + roomOutDate.Ticks < currentDate && x.RoundId == null)
                .ToArrayAsync();
            
            var allRound = await _repository.Rounds
                .Where(x => x.LastMoveTicks + roundOutDate.Ticks < currentDate)
                .ToArrayAsync();
            
            _rooms.RemoveRange(rooms);
            _repository.Rounds.RemoveRange(allRound);
            
            await _repository.SaveChangesAsync();

            return rooms.Length + allRound.Length;

        }
    }
}