using TechFood.Kitchen.Application.Events;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Kitchen.Application.Tests.Events;

public class UpdateOrderOnPreparationDoneHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly UpdateOrderOnPreparationDoneHandler _handler;

    public UpdateOrderOnPreparationDoneHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new UpdateOrderOnPreparationDoneHandler(_orderRepositoryMock.Object);
    }

    [Fact(DisplayName = "Should update order to ready status when preparation is done")]
    [Trait("Application", "UpdateOrderOnPreparationDoneHandler")]
    public async Task Handle_WithInPreparationOrder_ShouldUpdateStatusToReady()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Kitchen.Domain.Entities.Order(123, Guid.NewGuid());
        order.Receive();
        order.Prepare(); // Must be in preparation

        var notification = new PreparationDoneEvent(orderId);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        order.Status.Should().Be(OrderStatusType.Ready);
        _orderRepositoryMock.Verify(x => x.GetByIdAsync(orderId), Times.Once);
    }

    [Fact(DisplayName = "Should throw exception when order not found")]
    [Trait("Application", "UpdateOrderOnPreparationDoneHandler")]
    public async Task Handle_WithNonExistentOrder_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var notification = new PreparationDoneEvent(orderId);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync((Kitchen.Domain.Entities.Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Shared.Application.Exceptions.ApplicationException>(
            async () => await _handler.Handle(notification, CancellationToken.None));

        exception.Message.Should().Contain("not found");
    }

    [Fact(DisplayName = "Should throw exception when order is not in preparation status")]
    [Trait("Application", "UpdateOrderOnPreparationDoneHandler")]
    public async Task Handle_WithNonInPreparationOrder_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Kitchen.Domain.Entities.Order(123, Guid.NewGuid());
        // Order is in Pending status

        var notification = new PreparationDoneEvent(orderId);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act & Assert
        await Assert.ThrowsAsync<Shared.Domain.Exceptions.DomainException>(
            async () => await _handler.Handle(notification, CancellationToken.None));
    }
}
