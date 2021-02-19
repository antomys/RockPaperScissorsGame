using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RockPaperScissors.Server.Models;

namespace RockPaperScissors.Server.Services
{
    public class Storage<T> : IStorage<T> where T: class
    {
        private readonly ILogger<Storage<T>> _logger;
        
        private readonly IDeserializedObject<T> _deserializedObject; //todo: change into something else.

        public Storage(
            ILogger<Storage<T>> logger,
            IDeserializedObject<T> deserializedObject)
        {
            _logger = logger;
            _deserializedObject = deserializedObject;
        }
        
        public ICollection<T> GetAll()
        {
            return _deserializedObject.ConcurrentDictionary.Values; //todo: redo
        }

        public Task<IEnumerable<ItemWithId<T>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public T Get(Guid id)
        {
            return _deserializedObject.ConcurrentDictionary.TryGetValue(id, out var account) ? account : default;
        }

        public Task<T> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public int Add(T item)
        {
           // var guid = item.GetType().GetField("Id", BindingFlags.NonPublic | BindingFlags.Instance); //fix from stackoverflow

           var guid = GetGuidFromT(item);
           
           if(_deserializedObject.ConcurrentDictionary.TryGetValue((Guid) guid,out _))
           {
               return 404;
           }

           /*if (_deserializedObject.ConcurrentDictionary.ContainsKey((Guid) guid))
               return 404;*/

           //var guid = item.GetType().GUID; //TODO: CHTO ETO TAKOE BLYAT
           return _deserializedObject.ConcurrentDictionary.TryAdd((Guid) guid, item) ? 200 : 404; //todo: redo
        }

        private object GetGuidFromT(T item)
        {
            //METHOD TO GET GUID FROM T
            //***************************
            var t = item.GetType();
            var prop = t.GetProperty("Id");
            return prop?.GetValue(item); //TODO: CONDITIONAL ACCESS????
            // *************************************
        }

        public Task<int> AddAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(Guid id, T item)
        {
            //_deserializedObject.ConcurrentDictionary.TryGetValue(id, out var thisItem);
            _deserializedObject.ConcurrentDictionary[id] = item;
        }

        public Task AddOrUpdateAsync(Guid id, T item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            if (!_deserializedObject.ConcurrentDictionary.TryRemove(id, out var value)) return false;
            _logger.LogWarning($"This deleted item: {value}");
            return true;

        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}