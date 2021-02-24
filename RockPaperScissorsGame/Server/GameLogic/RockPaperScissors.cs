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
            playerMoves.TryUpdate(accountId, (RequiredGameMove)move, 
                playerMoves.FirstOrDefault(x=> x.Key.Equals(accountId)).Value);

            return playerMoves;
        }
        public static string MoveComparator( //Can be improved
            ConcurrentDictionary<string, RequiredGameMove> playerMoves)
        {
            int surrenderMove = 0;
            string winner = String.Empty;
            foreach (var m in playerMoves)
            {
                if (surrenderMove == 0 && String.IsNullOrEmpty(winner))
                {
                    winner = m.Key;
                    surrenderMove = (int)m.Value;
                }
                else
                {
                    if ((int)m.Value == surrenderMove)
                    {
                        return String.Empty;
                    }
                    else
                    {
                        if (surrenderMove == 1 && (int)m.Value == 3)
                            continue;
                        else if (surrenderMove == 2 && (int)m.Value == 1)
                            continue;
                        else if (surrenderMove == 3 && (int)m.Value == 2)
                            continue;
                        else if (surrenderMove == 1 && (int)m.Value == 2)
                            winner = m.Key;
                        else if (surrenderMove == 2 && (int)m.Value == 3)
                            winner = m.Key;
                        else if (surrenderMove == 3 && (int)m.Value == 1)
                            winner = m.Key;
                    }
                }
            }
            return winner;
        }
    }

}