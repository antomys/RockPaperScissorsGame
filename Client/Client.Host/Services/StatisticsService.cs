// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Client.Host.Models;
// using Client.Host.Models.Interfaces;
// using Client.Host.Services.RequestProcessor;
// using Client.Host.Services.RequestProcessor.RequestModels;
// using Client.Host.Services.RequestProcessor.RequestModels.Impl;
// using Mapster;
// using Newtonsoft.Json;
//
// namespace Client.Host.Services;
//
// public class StatisticsService: IStatisticsService
// {
//     private readonly IRequestPerformer _requestPerformer;
//
//     public StatisticsService(IRequestPerformer requestPerformer)
//     {
//         _requestPerformer = requestPerformer;
//     }
//
//     public async Task<IOverallStatistics[]> GetAllStatistics()
//     {
//         var options = new RequestOptions
//         {
//             ContentType = "none",
//             Address = "stats/all",
//             IsValid = true,
//             Method = RequestMethod.Get,
//             Name = "OverallStats"
//         };
//
//         var response = await _requestPerformer.PerformRequestAsync(options);
//
//         var toConvert = JsonConvert.DeserializeObject<Statistics[]>(response.Content);
//         return toConvert?.Adapt<IOverallStatistics[]>();
//     }
//         
//     public async Task<IOverallStatistics> GetPersonalStatistics(string token)
//     {
//         var options = new RequestOptions
//         {
//             Headers = new Dictionary<string, string>{{"Authorization",token}},
//             ContentType = "none",
//             Address = "stats/personal",
//             IsValid = true,
//             Method = RequestMethod.Get,
//             Name = "PersonalStats"
//         };
//
//         var response = await _requestPerformer.PerformRequestAsync(options);
//
//         return response.Content != null ? JsonConvert.DeserializeObject<Statistics>(response.Content) : null;
//     }
//
//     public Task PrintStatistics(IOverallStatistics[] statistics)
//     {
//         for(var i = 0; i < statistics.Length; i++)
//         {
//             TextWrite.Print($"{i+1}. User: {statistics[i].Account.Login}\n" +
//                             $"Score: {statistics[i].Score}", ConsoleColor.White);
//         }
//         Console.Write('\n');
//         return Task.CompletedTask;
//     }
// }
//
// public interface IStatisticsService
// {
//     Task<IOverallStatistics[]> GetAllStatistics();
//     Task<IOverallStatistics> GetPersonalStatistics(string token);
//     Task PrintStatistics(IOverallStatistics[] statistics);
// }