using km.Library.GenericDto;
using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Queries.Sentences;

public record GetAllSentencesQuery(RequestWithLocalQuery Queries) : IRequest<ResponseWithPageDto<SentenceDto>>;
