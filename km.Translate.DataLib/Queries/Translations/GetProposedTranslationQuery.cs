using km.Translate.DataLib.Data.Dto;
using MediatR;

namespace km.Translate.DataLib.Queries.Translations;

public record GetProposedTranslationQuery(int SentenceId) : IRequest<TranslationsDto>;
