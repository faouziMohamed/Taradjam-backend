using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories;
using MediatR;

namespace km.Translate.DataLib.Queries.Sentences;

public record IsSentenceTableEmptyQuery : IRequest<bool>;

public class IsSentenceTableEmptyQueryHandler : IRequestHandler<IsSentenceTableEmptyQuery, bool>
{
  private readonly ApplicationDbContext _context;
  private readonly SentenceRepository _sentenceRepository;
  public IsSentenceTableEmptyQueryHandler(ApplicationDbContext context)
  {
    _context = context;
    _sentenceRepository = new SentenceRepository(context);
  }

  public async Task<bool> Handle(IsSentenceTableEmptyQuery request, CancellationToken cancellationToken)
  {
    return await _sentenceRepository.CountAsync() == 0;
  }
}
