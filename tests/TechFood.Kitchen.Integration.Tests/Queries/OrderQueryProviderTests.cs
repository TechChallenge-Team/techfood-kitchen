// using Bogus;
// using Microsoft.EntityFrameworkCore;
// using TechFood.Kitchen.Application.Dto;
// using TechFood.Kitchen.Application.Services.Interfaces;
// using TechFood.Kitchen.Infra.Persistence.Contexts;
// using TechFood.Kitchen.Infra.Persistence.Queries;
// using TechFood.Shared.Domain.Enums;
//
// namespace TechFood.Kitchen.Integration.Tests.Queries;
//
// public class OrderQueryProviderTests : IDisposable
// {
//     private readonly OrderContext _context;
//     private readonly Mock<IBackofficeService> _backofficeServiceMock;
//     private readonly OrderQueryProvider _queryProvider;
//     private readonly Faker _faker;
//
//     public OrderQueryProviderTests()
//     {
//         var options = new DbContextOptionsBuilder<OrderContext>()
//             .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
//             .Options;
//
//         _context = new OrderContext(options);
//         _backofficeServiceMock = new Mock<IBackofficeService>();
//         _queryProvider = new OrderQueryProvider(_backofficeServiceMock.Object, _context);
//         _faker = new Faker();
//     }
//
//     [Fact(DisplayName = "Should return all orders with items")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetOrdersAsync_ShouldReturnAllOrders_WithItems()
//     {
//         // Arrange
//         var productId1 = Guid.NewGuid();
//         var productId2 = Guid.NewGuid();
//
//         var products = new List<ProductDto>
//         {
//             new ProductDto(productId1, "Product 1", "image1.jpg", 10.00m),
//             new ProductDto(productId2, "Product 2", "image2.jpg", 20.00m)
//         };
//
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(products);
//
//         var order1 = new Domain.Entities.Order(1, Guid.NewGuid());
//         order1.AddItem(new Domain.Entities.OrderItem(productId1, 10.00m, 2));
//         
//         var order2 = new Domain.Entities.Order(2, null);
//         order2.AddItem(new Domain.Entities.OrderItem(productId2, 20.00m, 1));
//
//         await _context.Orders.AddRangeAsync(order1, order2);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetOrdersAsync();
//
//         // Assert
//         result.Should().NotBeNull();
//         result.Should().HaveCount(2);
//         
//         var firstOrder = result.First();
//         firstOrder.Number.Should().Be(1);
//         firstOrder.Items.Should().HaveCount(1);
//         firstOrder.Items.First().Name.Should().Be("Product 1");
//         firstOrder.Items.First().ImageUrl.Should().Be("image1.jpg");
//         firstOrder.Items.First().Quantity.Should().Be(2);
//
//         var secondOrder = result.Last();
//         secondOrder.Number.Should().Be(2);
//         secondOrder.Items.Should().HaveCount(1);
//         secondOrder.Items.First().Name.Should().Be("Product 2");
//     }
//
//     [Fact(DisplayName = "Should return orders ordered by creation date")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetOrdersAsync_ShouldReturnOrders_OrderedByCreatedAt()
//     {
//         // Arrange
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new List<ProductDto>());
//
//         var order1 = new Domain.Entities.Order(1, Guid.NewGuid());
//         var order2 = new Domain.Entities.Order(2, Guid.NewGuid());
//         var order3 = new Domain.Entities.Order(3, Guid.NewGuid());
//
//         await _context.Orders.AddRangeAsync(order3, order1, order2);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetOrdersAsync();
//
//         // Assert
//         result.Should().HaveCount(3);
//         result.Select(o => o.Number).Should().BeInAscendingOrder();
//     }
//
//     [Fact(DisplayName = "Should return empty list when no orders exist")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetOrdersAsync_ShouldReturnEmptyList_WhenNoOrdersExist()
//     {
//         // Arrange
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new List<ProductDto>());
//
//         // Act
//         var result = await _queryProvider.GetOrdersAsync();
//
//         // Assert
//         result.Should().NotBeNull();
//         result.Should().BeEmpty();
//     }
//
//     [Fact(DisplayName = "Should return only ready orders")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetReadyOrdersAsync_ShouldReturnOnlyReadyOrders()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//         var products = new List<ProductDto>
//         {
//             new ProductDto(productId, "Product 1", "image1.jpg", 10.00m)
//         };
//
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(products);
//
//         var pendingOrder = new Domain.Entities.Order(1, Guid.NewGuid());
//         pendingOrder.AddItem(new Domain.Entities.OrderItem(productId, 10.00m, 1));
//
//         var receivedOrder = new Domain.Entities.Order(2, Guid.NewGuid());
//         receivedOrder.AddItem(new Domain.Entities.OrderItem(productId, 10.00m, 1));
//         receivedOrder.Receive();
//
//         var preparingOrder = new Domain.Entities.Order(3, Guid.NewGuid());
//         preparingOrder.AddItem(new Domain.Entities.OrderItem(productId, 10.00m, 1));
//         preparingOrder.Receive();
//         preparingOrder.Prepare();
//
//         var readyOrder1 = new Domain.Entities.Order(4, Guid.NewGuid());
//         readyOrder1.AddItem(new Domain.Entities.OrderItem(productId, 10.00m, 1));
//         readyOrder1.Receive();
//         readyOrder1.Prepare();
//         readyOrder1.Ready();
//
//         var readyOrder2 = new Domain.Entities.Order(5, Guid.NewGuid());
//         readyOrder2.AddItem(new Domain.Entities.OrderItem(productId, 10.00m, 1));
//         readyOrder2.Receive();
//         readyOrder2.Prepare();
//         readyOrder2.Ready();
//
//         await _context.Orders.AddRangeAsync(
//             pendingOrder, 
//             receivedOrder, 
//             preparingOrder, 
//             readyOrder1, 
//             readyOrder2);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetReadyOrdersAsync();
//
//         // Assert
//         result.Should().NotBeNull();
//         result.Should().HaveCount(2);
//         result.Should().OnlyContain(o => o.Status == OrderStatusType.Ready);
//         result.Select(o => o.Number).Should().Contain(new[] { 4, 5 });
//     }
//
//     [Fact(DisplayName = "Should return ready orders ordered by creation date")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetReadyOrdersAsync_ShouldReturnOrders_OrderedByCreatedAt()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//         var products = new List<ProductDto>
//         {
//             new ProductDto(productId, "Product", "image.jpg", 10.00m)
//         };
//
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(products);
//
//         var order1 = CreateReadyOrder(1, productId);
//         var order2 = CreateReadyOrder(2, productId);
//         var order3 = CreateReadyOrder(3, productId);
//
//         await _context.Orders.AddRangeAsync(order3, order1, order2);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetReadyOrdersAsync();
//
//         // Assert
//         result.Should().HaveCount(3);
//         result.Select(o => o.Number).Should().BeInAscendingOrder();
//     }
//
//     [Fact(DisplayName = "Should return empty list when no ready orders exist")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetReadyOrdersAsync_ShouldReturnEmptyList_WhenNoReadyOrdersExist()
//     {
//         // Arrange
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(new List<ProductDto>());
//
//         var pendingOrder = new Domain.Entities.Order(1, Guid.NewGuid());
//         await _context.Orders.AddAsync(pendingOrder);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetReadyOrdersAsync();
//
//         // Assert
//         result.Should().NotBeNull();
//         result.Should().BeEmpty();
//     }
//
//     [Fact(DisplayName = "Should include product information from backoffice service")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetOrdersAsync_ShouldIncludeProductInformation_FromBackofficeService()
//     {
//         // Arrange
//         var productId = Guid.NewGuid();
//         var expectedProductName = "Test Product";
//         var expectedImageUrl = "test-image.jpg";
//         var expectedPrice = 25.00m;
//
//         var products = new List<ProductDto>
//         {
//             new ProductDto(productId, expectedProductName, expectedImageUrl, expectedPrice)
//         };
//
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(products);
//
//         var order = new Domain.Entities.Order(1, Guid.NewGuid());
//         order.AddItem(new Domain.Entities.OrderItem(productId, expectedPrice, 3));
//
//         await _context.Orders.AddAsync(order);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetOrdersAsync();
//
//         // Assert
//         var orderDto = result.Single();
//         var itemDto = orderDto.Items.Single();
//         
//         itemDto.Name.Should().Be(expectedProductName);
//         itemDto.ImageUrl.Should().Be(expectedImageUrl);
//         itemDto.Price.Should().Be(expectedPrice);
//         itemDto.Quantity.Should().Be(3);
//     }
//
//     [Fact(DisplayName = "Should handle orders with multiple items")]
//     [Trait("Integration", "OrderQueryProvider")]
//     public async Task GetOrdersAsync_ShouldHandleOrders_WithMultipleItems()
//     {
//         // Arrange
//         var productId1 = Guid.NewGuid();
//         var productId2 = Guid.NewGuid();
//         var productId3 = Guid.NewGuid();
//
//         var products = new List<ProductDto>
//         {
//             new ProductDto(productId1, "Product 1", "image1.jpg", 10.00m),
//             new ProductDto(productId2, "Product 2", "image2.jpg", 15.00m),
//             new ProductDto(productId3, "Product 3", "image3.jpg", 20.00m)
//         };
//
//         _backofficeServiceMock
//             .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
//             .ReturnsAsync(products);
//
//         var order = new Domain.Entities.Order(1, Guid.NewGuid());
//         order.AddItem(new Domain.Entities.OrderItem(productId1, 10.00m, 2));
//         order.AddItem(new Domain.Entities.OrderItem(productId2, 15.00m, 1));
//         order.AddItem(new Domain.Entities.OrderItem(productId3, 20.00m, 3));
//
//         await _context.Orders.AddAsync(order);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _queryProvider.GetOrdersAsync();
//
//         // Assert
//         var orderDto = result.Single();
//         orderDto.Items.Should().HaveCount(3);
//         orderDto.Items.Select(i => i.Name).Should().Contain(new[] { "Product 1", "Product 2", "Product 3" });
//     }
//
//     private static Domain.Entities.Order CreateReadyOrder(int number, Guid productId)
//     {
//         var order = new Domain.Entities.Order(number, Guid.NewGuid());
//         order.AddItem(new Domain.Entities.OrderItem(productId, 10.00m, 1));
//         order.Receive();
//         order.Prepare();
//         order.Ready();
//         return order;
//     }
//
//     public void Dispose()
//     {
//         _context.Database.EnsureDeleted();
//         _context.Dispose();
//         GC.SuppressFinalize(this);
//     }
// }
