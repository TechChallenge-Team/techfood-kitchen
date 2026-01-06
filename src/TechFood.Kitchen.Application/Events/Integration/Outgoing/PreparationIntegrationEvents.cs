using System;
using TechFood.Shared.Application.Events;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Kitchen.Application.Events.Integration.Outgoing;

public class PreparationStartedIntegrationEvent : IIntegrationEvent
{
    public PreparationStartedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
        OrderStatus = OrderStatusType.InPreparation;
    }

    public Guid OrderId { get; set; }
    public OrderStatusType OrderStatus { get; private set; }
}

public class PreparationDoneIntegrationEvent : IIntegrationEvent
{
    public PreparationDoneIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
        OrderStatus = OrderStatusType.Ready;
    }

    public Guid OrderId { get; set; }
    public OrderStatusType OrderStatus { get; private set; }
}
