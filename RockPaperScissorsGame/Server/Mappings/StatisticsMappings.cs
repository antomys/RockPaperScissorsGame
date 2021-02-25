using System;
using Server.Contracts;
using Server.Models;

namespace Server.Mappings
{
    public static class StatisticsMappings
    {
        public static StatisticsDto ToStatisticsDto(this Statistics statistics)
        {
            return statistics == null
                ? null
                : new StatisticsDto
                {
                    Login = statistics.Login,
                    Score = statistics.Score
                };
        }

        /*public static AccountDto ToUserDto(this Account user)
        {
            return user == null
                ? null
                : new AccountDto
                {
                    Login= user.Login,
                    Password = user.Password,
                    /*Statistics = user.Statistics#1#
                };
        }*/
    }
}