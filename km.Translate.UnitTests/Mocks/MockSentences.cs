using km.Library.Utils;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace km.Translate.UnitTests.Mocks;

static public class MockRepository
{

  private static readonly IReadOnlyList<string> SentencesTexts = new List<string>
  {
    "Je suis une phrase", "Je suis une autre phrase", "Je suis une troisième phrase", "Je suis une quatrième phrase",
    "Je suis une cinquième phrase", "Je suis une sixième phrase", "Je suis une septième phrase", "Je suis une huitième phrase",
    "Je suis une neuvième phrase", "Je suis une dixième phrase", "Je suis une onzième phrase", "Je suis une douzième phrase"
  };

  private static readonly IReadOnlyList<string> TranslationTexts = new List<string>
  {
    "I am a sentence", "I am another sentence", "I am a third sentence", "I am a fourth sentence",
    "I am a fifth sentence", "I am a sixth sentence", "I am a seventh sentence", "I am an eighth sentence",
    "I am a ninth sentence", "I am a tenth sentence", "Just adding a sentence", "Cool this is a sentence"
  };

  private static List<Sentence> _sentences = CreateSentences(SentencesTexts, 1, 5);
  private static List<Proposition> _propositions = CreatePropositions(TranslationTexts, 1, 5);
  static public int GetSentencesCount => _sentences.Count;
  static public int GetSentencesTotalPageCount(int pageSize)
  {
    return (int)Utils.GetTotalPageCount(GetSentencesCount, pageSize);
  }

  static public IReadOnlyList<Sentence> GetSentences(int take = -1)
  {
    return take == -1 ? _sentences : _sentences.Take(take).ToList();
  }
  static public List<SentenceDto> GetSentenceDtos(int take = -1, string lang = "fr")
  {
    return GetSentences(take).Where(s => s.LanguageVo.ShortName == lang).Select(static s => SentenceDto.From(s)).ToList()!;
  }
  static public Proposition CreateOneProposition(int propId, int sentenceId, string translatedText, int langId)
  {
    return new Proposition
    {
      SentenceVoId = sentenceId,
      Id = propId,
      TranslatedText = translatedText,
      TranslationHash = translatedText.GenerateHash(),
      TranslationLangId = langId,
      TranslationLang = new Language { Id = langId, Name = "English", LongName = "English" }
    };
  }

  static public TranslationsDto GetOneTranslationDto(int sentenceVoId, int voLangId = 1)
  {
    _sentences = CreateSentences(SentencesTexts, sentenceVoId, voLangId, 100);
    return TranslationsDto.From(_sentences[sentenceVoId])!;
  }
  static public PropositionsDto GetOnePropositionsDto(int propositionId)
  {
    _propositions = CreatePropositions(TranslationTexts, propositionId, 100);
    return PropositionsDto.From(_propositions[propositionId])!;
  }
  private static SentenceDto GetOneSentenceDto(int sentenceVoId)
  {
    _sentences = CreateSentences(SentencesTexts, sentenceVoId, 100);
    return SentenceDto.From(_sentences[sentenceVoId])!;
  }

  static public List<Proposition> CreatePropositions(IReadOnlyList<string> sentences, int sentenceVoId, int langId, int entropy = 1)
  {
    var propId = 1;
    entropy = entropy < 1 ? 1 : entropy;
    var propositions = new List<Proposition>();

    for (var i = 0; i < entropy; i++)
    {
      List<Proposition> ps = sentences
        .Select(sentence => CreateOneProposition(propId++, sentenceVoId, sentence, langId))
        .ToList();

      propositions.AddRange(ps);
    }

    return propositions;
  }
  private static Language CreateLanguage(int id)
  {
    return new Language
    {
      Id = id, Name = "Français", LongName = "French", ShortName = "fr"
    };
  }
  /// <summary> Generate a List of sentences with some propositions</summary>
  static public List<Sentence> CreateSentences(IEnumerable<string> sentences, int sentenceVoId, int voLangId, int entropy = 1)
  {
    return sentences
      .Select(sentence => new Sentence
        {
          Id = sentenceVoId,
          SentenceVo = sentence,
          LanguageVoId = voLangId,
          LanguageVo = CreateLanguage(voLangId),
          Propositions = CreatePropositions(TranslationTexts, sentenceVoId, 2, entropy)
        }
      ).ToList();
  }
}
