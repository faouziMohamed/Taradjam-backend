using km.Library.Repositories;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Repositories.IRepositories;

namespace km.Translate.DataLib.Repositories;

public sealed class VoteRepository : GenericRepository<Vote, ApplicationDbContext>, IVoteRepository
{
  public VoteRepository(ApplicationDbContext context) : base(context)
  {
  }
}
