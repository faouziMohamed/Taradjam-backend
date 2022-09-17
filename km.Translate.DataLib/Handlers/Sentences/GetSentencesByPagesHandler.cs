using System.Linq.Expressions;
using km.Library.GenericDto;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Queries.Sentences;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using MediatR;

namespace km.Translate.DataLib.Handlers.Sentences;

public sealed class GetSentencesByPagesHandler : IRequestHandler<GetSentencesByPagesQuery, ResponseWithPageDto<SentenceDto>>
{
  private readonly ISentenceRepository _sentenceRepository;
  public GetSentencesByPagesHandler(ApplicationDbContext context)
  {
    _sentenceRepository = new SentenceRepository(context);
  }
  public GetSentencesByPagesHandler(ISentenceRepository sentenceRepository)
  {
    _sentenceRepository = sentenceRepository;
  }
  public async Task<ResponseWithPageDto<SentenceDto>> Handle(GetSentencesByPagesQuery request, CancellationToken cancellationToken)
  {
    var queries = request.Queries;
    // var (page, pageSize, shuffle, lang) = queries;
    int page = queries.Page ?? 0;
    int pageSize = queries.PageSize ?? 10;
    bool shuffle = queries.Shuffle ?? false;
    string? lang = queries.Lang;
    Expression<Func<Sentence, bool>>? filter = null;

    if (!string.IsNullOrEmpty(lang))
    {
      filter = s => s.LanguageVo.ShortName == lang;
    }

    ResponseWithPageDto<SentenceDto> sentencesDto = await _sentenceRepository
      .GetManyByPage(page, pageSize, shuffle, filter);

    return sentencesDto;
  }
}
