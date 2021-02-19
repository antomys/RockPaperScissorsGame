using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RockPaperScissors.Server.Services
{
    public interface IStorage<T> where T: class
    {
        IEnumerable<ItemWithId<T>> GetAll();
        Task<IEnumerable<ItemWithId<T>>> GetAllAsync();

        T Get(string login, string password);
        Task<T> GetAsync(Guid id);

        int Add(T item);
        Task<int> AddAsync(T item);

        void AddOrUpdate(int id, T item);
        Task AddOrUpdateAsync(int id, T item);

        bool Delete(int id);
        Task<bool> DeleteAsync(int id);
    }
}