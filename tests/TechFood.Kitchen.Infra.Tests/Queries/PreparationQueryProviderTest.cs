using Microsoft.EntityFrameworkCore;
using Moq;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;
using TechFood.Kitchen.Application.Services.Interfaces;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Infra.Persistence.Contexts;
using TechFood.Kitchen.Infra.Persistence.Queries;

namespace TechFood.Tests.Kitchen.Infra.Queries;

public class PreparationQueryProviderTest
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto_WhenPreparationExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<KitchenContext>()
            .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
            .Options;

        await using var context = new KitchenContext(options);

        var order = new Order(1, DateTime.UtcNow);
        await context.Orders.AddAsync(order);

        var preparation = new Preparation(order.Id);
        await context.Preparations.AddAsync(preparation);

        await context.SaveChangesAsync();

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
                     .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, context);

        // Act
        var result = await provider.GetByIdAsync(preparation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(preparation.Id, result!.Id);
        Assert.Equal(preparation.OrderId, result.OrderId);
        Assert.Equal(preparation.Status, result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<KitchenContext>()
            .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
            .Options;

        await using var context = new KitchenContext(options);

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
                     .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, context);

        // Act
        var result = await provider.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetDailyPreparationsAsync_ShouldReturnDailyDtos_WithEmptyItems_WhenNoOrderItems()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<KitchenContext>()
            .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
            .Options;

        await using var context = new KitchenContext(options);

        var order = new Order(10, DateTime.UtcNow);
        await context.Orders.AddAsync(order);

        var preparation = new Preparation(order.Id);
        await context.Preparations.AddAsync(preparation);

        await context.SaveChangesAsync();

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
            .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, context);

        // Act
        var result = await provider.GetDailyPreparationsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var dto = result.First();

        Assert.Equal(preparation.Id, dto.Id);
        Assert.Equal(order.Id, dto.OrderId);
        Assert.Equal(order.Number, dto.Number);
        Assert.Empty(dto.Items);
    }


    [Fact]
    public async Task GetTrackingItemsAsync_ShouldReturnTrackingDtos()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<KitchenContext>()
            .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
            .Options;

        await using var context = new KitchenContext(options);

        var order = new Order(5, DateTime.UtcNow);
        await context.Orders.AddAsync(order);

        var preparation = new Preparation(order.Id);
        await context.Preparations.AddAsync(preparation);

        await context.SaveChangesAsync();

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
            .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, context);

        // Act
        var result = await provider.GetTrackingItemsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var tracking = result.First();

        Assert.Equal(preparation.Id, tracking.Id);
        Assert.Equal(order.Id, tracking.OrderId);
        Assert.Equal(order.Number, tracking.Number);
        Assert.Equal(preparation.Status, tracking.Status);
    }
}
