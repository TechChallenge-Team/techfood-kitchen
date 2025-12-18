using MediatR;
using TechFood.Application.Preparations.Dto;
using TechFood.Kitchen.Api.Controllers;
using TechFood.Kitchen.Application.Commands.CompletePreparation;
using TechFood.Kitchen.Application.Commands.StartPreparation;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;
using TechFood.Kitchen.Application.Queries.GetPreparation;
using TechFood.Kitchen.Application.Queries.GetTrackingPreparations;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Kitchen.Api.Tests.Controllers;

public class PreparationsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PreparationsController _controller;

    public PreparationsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PreparationsController(_mediatorMock.Object);
    }

    [Fact(DisplayName = "GetDailyAsync should return Ok with daily preparations")]
    [Trait("Api", "PreparationsController")]
    public async Task GetDailyAsync_ShouldReturnOkWithDailyPreparations()
    {
        // Arrange
        // GetDailyPreparationsQuery -> List<DailyPreparationDto>
        var expected = new List<DailyPreparationDto>();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetDailyPreparationsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expected));

        // Act
        var result = await _controller.GetDailyAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.Value.Should().BeSameAs(expected);

        _mediatorMock.Verify(x => x.Send(
                It.IsAny<GetDailyPreparationsQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "GetTrackingAsync should return Ok with tracking preparations")]
    [Trait("Api", "PreparationsController")]
    public async Task GetTrackingAsync_ShouldReturnOkWithTrackingPreparations()
    {
        // Arrange
        var expected = new List<TrackingPreparationDto>();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetTrackingPreparationsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expected));

        // Act
        var result = await _controller.GetTrackingAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.Value.Should().BeSameAs(expected);

        _mediatorMock.Verify(x => x.Send(
                It.IsAny<GetTrackingPreparationsQuery>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "GetByIdAsync should return Ok with preparation details")]
    [Trait("Api", "PreparationsController")]
    public async Task GetByIdAsync_WithValidId_ShouldReturnOkWithPreparation()
    {
        // Arrange
        var id = Guid.NewGuid();

        var expected = new PreparationDto(
            Id: id,
            OrderId: Guid.NewGuid(),
            CreatedAt: DateTime.UtcNow,
            StartedAt: null,
            ReadyAt: null,
            Status: TechFood.Domain.Enums.PreparationStatusType.Pending);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetPreparationQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(expected));

        // Act
        var result = await _controller.GetByIdAsync(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.Value.Should().BeSameAs(expected);

        _mediatorMock.Verify(x => x.Send(
                It.Is<GetPreparationQuery>(q => q.Id == id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "StartAsync should return Ok when preparation is started")]
    [Trait("Api", "PreparationsController")]
    public async Task StartAsync_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<StartPreparationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.StartAsync(id);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(x => x.Send(
                It.Is<StartPreparationCommand>(cmd => cmd.Id == id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "CompleteAsync should return Ok when preparation is completed")]
    [Trait("Api", "PreparationsController")]
    public async Task CompleteAsync_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CompletePreparationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act
        var result = await _controller.CompleteAsync(id);

        // Assert
        result.Should().BeOfType<OkResult>();

        _mediatorMock.Verify(x => x.Send(
                It.Is<CompletePreparationCommand>(cmd => cmd.Id == id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
