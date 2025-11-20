// using MediatR;
// using TechFood.Kitchen.Api.Controllers;
// using TechFood.Kitchen.Application.Queries.GetOrders;
// using TechFood.Kitchen.Application.Commands.CreateOrder;
// using TechFood.Kitchen.Application.Commands.DeliverOrder;
// using TechFood.Kitchen.Application.Dto;
// using TechFood.Kitchen.Application.Queries.GetReadyOrders;
// using TechFood.Kitchen.Contracts.Orders;
// using TechFood.Shared.Domain.Enums;
//
// namespace TechFood.Kitchen.Api.Tests.Controllers;
//
// public class OrdersControllerTests
// {
//     private readonly Mock<IMediator> _mediatorMock;
//     private readonly OrdersController _controller;
//
//     public OrdersControllerTests()
//     {
//         _mediatorMock = new Mock<IMediator>();
//         _controller = new OrdersController(_mediatorMock.Object);
//     }
//
//     [Fact(DisplayName = "CreateAsync should return Ok with created order")]
//     [Trait("Api", "OrdersController")]
//     public async Task CreateAsync_WithValidRequest_ShouldReturnOkWithOrder()
//     {
//         // Arrange
//         var customerId = Guid.NewGuid();
//         var productId = Guid.NewGuid();
//
//         var request = new CreateOrderRequest(
//             customerId,
//             new List<CreateOrderRequest.Item>
//             {
//                 new CreateOrderRequest.Item(productId, 2)
//             });
//
//         var expectedOrder = new OrderDto(
//             Guid.NewGuid(),
//             123,
//             50.00m,
//             DateTime.Now,
//             customerId,
//             OrderStatusType.Pending,
//             new List<OrderItemDto>());
//
//         _mediatorMock
//             .Setup(x => x.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(expectedOrder);
//
//         // Act
//         var result = await _controller.CreateAsync(request);
//
//         // Assert
//         result.Should().BeOfType<OkObjectResult>();
//         var okResult = result as OkObjectResult;
//         okResult!.Value.Should().Be(expectedOrder);
//
//         _mediatorMock.Verify(x => x.Send(
//             It.Is<CreateOrderCommand>(cmd =>
//                 cmd.CustomerId == customerId &&
//                 cmd.Items.Count == 1),
//             It.IsAny<CancellationToken>()),
//             Times.Once);
//     }
//
//     [Fact(DisplayName = "CreateAsync should handle order without customer")]
//     [Trait("Api", "OrdersController")]
//     public async Task CreateAsync_WithoutCustomer_ShouldReturnOkWithOrder()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//
//         var request = new CreateOrderRequest(
//             null,
//             new List<CreateOrderRequest.Item>
//             {
//                 new CreateOrderRequest.Item(productId, 1)
//             });
//
//         var expectedOrder = new OrderDto(
//             Guid.NewGuid(),
//             124,
//             15.00m,
//             DateTime.Now,
//             null,
//             OrderStatusType.Pending,
//             new List<OrderItemDto>());
//
//         _mediatorMock
//             .Setup(x => x.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(expectedOrder);
//
//         // Act
//         var result = await _controller.CreateAsync(request);
//
//         // Assert
//         result.Should().BeOfType<OkObjectResult>();
//         var okResult = result as OkObjectResult;
//         var order = okResult!.Value as OrderDto;
//         order!.CustomerId.Should().BeNull();
//     }
//
//     [Fact(DisplayName = "GetAllAsync should return Ok with list of orders")]
//     [Trait("Api", "OrdersController")]
//     public async Task GetAllAsync_ShouldReturnOkWithOrders()
//     {
//         // Arrange
//         var expectedOrders = new List<OrderDto>
//         {
//             new OrderDto(
//                 Guid.NewGuid(),
//                 123,
//                 50.00m,
//                 DateTime.Now,
//                 Guid.NewGuid(),
//                 OrderStatusType.Pending,
//                 new List<OrderItemDto>()),
//             new OrderDto(
//                 Guid.NewGuid(),
//                 124,
//                 75.00m,
//                 DateTime.Now,
//                 null,
//                 OrderStatusType.Received,
//                 new List<OrderItemDto>())
//         };
//
//         _mediatorMock
//             .Setup(x => x.Send(It.IsAny<GetOrdersQuery>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(expectedOrders);
//
//         // Act
//         var result = await _controller.GetAllAsync();
//
//         // Assert
//         result.Should().BeOfType<OkObjectResult>();
//         var okResult = result as OkObjectResult;
//         var orders = okResult!.Value as List<OrderDto>;
//         orders.Should().HaveCount(2);
//         orders.Should().BeEquivalentTo(expectedOrders);
//     }
//
//     [Fact(DisplayName = "GetReadyAsync should return Ok with ready orders only")]
//     [Trait("Api", "OrdersController")]
//     public async Task GetReadyAsync_ShouldReturnOkWithReadyOrders()
//     {
//         // Arrange
//         var expectedOrders = new List<OrderDto>
//         {
//             new OrderDto(
//                 Guid.NewGuid(),
//                 123,
//                 50.00m,
//                 DateTime.Now,
//                 Guid.NewGuid(),
//                 OrderStatusType.Ready,
//                 new List<OrderItemDto>()),
//             new OrderDto(
//                 Guid.NewGuid(),
//                 124,
//                 75.00m,
//                 DateTime.Now,
//                 Guid.NewGuid(),
//                 OrderStatusType.Ready,
//                 new List<OrderItemDto>())
//         };
//
//         _mediatorMock
//             .Setup(x => x.Send(It.IsAny<GetReadyOrdersQuery>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(expectedOrders);
//
//         // Act
//         var result = await _controller.GetReadyAsync();
//
//         // Assert
//         result.Should().BeOfType<OkObjectResult>();
//         var okResult = result as OkObjectResult;
//         var orders = okResult!.Value as List<OrderDto>;
//         orders.Should().HaveCount(2);
//         orders.Should().OnlyContain(o => o.Status == OrderStatusType.Ready);
//     }
//
//     [Fact(DisplayName = "DeliverAsync should return Ok when order is delivered")]
//     [Trait("Api", "OrdersController")]
//     public async Task DeliverAsync_WithValidOrderId_ShouldReturnOk()
//     {
//         // Arrange
//         var orderId = Guid.NewGuid();
//
//         _mediatorMock
//             .Setup(x => x.Send(It.IsAny<DeliverOrderCommand>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(Unit.Value);
//
//         // Act
//         var result = await _controller.DeliverAsync(orderId);
//
//         // Assert
//         result.Should().BeOfType<OkResult>();
//
//         _mediatorMock.Verify(x => x.Send(
//             It.Is<DeliverOrderCommand>(cmd => cmd.Id == orderId),
//             It.IsAny<CancellationToken>()),
//             Times.Once);
//     }
//
//     [Fact(DisplayName = "GetAllAsync should return empty list when no orders")]
//     [Trait("Api", "OrdersController")]
//     public async Task GetAllAsync_WithNoOrders_ShouldReturnEmptyList()
//     {
//         // Arrange
//         _mediatorMock
//             .Setup(x => x.Send(It.IsAny<GetOrdersQuery>(), It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new List<OrderDto>());
//
//         // Act
//         var result = await _controller.GetAllAsync();
//
//         // Assert
//         result.Should().BeOfType<OkObjectResult>();
//         var okResult = result as OkObjectResult;
//         var orders = okResult!.Value as List<OrderDto>;
//         orders.Should().BeEmpty();
//     }
// }
