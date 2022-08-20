using km.Library.Exceptions;
using km.Library.GenericDto;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable InconsistentNaming

namespace km.Translate.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
  protected readonly IUnitOfWork _unitOfWork;

  protected BaseApiController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }
  protected ContentResult ExceptionToJsonResponse(DataException e, int httpCode)
  {
    var exception = new ExceptionBaseDto(title: e.Title, message: e.Message, hint: e.Hint);
    Response.StatusCode = httpCode;
    var response = Content(content: exception.ToString(), "application/json");
    return response;
  }
}
