using AutoFixture.Xunit3;
using FluentAssertions;
using LexiBridge.Application.Features.Learning.UseCases.CreateLearningItem;
using LexiBridge.Application.UnitTests.Common;
using LexiBridge.Domain.Aggregates.Learning;
using Moq;

namespace LexiBridge.Application.UnitTests.Features.Learning;

public class CreateLearningItemHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ValidRequest_ShouldCreateLearningItemAndReturnId(
        CreateLearningItemCommand command,
        [Frozen] Mock<ILearningItemRepository> repositoryMock,
        [Greedy] CreateLearningItemHandler sut,
        CancellationToken cancellationToken)
    {
        // Arrange
        command = command with
        {
            AudioStream = null,
            AudioFileName = null,
            AudioContentType = null,
            ImageStream = null,
            ImageFileName = null,
            ImageContentType = null
        };

        LearningItem capturedItem = null!;

        repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<LearningItem>(), cancellationToken))
            .Callback<LearningItem, CancellationToken>((item, _) => capturedItem = item)
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(x => x.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Value.Should().NotBeEmpty();

        capturedItem.Should().NotBeNull();
        capturedItem.Headword.Should().Be(command.Headword);
        capturedItem.PartOfSpeech.Should().Be(command.PartOfSpeech);
        capturedItem.Accent.Should().Be(command.Accent);
        capturedItem.Ipa.Should().Be(command.Ipa);
        capturedItem.Cloze.Should().Be(command.Cloze);
        capturedItem.Definition.Should().Be(command.Definition);
        capturedItem.Translation.Should().Be(command.Translation);
        capturedItem.AudioPath.Should().BeNull();
        capturedItem.ImagePath.Should().BeNull();
        capturedItem.DictionaryEntryId.Should().Be(command.DictionaryEntryId);

        capturedItem.Examples.Should().HaveCount(command.Examples.Count);
        capturedItem.Examples.Select(e => e.Text).Should().BeEquivalentTo(command.Examples);

        repositoryMock.Verify(
            x => x.AddAsync(It.IsAny<LearningItem>(), cancellationToken),
            Times.Once);

        repositoryMock.Verify(
            x => x.UnitOfWork.SaveChangesAsync(cancellationToken),
            Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_RequestWithNoExamples_ShouldCreateLearningItemWithoutExamples(
        CreateLearningItemCommand command,
        [Frozen] Mock<ILearningItemRepository> repositoryMock,
        [Greedy] CreateLearningItemHandler sut,
        CancellationToken cancellationToken)
    {
        // Arrange
        command = command with
        {
            Examples = [],
            AudioStream = null,
            AudioFileName = null,
            AudioContentType = null,
            ImageStream = null,
            ImageFileName = null,
            ImageContentType = null
        };

        LearningItem capturedItem = null!;

        repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<LearningItem>(), cancellationToken))
            .Callback<LearningItem, CancellationToken>((item, _) => capturedItem = item)
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(x => x.UnitOfWork.SaveChangesAsync(cancellationToken))
            .ReturnsAsync(1);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        capturedItem.Examples.Should().BeEmpty();
    }
}