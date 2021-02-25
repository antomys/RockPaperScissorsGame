using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Server.GameLogic.Models;
using Server.GameLogic.Models.Impl;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.GameLogic.LogicServices.Impl
{
    public class RoundCoordinator : IRoundCoordinator
    {
        private readonly IStorage<Round> _storageRounds;

        private readonly IStorage<Statistics> _storageStatistics;

        private readonly IAccountManager _accountManager;

        public ConcurrentDictionary<string, Round> ActiveRounds { get; set; }
        
        public RoundCoordinator(
            IStorage<Round> storageRounds,
            IAccountManager accountManager,
            IStorage<Statistics> storageStatistics)
        {
            _storageRounds = storageRounds;
            _accountManager = accountManager;
            _storageStatistics = storageStatistics;
            ActiveRounds = new ConcurrentDictionary<string, Round>();
        }
        

        public async Task<Round> MakeMove(string roomId, string sessionId, int move)
        {
            var tasks = Task.Factory.StartNew(async () =>
            {
                var accountId = _accountManager.GetActiveAccountBySessionId(sessionId).Id;
                ActiveRounds.TryGetValue(roomId, out var thisRound);

                if (thisRound == null)
                    return null; //todo: exception;

                if (thisRound.PlayerMoves.Any(x => x.Key.Equals("Bot")))
                    thisRound.PlayerMoves = RockPaperScissors.ChangeBotState(thisRound.PlayerMoves);
                thisRound.PlayerMoves = RockPaperScissors.UpdateMove(thisRound.PlayerMoves, accountId, move);

                if (thisRound.PlayerMoves.Values.All(x => x != RequiredGameMove.Default))
                {
                    var winner = RockPaperScissors.MoveComparator(thisRound.PlayerMoves);

                    if (string.IsNullOrEmpty(winner))
                    {
                        thisRound.IsDraw = true;
                        return null;
                    }
                    
                    thisRound.IsFinished = true;
                    thisRound.WinnerId = winner;
                    thisRound.LoserId = thisRound.PlayerMoves.FirstOrDefault(x => x.Key != winner).Key;
                    thisRound.TimeFinished = DateTime.Now;

                    await FillStatistics(thisRound,thisRound.WinnerId);
                    await FillStatistics(thisRound,thisRound.LoserId);
                    
                }
                
                await UpdateRound(thisRound);

                return thisRound;
                
            });
            return await await tasks;  //AWAIT AWAIT?
           
        }

        private async Task FillStatistics(IRound thisRound, string accountId)
        {
            var statistics = await _storageStatistics.GetAsync(thisRound.LoserId);
            if (thisRound.WinnerId.Equals(accountId))
            {
                statistics.Wins += 1;
                statistics.Score += 4;
            }
            else
            {
                statistics.Loss += 1;
                statistics.Score -= 2;
            }

            var playerMove =
                thisRound.PlayerMoves.FirstOrDefault(x => x.Key.Equals(accountId)).Value;
            switch (playerMove) //NOT TO ADD ANYTHING ELSE
            {
                case RequiredGameMove.Paper:
                    statistics.UsedPaper += 1;
                    break;
                case RequiredGameMove.Rock:
                    statistics.UsedRock += 1;
                    break;
                case RequiredGameMove.Scissors:
                    statistics.UsedScissors += 1;
                    break;
            }

            if (statistics.Loss != 0)
                statistics.WinLossRatio = (double)statistics.Wins / statistics.Loss;

            var allRound = await _storageRounds.GetAllAsync();
            
            int wins=0, loss=0;
            
            foreach (var round in allRound)
            {
                if (!InRange(round.TimeFinished,DateTime.Now.Date, DateTime.Now.Date.AddDays(-7))) continue;
                if (round.WinnerId.Equals(accountId))
                    wins++;
                else
                {
                    loss++;
                }
            }

            statistics.TimeSpent = loss != 0 
                ? $"Last 7 days win rate: {(double) wins / loss}%" 
                : $"Last 7 days win rate: {(double) wins}%";

            await _storageStatistics.UpdateAsync(accountId, statistics);
        }

        private static bool InRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }
        
        private async Task<Round> UpdateRound(Round updated)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var roomId = 
                    ActiveRounds.Where(x => x.Value
                        .Equals(updated)).Select(x => x.Key).ToString();
                
                if (updated.IsFinished && !updated.IsDraw)
                {
                    if(!updated.PlayerMoves.Any(x=> x.Key.Equals("Bot")))
                        await _storageRounds.AddAsync(updated);

                    ActiveRounds.TryRemove(roomId, out _);

                    return updated;
                }

                ActiveRounds.TryGetValue(roomId, out var oldRoom);
                ActiveRounds.TryUpdate(roomId, updated, oldRoom);

                return updated;
            });
            
            return await await task;
        }
        
        public async Task<Round> UpdateRound(string roomId)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                ActiveRounds.TryGetValue(roomId, out var updated);

                if (updated == null)
                    return null; //todo: add exception;

                if (updated.IsFinished)
                {
                    if(updated.PlayerMoves.Keys.Any(x=> x!="Bot"))
                        await _storageRounds.AddAsync(updated);

                    ActiveRounds.TryRemove(roomId, out _);

                    return updated;
                }

                ActiveRounds.TryGetValue(roomId, out var oldRoom); //Do something with this
                ActiveRounds.TryUpdate(roomId, updated, oldRoom);

                return updated;
            });

            return await await task; //Task<Task<round>>??????????????????
        }
        public async Task<Round> GetCurrentActiveRoundForSpecialRoom(string roundId)
        {
            var tasks = Task.Factory.StartNew(() => ActiveRounds.TryGetValue(roundId, out var thisRound) ? thisRound : null);
            //todo: change null to exception;
            return await tasks;
        }
    }

    internal class Datetime
    {
    }
}