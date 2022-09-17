using km.Translate.DataLib.Data.Dto;
using km.Translate.DataLib.Handlers.Translations;
using km.Translate.DataLib.Queries.Translations;
using km.Translate.DataLib.Repositories.IRepositories;
using Moq;
using Shouldly;
using MockRepository = km.Translate.UnitTests.Mocks.MockRepository;

// ReSharper disable RedundantArgumentDefaultValue

namespace km.Translate.UnitTests.Sentences.Queries;

public sealed class GetProposedTranslationHandlerTests
{

  private readonly Mock<IPropositionRepository> _propositionRepository = new();
  [Fact]
  public async Task GetProposedTranslation_Return_The_ProposedTranslations()
  {

    // Arrange
    const int sentenceVoId = 3;

    var proposedTranslation = MockRepository.GetOneTranslationDto(sentenceVoId);
    _propositionRepository.Setup(static r => r.GetProposedTranslations(sentenceVoId))
      .ReturnsAsync(proposedTranslation);

    var handler = new GetProposedTranslationHandler(_propositionRepository.Object);

    // Act
    var result = await handler.Handle(request: new GetProposedTranslationQuery(sentenceVoId), CancellationToken.None);
    result.ShouldNotBeNull();
    result.ShouldBeOfType<TranslationsDto>();
    result.SentenceVoId.ShouldBe(sentenceVoId);
    result.Propositions.ShouldAllBe(static p => p.SentenceVoId == sentenceVoId);
  }
}
