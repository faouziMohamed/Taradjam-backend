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

public class GetAllSentencesHandlers : IRequestHandler<GetAllSentencesQuery, ResponseWithPageDto<SentenceDto>>
{
  private readonly ISentenceRepository _sentenceRepository;
  public GetAllSentencesHandlers(ApplicationDbContext context)
  {
    _sentenceRepository = new SentenceRepository(context);
  }

  public async Task<ResponseWithPageDto<SentenceDto>> Handle(GetAllSentencesQuery request, CancellationToken cancellationToken)
  {
    var queries = request.Queries;
    // use the cancellationToken to cancel the task if needed
    int pageNumber = queries.Page ?? 0;
    int pageSize = queries.PageSize ?? 10;
    bool shuffle = queries.Shuffle ?? false;
    string? lang = queries.Lang;
    Expression<Func<Sentence, bool>> filter = s => s.LanguageVo.ShortName == lang;

    ResponseWithPageDto<SentenceDto> sentencesDto = lang switch
    {
      null => await _sentenceRepository.GetManyByPage(pageNumber, pageSize, shuffle),
      _ => await _sentenceRepository.GetManyByPage(pageNumber, pageSize, shuffle, filter)
    };

    return sentencesDto;
  }
}
