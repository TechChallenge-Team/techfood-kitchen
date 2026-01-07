using System;
using TechFood.Shared.Application.Events;

namespace TechFood.Kitchen.Application.Events.Integration.Outgoing;

public class PreparationDoneEvent : IIntegrationEvent
{
    public PreparationDoneEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
