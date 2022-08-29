using km.Library.Exceptions;
using km.Library.GenericDto;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

/**
 * <summary>Provide endpoints to perform CRUD operations on the proposed translations</summary>
 */
public class ProposedController : BaseApiController
{
  public ProposedController(IUnitOfWork unitOfWork) : base(unitOfWork)
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
      var proposedTranslations = await _unitOfWork.Propositions.GetProposedTranslations(sentenceId);
      return proposedTranslations;
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
      // check if sentence exists and is the proposed translation is not already proposed
      await _unitOfWork.Propositions.MakeSureSentenceExistsOrThrow(propositionDto.SentenceVoId);
      await _unitOfWork.Propositions.MakeSureTranslationDoesNotExistOrThrow(propositionDto);
      var newProposition = await _unitOfWork.Propositions.AddNewProposition(propositionDto);
      await _unitOfWork.CompleteAsync();
      return newProposition;
    }
    catch (NotFoundException e)
    {
      return ExceptionToJsonResponse(e, 404);
    }
    catch (AlreadyExistsException e)
    {
      return ExceptionToJsonResponse(e, 409);
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
      await _unitOfWork.Propositions.MakeSurePropositionExistOrThrow(propositionDto.PropositionId);
      var updatedProposition = await _unitOfWork.Propositions.UpdateProposition(propositionDto);
      await _unitOfWork.CompleteAsync();
      return updatedProposition;
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
      await _unitOfWork.Propositions.MakeSurePropositionExistOrThrow(propositionId);
      // making sure that the target is either (up or upvote) or (down or downvote)
      string vTarget = target.ToLowerInvariant() switch
      {
        "up" or "upvote" => "up",
        "down" or "downvote" => "down",
        _ => throw new InvalidTargetException(
          message: $"'{target}' is not expected as a target for a vote. Expected 'up' or 'down'",
          hint: "Target must be ('up', 'upvote') for an upvote or ('down', 'downvote') for a downVote",
          title: "Invalid target"
        )

      };

      long votes = await _unitOfWork.Propositions.DoAVote(propositionId, isUpVote: vTarget == "up");
      var votedDto = new ReturnedVotesDto { votes = votes };
      return Ok(votedDto);
    }
    catch (NotFoundException e)
    {
      return ExceptionToJsonResponse(e, 404);
    }
    catch (InvalidTargetException e)
    {
      return ExceptionToJsonResponse(e, 400);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
}
