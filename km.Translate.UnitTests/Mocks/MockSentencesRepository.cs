using km.Library.Utils;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace km.Translate.UnitTests.Mocks;

static public class MockSentencesRepository
{
  static public List<Sentence> Sentences => new()
  {
    new Sentence
    {
      Id = 1,
      SentenceVo = "Bonjour le monde",
      LanguageVo = new Language { Id = 1, LongName = "Français", ShortName = "fr" }
    },
    new Sentence
    {
      Id = 2,
      SentenceVo = "Comment allez-vous",
      LanguageVo = new Language { Id = 1, LongName = "Français", ShortName = "fr" }
    },
    new Sentence
    {
      Id = 3,
      SentenceVo = "La vie est belle",
      LanguageVo = new Language { Id = 1, LongName = "Français", ShortName = "fr" }
    },
    new Sentence
    {
      Id = 4,
      SentenceVo = "Le soleil brille",
      LanguageVo = new Language { Id = 1, LongName = "Français", ShortName = "fr" }
    },
    new Sentence
    {
      Id = 5,
      SentenceVo = "une pomme",
      LanguageVo = new Language { Id = 1, LongName = "English", ShortName = "en" }
    },
    new Sentence
    {
      Id = 6,
      SentenceVo = "How are you",
      LanguageVo = new Language { Id = 1, LongName = "English", ShortName = "en" }
    }
  };

  static public int GetSentencesCount => Sentences.Count;
  static public int GetTotalPageCount(int pageSize)
  {
    return (int)Utils.GetTotalPageCount(GetSentencesCount, pageSize);
  }

  static public List<Sentence> GetSentences(int take = -1)
  {
    return take == -1 ? Sentences : Sentences.Take(take).ToList();
  }
  static public List<SentenceDto> GetSentenceDtos(int take = -1)
  {
    return GetSentences(take).Select(static s => SentenceDto.From(s)).ToList()!;
  }
}
