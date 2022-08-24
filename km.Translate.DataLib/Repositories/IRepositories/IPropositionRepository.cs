using km.Library.Repositories;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;

namespace km.Translate.DataLib.Repositories.IRepositories;

public interface IPropositionRepository : IGenericRepository<Proposition>
{
  public Task<TranslationsDto> GetProposedTranslations(int sentenceId);
  public TranslationsDto GetProposedTranslations(Sentence sentence);
  public Task<PropositionsDto> GetOneByIdAsync(int id);
  public Task<bool> IsPropositionExists(NewPropositionDto proposition);
  public Task MakeSureTranslationDoesNotExistOrThrow(NewPropositionDto translationsDto);
  public Task MakeSureSentenceExistsOrThrow(NewPropositionDto proposition);
  public Task<CreatedPropositionDto> AddNewProposition(NewPropositionDto proposition);
}
