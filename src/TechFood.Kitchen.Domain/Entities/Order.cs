using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechFood.Shared.Domain.Entities;

namespace TechFood.Kitchen.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Order : Entity, IAggregateRoot
{
    private Order()
    {
    }

    public Order(
        int number,
        DateTime createdAt,
        Guid? customerId = null)
    {
        Number = number;
        CustomerId = customerId;
        CreatedAt = createdAt;
    }

    private readonly List<OrderItem> _items = [];

    public int Number { get; private set; }

    public Guid? CustomerId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
}
