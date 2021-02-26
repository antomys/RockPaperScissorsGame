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

                //************************************************************************************************************************************
                var elapsedTime = DateTime.Now.Subtract(thisRound.TimeFinished);
                if (elapsedTime.Seconds>= 200 &&
                    thisRound.PlayerMoves.Any(x => x.Value.Equals(RequiredGameMove.Default)))
                {
                    var dictionary = thisRound.PlayerMoves;
                    var first = dictionary.Keys.First();
                    var last = dictionary.Keys.Last();

                    if (dictionary[first] == dictionary[last])
                        thisRound.IsDraw = true;
                    else if (dictionary[first] == RequiredGameMove.Default)
                    {
                        thisRound.LoserId = first;
                        thisRound.WinnerId = last;
                    }
                    else
                    {
                        thisRound.LoserId = last;
                        thisRound.WinnerId = first;
                    }
                    thisRound.TimeFinished = DateTime.Now;
                    thisRound.IsFinished = true;
                    
                    await UpdateRound(thisRound);
                    return thisRound;
                }

                thisRound.TimeFinished = DateTime.Now;

                //************************************************************************************************************************

                var botPlays = false;
                if (thisRound.PlayerMoves.Any(x => x.Key.Equals("Bot")))
                {
                    thisRound.PlayerMoves = RockPaperScissors.ChangeBotState(thisRound.PlayerMoves);
                    botPlays = true;
                }
                thisRound.PlayerMoves = RockPaperScissors.UpdateMove(thisRound.PlayerMoves, accountId, move);

                if (thisRound.PlayerMoves.Values.All(x => x != RequiredGameMove.Default))
                {
                    var winner = RockPaperScissors.MoveComparator(thisRound.PlayerMoves);

                    if (string.IsNullOrEmpty(winner))
                    {
                        thisRound.IsDraw = true;
                       
                        await UpdateRound(thisRound);
                    }

                    if (botPlays)
                    {
                        thisRound.IsFinished = true;
                        thisRound.WinnerId = winner;
                    }
                    else
                    {
                        var loserId = thisRound.PlayerMoves.FirstOrDefault(x => x.Key != winner).Key;
                   
                        thisRound.IsFinished = true;
                        thisRound.WinnerId = _accountManager.AccountsActive.FirstOrDefault(x=> x.Value.Id==winner).Value.Login;
                        thisRound.LoserId = _accountManager.AccountsActive.FirstOrDefault(x=> x.Value.Id==loserId).Value.Login;
                        thisRound.TimeFinished = DateTime.Now;
                    }
                    
                }
                
                await UpdateRound(thisRound);

                return thisRound;
                
            });
            return await await tasks;  //AWAIT AWAIT?
           
        }

        private async Task FillStatistics(IRound thisRound)
        {
            var keys = thisRound.PlayerMoves.Keys;
            foreach (var key in keys) //FIX
            {
                var thisAccountLogin = _accountManager.AccountsActive.FirstOrDefault(x => x.Value.Id == key);
                var statistics = await _storageStatistics.GetAsync(key); //here is the problem.
                if (thisRound.WinnerId.Equals(thisAccountLogin.Value.Login))
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
                    thisRound.PlayerMoves.FirstOrDefault(x => x.Key.Equals(key)).Value;
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
                    // ReSharper disable once PossibleLossOfFraction
                    statistics.WinLossRatio = statistics.Wins / statistics.Loss * 100d;
                else
                {
                    statistics.WinLossRatio = 100d;
                }

                var allRound = await _storageRounds.GetAllAsync();
            
                int wins=0, loss=0;
            
                foreach (var round in allRound)
                {
                    if (!InRange(round.TimeFinished,DateTime.Now.AddDays(-7), DateTime.Now)) continue;
                    if (round.WinnerId.Equals(key))
                        wins++;
                    else if (round.LoserId.Equals(key))
                    {
                        loss++;
                    }
                }

                var winRate = 0d;
                if (loss == 0)
                    winRate = 100d;
                else
                {
                    winRate = (float) wins / loss * 100d;
                }
                
                statistics.TimeSpent = $"Last 7 days win rate: {winRate}%";

                await _storageStatistics.UpdateAsync(key, statistics);
                 
            }
        }

        private static bool InRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }
        
        
        //*****************************
        private async Task<Round> UpdateRound(Round updated)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var roomId =
                    ActiveRounds.Where(x => x.Value
                        .Equals(updated)).ToArray();
                


                if (updated.IsFinished)
                {
                    if (!updated.PlayerMoves.All(x => x.Key != "Bot") || updated.IsDraw) return updated;
                    await _storageRounds.AddAsync(updated);
                    await FillStatistics(updated);


                    //ActiveRounds.TryRemove(roomId[0].Key, out _);

                    return updated;
                }
                ActiveRounds.TryGetValue(roomId[0].Key, out var oldRoom);
                ActiveRounds.TryUpdate(roomId[0].Key, updated, oldRoom);

                return updated;
            });
            
            return await await task;
        }
        
        //********************************
        public async Task<Round> UpdateRound(string roomId)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                ActiveRounds.TryGetValue(roomId, out var updated);

                if (updated == null)
                    return null; //todo: add exception;
                
                //***************************************************************
                var elapsedTime = DateTime.Now.Subtract(updated.TimeFinished);
                if (elapsedTime.Seconds>= 20 &&
                    updated.PlayerMoves.Any(x => x.Value.Equals(RequiredGameMove.Default)))
                {
                    var dictionary = updated.PlayerMoves;
                    var first = dictionary.Keys.First();
                    var last = dictionary.Keys.First();

                    if (dictionary[first] == dictionary[last])
                        updated.IsDraw = true;
                    else if (dictionary[first] == RequiredGameMove.Default)
                    {
                        updated.LoserId = first;
                        updated.WinnerId = last;
                    }
                    else
                    {
                        updated.LoserId = last;
                        updated.WinnerId = first;
                    }
                    updated.TimeFinished = DateTime.Now;
                    updated.IsFinished = true;
                    
                    await UpdateRound(updated);
                    return updated;
                }
                //*****************************************************************
                
                if (updated.IsFinished && !updated.IsDraw)
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
}