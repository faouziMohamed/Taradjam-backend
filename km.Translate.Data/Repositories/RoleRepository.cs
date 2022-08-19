using km.Library.Repositories;
using km.Translate.Data.Data;
using km.Translate.Data.Data.Models;
using km.Translate.Data.Repositories.IRepositories;

namespace km.Translate.Data.Repositories;

public sealed class RoleRepository : GenericRepository<Role, ApplicationDbContext>, IRoleRepository
{
  public RoleRepository(ApplicationDbContext context) : base(context)
  {
  }
}
