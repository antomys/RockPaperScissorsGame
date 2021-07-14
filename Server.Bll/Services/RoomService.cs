using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Mapper;
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

        public async Task<OneOf<RoomModel, RoomException>> CreateRoom(int userId, bool isPrivate = false)
        {
            var doesRoomExist = await _repository.RoomPlayersEnumerable
                .FirstOrDefaultAsync(x=>x.FirstPlayerId == userId || x.SecondPlayerId == userId);
            if (doesRoomExist != null)
                return new RoomException(ExceptionTemplates.TwinkRoom, (int) HttpStatusCode.BadRequest);
            
            var room = new Room
            {
                IsPrivate = isPrivate,
                RoomCode = Guid.NewGuid().ToString("n")[..8],
                IsReady = false,
                IsFull = false,
                CreationTimeTicks = DateTimeOffset.Now.Ticks,
                IsRoundEnded = false,
            }; 
            await _rooms.AddAsync(room);
            await _repository.SaveChangesAsync();
            
            var thisUser = await _repository.Accounts.FindAsync(userId);

            var newRoomPlayers = new RoomPlayers
            {
                RoomId = room.Id,
                FirstPlayerId = userId,
                FirstPlayer = thisUser
            };
            
            await _repository.RoomPlayersEnumerable.AddAsync(newRoomPlayers);
            room.RoomPlayers = newRoomPlayers;
            _rooms.Update(room);
            
            await _repository.SaveChangesAsync();
            return room.ToRoomModel();
        }

        public async Task<OneOf<RoomModel, RoomException>> JoinRoom(int userId, string roomCode)
        {
            var foundRoom = await _rooms
                .Include(x=>x.RoomPlayers).
                FirstOrDefaultAsync(x => x.RoomCode == roomCode);

            if (foundRoom == null)
                return new RoomException(ExceptionTemplates.RoomNotExists, 400);

            if (foundRoom.RoomPlayers.FirstPlayerId == userId || foundRoom.RoomPlayers.SecondPlayerId == userId)
                return new RoomException(ExceptionTemplates.AlreadyInRoom, 400);
            if (foundRoom.RoomPlayers.FirstPlayerId != 0 && foundRoom.RoomPlayers.SecondPlayerId != 0)
                return new RoomException(ExceptionTemplates.RoomFull, 400);

            if (foundRoom.RoomPlayers.FirstPlayerId != 0)
            {
                foundRoom.RoomPlayers.FirstPlayerId = userId;
                _rooms.Update(foundRoom);

                await _repository.SaveChangesAsync();
                return foundRoom.Adapt<RoomModel>();
            }

            if (foundRoom.RoomPlayers.SecondPlayerId == 0) 
                return new RoomException(ExceptionTemplates.Unknown, 400);
            
            foundRoom.RoomPlayers.SecondPlayerId = userId;
            _rooms.Update(foundRoom);

            await _repository.SaveChangesAsync();
            return foundRoom.Adapt<RoomModel>();
        }

        public async Task<OneOf<RoomModel, RoomException>> GetRoom(int roomId)
        {
            var room = await _rooms.FindAsync(roomId);

            return room != null 
                ? room.Adapt<RoomModel>() 
                : new RoomException(ExceptionTemplates.RoomNotExists, 400);
        }
        
        public async Task<int?> UpdateRoom(RoomModel room)
        {
            var thisRoom = await _rooms.FindAsync(room.Id);
            if ( thisRoom == null)
                return 400;
            var updatedRoom = room.Adapt<Room>();

            thisRoom.IsFull = updatedRoom.IsFull;
            thisRoom.IsPrivate = updatedRoom.IsPrivate;
            thisRoom.IsReady = updatedRoom.IsReady;
            thisRoom.RoundId = updatedRoom.RoundId;
            
            return _repository.Entry(thisRoom).Properties.Any(x => x.IsModified) 
                ? 400 
                : 200;
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
        /// ONLY TO BE USED WITH I HOSTED SERVICE. REMOVES RANGE OF ROOMS
        /// </summary>
        /// <param name="rooms"></param>
        /// <returns></returns>
        [Obsolete(message:"Should be carefully used")]
        public async Task<bool> RemoveRoomRange(Room[] rooms)
        {
            try
            {
                _rooms.RemoveRange(rooms);

                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Only to use with IHOSTEDSERVICE!        
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        [Obsolete(message:"Should be carefully used")]
        public async Task<Room[]> GetRoomsByDate(TimeSpan timeSpan)
        {
            var currentDate = DateTimeOffset.Now.Ticks;
            return await _rooms
                .Where(x => x.CreationTimeTicks + timeSpan.Ticks < currentDate
                && x.RoundId == null)
                .ToArrayAsync();
        }
    }

    public interface IHostedRoomService
    {
        Task<Room[]> GetRoomsByDate(TimeSpan timeSpan);
        Task<bool> RemoveRoomRange(Room[] rooms);
    }
}