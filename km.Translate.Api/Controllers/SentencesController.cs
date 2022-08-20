using System.Linq.Expressions;
using km.Library.GenericDto;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace km.Translate.Api.Controllers;

public class SentencesController : BaseApiController
{
  // public record TranslationsDto : ResponseWithPageDto<Sentence>;
  public SentencesController(IUnitOfWork unitOfWork) : base(unitOfWork)
  {
  }

  [HttpGet]
  public async Task<ActionResult> GetSentences([FromQuery] RequestWithLocalDto dto)
  {
    try
    {
      int pageNumber = dto.PageNumber ?? 1;
      int pageSize = dto.PageSize ?? 10;
      bool shuffle = dto.Shuffle ?? false;
      string? lang = dto.Lang;
      Expression<Func<Sentence, bool>> filter = s => s.SrcLanguage.LanguageShortName == lang;

      ResponseWithPageDto<SentenceDto> sentencesDto = lang switch
      {
        null => await _unitOfWork.Sentences.GetManyByPage(pageNumber, pageSize, shuffle),
        _ => await _unitOfWork.Sentences.GetManyByPage(pageNumber, pageSize, shuffle, filter)
      };

      return sentencesDto.CurrentPageSize == 0 ? Problem("No sentences found", title: "No sentences found", statusCode: 404) : Ok(sentencesDto);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<SentenceDto>> GetSentence(int id)
  {
    try
    {
      var sentence = await _unitOfWork.Sentences.GetOneByIdAsync(id);

      if (sentence == null)
      {
        return NotFound();
      }

      var sentenceDto = SentenceDto.From(sentence);
      return sentenceDto;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return Problem(e.Message, title: "Error", statusCode: 500);
    }
  }
}
