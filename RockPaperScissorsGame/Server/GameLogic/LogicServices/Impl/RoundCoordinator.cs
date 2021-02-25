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

        private readonly IDeserializedObject<Round> _deserializedRounds;

        private readonly IStorage<Round> _storageRounds;
        

        public ConcurrentDictionary<string, Round> ActiveRounds { get; set; }
        
        public RoundCoordinator(
            IDeserializedObject<Round> deserializedRounds,
            IStorage<Round> storageRounds)
        {
            _deserializedRounds = deserializedRounds;
            _storageRounds = storageRounds;
            ActiveRounds = new ConcurrentDictionary<string, Round>();
        }
        public async Task<Round> GetCurrentActiveRoundForSpecialRoom(string roundId)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                if (ActiveRounds.TryGetValue(roundId, out var thisRound))
                    return thisRound;
                return null; //ToDo: exception
            });
            return await tasks;
        }

        public async Task<Round> MakeMove(string roomId, string accountId, int move)
        {
            var tasks = Task.Factory.StartNew(async () =>
            {
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
                    thisRound.LoserId = thisRound.PlayerMoves.FirstOrDefault(x => x.Key != winner).Value.ToString();
                    thisRound.TimeFinished = DateTime.Now;
                }
                
                await UpdateRound(thisRound);

                return thisRound;
                
            });

            return await await tasks;  //AWAIT AWAIT?
           
        }

        /*public async Task<Round> UpdateRound(string roomId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                
            });

            return await task;
        }*/

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

            return await await task; //Task<Task<round>>??????????????????
        }
    }

}
