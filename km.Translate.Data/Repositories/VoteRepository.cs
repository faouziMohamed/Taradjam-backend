using km.Library.Repositories;
using km.Translate.Data.Data;
using km.Translate.Data.Data.Models;
using km.Translate.Data.Repositories.IRepositories;

namespace km.Translate.Data.Repositories;

public sealed class VoteRepository : GenericRepository<Vote, ApplicationDbContext>, IVoteRepository
{
  public VoteRepository(ApplicationDbContext context) : base(context)
  {
  }
}
