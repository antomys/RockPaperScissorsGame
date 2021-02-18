using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RockPaperScissors.Models;
using RockPaperScissors.Models.Interfaces;

namespace RockPaperScissors.Services.ServerServices
{
    class AccountManager : IAccountManager<Account>
    {
        private readonly IAccount _account;
        private readonly IStatistics _statistics;
        private List<Account> _accounts;

        public AccountManager(
            IAccount account, 
            IStatistics statistics)
        {
            _account = account;
            _statistics = statistics;
           
        }
        
        public async Task<IEnumerable<ItemWithId<Account>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAsync(string nickName, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AddAsync(Account item)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}