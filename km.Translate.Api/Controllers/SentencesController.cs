using km.Library.GenericDto;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Queries.Sentences;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

/**
 * <summary>Manage original sentences with CRUD operations</summary>
 */
public class SentencesController : BaseResourceApiController
{
  // public record TranslationsDto : ResponseWithPageDto<Sentence>;
  public SentencesController(IMediator mediator) : base(mediator)
  {
  }
  /**
 * <summary>Get all sentences</summary>
 */
  [HttpGet]
  [Produces("application/json")]
  public async Task<ActionResult> GetSentencesByPage([FromQuery] RequestWithLocalDto dto, CancellationToken cancellationToken)
  {
    try
    {
      var query = new GetSentencesByPagesQuery(RequestWithLocalQuery.From(dto));
      ResponseWithPageDto<SentenceDto> sentencesDto = await _mediator.Send(query, cancellationToken);
      return sentencesDto.CurrentPageSize == 0 ?
        Problem("No sentences were found for this query", title: "No sentences found", statusCode: 404)
        : Ok(sentencesDto);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  /**
   * <summary>Get a specific sentence by providing its id</summary>
   */
  [HttpGet("{id:int}")]
  public async Task<ActionResult<SentenceDto>> GetSentenceById(int id)
  {
    try
    {
      var sentenceDto = await _mediator.Send(new GetSentenceByIdQuery(id));
      return sentenceDto == null ? NotFound() : sentenceDto;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return Problem(e.Message, title: "Error", statusCode: 500);
    }
  }
}
