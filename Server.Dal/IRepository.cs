using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Dal
{
    public interface IRepository<TEntity> : IDisposable
    {
        ValueTask<TEntity> GetById(string id);
        Task<IQueryable<TEntity>> GetAll();
        TEntity Add(TEntity entity);
        TEntity Remove(TEntity entity);
        void Update(TEntity entity);
        Task<int> SaveAsync();
        void EnsureCreated();
    }
}