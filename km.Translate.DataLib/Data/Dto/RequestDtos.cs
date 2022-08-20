using km.Library.GenericDto;

namespace km.Translate.DataLib.Data.Dto;

public sealed record RequestWithLocalDto : RequestBaseDto
{
  public string? Lang { get; init; } = "fr";
}
