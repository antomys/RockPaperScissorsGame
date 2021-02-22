using System.Threading.Tasks;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IRoomManager
    {
        Task<Room> CreateRoom(Account account);

        Task<Room> JoinRoom(string roomId, Account account);
    }
}