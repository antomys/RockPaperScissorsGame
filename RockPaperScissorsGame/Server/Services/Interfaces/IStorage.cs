using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IStorage<T> where T: class
    {
        ICollection<T> GetAll();
        Task<ICollection<T>> GetAllAsync();

        T Get(string id);
        Task<T> GetAsync(string id);

        int Add(T item);
        Task<int> AddAsync(T item);

        void AddOrUpdate(string id, T item);
        Task AddOrUpdateAsync(string id, T item);

        bool Delete(string id);
        Task<bool> DeleteAsync(string id);
        
    }
}