using System.Linq.Expressions;
using km.Library.Exceptions;
using km.Library.Extensions;
using km.Library.GenericDto;
using km.Library.Repositories;
using km.Library.Utils;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace km.Translate.DataLib.Repositories;

public sealed class PropositionRepository : GenericRepository<Proposition, ApplicationDbContext>, IPropositionRepository
{
  public PropositionRepository(ApplicationDbContext context) : base(context)
  {

  }
  public TranslationsDto GetProposedTranslations(Sentence sentence)
  {
    return TranslationsDto.From(sentence);
  }

  public async Task<TranslationsDto> GetProposedTranslations(int sentenceId)
  {
    var sentence = await MakeSureSentenceExistsOrThrow(sentenceId);

    return GetProposedTranslations(sentence!);
  }

  public async Task<PropositionsDto> GetOneByIdAsync(int id)
  {
    var proposed = await GetByIdAsync(id);

    if (proposed == null)
    {
      throw new NotFoundException(
        message: $"No proposition with Id '${id}' exists in the database",
        hint: "Make sure you have the right proposition Id",
        title: "Proposition not found"
      );
    }

    return PropositionsDto.From(proposed);
  }
  public async Task<bool> IsPropositionExists(NewPropositionDto proposition)
  {
    // proposition exists if it the same sentenceVo, langId and hash
    int sentenceId = proposition.TextVoId;
    int langId = proposition.TranslationLangId;
    string hash = GenerateHash(proposition);
    return await ExistsAsync(
      p =>
        p.SentenceVoId == sentenceId &&
        p.TargetLanguageId == langId &&
        p.TranslationHash == hash
    );
  }
  public async Task<CreatedPropositionDto> AddNewProposition(NewPropositionDto proposition)
  {
    // add new proposition
    var newProposition = await CreateNewProposition(proposition);
    EntityEntry<Proposition> entity = await AddAsync(newProposition);
    await _context.SaveChangesAsync();
    return CreatedPropositionDto.From(entity.Entity, proposition.TextVoId);
  }
  public override Expression<Func<Proposition, object>>[] GetInnerJoinsExpressions()
  {
    return new Expression<Func<Proposition, object>>[]
    {
      static p => p.TargetLanguage,
      static p => p.ApprovedBy,
      static p => p.Votes
    };
  }
  public async Task MakeSureTranslationDoesNotExistOrThrow(NewPropositionDto translationsDto)
  {
    MakeSureTranslationDoesNotExistOrThrow(await IsPropositionExists(translationsDto));
  }
  public async Task MakeSureSentenceExistsOrThrow(NewPropositionDto proposition)
  {
    var sentenceRepository = new SentenceRepository(_context);
    bool isSentenceExists = await sentenceRepository.ExistsAsync(s => s.Id == proposition.TextVoId);

    if (!isSentenceExists)
    {
      throw new NotFoundException(
        "You cannot add a proposition for a sentence that does not exist",
        hint: "Make sure you have the right sentence id",
        title: "Sentence not found"
      );
    }
  }
  private async Task<Sentence?> MakeSureSentenceExistsOrThrow(int sentenceId)
  {
    var s = new SentenceRepository(_context);
    var sentence = await s.GetOneByIdAsync(sentenceId);

    if (sentence != null)
    {
      var proposotionRepository = new PropositionRepository(_context);
      List<Proposition> propositions = await _context.Propositions.Where(p => p.SentenceVoId == sentenceId)
        .SqlJoins(proposotionRepository.GetInnerJoinsExpressions()).ToListAsync();

      sentence.Propositions = propositions.OrderByDescending(static p => p.Id).ToList();
      return sentence;
    }

    throw new NotFoundException(
      message: $"Sentence with Id '${sentenceId}' not found",
      hint: "Make sure you have the right sentence id"
    );

  }
  private async Task<Proposition> CreateNewProposition(NewPropositionDto proposition)
  {
    var voteRepository = new VoteRepository(_context);
    EntityEntry<Vote> vote = await voteRepository.AddAsync(new Vote { UpVotes = 0, DownVotes = 0 });
    await _context.SaveChangesAsync();
    int voteId = vote.Entity.Id;
    return new Proposition
    {
      SentenceVoId = proposition.TextVoId,
      TargetLanguageId = proposition.TranslationLangId,
      TranslatedText = proposition.TranslatedText.Trim(),
      TranslationHash = GenerateHash(proposition),
      TranslationDate = proposition.TranslationDate,
      TranslatedBy = proposition.TranslatedBy.Trim(),
      VotesId = voteId
    };
  }
  private static string GenerateHash(NewPropositionDto proposition)
  {
    return proposition.TranslatedText.Trim().GenerateUniqueId().LongHash;
  }

  public async Task<ResponseWithPageDto<Proposition>> GetManyByPage(int pageNumber, int pageSize = 10, bool shuffle = false,
    Expression<Func<Proposition, bool>>? filterPredicate = null)
  {
    Expression<Func<Proposition, object>>[] innerJoins = GetInnerJoinsExpressions();
    ResponseWithPageDto<Proposition> response = await GetManyAsync(static p => p.Id,
      filterPredicate: filterPredicate!,
      pageNumber,
      pageSize,
      shuffle,
      innerJoins
    );

    return response;
  }

  private static void MakeSureTranslationDoesNotExistOrThrow(bool translationExists)
  {
    if (!translationExists) return;
    throw new AlreadyExistsException(
      "An exact translation with the same language already exists for this sentence.",
      "The translation you are trying to add already exists.",
      "Please try again with a different translation."
    );
  }
}
