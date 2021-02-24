using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.GameLogic.Models.Impl;
using Server.Services.Interfaces;

namespace Server.GameLogic.LogicServices.Impl
{
    public class RoundCoordinator : IRoundCoordinator
    {
        private readonly IRoomCoordinator _roomCoordinator;

        private readonly IDeserializedObject<Round> _deserializedRounds;

        private readonly IStorage<Round> _storageRounds;
        
        public ConcurrentDictionary<string, Round> ActiveRound { get; set; }
        
        

        public RoundCoordinator(
            IRoomCoordinator roomCoordinator,
            IDeserializedObject<Round> deserializedRounds,
            IStorage<Round> storageRounds)
        {
            _deserializedRounds = deserializedRounds;
            _roomCoordinator = roomCoordinator;
            _storageRounds = storageRounds;
        }

        public Task<Round> MakeMove(string roomId, string accountId, int move)
        {
            ActiveRound.TryGetValue(roomId, out var thisRound);
            
            
        }


    }
}
