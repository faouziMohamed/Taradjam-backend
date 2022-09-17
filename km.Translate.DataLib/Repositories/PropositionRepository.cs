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
    var sentence = await GetSentenceIfExistsOrThrow(sentenceId);

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
  public override Expression<Func<Proposition, object>>[] GetPropertiesToJoinsExpressions()
  {
    return new Expression<Func<Proposition, object>>[]
    {
      static p => p.TranslationLang,
      static p => p.ApprovedBy
    };
  }
  public async Task<PropositionsDto> AddNewProposition(PostNewPropositionDto proposition)
  {
    // add new proposition
    var newProposition = CreateNewProposition(proposition);
    EntityEntry<Proposition> newlyAddedProposition = await AddAsync(newProposition);
    await _context.SaveChangesAsync();
    return PropositionsDto.From(newlyAddedProposition.Entity);
  }
  public async Task<PropositionsDto> UpdateProposition(PutPropositionDto propositionDto)
  {
    // Update proposition
    var proposition = await GetPropositionOrThrow(propositionDto.PropositionId);
    UpdateProposition(propositionDto, proposition);
    return PropositionsDto.From(proposition);
  }
  public async Task MakeSurePropositionExistOrThrow(int propositionId)
  {
    await GetPropositionOrThrow(propositionId);
  }
  public async Task MakeSureTranslationDoesNotExistOrThrow(AbstractBasePropositionDtoWithoutId translationDto)
  {
    MakeSureTranslationDoesNotExistOrThrow(await IsPropositionExists(translationDto));
  }
  public async Task MakeSureSentenceExistsOrThrow(int sentenceId)
  {
    var sentenceRepository = new SentenceRepository(_context);
    bool isSentenceExists = await sentenceRepository.ExistsAsync(s => s.Id == sentenceId);

    if (!isSentenceExists)
    {
      throw new NotFoundException(
        message: $"You cannot add a proposition for a sentence that does not exist, passed sentence id: {sentenceId}",
        hint: "Make sure you have the right sentence id",
        title: $"Sentence with id={sentenceId} not found"
      );
    }
  }
  public async Task<long> DoAnUpVote(int propositionId)
  {
    var proposition = await GetPropositionOrThrow(propositionId);
    proposition.Votes++;
    await _context.SaveChangesAsync();
    return proposition.Votes;
  }

  public async Task<long> DoADownVote(int propositionId)
  {
    var proposition = await GetPropositionOrThrow(propositionId);
    proposition.Votes--;
    await _context.SaveChangesAsync();
    return proposition.Votes;
  }

  public async Task<long> DoAVote(int propositionId, bool isUpVote)
  {
    var proposition = await GetPropositionOrThrow(propositionId);
    long votes = isUpVote ? ++proposition.Votes : --proposition.Votes;
    await _context.SaveChangesAsync();
    return votes;
  }
  public async Task<bool> IsPropositionExists(AbstractBasePropositionDtoWithoutId proposition)
  {
    // proposition exists if it the same sentenceVo, langId and hash
    int sentenceId = proposition.SentenceVoId;
    int langId = proposition.TranslationLangId;
    string hash = proposition.TranslatedText.GenerateHash();
    return await ExistsAsync(
      p =>
        p.SentenceVoId == sentenceId &&
        p.TranslationLangId == langId &&
        p.TranslationHash == hash
    );
  }
  private void UpdateProposition(PutPropositionDto propositionDto, Proposition proposition)
  {
    proposition.TranslationLangId = propositionDto.TranslationLangId;
    proposition.TranslationHash = propositionDto.TranslatedText.GenerateHash();
    proposition.TranslatedText = propositionDto.TranslatedText.Trim();
    proposition.TranslationDate = propositionDto.TranslationDate;
    proposition.TranslatedBy = propositionDto.TranslatedBy ?? "Anonymous";
    proposition.Votes = propositionDto.Votes;
    Update(proposition);
  }
  private async Task<Proposition> GetPropositionOrThrow(int propositionId)
  {
    var proposition = await GetByIdAsync(propositionId);
    if (proposition != null) return proposition;

    throw new NotFoundException(
      message: $"No proposition with Id '{propositionId}' exists in the database",
      hint: "Make sure you have the right proposition Id",
      title: "Proposition not found"
    );

  }

  private async Task<Sentence> GetSentenceIfExistsOrThrow(int sentenceId)
  {
    var sentenceRepository = new SentenceRepository(_context);
    var sentence = await sentenceRepository.GetOneByIdAsync(sentenceId);

    if (sentence != null)
    {
      return await JoinsPropositionsOnSentence(sentence);
    }

    // sentence does not exist throw
    throw new NotFoundException(
      message: $"Sentence with Id '${sentenceId}' not found",
      hint: "Make sure you have the right sentence id"
    );
  }
  private async Task<Sentence> JoinsPropositionsOnSentence(Sentence sentence)
  {
    var propositionRepository = new PropositionRepository(_context);
    List<Proposition> propositions = await _context
      .Propositions.Where(p => p.SentenceVoId == sentence.Id)
      .SqlJoins(propositionRepository.GetPropertiesToJoinsExpressions())
      .OrderByDescending(static p => p.Id)
      .ToListAsync();

    sentence.Propositions = propositions;
    return sentence;
  }
  private static Proposition CreateNewProposition(PostNewPropositionDto proposition)
  {
    return new Proposition
    {
      SentenceVoId = proposition.SentenceVoId,
      TranslationLangId = proposition.TranslationLangId,
      TranslatedText = proposition.TranslatedText.Trim(),
      TranslationHash = proposition.TranslatedText.GenerateHash(),
      TranslationDate = proposition.TranslationDate,
      TranslatedBy = proposition.TranslatedBy.Trim(),
      Votes = 0
    };
  }

  public async Task<ResponseWithPageDto<Proposition>> GetManyByPage(int pageNumber, int pageSize = 10, bool shuffle = false,
    Expression<Func<Proposition, bool>>? filterPredicate = null)
  {
    Expression<Func<Proposition, object>>[] innerJoins = GetPropertiesToJoinsExpressions();
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
