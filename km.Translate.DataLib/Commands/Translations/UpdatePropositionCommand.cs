using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Commands.Translations;

public record UpdatePropositionCommand(PutPropositionCommand Proposition) : IRequest<PropositionsDto>;
