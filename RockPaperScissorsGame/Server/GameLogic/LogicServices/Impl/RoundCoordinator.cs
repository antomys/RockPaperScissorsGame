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

        public Task<Round> MakeMove(string sessionId, int move)
        {
            throw new System.NotImplementedException();
        }

        public ConcurrentDictionary<string, Round> ActiveRounds { get; set; }
        
        public RoundCoordinator(
            IDeserializedObject<Round> deserializedRounds,
            IStorage<Round> storageRounds)
        {
            _deserializedRounds = deserializedRounds;
            //_roomCoordinator = roomCoordinator;
            _storageRounds = storageRounds;
            ActiveRounds = new ConcurrentDictionary<string, Round>();
        }
        public async Task<Round> GetCurrentActiveRoundForSpecialRoom(string roomId)
        {
            var thread = Task.Factory.StartNew(() =>
            {
                if (ActiveRounds.TryGetValue(roomId, out var thisRound))
                    return thisRound;
                return null;
            });
            return await thread;
        }

        public void MakeMove(string roomId, string accountId, int move)
        {
            ActiveRounds.TryGetValue(roomId, out var thisRound);

            thisRound.PlayerMoves = RockPaperScissors.UpdateMove(thisRound.PlayerMoves, accountId, move);

            if (thisRound.PlayerMoves.Values.All(x => x != RequiredGameMove.Default))
            {
                var winner = RockPaperScissors.MoveComparator(thisRound.PlayerMoves);
            }
        }
    }

}
