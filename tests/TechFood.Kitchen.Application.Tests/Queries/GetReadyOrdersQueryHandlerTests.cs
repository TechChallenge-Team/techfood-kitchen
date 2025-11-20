using TechFood.Kitchen.Application.Queries;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries;
using TechFood.Kitchen.Application.Queries.GetReadyOrders;

namespace TechFood.Kitchen.Application.Tests.Queries;

public class GetReadyOrdersQueryHandlerTests
{
    private readonly Mock<IOrderQueryProvider> _queryProviderMock;
    private readonly GetReadyOrdersQueryHandler _handler;

    public GetReadyOrdersQueryHandlerTests()
    {
        _queryProviderMock = new Mock<IOrderQueryProvider>();
        _handler = new GetReadyOrdersQueryHandler(_queryProviderMock.Object);
    }

    [Fact(DisplayName = "Should return only ready orders")]
    [Trait("Application", "GetReadyOrdersQueryHandler")]
    public async Task Handle_ShouldReturnOnlyReadyOrders()
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
                Shared.Domain.Enums.OrderStatusType.Ready,
                new List<OrderItemDto>()),
            new OrderDto(
                Guid.NewGuid(),
                124,
                75.00m,
                DateTime.Now,
                null,
                Shared.Domain.Enums.OrderStatusType.Ready,
                new List<OrderItemDto>())
        };

        _queryProviderMock
            .Setup(x => x.GetReadyOrdersAsync())
            .ReturnsAsync(expectedOrders);

        var query = new GetReadyOrdersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(o => o.Status == Shared.Domain.Enums.OrderStatusType.Ready);
        result.Should().BeEquivalentTo(expectedOrders);
        _queryProviderMock.Verify(x => x.GetReadyOrdersAsync(), Times.Once);
    }

    [Fact(DisplayName = "Should return empty list when no ready orders exist")]
    [Trait("Application", "GetReadyOrdersQueryHandler")]
    public async Task Handle_WithNoReadyOrders_ShouldReturnEmptyList()
    {
        // Arrange
        var expectedOrders = new List<OrderDto>();

        _queryProviderMock
            .Setup(x => x.GetReadyOrdersAsync())
            .ReturnsAsync(expectedOrders);

        var query = new GetReadyOrdersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
