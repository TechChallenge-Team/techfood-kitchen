using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;

namespace TechFood.Kitchen.Application.Tests.Queries.GetDailyPreparations;

public class GetDailyPreparationsQueryHandlerTests
{
    [Fact]
    public async Task Handle_calls_provider_and_returns_result()
    {
        // Arrange
        var expected = new List<DailyPreparationDto>
        {
        };

        var queries = new Mock<IPreparationQueryProvider>(MockBehavior.Strict);
        queries.Setup(q => q.GetDailyPreparationsAsync())
               .ReturnsAsync(expected);

        var handler = new GetDailyPreparationsQueryHandler(queries.Object);

        // Act
        var result = await handler.Handle(new GetDailyPreparationsQuery(), CancellationToken.None);

        // Assert
        Assert.Same(expected, result);
        queries.Verify(q => q.GetDailyPreparationsAsync(), Times.Once);
        queries.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_propagates_exception_from_provider()
    {
        // Arrange
        var queries = new Mock<IPreparationQueryProvider>(MockBehavior.Strict);
        queries.Setup(q => q.GetDailyPreparationsAsync())
               .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new GetDailyPreparationsQueryHandler(queries.Object);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new GetDailyPreparationsQuery(), CancellationToken.None));

        queries.Verify(q => q.GetDailyPreparationsAsync(), Times.Once);
        queries.VerifyNoOtherCalls();
    }
}
