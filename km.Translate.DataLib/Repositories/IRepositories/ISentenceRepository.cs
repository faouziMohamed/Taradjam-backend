using System.Linq.Expressions;
using km.Library.GenericDto;
using km.Library.Repositories;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;

namespace km.Translate.DataLib.Repositories.IRepositories;

public interface ISentenceRepository : IGenericRepository<Sentence>
{
  public Task<ResponseWithPageDto<SentenceDto>> GetManyByPage(int page, int pageSize = 10, bool shuffle = false,
    Expression<Func<Sentence, bool>>? filterPredicate = null);

  public Task<Sentence?> GetOneByIdAsync(int id);
}
