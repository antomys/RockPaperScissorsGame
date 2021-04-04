using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Exceptions.Register;

namespace Server.Services.Interfaces
{
    public interface IStorage<T> where T: class
    {
        /// <summary>
        /// Gets all elements from T collection
        /// </summary>
        /// <returns>ICollection</returns>
        ICollection<T> GetAll();
        
        /// <summary>
        /// Gets asynchronously all elements from T collection
        /// </summary>
        /// <returns>Task of ICollection</returns>
        Task<ICollection<T>> GetAllAsync();

        /// <summary>
        /// Gets element by Id
        /// </summary>
        /// <param name="id"> Id of an element</param>
        /// <returns>T item</returns>
        T Get(string id);
        
        /// <summary>
        /// Gets asynchronously element by Id
        /// </summary>
        /// <param name="id">string If of an Element</param>
        /// <returns>Task T item</returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Adds T element to the collection
        /// </summary>
        /// <param name="item">T item</param>
        /// <returns>int</returns>
        /// <exception cref="AlreadyExistsException">This account is already exists</exception>
        /// <exception cref="UnknownReasonException">Generic for custom user exceptions</exception>
        int Add(T item);
        
        /// <summary>
        /// Adds asynchronously an item to ConcurrentDictionary
        /// </summary>
        /// <param name="item">T item</param>
        /// <returns>int</returns>
        Task<int> AddAsync(T item);
        
        /// <summary>
        /// Adds or updates item in collection
        /// </summary>
        /// <param name="id">id of item</param>
        /// <param name="item">T item</param>
        void AddOrUpdate(string id, T item);
        
        /// <summary>
        /// Asynchronously adds or updates item in collection
        /// </summary>
        /// <param name="id">id of item</param>
        /// <param name="item">T item</param>
        Task AddOrUpdateAsync(string id, T item);

        /// <summary>
        /// Updates value in collection
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns>Task</returns>
        Task UpdateAsync(string id, T item);

        /// <summary>
        /// Deletes item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        bool Delete(string id);
        
        /// <summary>
        /// Asynchronously deletes item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<bool> DeleteAsync(string id);
        
    }
}