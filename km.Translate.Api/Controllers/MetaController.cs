using System.Diagnostics;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

public class MetaController : BaseApiController
{
  public MetaController(IUnitOfWork unitOfWork) : base(unitOfWork)
  {
  }
  /// <summary>
  ///   Give some information about the server
  /// </summary>
  [HttpGet("/info")]
  public ActionResult<string> Info()
  {
    var assembly = typeof(WebMarker).Assembly;
    var creationDate = System.IO.File.GetCreationTime(assembly.Location);
    string? version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
    return Ok($"Version: {version}, Last Updated: {creationDate:f}");
  }
}
