using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.GameLogic.Models;

namespace Server.GameLogic
{
    public static class RockPaperScissors
    {
        public static ConcurrentDictionary<string, RequiredGameMove> UpdateMove(
            ConcurrentDictionary<string,RequiredGameMove> playerMoves, string accountId, int move)
        {
            //
            
            playerMoves.TryUpdate(accountId, (RequiredGameMove)move, 
                playerMoves.FirstOrDefault(x=> x.Key.Equals(accountId)).Value);

            return playerMoves;
        }
    }
}