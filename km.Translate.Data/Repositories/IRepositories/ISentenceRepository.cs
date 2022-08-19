using System.Linq.Expressions;
using km.Library.GenericDto;
using km.Library.Repositories;
using km.Translate.Data.Data.ApiModels;
using km.Translate.Data.Data.Models;

namespace km.Translate.Data.Repositories.IRepositories;

public interface ISentenceRepository : IGenericRepository<Sentence>
{
  public Task<ResponseWithPageDto<SentenceDto>> GetManyByPage(int pageNumber, int pageSize = 10, bool shuffle = false,
    Expression<Func<Sentence, bool>>? filterPredicate = null);

  public Task<Sentence?> GetOneByIdAsync(int id);
}
