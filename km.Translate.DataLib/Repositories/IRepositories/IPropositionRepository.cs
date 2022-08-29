using km.Library.Repositories;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;

namespace km.Translate.DataLib.Repositories.IRepositories;

public interface IPropositionRepository : IGenericRepository<Proposition>
{
  public Task<TranslationsDto> GetProposedTranslations(int sentenceId);
  public TranslationsDto GetProposedTranslations(Sentence sentence);
  public Task<PropositionsDto> GetOneByIdAsync(int id);
  public Task<long> DoAnUpVote(int propositionId);
  public Task<long> DoADownVote(int propositionId);
  public Task<long> DoAVote(int propositionId, bool isUpVote);

  public Task<bool> IsPropositionExists(AbstractBasePropositionDtoWithoutId proposition);
  public Task MakeSureTranslationDoesNotExistOrThrow(AbstractBasePropositionDtoWithoutId translationsDto);
  public Task MakeSureSentenceExistsOrThrow(int sentenceId);
  public Task MakeSurePropositionExistOrThrow(int propositionDto);
  public Task<PropositionsDto> AddNewProposition(PostNewPropositionDto proposition);
  public Task<PropositionsDto> UpdateProposition(PutPropositionDto proposition);
}
