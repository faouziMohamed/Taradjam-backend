#nullable disable
using km.Library.Repositories;

namespace km.Translate.DataLib.Data.Models;

public sealed class Vote : BaseEntity
{
  public long UpVotes { get; set; }
  public long DownVotes { get; set; }
}
