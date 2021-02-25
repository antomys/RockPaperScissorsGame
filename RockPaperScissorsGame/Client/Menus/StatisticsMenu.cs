using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Menus.Interfaces;
using Client.Models;
using Client.Services.RequestProcessor;
using Client.Services.RequestProcessor.RequestModels.Impl;
using Newtonsoft.Json;

namespace Client.Menus
{
    internal class StatisticsMenu : IStatisticsMenu
    {
        private readonly string _baseAddress;
        private readonly IRequestPerformer _performer;

        public StatisticsMenu(string baseAddress,
            IRequestPerformer performer)
        {
            _baseAddress = baseAddress;
            _performer = performer;
        }

        public async Task OverallStatistics()
        {
            var options = new RequestOptions
            {
                Address = _baseAddress + "statistics/overallStatistics",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (reachedResponse.Code == 500)
            {
                Console.WriteLine("Server side problem.");
            }
            
            else if(reachedResponse.Content == null || reachedResponse.Code == 404)
            {
                Console.WriteLine("Statistics is currently unavailable.\n Not enough data.");
            }
            
            else
            {
                var deserializeObject = JsonConvert.DeserializeObject<IEnumerable<Statistics>>(reachedResponse.Content);
            
                Console.WriteLine(deserializeObject.Select(x=> x.ToString()).ToArray());
            }
        }
        
        public async Task PersonalStatistics()
        {
            var options = new RequestOptions
            {
                Address = _baseAddress + "statistics/overallStatistics",
                IsValid = true,
                Method = Services.RequestModels.RequestMethod.Get
            };
            var reachedResponse = await _performer.PerformRequestAsync(options);

            if (reachedResponse.Code == 500)
            {
                Console.WriteLine("Server side problem.");
            }
            
            else if(reachedResponse.Content == null || reachedResponse.Code == 404)
            {
                Console.WriteLine("Statistics is currently unavailable.\n Not enough data.");
            }
            
            else
            {
                var deserializeObject = JsonConvert.DeserializeObject<IEnumerable<Statistics>>(reachedResponse.Content);
            
                Console.WriteLine(deserializeObject.Select(x=> x.ToString()).ToArray());
            }
        }

    }
}