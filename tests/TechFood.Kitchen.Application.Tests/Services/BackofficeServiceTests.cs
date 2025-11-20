using System.Net;
using System.Net.Http.Json;
using Moq.Protected;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Infra.Services;

namespace TechFood.Kitchen.Application.Tests.Services;

public class BackofficeServiceTests
{
    [Fact(DisplayName = "Should return products successfully")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetProductsAsync_ShouldReturnProducts_WhenRequestIsSuccessful()
    {
        // Arrange
        var expectedProducts = new List<ProductDto>
        {
            new ProductDto(Guid.NewGuid(), "Product 1", "image1.jpg", 10.00m),
            new ProductDto(Guid.NewGuid(), "Product 2", "image2.jpg", 20.00m)
        };

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().Contains("/v1/products")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedProducts)
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        var service = new BackofficeService(httpClient);

        // Act
        var result = await service.GetProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact(DisplayName = "Should return empty list when no products exist")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(new List<ProductDto>())
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        var service = new BackofficeService(httpClient);

        // Act
        var result = await service.GetProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Should throw exception when request fails")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetProductsAsync_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        var service = new BackofficeService(httpClient);

        // Act
        Func<Task> act = async () => await service.GetProductsAsync();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact(DisplayName = "Should handle cancellation token")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetProductsAsync_ShouldRespectCancellationToken()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        var service = new BackofficeService(httpClient);

        // Act
        Func<Task> act = async () => await service.GetProductsAsync(cts.Token);

        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>();
    }

    [Fact(DisplayName = "Should handle null response content")]
    [Trait("Infra", "BackofficeService")]
    public async Task GetProductsAsync_ShouldReturnEmptyList_WhenResponseContentIsNull()
    {
        // Arrange
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create<IEnumerable<ProductDto>>(null!)
            });

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        var service = new BackofficeService(httpClient);

        // Act
        var result = await service.GetProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
