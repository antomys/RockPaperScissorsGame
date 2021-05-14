using System.Threading.Tasks;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces
{
    public interface IRoomCoordinator
    {
        Task<RoomModel> CreateRoom();
        Task<RoomModel> JoinRoom();
        Task<RoomModel> UpdateRoom();
        Task<bool> DeleteRoom();
    }
}
