using System;
using RockPaperScissors.Models;
using RockPaperScissors.Server.Models;

namespace RockPaperScissors.Server.Mappings
{
    internal static class AccountMappings
    {
        public static Account ToUser(this AccountDto dto)
        {
            var guid = Guid.NewGuid();
            return dto == null
                ? null
                : new Account
                {
                    Id = guid,
                    Login = dto.Login,
                    Password = dto.Password,
                    Statistics = new Statistics
                    {
                        Id = Guid.NewGuid(),
                        Userid = guid
                    }
                };
        }

        public static AccountDto ToUserDto(this Account user)
        {
            return user == null
                ? null
                : new AccountDto
                {
                    Login= user.Login,
                    Password = user.Password
                    /*Statistic = user.Statistics*/
                };
        }
    }
}