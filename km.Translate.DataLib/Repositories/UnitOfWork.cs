using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories.IRepositories;

namespace km.Translate.DataLib.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
  private readonly ApplicationDbContext _context;

  public UnitOfWork(ApplicationDbContext context)
  {
    _context = context;
    Sentences = new SentenceRepository(_context);
    Propositions = new PropositionRepository(_context);
    UserDetails = new UserDetailsRepository(_context);
    Users = new UserRepository(_context);
    Roles = new RoleRepository(_context);
    Votes = new VoteRepository(_context);
    DatabaseInitializer = new DatabaseInitializer(_context);
  }


  public ISentenceRepository Sentences { get; }
  public IPropositionRepository Propositions { get; }
  public IUserDetailsRepository UserDetails { get; }
  public IUserRepository Users { get; }
  public IRoleRepository Roles { get; }
  public IVoteRepository Votes { get; }
  public IDatabaseInitializer DatabaseInitializer { get; }

  public async Task<int> CompleteAsync()
  {
    return await _context.SaveChangesAsync();
  }

  public void Dispose()
  {
    _context.Dispose();
  }
}
