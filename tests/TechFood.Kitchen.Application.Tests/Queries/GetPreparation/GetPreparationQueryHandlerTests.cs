using TechFood.Application.Preparations.Dto;
using TechFood.Domain.Enums;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;
using TechFood.Kitchen.Application.Queries.GetPreparation;

namespace TechFood.Tests.Kitchen.Application.Queries.GetPreparation;

public class GetPreparationQueryHandlerTests
{
    [Fact]
    public async Task Handle_calls_provider_with_request_id_and_returns_result()
    {
        var id = Guid.NewGuid();
        var request = new GetPreparationQuery(id);

        var expected = new PreparationDto(
            Id: id,
            OrderId: Guid.NewGuid(),
            CreatedAt: new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            StartedAt: null,
            ReadyAt: null,
            Status: (PreparationStatusType.Pending)
        );

        var queries = new Mock<IPreparationQueryProvider>(MockBehavior.Strict);
        queries.Setup(q => q.GetByIdAsync(id)).ReturnsAsync(expected);

        var handler = new GetPreparationQueryHandler(queries.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Same(expected, result);
        queries.Verify(q => q.GetByIdAsync(id), Times.Once);
        queries.VerifyNoOtherCalls();
    }


    [Fact]
    public async Task Handle_returns_null_when_provider_returns_null()
    {
        // Arrange
        var id = Guid.NewGuid();

         var request = new GetPreparationQuery(id);

        var queries = new Mock<IPreparationQueryProvider>(MockBehavior.Strict);
        queries.Setup(q => q.GetByIdAsync(id))
               .ReturnsAsync((PreparationDto?)null);

        var handler = new GetPreparationQueryHandler(queries.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        queries.Verify(q => q.GetByIdAsync(id), Times.Once);
        queries.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_propagates_exception_from_provider()
    {
        // Arrange
        var id = Guid.NewGuid();

         var request = new GetPreparationQuery(id);

        var queries = new Mock<IPreparationQueryProvider>(MockBehavior.Strict);
        queries.Setup(q => q.GetByIdAsync(id))
               .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new GetPreparationQueryHandler(queries.Object);

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(request, CancellationToken.None));

        queries.Verify(q => q.GetByIdAsync(id), Times.Once);
        queries.VerifyNoOtherCalls();
    }
}
