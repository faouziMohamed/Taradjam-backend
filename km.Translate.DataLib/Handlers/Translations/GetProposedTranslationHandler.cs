using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Queries.Translations;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using MediatR;

namespace km.Translate.DataLib.Handlers.Translations;

public class GetProposedTranslationHandler : IRequestHandler<GetProposedTranslationQuery, TranslationsDto>
{
  private readonly IPropositionRepository _propositionRepository;
  public GetProposedTranslationHandler(ApplicationDbContext context)
  {
    _propositionRepository = new PropositionRepository(context);
  }
  public GetProposedTranslationHandler(IPropositionRepository propositionRepository)
  {
    _propositionRepository = propositionRepository;
  }
  public async Task<TranslationsDto> Handle(GetProposedTranslationQuery request, CancellationToken cancellationToken)
  {
    var result = await _propositionRepository.GetProposedTranslations(request.SentenceId);

    return result;
  }
}
