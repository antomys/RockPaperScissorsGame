using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Server.Bll.Exceptions;
using Server.Bll.Models;
using Server.Dal.Context;
using Server.Dal.Entities;

namespace Server.Bll.Services
{
    public class RoomService : IRoomService
    {
        private readonly DbSet<Room> _rooms;
        private readonly ServerContext _repository;

        public RoomService(ServerContext repository)
        {
            _repository = repository;
            _rooms = _repository.Rooms;
        }

        public async Task<OneOf<RoomModel, RoomException>> CreateRoom(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<OneOf<RoomModel, RoomException>> GetRoom()
        {
            throw new System.NotImplementedException();
        }

        public async Task<int?> UpdateRoom()
        {
            throw new System.NotImplementedException();
        }

        public async Task<int?> DeleteRoom()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IRoomService
    {
        Task<OneOf<RoomModel, RoomException>> CreateRoom(int userId);
        Task<OneOf<RoomModel, RoomException>> GetRoom();
        Task<int?> UpdateRoom();
        Task<int?> DeleteRoom();
    }
}