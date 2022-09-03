using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Queries.Sentences;

public record GetSentenceByIdQuery(int SentenceId) : IRequest<SentenceDto?>;
