using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Server.GameLogic.Models;
using Server.GameLogic.Models.Impl;
using Server.Services.Interfaces;

namespace Server.GameLogic.LogicServices.Impl
{
    public class RoundCoordinator : IRoundCoordinator
    {
        private readonly IStorage<Round> _storageRounds;

        private readonly IAccountManager _accountManager;

        public ConcurrentDictionary<string, Round> ActiveRounds { get; set; }
        
        public RoundCoordinator(
            IStorage<Round> storageRounds,
            IAccountManager accountManager)
        {
            _storageRounds = storageRounds;
            _accountManager = accountManager;
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

                thisRound.PlayerMoves = RockPaperScissors.UpdateMove(thisRound.PlayerMoves, accountId, move);

                if (thisRound.PlayerMoves.Values.All(x => x != RequiredGameMove.Default))
                {
                    var winner = RockPaperScissors.MoveComparator(thisRound.PlayerMoves);

                    if (string.IsNullOrEmpty(winner))
                        return null;

                    thisRound.IsFinished = true;
                    thisRound.WinnerId = winner;
                    thisRound.LoserId = thisRound.PlayerMoves.FirstOrDefault(x => x.Key != winner).Key;
                    thisRound.TimeFinished = DateTime.Now;
                }
                
                await UpdateRound(thisRound);

                return thisRound;
                
            });
            return await await tasks;  //AWAIT AWAIT?
           
        }
        private async Task<Round> UpdateRound(Round updated)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var roomId = 
                    ActiveRounds.Where(x => x.Value
                        .Equals(updated)).Select(x => x.Key).ToString();
                if (updated.IsFinished)
                {
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

}