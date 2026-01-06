using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Infra.Persistence.Contexts;
using TechFood.Kitchen.Infra.Persistence.Repositories;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.Tests.Kitchen.Infra.Repositories
{
    public class PreparationRepositoryTest
    {
        private readonly KitchenContext _context;

        public PreparationRepositoryTest()
        {
            // IOptions of InfraOptions is not needed for in-memory tests
            var infraOptions = Options.Create(new InfraOptions());

            var options = new DbContextOptionsBuilder<KitchenContext>()
                .UseInMemoryDatabase($"db_{Guid.NewGuid()}")
                .Options;

            _context = new KitchenContext(infraOptions, options);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnId_AndPersistPreparation()
        {
            // Arrange
            var repository = new PreparationRepository(_context);

            var preparationId = Guid.NewGuid();

            await _context.SaveChangesAsync();

            var preparation = new Preparation(preparationId);

            // Act
            var returnedId = await repository.AddAsync(preparation);
            await _context.SaveChangesAsync();

            // Assert
            Assert.Equal(preparation.Id, returnedId);

            var persisted = await _context.Preparations.FirstOrDefaultAsync(p => p.Id == preparation.Id);
            Assert.NotNull(persisted);
            Assert.Equal(preparation.OrderId, persisted!.OrderId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPreparation_WhenExists()
        {
            // Arrange
            var preparationId = Guid.NewGuid();

            var preparation = new Preparation(preparationId);
            await _context.Preparations.AddAsync(preparation);
            await _context.SaveChangesAsync();

            var repository = new PreparationRepository(_context);

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
            var preparationId = Guid.NewGuid();
            var preparation = new Preparation(preparationId);
            await _context.Preparations.AddAsync(preparation);
            await _context.SaveChangesAsync();

            var repository = new PreparationRepository(_context);

            // Act
            var found = await repository.GetByOrderIdAsync(preparationId);

            // Assert
            Assert.NotNull(found);
            Assert.Equal(preparation.Id, found!.Id);
        }
    }
}
