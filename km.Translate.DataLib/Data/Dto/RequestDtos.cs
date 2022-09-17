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
  // create a deconstruct method
  public void Deconstruct(out int page, out int pageSize, out bool shuffle, out string? lang)
  {
    (page, pageSize, shuffle, lang) = (Page ?? 0, PageSize ?? 10, Shuffle ?? false, Lang);
  }
}
