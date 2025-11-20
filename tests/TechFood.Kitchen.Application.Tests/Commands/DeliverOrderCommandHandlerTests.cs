using TechFood.Kitchen.Application.Commands.DeliverOrder;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Kitchen.Application.Tests.Commands;

public class DeliverOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly DeliverOrderCommandHandler _handler;

    public DeliverOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new DeliverOrderCommandHandler(_orderRepositoryMock.Object);
    }

    [Fact(DisplayName = "Should deliver order successfully when order is ready")]
    [Trait("Application", "DeliverOrderCommandHandler")]
    public async Task Handle_WithReadyOrder_ShouldDeliverSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Kitchen.Domain.Entities.Order(123, Guid.NewGuid());

        // Simulate order progression to Ready status
        order.Receive();
        order.Prepare();
        order.Ready();

        var command = new DeliverOrderCommand(orderId);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        order.Status.Should().Be(OrderStatusType.Delivered);
        _orderRepositoryMock.Verify(x => x.GetByIdAsync(orderId), Times.Once);
    }

    [Fact(DisplayName = "Should throw exception when order not found")]
    [Trait("Application", "DeliverOrderCommandHandler")]
    public async Task Handle_WithNonExistentOrder_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var command = new DeliverOrderCommand(orderId);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync((Kitchen.Domain.Entities.Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Shared.Application.Exceptions.ApplicationException>(
            async () => await _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("not found");
    }

    [Fact(DisplayName = "Should throw exception when order is not in ready status")]
    [Trait("Application", "DeliverOrderCommandHandler")]
    public async Task Handle_WithNonReadyOrder_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Kitchen.Domain.Entities.Order(123, Guid.NewGuid());
        // Order is in Pending status

        var command = new DeliverOrderCommand(orderId);

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act & Assert
        await Assert.ThrowsAsync<Shared.Domain.Exceptions.DomainException>(
            async () => await _handler.Handle(command, CancellationToken.None));
    }
}
