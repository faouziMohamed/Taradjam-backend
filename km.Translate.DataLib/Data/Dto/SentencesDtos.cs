// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

// ReSharper disable UnusedAutoPropertyAccessor.Global

using km.Translate.DataLib.Data.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace km.Translate.DataLib.Data.Dto;

public sealed record SentenceDto
{
  private SentenceDto(Sentence sentence)
  {
    SentenceVoId = sentence.Id;
    SentenceVo = sentence.SentenceVo;
    SrcLanguage = new LanguageDto
    {
      LangId = sentence.LanguageVo.Id,
      LangName = sentence.LanguageVo.LongName,
      LangShortName = sentence.LanguageVo.ShortName
    };
  }
  public int SentenceVoId { get; init; }
  public string SentenceVo { get; init; } = string.Empty;
  public LanguageDto SrcLanguage { get; init; } = null!;

  static public SentenceDto? From(Sentence sentence)
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
