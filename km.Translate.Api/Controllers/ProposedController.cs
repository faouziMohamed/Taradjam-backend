using km.Library.Exceptions;
using km.Library.GenericDto;
using km.Translate.DataLib.Commands.Translations;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Queries.Translations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

/**
 * <summary>Provide endpoints to perform CRUD operations on the proposed translations</summary>
 */
public class ProposedController : BaseResourceApiController
{
  public ProposedController(IMediator mediator) : base(mediator)
  {
  }
  /**
   * <summary>
   *   Get proposed translations for a particular sentence knowing it Id
   * </summary>
   */
  [Produces("application/json")]
  [HttpGet("translations/{sentenceId:int}")]
  public async Task<ActionResult<TranslationsDto>> GetProposedTranslations(int sentenceId)
  {
    try
    {
      return await _mediator.Send(new GetProposedTranslationQuery(sentenceId));
    }
    catch (NotFoundException e)
    {
      var exception = new ExceptionBaseDto(title: e.Title, message: e.Message, hint: e.Hint);
      Response.StatusCode = 404;
      return Content(content: exception.ToString(), "application/json");
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  /**
   * <summary>
   *   Create a new proposed translation for a particular sentence knowing it Id
   * </summary>
   */
  [Produces("application/json")]
  [HttpPost("/api/propose/new/translation")]
  public async Task<ActionResult<PropositionsDto>> PostNewTranslation([FromBody] PostNewPropositionDto propositionDto)
  {
    try
    {
      var proposition = PostNewPropositionCommand.From(propositionDto);
      return await _mediator.Send(new InsertPropositionCommand(proposition));
    }
    catch (DataException e) when (e is NotFoundException or AlreadyExistsException)
    {
      int code = e is NotFoundException ? 404 : 409;
      return ExceptionToJsonResponse(e, code);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  /**
   * <summary>
   *   Update a proposed translation with new content
   * </summary>
   */
  [HttpPut("/api/update/proposed/translation")]
  [Produces("application/json")]
  public async Task<ActionResult<PropositionsDto>> UpdateProposedTranslation([FromBody] PutPropositionDto propositionDto)
  {
    try
    {
      return await _mediator.Send(new UpdatePropositionCommand(PutPropositionCommand.From(propositionDto)));
    }
    catch (NotFoundException e)
    {
      return ExceptionToJsonResponse(e, 404);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  /**
   * <summary>
   *   Handle voting (Upvote or DownVote) for a proposed translation
   * </summary>
   */
  [HttpPut("/api/reaction/{propositionId:int}")]
  [Produces("application/json")]
  public async Task<ActionResult> UpdateVote([FromRoute] int propositionId, [FromQuery] string target)
  {
    try
    {
      // making sure that the target is either (up or upvote) or (down or downvote)
      string vTarget = ValidateTargetParamQuery(target);
      var voted = await _mediator.Send(new UpdateVotePropositionCommand(propositionId, IsUpVote: vTarget == "up"));
      return Ok(voted);
    }
    catch (DataException e) when (e is NotFoundException or InvalidTargetException)
    {
      int code = e is NotFoundException ? 404 : 400;
      return ExceptionToJsonResponse(e, code);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
  private static string ValidateTargetParamQuery(string target)
  {
    return target.ToLowerInvariant() switch
    {
      "up" or "upvote" => "up",
      "down" or "downvote" => "down",
      _ => throw new InvalidTargetException(
        message: $"'{target}' is not expected as a target for a vote. Expected 'up' or 'down'",
        hint: "Target must be ('up', 'upvote') for an upvote or ('down', 'downvote') for a downVote",
        title: "Invalid target"
      )
    };
  }
}
