using System;
using TechFood.Domain.Enums;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Shared.Domain.Exceptions;
using Xunit;

namespace TechFood.Tests.Kitchen.Domain.Entities
{
    public class PreparationTests
    {
        [Fact]
        public void Ctor_ShouldInitializeWithPendingStatusAndCreatedAt()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            // Act
            var preparation = new Preparation(orderId);

            // Assert
            Assert.Equal(orderId, preparation.OrderId);
            Assert.Equal(PreparationStatusType.Pending, preparation.Status);
            Assert.True(preparation.CreatedAt <= DateTime.Now);
            Assert.Null(preparation.StartedAt);
            Assert.Null(preparation.ReadyAt);
            Assert.Null(preparation.CancelledAt);
        }

        [Fact]
        public void Start_ShouldSetStatusStartedAndStartedAt_WhenPending()
        {
            // Arrange
            var preparation = new Preparation(Guid.NewGuid());

            // Act
            preparation.Start();

            // Assert
            Assert.Equal(PreparationStatusType.Started, preparation.Status);
            Assert.NotNull(preparation.StartedAt);
            Assert.True(preparation.StartedAt <= DateTime.Now);
        }

        [Fact]
        public void Start_ShouldThrowDomainException_WhenNotPending()
        {
            // Arrange
            var preparation = new Preparation(Guid.NewGuid());
            preparation.Start();

            // Act / Assert
            Assert.Throws<DomainException>(preparation.Start);
        }

        [Fact]
        public void Ready_ShouldSetStatusReadyAndReadyAt_WhenStarted()
        {
            // Arrange
            var preparation = new Preparation(Guid.NewGuid());
            preparation.Start();

            // Act
            preparation.Ready();

            // Assert
            Assert.Equal(PreparationStatusType.Ready, preparation.Status);
            Assert.NotNull(preparation.ReadyAt);
            Assert.True(preparation.ReadyAt <= DateTime.Now);
        }

        [Fact]
        public void Ready_ShouldThrowDomainException_WhenNotStarted()
        {
            // Arrange
            var preparation = new Preparation(Guid.NewGuid());

            // Act / Assert
            Assert.Throws<DomainException>(preparation.Ready);
        }

        [Fact]
        public void Cancel_ShouldSetStatusCancelledAndCancelledAt()
        {
            // Arrange
            var preparation = new Preparation(Guid.NewGuid());

            // Act
            preparation.Cancel();

            // Assert
            Assert.Equal(PreparationStatusType.Cancelled, preparation.Status);
            Assert.NotNull(preparation.CancelledAt);
            Assert.True(preparation.CancelledAt <= DateTime.Now);
        }

        [Fact]
        public void Cancel_ShouldThrowInvalidOperationException_WhenAlreadyCancelled()
        {
            // Arrange
            var preparation = new Preparation(Guid.NewGuid());
            preparation.Cancel();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(preparation.Cancel);
        }
    }
}
