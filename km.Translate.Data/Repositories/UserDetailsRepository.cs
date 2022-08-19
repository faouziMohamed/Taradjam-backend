using km.Library.Repositories;
using km.Translate.Data.Data;
using km.Translate.Data.Data.Models;

namespace km.Translate.Data.Repositories;

public interface IUserDetailsRepository : IGenericRepository<UserDetails>
{
}

public sealed class UserDetailsRepository : GenericRepository<UserDetails, ApplicationDbContext>, IUserDetailsRepository
{

  public UserDetailsRepository(ApplicationDbContext context) : base(context)
  {
  }
}
