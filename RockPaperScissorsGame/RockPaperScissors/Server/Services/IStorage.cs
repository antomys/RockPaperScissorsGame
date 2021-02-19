using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RockPaperScissors.Server.Services
{
    public interface IStorage<T> where T: class
    {
        ICollection<T> GetAll();
        Task<IEnumerable<ItemWithId<T>>> GetAllAsync();

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