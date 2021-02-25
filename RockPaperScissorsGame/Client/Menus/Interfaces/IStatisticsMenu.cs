using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models;

namespace Client.Menus.Interfaces
{
    internal interface IStatisticsMenu
    {
        Task OverallStatistics();

        Task PersonalStatistics();

    }
}