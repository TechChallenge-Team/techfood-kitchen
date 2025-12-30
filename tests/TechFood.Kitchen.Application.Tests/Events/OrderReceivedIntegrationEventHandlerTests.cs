using TechFood.Kitchen.Application.Events;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Domain.Repositories;

namespace TechFood.Kitchen.Application.Tests.Events;

public class OrderReceivedIntegrationEventHandlerTests
{
    private readonly Mock<IPreparationRepository> _mockRepository;
    private readonly OrderReceivedIntegrationEventHandler _handler;

    public OrderReceivedIntegrationEventHandlerTests()
    {
        _mockRepository = new Mock<IPreparationRepository>();
        _handler = new OrderReceivedIntegrationEventHandler(_mockRepository.Object);
    }

    [Fact(DisplayName = "Should create preparation when order received event is handled")]
    [Trait("Application", "OrderReceivedIntegrationEventHandler")]
    public async Task Handle_ShouldCreatePreparation_WhenOrderReceivedEventIsHandled()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var integrationEvent = new OrderReceivedIntegrationEvent(orderId);
        var cancellationToken = CancellationToken.None;

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Preparation>()))
            .ReturnsAsync(Guid.NewGuid());

        // Act
        await _handler.Handle(integrationEvent, cancellationToken);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.Is<Preparation>(p => 
            p.OrderId == orderId)), Times.Once);
    }

    [Fact(DisplayName = "Should pass correct order id to preparation")]
    [Trait("Application", "OrderReceivedIntegrationEventHandler")]
    public async Task Handle_ShouldPassCorrectOrderId_WhenCreatingPreparation()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var integrationEvent = new OrderReceivedIntegrationEvent(orderId);
        var cancellationToken = CancellationToken.None;
        Preparation? capturedPreparation = null;

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Preparation>()))
            .Callback<Preparation>(p => capturedPreparation = p)
            .ReturnsAsync(Guid.NewGuid());

        // Act
        await _handler.Handle(integrationEvent, cancellationToken);

        // Assert
        capturedPreparation.Should().NotBeNull();
        capturedPreparation!.OrderId.Should().Be(orderId);
    }
}
