using System.Diagnostics.CodeAnalysis;

namespace TechFood.Kitchen.Contracts.Orders;

[ExcludeFromCodeCoverage]
public record CreateOrderRequest(
    Guid? CustomerId,
    List<CreateOrderRequest.Item> Items)
{
    public record Item(Guid ProductId, int Quantity);
}
