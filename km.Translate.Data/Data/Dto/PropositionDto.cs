// ReSharper disable  UnusedAutoPropertyAccessor.Global

using km.Translate.Data.Data.Models;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

#pragma warning disable CS8618

namespace km.Translate.Data.Data.ApiModels;

public sealed record TranslationsDto
{
  private TranslationsDto(Sentence sentence)
  {
    SentenceId = sentence.Id;
    ICollection<Proposition>? propositions = sentence.Propositions;
    Propositions = propositions.Select(PropositionsDto.From).ToList();
  }


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
  public string TranslatedText { get; init; }
  public string TranslationHash { get; init; }
  public string TranslatedBy { get; init; }
  public DateTime TranslationDate { get; init; }
  public VotesDto Votes { get; init; }
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
  public int TextVoId { get; init; }
  public string TranslatedText { get; init; }
  public string TranslatedBy { get; init; }
  public DateTime TranslationDate { get; init; }
  public string TranslationHash { get; init; }
  public int TranslationLangId { get; init; }
}
