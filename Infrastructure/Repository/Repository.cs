using Microsoft.EntityFrameworkCore;

using Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;
        public Repository(DbContext dbcontext)
        {
            this.dbSet = dbcontext.Set<TEntity>();
        }

        public bool DeleteByEntity(TEntity entity)
        {
            this.dbSet.Remove(entity);

            return true;
        }

        public async Task<bool> DeleteByExpression(Expression<Func<TEntity, bool>> expression)
        {
            await this.dbSet.Where(expression).ExecuteDeleteAsync();
            
            return true;
        }

        public async Task<bool> DeleteById(TId id)
        {
            TEntity entity = await Get(id);

            if (entity is null)
                return false;

            return DeleteByEntity(entity);
        }
        public async Task<TEntity> FindAsync(Guid id)
            => await this.dbSet.FindAsync(id);

        public async Task<TEntity> Get(TId? id)
           => await this.dbSet.FindAsync(id);

        public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
            => await this.dbSet.Where(predicate).ToListAsync();
        
        public async Task Insert(TEntity entity) => await this.dbSet.AddAsync(entity);
     
        public void Update(TEntity entity) => this.dbSet.Update(entity);
    }
}
