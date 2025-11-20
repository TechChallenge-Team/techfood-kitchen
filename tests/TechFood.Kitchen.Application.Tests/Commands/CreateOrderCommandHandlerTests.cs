using TechFood.Kitchen.Application.Commands.CreateOrder;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Services.Interfaces;
using TechFood.Kitchen.Domain.Repositories;

namespace TechFood.Kitchen.Application.Tests.Commands;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IBackofficeService> _backofficeServiceMock;
    private readonly Mock<IOrderNumberService> _orderNumberServiceMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _backofficeServiceMock = new Mock<IBackofficeService>();
        _orderNumberServiceMock = new Mock<IOrderNumberService>();

        _handler = new CreateOrderCommandHandler(
            _orderRepositoryMock.Object,
            _backofficeServiceMock.Object,
            _orderNumberServiceMock.Object);
    }

    [Fact(DisplayName = "Should create order successfully with valid items")]
    [Trait("Application", "CreateOrderCommandHandler")]
    public async Task Handle_WithValidItems_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var orderNumber = 123;
        var productPrice = 10.50m;
        var orderId = Guid.NewGuid();

        var products = new List<ProductDto>
        {
            new ProductDto(productId, "Pizza Margherita", "image.jpg", productPrice)
        };

        var command = new CreateOrderCommand(
            customerId,
            new List<CreateOrderCommand.Item>
            {
                new CreateOrderCommand.Item(productId, 2)
            });

        _orderNumberServiceMock
            .Setup(x => x.GetAsync())
            .ReturnsAsync(orderNumber);

        _backofficeServiceMock
            .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _orderRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Kitchen.Domain.Entities.Order>()))
            .ReturnsAsync(orderId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Number.Should().Be(orderNumber);
        result.CustomerId.Should().Be(customerId);
        result.Items.Should().HaveCount(1);
        result.Items[0].Quantity.Should().Be(2);
        result.Items[0].Price.Should().Be(productPrice);
        result.Amount.Should().Be(productPrice * 2);

        _orderRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Kitchen.Domain.Entities.Order>()), Times.Once);
    }

    [Fact(DisplayName = "Should create order without customer")]
    [Trait("Application", "CreateOrderCommandHandler")]
    public async Task Handle_WithoutCustomerId_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var orderNumber = 456;

        var products = new List<ProductDto>
        {
            new ProductDto(productId, "Spaghetti", "image.jpg", 15.00m)
        };

        var command = new CreateOrderCommand(
            null,
            new List<CreateOrderCommand.Item>
            {
                new CreateOrderCommand.Item(productId, 1)
            });

        _orderNumberServiceMock
            .Setup(x => x.GetAsync())
            .ReturnsAsync(orderNumber);

        _backofficeServiceMock
            .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CustomerId.Should().BeNull();
        result.Items.Should().HaveCount(1);
    }

    [Fact(DisplayName = "Should create order with multiple items")]
    [Trait("Application", "CreateOrderCommandHandler")]
    public async Task Handle_WithMultipleItems_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();
        var orderNumber = 789;

        var products = new List<ProductDto>
        {
            new ProductDto(product1Id, "Pizza", "image1.jpg", 20.00m),
            new ProductDto(product2Id, "Salad", "image2.jpg", 10.00m)
        };

        var command = new CreateOrderCommand(
            customerId,
            new List<CreateOrderCommand.Item>
            {
                new CreateOrderCommand.Item(product1Id, 2),
                new CreateOrderCommand.Item(product2Id, 3)
            });

        _orderNumberServiceMock
            .Setup(x => x.GetAsync())
            .ReturnsAsync(orderNumber);

        _backofficeServiceMock
            .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Amount.Should().Be((20.00m * 2) + (10.00m * 3));
    }

    [Fact(DisplayName = "Should throw exception when product not found")]
    [Trait("Application", "CreateOrderCommandHandler")]
    public async Task Handle_WithInvalidProductId_ShouldThrowException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var invalidProductId = Guid.NewGuid();

        var products = new List<ProductDto>
        {
            new ProductDto(productId, "Pizza", "image.jpg", 20.00m)
        };

        var command = new CreateOrderCommand(
            customerId,
            new List<CreateOrderCommand.Item>
            {
                new CreateOrderCommand.Item(invalidProductId, 1)
            });

        _orderNumberServiceMock
            .Setup(x => x.GetAsync())
            .ReturnsAsync(123);

        _backofficeServiceMock
            .Setup(x => x.GetProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _handler.Handle(command, CancellationToken.None));
    }
}
