using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Commands.Translations;

public record UpdateVotePropositionCommand(int PropositionId, bool IsUpVote) : IRequest<ReturnedVotesDto>;
