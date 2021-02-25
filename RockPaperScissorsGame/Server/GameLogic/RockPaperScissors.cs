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
        public static string MoveComparator(
            ConcurrentDictionary<string, 
                RequiredGameMove> playerMoves)
        {
            var surrenderMove = 0;
            var winner = string.Empty;
            
            foreach (var (key, value) in playerMoves)
            {
                
                if (surrenderMove == 0 && string.IsNullOrEmpty(winner))
                {
                    winner = key;
                    surrenderMove = (int)value;
                }
                else
                {
                    if ((int)value == surrenderMove)
                    {
                        return string.Empty;
                    }
                    
                    if (surrenderMove == 1 && (int)value == 3)
                        continue;
                    if (surrenderMove == 2 && (int)value == 1)
                        continue;
                    else if (surrenderMove == 3 && (int)value == 2)
                        continue;
                    else if (surrenderMove == 1 && (int)value == 2)
                        winner = key;
                    else if (surrenderMove == 2 && (int)value == 3)
                        winner = key;
                    else if (surrenderMove == 3 && (int)value == 1)
                        winner = key;
                }
            }
            return winner;
        }

        public static ConcurrentDictionary<string,RequiredGameMove> ChangeBotState(
            ConcurrentDictionary<string,
                RequiredGameMove> playerMoves)
        {
            var (key, value) = playerMoves.FirstOrDefault(x => x.Key.Equals("Bot"));
            playerMoves.TryUpdate(key, GenerateRandomMove(), value);

            return playerMoves;
        }

        private static RequiredGameMove GenerateRandomMove()
        {
            var r = new Random();
            var rInt = r.Next(1, 4);

            return (RequiredGameMove)rInt;
        }
    }

}