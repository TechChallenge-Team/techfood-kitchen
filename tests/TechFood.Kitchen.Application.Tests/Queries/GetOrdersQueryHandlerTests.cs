using TechFood.Kitchen.Application.Queries;
using TechFood.Kitchen.Application.Queries.GetOrders;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries;

namespace TechFood.Kitchen.Application.Tests.Queries;

public class GetOrdersQueryHandlerTests
{
    private readonly Mock<IOrderQueryProvider> _queryProviderMock;
    private readonly GetOrdersQueryHandler _handler;

    public GetOrdersQueryHandlerTests()
    {
        _queryProviderMock = new Mock<IOrderQueryProvider>();
        _handler = new GetOrdersQueryHandler(_queryProviderMock.Object);
    }

    [Fact(DisplayName = "Should return all orders")]
    [Trait("Application", "GetOrdersQueryHandler")]
    public async Task Handle_ShouldReturnAllOrders()
    {
        // Arrange
        var expectedOrders = new List<OrderDto>
        {
            new OrderDto(
                Guid.NewGuid(),
                123,
                50.00m,
                DateTime.Now,
                Guid.NewGuid(),
                Shared.Domain.Enums.OrderStatusType.Pending,
                new List<OrderItemDto>()),
            new OrderDto(
                Guid.NewGuid(),
                124,
                75.00m,
                DateTime.Now,
                null,
                Shared.Domain.Enums.OrderStatusType.Received,
                new List<OrderItemDto>())
        };

        _queryProviderMock
            .Setup(x => x.GetOrdersAsync())
            .ReturnsAsync(expectedOrders);

        var query = new GetOrdersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedOrders);
        _queryProviderMock.Verify(x => x.GetOrdersAsync(), Times.Once);
    }

    [Fact(DisplayName = "Should return empty list when no orders exist")]
    [Trait("Application", "GetOrdersQueryHandler")]
    public async Task Handle_WithNoOrders_ShouldReturnEmptyList()
    {
        // Arrange
        var expectedOrders = new List<OrderDto>();

        _queryProviderMock
            .Setup(x => x.GetOrdersAsync())
            .ReturnsAsync(expectedOrders);

        var query = new GetOrdersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
