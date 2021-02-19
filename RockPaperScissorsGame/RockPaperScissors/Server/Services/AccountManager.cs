using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RockPaperScissors.Server.Models;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Services
{
    public class AccountManager<TAccount> : IStorage<TAccount> where TAccount : class
    {
        private readonly ILogger<AccountManager<TAccount>> _logger;
        
        private readonly IDeserializedObject _deserializedObject; //todo: change into something else.
        //private readonly IStatistics _statistics;

        public AccountManager(
            ILogger<AccountManager<TAccount>> logger,
            IDeserializedObject deserializedObject) //todo: change into something else
        {
            _logger = logger;
            _deserializedObject = deserializedObject;
        }
        
        public IEnumerable<ItemWithId<TAccount>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemWithId<TAccount>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public TAccount Get(string login, string password)
        {
            
            if (!_deserializedObject.ConcurrentDictionary.Values.Any(x => x.Login == login && x.Password == password)) return null;
            {
                var thisAccount = 
                    _deserializedObject.ConcurrentDictionary.Values.FirstOrDefault(x => x.Login == login && x.Password == password);

                if (thisAccount == null) return null;
                _deserializedObject.ConcurrentDictionary.TryGetValue(thisAccount.Id, out var result);//REDO!
                return result as TAccount; //bullshit
            }
        }

        public Task<TAccount> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public int Add(TAccount item)
        {
            var thisItem = (IAccount) item; //todo:change
            if (_deserializedObject.ConcurrentDictionary.Values.Any(x => x.Id == thisItem.Id))
            {
                return 404;
            }

            _deserializedObject.ConcurrentDictionary.TryAdd(thisItem.Id, (Account) thisItem);  //todo: solve this cast problem 
            return 200;
        }

        public Task<int> AddAsync(TAccount item)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(int id, TAccount item)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateAsync(int id, TAccount item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}