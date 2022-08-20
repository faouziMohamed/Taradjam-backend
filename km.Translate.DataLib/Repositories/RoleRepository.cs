using km.Library.Repositories;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Repositories.IRepositories;

namespace km.Translate.DataLib.Repositories;

public sealed class RoleRepository : GenericRepository<Role, ApplicationDbContext>, IRoleRepository
{
  public RoleRepository(ApplicationDbContext context) : base(context)
  {
  }
}
