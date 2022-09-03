using km.Library.Exceptions;
using km.Translate.DataLib.Commands;
using km.Translate.DataLib.Queries.Sentences;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

/**
 * <summary>Provide endpoints to control the creation, reinitialization of the database or credentials</summary>
 */
public class StarterController : BaseResourceApiController
{
  public StarterController(IMediator mediator) : base(mediator)
  {

  }
  /**
   * <summary>Create the database or recreate it if it already created</summary>
   * <param name="resetToken">Credential Token required to recreate the database</param>
   * <returns>The result an acknowledge message of the operation</returns>
   */
  [HttpGet("initialize")]
  public async Task<IActionResult> Initialize(string resetToken)
  {
    try
    {
      bool sentenceTableEmpty = await _mediator.Send(new IsSentenceTableEmptyQuery());

      if (!sentenceTableEmpty)
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

      await _mediator.Send(new InitializeDatabaseCommand(resetToken));
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
}
