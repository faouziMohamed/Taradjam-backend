using km.Library.GenericDto;
using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Handlers.Sentences;
using km.Translate.DataLib.Queries.Sentences;
using km.Translate.DataLib.Repositories.IRepositories;
using Moq;
using Shouldly;
using MockRepository = km.Translate.UnitTests.Mocks.MockRepository;

namespace km.Translate.UnitTests.Sentences.Queries;

public sealed class GetSentencesByPageHandlerTests
{

  private readonly Mock<ISentenceRepository> _sentenceRepository = new();

  [Fact]
  public async Task GetSentencesByPage_Return_TheCorrectAmountOfSentences()
  {
    // Arrange
    const int pageSize = 6, page = 0;
    const bool shuffle = false;
    const string lang = "fr";

    var paginatedResult = new ResponseWithPageDto<SentenceDto>
    {
      NextPage = page + 1,
      CurrentPage = page,
      TotalPageCount = MockRepository.GetSentencesTotalPageCount(pageSize),
      CurrentPageSize = pageSize,
      Data = MockRepository.GetSentenceDtos(pageSize)
    };

    _sentenceRepository
      .Setup(static r => r.GetManyByPage(page, pageSize, shuffle, static s => s.LanguageVo.ShortName == lang))
      .ReturnsAsync(paginatedResult);

    var queryObj = RequestWithLocalQuery.From(
      new RequestWithLocalDto { Page = page, PageSize = pageSize, Shuffle = shuffle, Lang = lang }
    );

    var handler = new GetSentencesByPagesHandler(_sentenceRepository.Object);

    // Act
    ResponseWithPageDto<SentenceDto> result = await handler
      .Handle(request: new GetSentencesByPagesQuery(queryObj), default);

    // Assert
    result.ShouldNotBeNull();
    result.CurrentPage.ShouldBe(page);
    result.CurrentPageSize.ShouldBe(pageSize);
    result.Data.Count().ShouldBeLessThanOrEqualTo(pageSize);
    result.Data.ShouldAllBe(static s => s.SrcLanguage.LangShortName.ToLower() == lang);
  }
}
