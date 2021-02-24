using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.GameLogic.Models;

namespace Server.GameLogic
{
    public static class RockPaperScissors
    {
        public static ConcurrentDictionary<string, int> UpdateMove(
            ConcurrentDictionary<string,int> playerMoves, string accountId, int move)
        {
            //
            
            playerMoves.TryUpdate(accountId, move, 
                playerMoves.FirstOrDefault(x=> x.Key.Equals(accountId)).Value);

            return playerMoves;
        }
    }
}