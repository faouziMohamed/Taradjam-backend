// ReSharper disable  UnusedAutoPropertyAccessor.Global

using System.ComponentModel.DataAnnotations;
using km.Translate.DataLib.Data.Models;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#pragma warning disable CS8618

namespace km.Translate.DataLib.Data.Dto;

public sealed record TranslationsDto
{
  private TranslationsDto(Sentence sentence)
  {
    SentenceId = sentence.Id;
    ICollection<Proposition>? propositions = sentence.Propositions;
    Propositions = propositions.Select(PropositionsDto.From).ToList();
  }

  [Required]
  public long SentenceId { get; init; }

  public IReadOnlyList<PropositionsDto> Propositions { get; init; }

  static public TranslationsDto From(Sentence sentence)
  {
    return new TranslationsDto(sentence);
  }
}

public record PropositionsDto
{
  protected PropositionsDto(Proposition p)
  {
    PropositionId = p.Id;
    TranslatedText = p.TranslatedText;
    TranslationDate = p.TranslationDate;
    TranslationHash = p.TranslationHash;
    Votes = new VotesDto { UpVotes = p.Votes.UpVotes, DownVotes = p.Votes.DownVotes };
  }

  [Required]
  public string TranslatedText { get; init; }

  [Required]
  public string TranslationHash { get; init; }

  public string TranslatedBy { get; init; } = "Anonymous";

  [Required]
  public DateTime TranslationDate { get; init; }

  public VotesDto Votes { get; init; }

  [Required]
  public int PropositionId { get; init; }

  static public PropositionsDto From(Proposition p)
  {
    return new PropositionsDto(p);
  }
}

public sealed record CreatedPropositionDto : PropositionsDto
{
  public CreatedPropositionDto(Proposition p, int sentenceId) : base(p)
  {
    SentenceId = sentenceId;
  }
  public int SentenceId { get; init; }
  static public CreatedPropositionDto From(Proposition proposition, int sentenceId)
  {
    return new CreatedPropositionDto(proposition, sentenceId);
  }
}

public sealed record VotesDto
{
  public long UpVotes { get; init; }
  public long DownVotes { get; init; }
}

public sealed record NewPropositionDto
{
  [Required]
  public int TextVoId { get; init; }

  [Required]
  public string TranslatedText { get; init; }

  public string TranslatedBy { get; init; }

  [Required]
  public DateTime TranslationDate { get; init; }

  [Required]

  public int TranslationLangId { get; init; }
}
