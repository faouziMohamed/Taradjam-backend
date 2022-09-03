using km.Translate.DataLib.Data;
using km.Translate.DataLib.Repositories;
using km.Translate.DataLib.Repositories.IRepositories;
using MediatR;

namespace km.Translate.DataLib.Commands;

public sealed record InitializeDatabaseCommand(string ResetToken) : IRequest<bool>;

public class InitializeDatabaseHandler : IRequestHandler<InitializeDatabaseCommand, bool>
{
  private readonly IDatabaseInitializer _databaseInitializer;
  public InitializeDatabaseHandler(ApplicationDbContext context)
  {
    _databaseInitializer = new DatabaseInitializer(context);
  }
  public async Task<bool> Handle(InitializeDatabaseCommand request, CancellationToken cancellationToken)
  {
    await _databaseInitializer.ReinitializeDatabaseAsync();
    return true;
  }
}
