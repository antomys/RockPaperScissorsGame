using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Server.Exceptions.Register;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class Storage<T> : IStorage<T> where T: class
    {
        private readonly IDeserializedObject<T> _deserializedObject; //todo: change into something else.

        public Storage(
            IDeserializedObject<T> deserializedObject)
        {
            _deserializedObject = deserializedObject;
        }
        
        /// <summary>
        /// Gets all elements from T collection
        /// </summary>
        /// <returns>ICollection</returns>
        
        public ICollection<T> GetAll()
        {
            return _deserializedObject.ConcurrentDictionary.Values;
        }

        /// <summary>
        /// Gets asynchronously all elements from T collection
        /// </summary>
        /// <returns>Task of ICollection</returns>
        public async Task<ICollection<T>> GetAllAsync()
        {
            var result = Task.Run(GetAll);
            
            
            return await result;
        }
        
        /// <summary>
        /// Gets element by Id
        /// </summary>
        /// <param name="id"> Id of an element</param>
        /// <returns>T item</returns>
        public T Get(string id)
        {
            return _deserializedObject.ConcurrentDictionary.TryGetValue(id, out var account) ? account : default;
        }
        
        /// <summary>
        /// Gets asynchronously element by Id
        /// </summary>
        /// <param name="id">string If of an Element</param>
        /// <returns>Task T item</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<T> GetAsync(string id)
        {
            return Task.Run(() => Get(id));
        }

        /// <summary>
        /// Adds T element to the collection
        /// </summary>
        /// <param name="item">T item</param>
        /// <returns>int</returns>
        /// <exception cref="AlreadyExistsException"></exception>
        /// <exception cref="UnknownReasonException"></exception>
        public int Add(T item)
        {
            var guid = GetGuidFromT(item);
            if (typeof(T).Name.Contains("Round"))
            {
                if (!_deserializedObject.ConcurrentDictionary.TryAdd(guid.ToString(), item)) throw new UnknownReasonException(item.GetType().ToString());
                _deserializedObject.UpdateData();
                return (int)HttpStatusCode.OK;
            } 
            
            if (CheckIfExists(item))
                throw new AlreadyExistsException(item.GetType().ToString());
           
            if (!_deserializedObject.ConcurrentDictionary.TryAdd(guid.ToString(), item)) throw new UnknownReasonException(item.GetType().ToString());
            _deserializedObject.UpdateData();
            return (int)HttpStatusCode.OK;
        }

        /// <summary>
        /// Adds asynchronously an item to ConcurrentDictionary
        /// </summary>
        /// <param name="item">T item</param>
        /// <returns>int</returns>
        public Task<int> AddAsync(T item)
        {
            return Task.Factory.StartNew(() => Add(item));
        }

        /// <summary>
        /// Adds or updates item in collection
        /// </summary>
        /// <param name="id">id of item</param>
        /// <param name="item">T item</param>
        public void AddOrUpdate(string id, T item)
        {
            _deserializedObject.ConcurrentDictionary[id] = item;
        }
        /// <summary>
        /// Asynchronously adds or updates item in collection
        /// </summary>
        /// <param name="id">id of item</param>
        /// <param name="item">T item</param>
        public Task AddOrUpdateAsync(string id, T item)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Updates value in collection
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>Task</returns>
        public Task UpdateAsync(string id, T item)
        {
            return Task.Factory.StartNew(() =>
            {
                var thisItem = _deserializedObject.ConcurrentDictionary[id];
                _deserializedObject.ConcurrentDictionary.TryUpdate(id, item, thisItem);
                _deserializedObject.UpdateData();
            });
        }

        /// <summary>
        /// Deletes item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool Delete(string id)
        {
            return _deserializedObject.ConcurrentDictionary.TryRemove(id, out _);
        }

        /// <summary>
        /// Asynchronously deletes item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        #region PrivateMethods

        /// <summary>
        /// Gets Guid from T item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>object string</returns>
        private object GetGuidFromT(T item)
        {
            //METHOD TO GET GUID FROM T
            //***************************
            var t = item.GetType();
            var prop = t.GetProperty("Id");
            return prop?.GetValue(item);
            // *************************************
        }

        /// <summary>
        /// Gets login property from T item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>object string</returns>
        private object GetLoginString(T item)
        {
            return item.GetType().GetProperty("Login") != null ? item.GetType().GetProperty("Login")?.GetValue(item) : null;
        }

        /// <summary>
        /// Checks if T item exists in collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns>bool</returns>
        private bool CheckIfExists(T item)
        {
            var flattenList = _deserializedObject.ConcurrentDictionary.Values;           //THIS IS NOT ASYNC
            return GetLoginString(item) != null && flattenList.Any(T => GetLoginString(T).Equals(GetLoginString(item)));
        }

        #endregion
    }
}