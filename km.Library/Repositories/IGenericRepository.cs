using System.Linq.Expressions;
using km.Library.GenericDto;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace km.Library.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
  public Task<int> TruncateAsync();
  public Task<int> ClearTableAsync();
  public Task<int> ResetIdentity();

  public Task ClearAndResetIdentity();

  public Task<int> CountAsync();
  public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

  public Expression<Func<TEntity, object>>[] GetPropertiesToJoinsExpressions();

  public Task<TEntity?> GetByIdAsync(int id);
  public Task<IEnumerable<TEntity>> GetAllAsync();

  public Task<ResponseWithPageDto<TEntity>> GetManyAsync(Expression<Func<TEntity, object>> orderByPredicate,
    Expression<Func<TEntity, bool>>? filterPredicate,
    int pageNumber, int pageSize = 10, bool shuffle = false);


  public Task<ResponseWithPageDto<TEntity>> GetManyAsync(Expression<Func<TEntity, object>> orderByPredicate,
    Expression<Func<TEntity, bool>> filterPredicate,
    int pageNumber, int pageSize = 10, bool shuffle = false,
    params Expression<Func<TEntity, object>>[] includePredicates);

  public Task<TEntity?> GetOneAsync(
    Expression<Func<TEntity, bool>> filterPredicate,
    params Expression<Func<TEntity, object>>[] joinsPredicates
  );

  public IEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
  public Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
  public Task AddRangeAsync(IEnumerable<TEntity> entities);
  public Task Remove(int id);
  public void Remove(TEntity entity);
  public void RemoveRange(IEnumerable<TEntity> entities);
  public void Update(TEntity entity);
  public void UpdateRange(IEnumerable<TEntity> entities);
}
