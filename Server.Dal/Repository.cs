using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Context;

namespace Server.Dal
{
    public class Repository<TEntity> :IRepository<TEntity> where TEntity: class, new()
    {
        private bool _disposed;
        private ServerContext ServerContext { get;}

        public Repository(ServerContext serverContext)
        {
            ServerContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));
        }
        public virtual async void Dispose()
        {
            await Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask<TEntity> GetById(string id)
        {
            return await ServerContext.FindAsync<TEntity>(id);
        }

        public async Task<IQueryable<TEntity>> GetAll()
        {
            return await Task.FromResult(ServerContext.Set<TEntity>().AsQueryable());
        }

        public TEntity Add(TEntity entity)
        {
            var addEntityTask = ServerContext.AddAsync(entity);

            return addEntityTask.IsCompletedSuccessfully 
                ? addEntityTask.Result.Entity : null;
        }

        public TEntity Remove(TEntity entity)
        {
            return ServerContext.Remove(entity).Entity;
        }

        public void Update(TEntity entity)
        {
            ServerContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> SaveAsync()
        {
            return await ServerContext.SaveChangesAsync();
        }

        public void EnsureCreated()
        {
            throw new System.NotImplementedException();
        }

        private async Task Dispose(bool dispose)
        {
            if (!dispose || _disposed)
                return;
            _disposed = true;
            await ServerContext.DisposeAsync();
        }
    }
}