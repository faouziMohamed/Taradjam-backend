using km.Library.Exceptions;
using km.Library.GenericDto;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

public class ProposedController : BaseApiController
{
  public ProposedController(IUnitOfWork unitOfWork) : base(unitOfWork)
  {
  }
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
  [Produces("application/json")]
  [HttpPost("/api/propose/new/translation")]
  public async Task<ActionResult<CreatedPropositionDto>> PostNewTranslation([FromBody] NewPropositionDto propositionDto)
  {
    try
    {
      // check if sentence exists and is the proposed translation is not already proposed
      await _unitOfWork.Propositions.MakeSureSentenceExistsOrThrow(propositionDto);
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
}
