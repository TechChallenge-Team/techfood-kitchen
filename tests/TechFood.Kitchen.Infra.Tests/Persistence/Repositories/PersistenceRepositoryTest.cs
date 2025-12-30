using Microsoft.EntityFrameworkCore;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Infra.Persistence.Contexts;
using TechFood.Kitchen.Infra.Persistence.Repositories;

namespace TechFood.Tests.Kitchen.Infra.Repositories
{
    public class PreparationRepositoryTest
    {
        [Fact]
        public async Task AddAsync_ShouldReturnId_AndPersistPreparation()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<KitchenContext>()
                .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
                .Options;

            await using var context = new KitchenContext(options);

            var repository = new PreparationRepository(context);

            var preparationId = Guid.NewGuid();

            await context.SaveChangesAsync();

            var preparation = new Preparation(preparationId);

            // Act
            var returnedId = await repository.AddAsync(preparation);
            await context.SaveChangesAsync();

            // Assert
            Assert.Equal(preparation.Id, returnedId);

            var persisted = await context.Preparations.FirstOrDefaultAsync(p => p.Id == preparation.Id);
            Assert.NotNull(persisted);
            Assert.Equal(preparation.OrderId, persisted!.OrderId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPreparation_WhenExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<KitchenContext>()
                .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
                .Options;

            await using var context = new KitchenContext(options);

            var preparationId = Guid.NewGuid();

            var preparation = new Preparation(preparationId);
            await context.Preparations.AddAsync(preparation);
            await context.SaveChangesAsync();

            var repository = new PreparationRepository(context);

            // Act
            var found = await repository.GetByIdAsync(preparation.Id);

            // Assert
            Assert.NotNull(found);
            Assert.Equal(preparation.Id, found!.Id);
        }

        [Fact]
        public async Task GetByOrderIdAsync_ShouldReturnPreparation_WhenExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<KitchenContext>()
                .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
                .Options;

            await using var context = new KitchenContext(options);

            var preparationId = Guid.NewGuid();
            var preparation = new Preparation(preparationId);
            await context.Preparations.AddAsync(preparation);
            await context.SaveChangesAsync();

            var repository = new PreparationRepository(context);

            // Act
            var found = await repository.GetByOrderIdAsync(preparationId);

            // Assert
            Assert.NotNull(found);
            Assert.Equal(preparation.Id, found!.Id);
        }
    }
}
