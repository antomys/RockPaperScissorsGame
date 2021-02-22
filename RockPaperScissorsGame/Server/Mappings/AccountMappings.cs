using System;
using Server.Contracts;
using Server.Models;

namespace Server.Mappings
{
    public static class AccountMappings
    {
        public static Account ToUser(this AccountDto dto)
        {
            var guid = Guid.NewGuid();
            return dto == null
                ? null
                : new Account
                {
                    Id = guid.ToString(),
                    Login = dto.Login,
                    Password = dto.Password
                };
            
        }

        public static AccountDto ToUserDto(this Account user)
        {
            return user == null
                ? null
                : new AccountDto
                {
                    Login= user.Login,
                    Password = user.Password,
                    /*Statistics = user.Statistics*/
                };
        }
    }
}