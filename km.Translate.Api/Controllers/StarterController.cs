using km.Library.Exceptions;
using km.Translate.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

public class StarterController : BaseApiController
{
  public StarterController(IUnitOfWork unitOfWork) : base(unitOfWork)
  {

  }
  [HttpGet("initialize")]
  public async Task<IActionResult> Initialize(bool force = false)
  {
    try
    {
      await (
        force ?
          // Empty the tables and repopulate with seed data
          ReinitializeDatabaseAsync() :
          // Fill the tables with seed data assuming they are empty
          InitializeDatabaseAsync()
      );

      await _unitOfWork.CompleteAsync();

      return Ok("Database initialized");
    }
    catch (AlreadyCreatedException e)
    {
      return ExceptionToJsonResponse(e, 409);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
  private async Task ReinitializeDatabaseAsync()
  {

    await _unitOfWork.DatabaseInitializer.ReinitializeDatabaseAsync();
  }
  private async Task InitializeDatabaseAsync()
  {
    int sentencesRowCount = await _unitOfWork.Sentences.CountAsync();

    if (sentencesRowCount != 0)
    {
      throw new AlreadyCreatedException(
        title: "Database already initialized",
        hint: "Use the query params '?force=true&resetToken=<your-reset-token>' to reinitialize the database",
        message:
        "The database has already been initialized, " +
        "if you want to re-initialize the database, " +
        "please delete the database recreate it by hitting the endpoint with the query '?force=true&resetToken=<your-reset-token>'"
      );
    }

    await _unitOfWork.DatabaseInitializer.InitializeDatabaseAsync();
  }
}
