using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IStorage<T> where T: class
    {
        ICollection<T> GetAll();
        Task<ICollection<T>> GetAllAsync();

        T Get(Guid id);
        Task<T> GetAsync(Guid id);

        int Add(T item);
        Task<int> AddAsync(T item);

        void AddOrUpdate(Guid id, T item);
        Task AddOrUpdateAsync(Guid id, T item);

        bool Delete(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}