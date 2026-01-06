using MediatR;
using TechFood.Kitchen.Application.Commands.CompletePreparation;
using TechFood.Kitchen.Application.Events.Integration.Outgoing;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Domain.Repositories;

namespace TechFood.Kitchen.Application.Tests.Commands;

public class CompletePreparationCommandHandlerTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly Mock<IMediator> _mockMediator;
    private readonly CompletePreparationCommandHandler _handler;

    public CompletePreparationCommandHandlerTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _mockMediator = new Mock<IMediator>();
        _handler = new CompletePreparationCommandHandler(_mockRepository.Object, _mockMediator.Object);
    }

    [Fact(DisplayName = "Should complete preparation and publish event when preparation exists")]
    [Trait("Application", "CompletePreparationCommandHandler")]
    public async Task Handle_ShouldCompletePreparationAndPublishEvent_WhenPreparationExists()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var preparation = new Preparation(orderId);
        preparation.Start(); 
        var command = new CompletePreparationCommand(preparationId);
        var cancellationToken = CancellationToken.None;

        _mockRepository
            .Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);

        _mockMediator
            .Setup(m => m.Publish(It.IsAny<PreparationDoneEvent>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().Be(Unit.Value);
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockMediator.Verify(m => m.Publish(
            It.Is<PreparationDoneEvent>(e => e.OrderId == orderId),
            cancellationToken), Times.Once);
    }

    [Fact(DisplayName = "Should throw exception when preparation not found")]
    [Trait("Application", "CompletePreparationCommandHandler")]
    public async Task Handle_ShouldThrowException_WhenPreparationNotFound()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var command = new CompletePreparationCommand(preparationId);
        var cancellationToken = CancellationToken.None;

        _mockRepository
            .Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync((Preparation?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<TechFood.Shared.Application.Exceptions.ApplicationException>();
        _mockRepository.Verify(r => r.GetByIdAsync(preparationId), Times.Once);
        _mockMediator.Verify(m => m.Publish(
            It.IsAny<PreparationDoneEvent>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Should publish event with correct order id")]
    [Trait("Application", "CompletePreparationCommandHandler")]
    public async Task Handle_ShouldPublishEventWithCorrectOrderId_WhenPreparationCompleted()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var preparation = new Preparation(orderId);
        preparation.Start(); 
        var command = new CompletePreparationCommand(preparationId);
        var cancellationToken = CancellationToken.None;
        PreparationDoneEvent? publishedEvent = null;

        _mockRepository
            .Setup(r => r.GetByIdAsync(preparationId))
            .ReturnsAsync(preparation);

        _mockMediator
            .Setup(m => m.Publish(It.IsAny<PreparationDoneEvent>(), cancellationToken))
            .Callback<INotification, CancellationToken>((e, _) => publishedEvent = e as PreparationDoneEvent)
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        publishedEvent.Should().NotBeNull();
        publishedEvent!.OrderId.Should().Be(orderId);
    }
}
