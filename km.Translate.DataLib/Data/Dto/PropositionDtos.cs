// ReSharper disable  UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using km.Translate.DataLib.Data.Models;


// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#pragma warning disable CS8618

namespace km.Translate.DataLib.Data.Dto;

public sealed class TranslationsDto
{
  // public TranslationsDto(Sentence sentence)
  // {
  //   SentenceVoId = sentence.Id;
  //   List<Proposition>? propositions = sentence.Propositions;
  //   Propositions = propositions.Select(PropositionsDto.From).ToList();
  // }

  [Required]
  public long SentenceVoId { get; init; }

  public IReadOnlyList<PropositionsDto> Propositions { get; init; }

  static public TranslationsDto From(Sentence sentence)
  {
    return new TranslationsDto
    {
      SentenceVoId = sentence.Id,
      Propositions = sentence.Propositions.Select(PropositionsDto.From).ToList()
    };
  }
}

public abstract class AbstractBasePropositionDtoWithoutId
{
  [Required]
  public int SentenceVoId { get; init; }

  [Required]
  public int TranslationLangId { get; init; }

  [Required]
  public string TranslatedText { get; init; }
}

public abstract class AbstractBasePropositionDto : AbstractBasePropositionDtoWithoutId
{
  [Required]
  public int PropositionId { get; init; }
}

public sealed class PropositionsDto : AbstractBasePropositionDto
{
  private PropositionsDto(Proposition p)
  {
    PropositionId = p.Id;
    SentenceVoId = p.SentenceVoId;
    TranslatedText = p.TranslatedText;
    TranslationDate = p.TranslationDate;
    TranslationHash = p.TranslationHash;
    TranslatedBy = p.TranslatedBy ?? "Anonymous";
    Votes = p.Votes;
    TranslationLangId = p.TranslationLangId;
  }

  [Required]
  public string TranslationHash { get; init; }

  public string? TranslatedBy { get; init; }

  [Required]
  public DateTime TranslationDate { get; init; }

  public long Votes { get; init; }

  static public PropositionsDto From(Proposition p)
  {
    return new PropositionsDto(p);
  }
}

public class PostNewPropositionDto : AbstractBasePropositionDtoWithoutId
{
  public string TranslatedBy { get; init; }

  [Required]
  public DateTime TranslationDate { get; init; }
}

public sealed class PostNewPropositionCommand : PostNewPropositionDto
{
  static public PostNewPropositionCommand From(PostNewPropositionDto dto)
  {
    return new PostNewPropositionCommand
    {
      SentenceVoId = dto.SentenceVoId,
      TranslationLangId = dto.TranslationLangId,
      TranslatedText = dto.TranslatedText,
      TranslatedBy = dto.TranslatedBy,
      TranslationDate = dto.TranslationDate
    };
  }
}

public class PutPropositionDto : AbstractBasePropositionDto
{
  public long Votes { get; init; }
  public int ApprovedById { get; init; }
  public string? TranslatedBy { get; init; }

  [Required]
  public DateTime TranslationDate { get; init; }
}

public sealed class PutPropositionCommand : PutPropositionDto
{
  static public PutPropositionCommand From(PutPropositionDto dto)
  {
    return new PutPropositionCommand
    {
      PropositionId = dto.PropositionId,
      SentenceVoId = dto.SentenceVoId,
      TranslationLangId = dto.TranslationLangId,
      TranslatedText = dto.TranslatedText,
      Votes = dto.Votes,
      ApprovedById = dto.ApprovedById,
      TranslatedBy = dto.TranslatedBy,
      TranslationDate = dto.TranslationDate
    };
  }
}
