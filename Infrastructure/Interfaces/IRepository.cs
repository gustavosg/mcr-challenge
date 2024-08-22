using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        bool DeleteByEntity(TEntity entity);
        Task<bool> DeleteByExpression(Expression<Func<TEntity, bool>> expression);
        Task<bool> DeleteById(TId id);
        Task<TEntity> FindAsync(Guid id);
        Task<TEntity> Get(TId? id);
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
        Task Insert(TEntity entity);
        void Update(TEntity entity);
    }
}
