using km.Library.Exceptions;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

/**
 * <summary>Provide endpoints to control the creation, reinitialization of the database or credentials</summary>
 */
public class StarterController : BaseApiController
{
  public StarterController(IUnitOfWork unitOfWork) : base(unitOfWork)
  {

  }
  /**
   * <summary>Create the database or recreate it if it already created</summary>
   * <param name="force">
   *   Set this query to true to recreate the database. This will delete all data in the database and recreate it with seed
   *   data. <br />
   *   Set this query to false to create the database. This will create the database if it does not
   *   already exist.
   *   If set to true it will require to pass an api token <see cref="token" />
   * </param>
   * <param name="token">Credential Token required to recreate the database</param>
   * <returns>The result an acknowledge message of the operation</returns>
   */
  [HttpGet("initialize")]
  public async Task<IActionResult> Initialize(bool force = false, string token = "")
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
