using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Server.Bll.Models;
using Server.Bll.Services.Interfaces;
using Server.Exceptions.LogIn;

namespace Server.Bll.Services
{
    public class AccountService : IAccountService
    {
        private readonly MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        public Task<bool> RegisterAsync(AccountModel accountModel)
        {
            /*if (accountModel is null)
                throw new InvalidCredentialsException(nameof(accountModel));*/
            throw new NotImplementedException();
        }

        public Task<string> LogInAsync(AccountModel accountModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> LogOutAsync(string accountToken)
        {
            throw new System.NotImplementedException();
        }
    }
}