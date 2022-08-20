using km.Library.Repositories;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Models;

namespace km.Translate.DataLib.Repositories;

public interface IUserDetailsRepository : IGenericRepository<UserDetails>
{
}

public sealed class UserDetailsRepository : GenericRepository<UserDetails, ApplicationDbContext>, IUserDetailsRepository
{

  public UserDetailsRepository(ApplicationDbContext context) : base(context)
  {
  }
}
