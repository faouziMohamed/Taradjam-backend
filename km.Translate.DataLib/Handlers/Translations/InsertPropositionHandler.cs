using km.Translate.DataLib.Commands.Translations;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Handlers.Translations;

public class InsertPropositionHandler : BaseCommandPropositionHandler, IRequestHandler<InsertPropositionCommand, PropositionsDto>
{
  public InsertPropositionHandler(ApplicationDbContext context) : base(context)
  {
  }

  public async Task<PropositionsDto> Handle(InsertPropositionCommand request, CancellationToken cancellationToken)
  {
    var proposition = request.Proposition;
    // check if sentence exists and is the proposed translation is not already proposed
    await _propositionsRepository.MakeSureSentenceExistsOrThrow(proposition.SentenceVoId);
    await _propositionsRepository.MakeSureTranslationDoesNotExistOrThrow(proposition);
    var newProposition = await _propositionsRepository.AddNewProposition(proposition);
    await _context.SaveChangesAsync(cancellationToken);
    return newProposition;
  }
}
