// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

using km.Translate.DataLib.Data.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace km.Translate.DataLib.Data.Dto;

public record SentenceDto
{
  private SentenceDto(Sentence sentence)
  {
    TextId = sentence.Id;
    SentenceVo = sentence.SentenceVo;
    SrcLanguage = new LanguageDto
    {
      LangId = sentence.SrcLanguage.Id,
      LangName = sentence.SrcLanguage.LongName,
      LangShortName = sentence.SrcLanguage.ShortName
    };
  }
  public int TextId { get; init; }
  public string SentenceVo { get; init; } = string.Empty;
  public LanguageDto SrcLanguage { get; init; } = null!;

  static public SentenceDto From(Sentence sentence)
  {
    return new SentenceDto(sentence);
  }
}

public sealed record LanguageDto
{
  public int LangId { get; init; }
  public string LangName { get; init; } = string.Empty;
  public string LangShortName { get; init; } = string.Empty;
}
