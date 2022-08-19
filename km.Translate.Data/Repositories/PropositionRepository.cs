using System.Linq.Expressions;
using km.Library.Exceptions;
using km.Library.GenericDto;
using km.Library.Repositories;
using km.Translate.Data.Data;
using km.Translate.Data.Data.ApiModels;
using km.Translate.Data.Data.Models;
using km.Translate.Data.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace km.Translate.Data.Repositories;

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
    var s = new SentenceRepository(_context);
    var sentence = await s.GetOneByIdAsync(sentenceId);

    if (sentence == null)
    {
      throw new NotFoundException(
        message: $"Sentence with Id '${sentenceId}' not found",
        hint: "Make sure you have the right sentence id"
      );
    }

    return GetProposedTranslations(sentence);
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
    string hash = proposition.TranslationHash;
    int langId = proposition.TranslationLangId;
    bool isTranslationExists = await ExistsAsync(t => t.TranslationHash == hash && t.TargetLanguageId == langId);
    return isTranslationExists;
  }
  public async Task<CreatedPropositionDto> AddNewProposition(NewPropositionDto proposition)
  {
    MakeSureTranslationDoesNotExistOrThrow(await IsPropositionExists(proposition));
    // check if sentence exists
    await MakeSureSentenceExistsOrThrow(proposition);

    // add new proposition
    var newProposition = CreateNewProposition(proposition);
    EntityEntry<Proposition> entity = await AddAsync(newProposition);
    await _context.SaveChangesAsync();
    return CreatedPropositionDto.From(entity.Entity, proposition.TextVoId);
  }
  public override Expression<Func<Proposition, object>>[] GetInnerJoinsExpressions()
  {
    return new Expression<Func<Proposition, object>>[]
    {
      static p => p.TargetLanguage,
      static p => p.TranslatedBy,
      static p => p.ApprovedBy,
      static p => p.Votes
    };
  }
  private static Proposition CreateNewProposition(NewPropositionDto proposition)
  {
    return new Proposition
    {
      SentenceVoId = proposition.TextVoId,
      TargetLanguageId = proposition.TranslationLangId,
      TranslatedText = proposition.TranslatedText,
      TranslationHash = proposition.TranslationHash,
      TranslationDate = proposition.TranslationDate,
      TranslatedBy = proposition.TranslatedBy,
      Votes = new Vote { UpVotes = 0, DownVotes = 0 }
    };
  }
  private async Task MakeSureSentenceExistsOrThrow(NewPropositionDto proposition)
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

  private static void MakeSureTranslationDoesNotExistOrThrow(bool isTranslationExists)
  {
    if (isTranslationExists)
    {
      throw new AlreadyExistsException(
        "An exact translation with the same language already exists for this sentence.",
        "The translation you are trying to add already exists.",
        "Please try again with a different translation."
      );
    }
  }
}
