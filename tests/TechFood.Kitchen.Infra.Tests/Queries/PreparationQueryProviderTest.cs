using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Services.Interfaces;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Infra.Persistence.Contexts;
using TechFood.Kitchen.Infra.Persistence.Queries;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.Tests.Kitchen.Infra.Queries;

public class PreparationQueryProviderTest
{
    private readonly KitchenContext _context;

    public PreparationQueryProviderTest()
    {
        // IOptions of InfraOptions is not needed for in-memory tests
        var infraOptions = Options.Create(new InfraOptions());

        var options = new DbContextOptionsBuilder<KitchenContext>()
            .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
            .Options;

        _context = new KitchenContext(infraOptions, options);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto_WhenPreparationExists()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        var preparation = new Preparation(preparationId);
        await _context.Preparations.AddAsync(preparation);

        await _context.SaveChangesAsync();

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
                     .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, _context);

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
        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
                     .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, _context);

        // Act
        var result = await provider.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetDailyPreparationsAsync_ShouldReturnDailyDtos_WithEmptyItems_WhenNoOrderItems()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        var preparation = new Preparation(preparationId);
        await _context.Preparations.AddAsync(preparation);

        await _context.SaveChangesAsync();

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
            .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, _context);

        // Act
        var result = await provider.GetDailyPreparationsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var dto = result.First();

        Assert.Equal(preparation.Id, dto.Id);
        Assert.Equal(preparationId, dto.OrderId);
    }


    [Fact]
    public async Task GetTrackingItemsAsync_ShouldReturnTrackingDtos()
    {
        // Arrange
        var preparationId = Guid.NewGuid();

        var preparation = new Preparation(preparationId);
        await _context.Preparations.AddAsync(preparation);

        await _context.SaveChangesAsync();

        var backofficeMock = new Mock<IBackofficeService>();
        backofficeMock.Setup(x => x.GetProductsAsync(default))
            .ReturnsAsync(Array.Empty<ProductDto>());

        var provider = new PreparationQueryProvider(backofficeMock.Object, _context);

        // Act
        var result = await provider.GetTrackingItemsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var tracking = result.First();

        Assert.Equal(preparation.Id, tracking.Id);
        Assert.Equal(preparationId, tracking.OrderId);
        Assert.Equal(preparation.Status, tracking.Status);
    }
}
