namespace km.Translate.DataLib.Repositories.IRepositories;

public interface IUnitOfWork : IDisposable
{
  public ISentenceRepository Sentences { get; }
  public IPropositionRepository Propositions { get; }
  public IUserDetailsRepository UserDetails { get; }
  public IUserRepository Users { get; }
  public IRoleRepository Roles { get; }
  public IDatabaseInitializer DatabaseInitializer { get; }
  public Task<int> CompleteAsync();
}
