using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Server.Exceptions.Register;
using Server.Services.Interfaces;

namespace Server.Services
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

        public async Task<ICollection<T>> GetAllAsync()
        {
            //var result = Task.Factory.StartNew(GetAll).Result;

            var result = Task.Run(GetAll);

            await Task.WhenAll(result);
            return result.Result;
        }
        
        public T Get(Guid id)
        {
            return _deserializedObject.ConcurrentDictionary.TryGetValue(id.ToString(), out var account) ? account : default;
        }

        public Task<T> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public int Add(T item)
        {

           var guid = GetGuidFromT(item);

           var check = GetLoginString(item);
           
           if (CheckIfExists(item))
               throw new AlreadyExistsException(item.GetType().ToString());
           
          
           //var guid = item.GetType().GUID; //TODO: CHTO ETO TAKOE
           if (!_deserializedObject.ConcurrentDictionary.TryAdd(guid.ToString(), item)) throw new UnknownReasonException(item.GetType().ToString());
           _deserializedObject.UpdateData();
           return (int)HttpStatusCode.OK;
        }

        

        public Task<int> AddAsync(T item)
        {
            return Task.Factory.StartNew(() => Add(item));
        }

        public void AddOrUpdate(Guid id, T item)
        {
            //_deserializedObject.ConcurrentDictionary.TryGetValue(id, out var thisItem);
            _deserializedObject.ConcurrentDictionary[id.ToString()] = item;
        }

        public Task AddOrUpdateAsync(Guid id, T item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            if (!_deserializedObject.ConcurrentDictionary.TryRemove(id.ToString(), out var value)) return false;
            _logger.LogWarning($"This deleted item" + $": {value}"); //todo: delete
            return true;

        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        #region PrivateMethods

        
        private object GetGuidFromT(T item)
        {
            //METHOD TO GET GUID FROM T
            //***************************
            var t = item.GetType();
            var prop = t.GetProperty("Id");
            return prop?.GetValue(item); //TODO: CONDITIONAL ACCESS????
            // *************************************
        }

        private object GetLoginString(T item)
        {
            return item.GetType().GetProperty("Login") != null ? item.GetType().GetProperty("Login")?.GetValue(item) : null;
        }

        private bool CheckIfExists(T item)
        {
            var flattenList = _deserializedObject.ConcurrentDictionary.Values;           //THIS IS NOT ASYNC
            return GetLoginString(item) != null && flattenList.Any(T => GetLoginString(T).Equals(GetLoginString(item)));
        }

        #endregion
    }
}