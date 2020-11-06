using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : BaseRepository, IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(Func<IArtAlleyDbContext> getDbContext) : base(getDbContext)
        {
        }

        public virtual IEnumerable<TEntity> Get()
            => ExecuteDbContext(context =>
            {
                var _dbSet = context.Set<TEntity>();
                return _dbSet.AsNoTracking().ToList();
            });

        public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
             => ExecuteDbContext(context =>
             {
                 var _dbSet = context.Set<TEntity>();
                 return _dbSet.AsNoTracking().Where(predicate).ToList();
             });

        public virtual TEntity Create(TEntity item)
            => ExecuteDbContext(dbContext =>
            {
                var _dbSet = dbContext.Set<TEntity>();
                _dbSet.Add(item);
                dbContext.SaveChanges();
                return item;
            });

        public virtual void Remove(TEntity item)
            => ExecuteDbContext(context =>
            {
                var _dbSet = context.Set<TEntity>();
                _dbSet.Remove(item);
                context.SaveChanges();
            });


        public virtual TEntity Update(TEntity item)
            => ExecuteDbContext(context =>
            {
                var _dbSet = context.Set<TEntity>();
                _dbSet.Update(item);
                context.SaveChanges();
                return item;
            });

        public virtual TEntity FindById(int id) =>
            ExecuteDbContext(context =>
            {
                var _dbSet = context.Set<TEntity>();
                return _dbSet.Find(id);
            });

        public void RemoveById(int id) =>
            ExecuteDbContext(context =>
            {
                var _dbSet = context.Set<TEntity>();
                var item = _dbSet.Find(id);
                if (item != null)
                {
                    _dbSet.Remove(item);
                }
            });
    }
}
