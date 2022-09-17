using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Queries.Sentences;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using MediatR;

namespace km.Translate.DataLib.Handlers.Sentences;

public sealed class GetSentenceByIdHandler : IRequestHandler<GetSentenceByIdQuery, SentenceDto?>
{
  private readonly ISentenceRepository _sentenceRepository;
  public GetSentenceByIdHandler(ApplicationDbContext context)
  {
    _sentenceRepository = new SentenceRepository(context);
  }

  public GetSentenceByIdHandler(ISentenceRepository sentenceRepository)
  {
    _sentenceRepository = sentenceRepository;
  }
  public async Task<SentenceDto?> Handle(GetSentenceByIdQuery request, CancellationToken cancellationToken)
  {
    var sentence = await _sentenceRepository.GetOneByIdAsync(request.SentenceId);
    return sentence != null ? SentenceDto.From(sentence) : null;
  }
}
