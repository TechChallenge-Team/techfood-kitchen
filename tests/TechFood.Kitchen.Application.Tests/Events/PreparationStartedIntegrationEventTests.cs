using TechFood.Kitchen.Application.Events;
using TechFood.Kitchen.Application.Events.Integration.Outgoing;

namespace TechFood.Kitchen.Application.Tests.Events;

public class PreparationStartedIntegrationEventTests
{
    [Fact(DisplayName = "Should initialize with correct order id and status")]
    [Trait("Application", "PreparationStartedIntegrationEvent")]
    public void Ctor_ShouldInitializeWithCorrectOrderIdAndStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var integrationEvent = new PreparationDoneEvent(orderId);

        // Assert
        integrationEvent.OrderId.Should().Be(orderId);
    }

    [Fact(DisplayName = "Should maintain order id after initialization")]
    [Trait("Application", "PreparationStartedIntegrationEvent")]
    public void OrderId_ShouldMaintainValue_AfterInitialization()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var integrationEvent = new PreparationDoneEvent(orderId);

        // Act
        var retrievedOrderId = integrationEvent.OrderId;

        // Assert
        retrievedOrderId.Should().Be(orderId);
    }
}
