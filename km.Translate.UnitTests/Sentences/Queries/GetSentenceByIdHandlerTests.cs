using km.Translate.DataLib.Data.Models;
using km.Translate.DataLib.Handlers.Sentences;
using km.Translate.DataLib.Queries.Sentences;
using km.Translate.DataLib.Repositories.IRepositories;
using Moq;
using Shouldly;

namespace km.Translate.UnitTests.Sentences.Queries;

public sealed class GetSentenceByIdHandlerTests
{
  private readonly Mock<ISentenceRepository> _sentenceRepository = new();
  [Fact]
  public async Task GetSentenceById_Return_The_Correct_Sentence()
  {
    // Arrange
    const int id = 1;
    var sentence = new Sentence
    {
      Id = id,
      SentenceVo = "some values",
      LanguageVo = new Language
      {
        Id = 1,
        LongName = "some values",
        ShortName = "some values"
      }
    };

    _sentenceRepository.Setup(static r => r.GetOneByIdAsync(id)).ReturnsAsync(sentence);
    var handler = new GetSentenceByIdHandler(_sentenceRepository.Object);

    // Act
    var sentenceDto = await handler.Handle(request: new GetSentenceByIdQuery(id), CancellationToken.None);
    sentenceDto.ShouldNotBeNull();
    sentenceDto.SentenceVoId.ShouldBe(id);
    sentenceDto.SentenceVo.ShouldBe(sentence.SentenceVo);
  }
}
