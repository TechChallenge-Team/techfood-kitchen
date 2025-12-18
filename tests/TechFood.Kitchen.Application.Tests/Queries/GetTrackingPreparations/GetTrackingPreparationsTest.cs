using TechFood.Application.Preparations.Dto;
using TechFood.Domain.Enums;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;
using TechFood.Kitchen.Application.Queries.GetTrackingPreparations;

namespace TechFood.Tests.Kitchen.Application.Queries.GetTrackingPreparations
{
    public class GetTrackingPreparationsQueryHandlerTests
    {
        private readonly Mock<IPreparationQueryProvider> _mockQueries;
        private readonly GetTrackingPreparationsQueryHandler _handler;

        public GetTrackingPreparationsQueryHandlerTests()
        {
            _mockQueries = new Mock<IPreparationQueryProvider>();
            _handler = new GetTrackingPreparationsQueryHandler(_mockQueries.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrackingPreparationDtos_WhenCalled()
        {
            // Arrange
            var mockTrackingPreparations = new List<TrackingPreparationDto>
            {
                new TrackingPreparationDto(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    1,
                    PreparationStatusType.Started
                ),
                new TrackingPreparationDto(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    2,
                    PreparationStatusType.Ready
                )
            };

            _mockQueries.Setup(q => q.GetTrackingItemsAsync())
                        .ReturnsAsync(mockTrackingPreparations);

            var query = new GetTrackingPreparationsQuery();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mockTrackingPreparations.Count, result.Count);
            Assert.Equal(mockTrackingPreparations, result);

            _mockQueries.Verify(q => q.GetTrackingItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTrackingPreparationsFound()
        {
            // Arrange
            var emptyList = new List<TrackingPreparationDto>();
            _mockQueries.Setup(q => q.GetTrackingItemsAsync())
                        .ReturnsAsync(emptyList);

            var query = new GetTrackingPreparationsQuery();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockQueries.Verify(q => q.GetTrackingItemsAsync(), Times.Once);
        }
    }
}
