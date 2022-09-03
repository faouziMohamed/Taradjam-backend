using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;

namespace km.Translate.DataLib.Handlers.Translations;

public abstract class BaseCommandPropositionHandler
{
  protected ApplicationDbContext _context;
  protected IPropositionRepository _propositionsRepository;
  protected BaseCommandPropositionHandler(ApplicationDbContext context)
  {
    _context = context;
    _propositionsRepository = new PropositionRepository(context);
  }
}
