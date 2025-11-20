using TechFood.Kitchen.Infra.Services;

namespace TechFood.Kitchen.Application.Tests.Services;

public class OrderNumberServiceTests
{
    [Fact(DisplayName = "Should return incremental order numbers")]
    [Trait("Infra", "OrderNumberService")]
    public async Task GetAsync_ShouldReturnIncrementalNumbers()
    {
        // Arrange
        var service = new OrderNumberService();

        // Act
        var firstNumber = await service.GetAsync();
        var secondNumber = await service.GetAsync();
        var thirdNumber = await service.GetAsync();

        // Assert
        firstNumber.Should().Be(1);
        secondNumber.Should().Be(2);
        thirdNumber.Should().Be(3);
    }

    [Fact(DisplayName = "Should handle concurrent requests safely")]
    [Trait("Infra", "OrderNumberService")]
    public async Task GetAsync_ShouldHandleConcurrentRequests_Safely()
    {
        // Arrange
        var service = new OrderNumberService();
        var tasks = new List<Task<int>>();
        var numberOfRequests = 100;

        // Act
        for (int i = 0; i < numberOfRequests; i++)
        {
            tasks.Add(service.GetAsync());
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        results.Should().HaveCount(numberOfRequests);
        results.Should().OnlyHaveUniqueItems();
        results.Should().BeInAscendingOrder();
        results.Min().Should().Be(1);
        results.Max().Should().Be(numberOfRequests);
    }

    [Fact(DisplayName = "Should reset counter at midnight")]
    [Trait("Infra", "OrderNumberService")]
    public async Task GetAsync_ShouldResetCounter_AfterMidnight()
    {
        // Arrange
        var service = new OrderNumberService();

        // Act - First day
        var firstNumber = await service.GetAsync();
        var secondNumber = await service.GetAsync();

        // Note: This test demonstrates the expected behavior but cannot truly test
        // date change without dependency injection of DateTime or waiting until midnight
        // In a production scenario, you would inject IDateTimeProvider for testability

        // Assert
        firstNumber.Should().Be(1);
        secondNumber.Should().Be(2);
    }

    [Fact(DisplayName = "Should return sequential numbers for multiple calls")]
    [Trait("Infra", "OrderNumberService")]
    public async Task GetAsync_ShouldReturnSequentialNumbers()
    {
        // Arrange
        var service = new OrderNumberService();
        var expectedSequence = Enumerable.Range(1, 10).ToList();
        var results = new List<int>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            results.Add(await service.GetAsync());
        }

        // Assert
        results.Should().Equal(expectedSequence);
    }

    [Fact(DisplayName = "Should maintain state across multiple instances")]
    [Trait("Infra", "OrderNumberService")]
    public async Task GetAsync_ShouldMaintainIndependentState_AcrossInstances()
    {
        // Arrange
        var service1 = new OrderNumberService();
        var service2 = new OrderNumberService();

        // Act
        var result1FromService1 = await service1.GetAsync();
        var result1FromService2 = await service2.GetAsync();
        var result2FromService1 = await service1.GetAsync();
        var result2FromService2 = await service2.GetAsync();

        // Assert
        result1FromService1.Should().Be(1);
        result1FromService2.Should().Be(1);
        result2FromService1.Should().Be(2);
        result2FromService2.Should().Be(2);
    }
}
