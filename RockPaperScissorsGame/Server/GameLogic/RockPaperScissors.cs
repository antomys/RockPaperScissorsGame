using System.Collections.Concurrent;
using System.Linq;

namespace Server.GameLogic
{
    public class RockPaperScissors
    {
        public ConcurrentDictionary<string, int> UpdateMoves(
            ConcurrentDictionary<string,int> playerMoves, string accountId, int move)
        {
            //playerMoves.Values.All(x=> 0)
            
            playerMoves.TryUpdate(accountId,move,)
        }
    }
}