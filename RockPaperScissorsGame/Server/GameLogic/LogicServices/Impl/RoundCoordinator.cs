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

        public RoundCoordinator(
            IRoomCoordinator roomCoordinator,
            IDeserializedObject<Round> deserializedRounds,
            IStorage<Round> storageRounds)
        {
            _deserializedRounds = deserializedRounds;
            _roomCoordinator = roomCoordinator;
            _storageRounds = storageRounds;
        }

        public Task<Round> MakeMove(string sessionId, int move)
        {
            throw new System.NotImplementedException();
        }
    }
}
