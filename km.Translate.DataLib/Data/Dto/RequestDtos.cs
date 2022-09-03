using km.Library.GenericDto;

namespace km.Translate.DataLib.Data.Dto;

public record RequestWithLocalDto : RequestBaseDto
{
  public string? Lang { get; init; } = "fr";
}

public sealed record RequestWithLocalQuery : RequestWithLocalDto
{
  static public RequestWithLocalQuery From(RequestWithLocalDto dto)
  {
    return new RequestWithLocalQuery
    {
      Page = dto.Page,
      PageSize = dto.PageSize,
      Shuffle = dto.Shuffle,
      Lang = dto.Lang
    };

  }
}
