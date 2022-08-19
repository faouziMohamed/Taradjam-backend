using km.Library.Repositories;
using km.Translate.Data.Data;
using km.Translate.Data.Data.Models;

namespace km.Translate.Data.Repositories.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
}

internal sealed class UserRepository : GenericRepository<User, ApplicationDbContext>, IUserRepository
{
  public UserRepository(ApplicationDbContext context) : base(context)
  {
  }
}
