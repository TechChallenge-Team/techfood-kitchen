using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Application.Events;
using TechFood.Kitchen.Application.Events.Integration.Outgoing;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Kitchen.Domain.Resources;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.Kitchen.Application.Commands.StartPreparation;

[ExcludeFromCodeCoverage]
public class StartPreparationCommandHandler(
    IPreparationRepository repo,
    IMediator mediator) : IRequestHandler<StartPreparationCommand, Unit>
{
    public async Task<Unit> Handle(StartPreparationCommand request, CancellationToken cancellationToken)
    {
        var preparation = await repo.GetByIdAsync(request.Id);

        if (preparation == null)
            throw new ApplicationException(Exceptions.Preparation_PreparationNotFound);

        preparation.Start();

        await mediator.Publish(new PreparationStartedIntegrationEvent(preparation.OrderId), cancellationToken);

        return Unit.Value;
    }
}
