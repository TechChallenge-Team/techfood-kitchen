using TechFood.Kitchen.Application.Events;
using TechFood.Shared.Domain.Enums;

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
        var integrationEvent = new PreparationStartedIntegrationEvent(orderId);

        // Assert
        integrationEvent.OrderId.Should().Be(orderId);
        integrationEvent.OrderStatus.Should().Be(OrderStatusType.InPreparation);
    }

    [Fact(DisplayName = "Should set order status to InPreparation")]
    [Trait("Application", "PreparationStartedIntegrationEvent")]
    public void Ctor_ShouldSetOrderStatusToInPreparation()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var integrationEvent = new PreparationStartedIntegrationEvent(orderId);

        // Assert
        integrationEvent.OrderStatus.Should().Be(OrderStatusType.InPreparation);
    }

    [Fact(DisplayName = "Should maintain order id after initialization")]
    [Trait("Application", "PreparationStartedIntegrationEvent")]
    public void OrderId_ShouldMaintainValue_AfterInitialization()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var integrationEvent = new PreparationStartedIntegrationEvent(orderId);

        // Act
        var retrievedOrderId = integrationEvent.OrderId;

        // Assert
        retrievedOrderId.Should().Be(orderId);
    }
}
