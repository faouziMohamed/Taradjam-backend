using km.Library.Exceptions;
using km.Library.GenericDto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable InconsistentNaming

namespace km.Translate.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{

  protected ContentResult ExceptionToJsonResponse(DataException e, int httpCode)
  {
    var exception = new ExceptionBaseDto(title: e.Title, message: e.Message, hint: e.Hint);
    Response.StatusCode = httpCode;
    return Content(content: exception.ToString(), "application/json");
  }
}

public abstract class BaseResourceApiController : BaseApiController
{
  protected readonly IMediator _mediator;

  protected BaseResourceApiController(IMediator mediator)
  {
    _mediator = mediator;
  }
}
