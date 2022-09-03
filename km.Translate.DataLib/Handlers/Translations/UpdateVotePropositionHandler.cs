using km.Translate.DataLib.Commands.Translations;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Handlers.Translations;

public class UpdateVotePropositionHandler : BaseCommandPropositionHandler, IRequestHandler<UpdateVotePropositionCommand, ReturnedVotesDto>
{
  public UpdateVotePropositionHandler(ApplicationDbContext context) : base(context)
  {
  }
  public async Task<ReturnedVotesDto> Handle(UpdateVotePropositionCommand request, CancellationToken cancellationToken)
  {
    await _propositionsRepository.MakeSurePropositionExistOrThrow(request.PropositionId);
    long votes = await _propositionsRepository.DoAVote(request.PropositionId, request.IsUpVote);
    return new ReturnedVotesDto { Votes = votes };
  }
}
