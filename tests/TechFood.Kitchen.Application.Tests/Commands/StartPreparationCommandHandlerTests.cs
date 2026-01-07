using MediatR;
using TechFood.Kitchen.Application.Commands.StartPreparation;
using TechFood.Kitchen.Application.Events.Integration.Outgoing;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Domain.Repositories;

namespace TechFood.Kitchen.Application.Tests.Commands;

public class StartPreparationCommandHandlerTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly Mock<IMediator> _mockMediator;
    private readonly StartPreparationCommandHandler _handler;

    public StartPreparationCommandHandlerTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _mockMediator = new Mock<IMediator>();
        _handler = new StartPreparationCommandHandler(_mockRepository.Object, _mockMediator.Object);
    }

    [Fact(DisplayName = "Should start preparation and publish event when preparation exists")]
    [Trait("Application", "StartPreparationCommandHandler")]
    public async Task Handle_ShouldStartPreparationAndPublishEvent_WhenPreparationExists()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var preparation = new Preparation(orderId);
        var command = new StartPreparationCommand(preparationId);
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
    [Trait("Application", "StartPreparationCommandHandler")]
    public async Task Handle_ShouldThrowException_WhenPreparationNotFound()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var command = new StartPreparationCommand(preparationId);
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
    [Trait("Application", "StartPreparationCommandHandler")]
    public async Task Handle_ShouldPublishEventWithCorrectOrderId_WhenPreparationStarted()
    {
        // Arrange
        var preparationId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var preparation = new Preparation(orderId);
        var command = new StartPreparationCommand(preparationId);
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
