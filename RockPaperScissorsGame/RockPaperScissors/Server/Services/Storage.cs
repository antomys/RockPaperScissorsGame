/*using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RockPaperScissors.Server.Services
{
    public class Storage<T> : IStorage<T> where T: class
    {
        private readonly ILogger<Storage<T>> _logger;
        private readonly object _deserializedObject; //todo: change into something else.

        public Storage(
            ILogger<Storage<T>> logger,
            object deserializedObject) //todo: change into something else
        {
            _logger = logger;
            _deserializedObject = deserializedObject;
        }
        
        public IEnumerable<ItemWithId<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemWithId<T>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public T Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public int Add(T item)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(int id, T item)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateAsync(int id, T item)
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
}*/