using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.GameLogic.Models;

namespace Server.GameLogic.LogicServices.Interfaces
{
    public interface IRoundCoordinator
    {
        /// <summary>
        /// Task to make a move
        /// </summary>
        /// <param name="roomId">current room Id</param>
        /// <param name="accountId">Current account Id</param>
        /// <param name="move">Move that he has done</param>
        /// <returns>Round</returns>
        Task<Round> MakeMove(string roomId, string accountId, int move);
        
        /// <summary>
        /// Dictionary of active rounds. Made for monitoring
        /// </summary>
        ConcurrentDictionary<string, Round> ActiveRounds { get; set; }
        
        /// <summary>
        /// Gets current active round for special room
        /// </summary>
        /// <param name="roundId">id of this round</param>
        /// <returns>Round</returns>
        Task<Round> GetCurrentActiveRoundForSpecialRoom(string roundId);
        
        /// <summary>
        /// Updates rounds
        /// </summary>
        /// <param name="roomId">id of this room</param>
        /// <returns>Round</returns>
        Task<Round> UpdateRound(string roomId);
    }
}