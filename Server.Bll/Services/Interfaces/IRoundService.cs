using System.Threading.Tasks;
using Server.Bll.Models;

namespace Server.Bll.Services.Interfaces
{
    public interface IRoundService
    {
        Task<RoomPlayersModel> CreateRoundAsync();
        Task<RoundModel> MakeMoveAsync();
        Task<RoomPlayersModel> UpdateRoundAsync();
        Task<RoundModel> UpdateRound();
    }
}