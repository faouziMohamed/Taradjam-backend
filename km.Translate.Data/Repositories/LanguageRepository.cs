using km.Library.Repositories;
using km.Translate.Data.Data;
using km.Translate.Data.Data.Models;

namespace km.Translate.Data.Repositories;

public sealed class LanguageRepository : GenericRepository<Language, ApplicationDbContext>, ILanguageRepository
{
  public LanguageRepository(ApplicationDbContext context) : base(context)
  {
  }
}

public interface ILanguageRepository : IGenericRepository<Language>
{
}
