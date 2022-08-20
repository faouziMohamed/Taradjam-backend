namespace km.Translate.DataLib.Repositories.IRepositories;

public interface IDatabaseInitializer
{
  /// <summary>
  ///   Remove all data and reseed the identity from all tables and reinitialize the database.
  /// </summary>
  public Task InitializeDatabaseAsync();

  /// <summary>
  ///   Remove all data and reseed the identity from all tables.
  /// </summary>
  public Task EmptyDatabaseAsync();

  /// <summary>
  ///   Empty the database and reinitialize it. using <see cref="EmptyDatabaseAsync" /> and
  ///   <see cref="InitializeDatabaseAsync" />
  /// </summary>
  public Task ReinitializeDatabaseAsync();
}
