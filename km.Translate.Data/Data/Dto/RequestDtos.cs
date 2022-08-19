using km.Library.GenericDto;

namespace km.Translate.Data.Data.ApiModels;

public sealed record RequestWithLocalDto : RequestBaseDto
{
  public string? Lang { get; init; } = "fr";
}
