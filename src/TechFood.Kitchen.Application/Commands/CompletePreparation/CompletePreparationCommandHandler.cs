using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Application.Events;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Kitchen.Domain.Resources;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.Kitchen.Application.Commands.CompletePreparation;

[ExcludeFromCodeCoverage]
public class CompletePreparationCommandHandler(
    IPreparationRepository repo,
    IMediator mediator) : IRequestHandler<CompletePreparationCommand, Unit>
{
    public async Task<Unit> Handle(CompletePreparationCommand request, CancellationToken cancellationToken)
    {
        var preparation = await repo.GetByIdAsync(request.Id);

        if (preparation == null)
            throw new ApplicationException(Exceptions.Preparation_PreparationNotFound);

        preparation.Ready();

        await mediator.Publish(new PreparationDoneIntegrationEvent(preparation.OrderId), cancellationToken);

        return Unit.Value;
    }
}
