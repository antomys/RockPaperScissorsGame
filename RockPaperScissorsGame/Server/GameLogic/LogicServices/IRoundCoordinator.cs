using System.Threading.Tasks;
using Server.GameLogic.Models.Impl;

namespace Server.GameLogic.LogicServices
{
    public interface IRoundCoordinator
    {
        Task<Round> MakeMove(string sessionId, int move);
        
    }
}