using Server.Models;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly IDeserializedObject<Statistics> _statisticsStorage;

        private readonly IAccountManager _activeAccounts;

        private readonly object _gameRooms;

        public StatisticsManager(
            IDeserializedObject<Statistics> statisticsStorage,
            IAccountManager activeAccounts,
            object gameRooms)
        {
            _activeAccounts = activeAccounts;
            _statisticsStorage = statisticsStorage;
            _gameRooms = gameRooms;
        }
    }
}