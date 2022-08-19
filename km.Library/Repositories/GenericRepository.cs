using System.Linq.Expressions;
using km.Library.Extensions;
using km.Library.GenericDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace km.Library.Repositories;

public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
  where TContext : DbContext
  where TEntity : BaseEntity
{
  // ReSharper disable once InconsistentNaming
  protected readonly TContext _context;

  protected GenericRepository(TContext context)
  {
    _context = context;
  }

  virtual public Expression<Func<TEntity, object>>[] GetInnerJoinsExpressions()
  {
    throw new NotImplementedException();
  }

  public IEnumerable<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return _context.Set<TEntity>().Where(predicate);
  }

  public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
  {
    return await _context.Set<TEntity>().AddAsync(entity);
  }

  public async Task AddRangeAsync(IEnumerable<TEntity> entities)
  {
    await _context.Set<TEntity>().AddRangeAsync(entities);
  }

  public void Update(TEntity entity)
  {
    _context.Set<TEntity>().Update(entity);
  }

  public void UpdateRange(IEnumerable<TEntity> entities)
  {
    _context.Set<TEntity>().UpdateRange(entities);
  }

  public void Remove(TEntity entity)
  {
    _context.Set<TEntity>().Remove(entity);
  }

  public async Task Remove(int id)
  {
    var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
    if (entity == null) return;
    _context.Set<TEntity>().Remove(entity);
  }

  public void RemoveRange(IEnumerable<TEntity> entities)
  {
    _context.Set<TEntity>().RemoveRange(entities);
  }

  public async Task<IEnumerable<TEntity>> GetAllAsync()
  {
    return await _context.Set<TEntity>().ToListAsync();
  }
  public async Task<int> TruncateAsync()
  {
    return await _context.TruncateTable<TEntity>();
  }
  public async Task<int> ClearTableAsync()
  {
    return await _context.DeleteAllRows<TEntity>();
  }

  public async Task<int> ResetIdentity()
  {
    return await _context.ResetIdentity<TEntity>();
  }
  public async Task ClearAndResetIdentity()
  {
    await ClearTableAsync();
    await ResetIdentity();
  }


  public async Task<int> CountAsync()
  {
    return await _context.Set<TEntity>().CountAsync();
  }

  public async Task<TEntity?> GetOneAsync(
    Expression<Func<TEntity, bool>> filterPredicate,
    params Expression<Func<TEntity, object>>[] joinsPredicates
  )
  {
    var row = await _context.Set<TEntity>()
      .InnerJoins(joinsPredicates)
      .FirstOrDefaultAsync(filterPredicate);

    return row;
  }

  virtual public async Task<TEntity?> GetByIdAsync(int id)
  {
    return await _context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
  }

  public async Task<Task<ResponseWithPageDto<TEntity>>> GetManyAsync(Expression<Func<TEntity, object>> orderByPredicate,
    Expression<Func<TEntity, bool>>? filterPredicate,
    int pageNumber, int pageSize = 10, bool shuffle = false)
  {
    IQueryable<TEntity> result = GetManyWithFilter(orderByPredicate, filterPredicate, pageNumber, pageSize, shuffle);
    return CreateResponseWithPageInfoDto(data: await result.ToListAsync(), pageNumber, pageSize);
  }

  public async Task<ResponseWithPageDto<TEntity>> GetManyAsync(Expression<Func<TEntity, object>> orderByPredicate,
    Expression<Func<TEntity, bool>> filterPredicate,
    int pageNumber, int pageSize = 10, bool shuffle = false,
    params Expression<Func<TEntity, object>>[] includePredicates)
  {
    IQueryable<TEntity> rows = GetManyWithFilter(orderByPredicate, filterPredicate, pageNumber, pageSize, shuffle);
    List<TEntity> result = await rows.InnerJoins(includePredicates).ToListAsync();
    return await CreateResponseWithPageInfoDto(result, pageNumber, pageSize);
  }
  public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
  {
    return await _context.Set<TEntity>().AnyAsync(predicate);
  }

  private IQueryable<TEntity> GetManyWithFilter(Expression<Func<TEntity, object>> orderByPredicate,
    Expression<Func<TEntity, bool>>? filterPredicate,
    int pageNumber = 1, int pageSize = 10, bool shuffle = false)
  {
    int skip = (pageNumber - 1)*pageSize;
    filterPredicate ??= static x => true;
    IQueryable<TEntity> rows = shuffle
      ? _context.Set<TEntity>().Where(filterPredicate).OrderBy(static x => Guid.NewGuid())
      : _context.Set<TEntity>().Where(filterPredicate).OrderBy(orderByPredicate);

    IQueryable<TEntity> result = rows.Skip(skip).Take(pageSize);
    return result;
  }

  protected async Task<ResponseWithPageDto<TData>> CreateResponseWithPageInfoDto<TData>(IList<TData> data, int currentPage, int pageSize)
  {
    int rowsCount = await CountAsync();
    long totalPageCount = GetTotalPageCount(rowsCount, pageSize);

    var response = new ResponseWithPageDto<TData>
    {
      CurrentPageSize = data.Count,
      Data = data,
      NextPage = currentPage + 1,
      CurrentPage = currentPage,
      TotalPageCount = totalPageCount,
      TotalRecordCount = rowsCount

    };

    return response;
  }

  static public long GetTotalPageCount(long totalCount, int pageSize)
  {
    long totalPageCount = totalCount/pageSize; // total page count
    // Make sure we don't have a remainder
    if (totalCount%pageSize == 0) return totalPageCount;
    return totalPageCount + 1;
  }
}
