using System.Linq.Expressions;
using System.Reflection;
using km.Library.GenericDto;
using km.Library.Repositories;
using km.Translate.DataLib.Data;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Repositories.IRepositories;

namespace km.Translate.DataLib.Repositories;

public sealed class SentenceRepository : GenericRepository<Sentence, ApplicationDbContext>, ISentenceRepository
{
  public SentenceRepository(ApplicationDbContext context) : base(context)
  {
  }

  private static string BinPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;


  public async Task<Sentence?> GetOneByIdAsync(int id)
  {
    Expression<Func<Sentence, object>>[] innerJoins = GetInnerJoinsExpressions();
    var sentence = await GetOneAsync(s => s.Id == id, innerJoins);
    return sentence;
  }

  public async Task<ResponseWithPageDto<SentenceDto>> GetManyByPage(
    int pageNumber,
    int pageSize,
    bool shuffle = false,
    Expression<Func<Sentence, bool>>? filterPredicate = null)
  {
    Expression<Func<Sentence, object>>[] innerJoins = GetInnerJoinsExpressions();
    ResponseWithPageDto<Sentence> response = await GetManyAsync(static s => s.Id,
      filterPredicate: filterPredicate!,
      pageNumber,
      pageSize,
      shuffle,
      innerJoins
    );

    List<Sentence> data = response.Data.ToList();
    var r = SentenceDto.From(data[0]);

    Console.WriteLine(r);

    return response.Map(static s => SentenceDto.From(s));
  }

  public override Expression<Func<Sentence, object>>[] GetInnerJoinsExpressions()
  {
    var joins = new Expression<Func<Sentence, object>>[]
    {
      static s => s.SrcLanguage
    };

    return joins;
  }
}
