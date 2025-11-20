using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Kitchen.Domain.Resources;
using TechFood.Shared.Application.Exceptions;

namespace TechFood.Kitchen.Application.Commands.StartPreparation;

public class StartPreparationCommandHandler(IPreparationRepository repo) : IRequestHandler<StartPreparationCommand, Unit>
{
    public async Task<Unit> Handle(StartPreparationCommand request, CancellationToken cancellationToken)
    {
        var preparation = await repo.GetByIdAsync(request.Id);
        if (preparation == null)
        {
            throw new ApplicationException(Exceptions.Preparation_PreparationNotFound);
        }

        preparation.Start();

        return Unit.Value;
    }
}
