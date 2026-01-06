using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Shared.Application.Events;

namespace TechFood.Kitchen.Application.Events.Integration.Incoming.Handlers;

public record OrderReceivedEvent(Guid OrderId) : IIntegrationEvent;

internal class OrderReceivedEventHandler(IPreparationRepository preparationRepo) : INotificationHandler<OrderReceivedEvent>
{
    public async Task Handle(OrderReceivedEvent notification, CancellationToken cancellationToken)
    {
        var preparation = new Preparation(notification.OrderId);

        await preparationRepo.AddAsync(preparation);
    }
}
