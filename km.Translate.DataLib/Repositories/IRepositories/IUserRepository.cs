using km.Library.Repositories;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Models;

namespace km.Translate.DataLib.Repositories.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
}

internal sealed class UserRepository : GenericRepository<User, ApplicationDbContext>, IUserRepository
{
  public UserRepository(ApplicationDbContext context) : base(context)
  {
  }
}
