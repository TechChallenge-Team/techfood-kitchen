using System;
using TechFood.Shared.Application.Events;

namespace TechFood.Kitchen.Application.Events;

public class PreparationStartedIntegrationEvent : IIntegrationEvent
{
    public PreparationStartedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}

public class PreparationDoneIntegrationEvent : IIntegrationEvent
{
    public PreparationDoneIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
