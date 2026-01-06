using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Shared.Application.Events;

namespace TechFood.Kitchen.Application.Events.Integration.Incoming.Handlers;

public record OrderReceivedIntegrationEvent(Guid OrderId) : IIntegrationEvent;

internal class OrderReceivedIntegrationEventHandler(
    IPreparationRepository preparationRepo)
    : INotificationHandler<OrderReceivedIntegrationEvent>
{
    public async Task Handle(OrderReceivedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var preparation = new Preparation(notification.OrderId);

        await preparationRepo.AddAsync(preparation);
    }
}
