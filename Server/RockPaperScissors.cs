using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Models;

namespace Server
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
        
        /// <summary>
        /// Implementation or Rock Paper Scissors Game. 
        /// </summary>
        /// <param name="playerMoves">ConcurrentDictionary of players</param>
        /// <returns>string Winner</returns>
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

        /// <summary>
        /// Changes Bot move if he is in room
        /// </summary>
        /// <param name="playerMoves">ConcurrentDictionary of players</param>
        /// <returns>ConcurrentDictionary of players</returns>
        public static ConcurrentDictionary<string,RequiredGameMove> ChangeBotState(
            ConcurrentDictionary<string,
                RequiredGameMove> playerMoves)
        {
            var (key, value) = playerMoves.FirstOrDefault(x => x.Key.Equals("Bot"));
            playerMoves.TryUpdate(key, GenerateRandomMove(), value);

            return playerMoves;
        }

        /// <summary>
        /// Gets randomized move for the bot
        /// </summary>
        /// <returns>Enumerator RequiredGameMove</returns>
        private static RequiredGameMove GenerateRandomMove()
        {
            var r = new Random();
            var rInt = r.Next(1, 4);

            return (RequiredGameMove)rInt;
        }
    }

}