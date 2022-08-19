using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace km.Library.Extensions;

static public class EntityFrameworkExtensions
{
  static public IQueryable<TEntity> InnerJoins<TEntity>(this IQueryable<TEntity> queryable, params Expression<Func<TEntity, object>>[] predicates)
    where TEntity : class
  {
    IQueryable<TEntity> query = predicates
      .Aggregate(queryable, static (current, predicate) => current.Include(predicate));

    return query;
  }
  static public async Task<int> TruncateTable<T>(this DbContext dbContext) where T : class
  {
    return await dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {dbContext.GetTableName<T>()}");
  }

  static public async Task<int> DeleteAllRows<T>(this DbContext dbContext) where T : class
  {
    return await dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {dbContext.GetTableName<T>()}");
  }

  static public async Task<int> ResetIdentity<T>(this DbContext dbContext) where T : class
  {
    return await dbContext.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('{dbContext.GetTableName<T>()}', RESEED, 0)");
  }

  //Get DB Table Name
  static public string? GetTableName<TEntity>(this DbContext context) where TEntity : class
  {
    // We need dbcontext to access the models
    var models = context.Model;

    // Get all the entity types information
    IEnumerable<IEntityType> entityTypes = models.GetEntityTypes();

    var entityType = entityTypes.First(static t => t.ClrType == typeof(TEntity));

    var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
    var tableName = tableNameAnnotation.Value?.ToString();
    return tableName;
  }
}
