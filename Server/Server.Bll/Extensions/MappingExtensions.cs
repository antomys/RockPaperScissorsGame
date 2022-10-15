// using Mapster;
// using Server.Bll.Models;
// using Server.Data.Entities;
//
// namespace Server.Bll.Extensions;
//
// internal static class MappingExtensions
// {
//     internal static readonly TypeAdapterSetter<Statistics, ShortStatisticsModel> StatisticsAdapterConfig = 
//         TypeAdapterConfig<Statistics, ShortStatisticsModel>
//             .NewConfig()
//             .Map(shortStatisticsModel => shortStatisticsModel.Login, statistics => statistics.Account.Login)
//             .Map(shortStatisticsModel => shortStatisticsModel.Score, statistics => statistics.Score);
// }