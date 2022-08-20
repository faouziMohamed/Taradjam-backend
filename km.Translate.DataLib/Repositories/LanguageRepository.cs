using km.Library.Repositories;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Models;

namespace km.Translate.DataLib.Repositories;

public sealed class LanguageRepository : GenericRepository<Language, ApplicationDbContext>, ILanguageRepository
{
  public LanguageRepository(ApplicationDbContext context) : base(context)
  {
  }
}

public interface ILanguageRepository : IGenericRepository<Language>
{
}
