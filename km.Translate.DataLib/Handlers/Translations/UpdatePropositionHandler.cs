using km.Translate.DataLib.Commands.Translations;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Handlers.Translations;

public class UpdatePropositionHandler : BaseCommandPropositionHandler, IRequestHandler<UpdatePropositionCommand, PropositionsDto>
{
  public UpdatePropositionHandler(ApplicationDbContext context) : base(context)
  {
  }

  public async Task<PropositionsDto> Handle(UpdatePropositionCommand request, CancellationToken cancellationToken)
  {
    var proposition = request.Proposition;
    await _propositionsRepository.MakeSurePropositionExistOrThrow(proposition.PropositionId);
    var updatedProposition = await _propositionsRepository.UpdateProposition(proposition);
    await _context.SaveChangesAsync(cancellationToken);
    return updatedProposition;
  }
}
