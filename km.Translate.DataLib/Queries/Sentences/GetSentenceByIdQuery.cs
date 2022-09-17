using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Queries.Sentences;

public sealed record GetSentenceByIdQuery(int SentenceId) : IRequest<SentenceDto?>;
