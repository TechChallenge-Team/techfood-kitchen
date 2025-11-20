using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Application.Preparations.Dto;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;

namespace TechFood.Kitchen.Application.Queries.GetPreparation;

public class GetPreparationQueryHandler(IPreparationQueryProvider queries) : IRequestHandler<GetPreparationQuery, PreparationDto?>
{
    public Task<PreparationDto?> Handle(GetPreparationQuery request, CancellationToken cancellationToken)
        => queries.GetByIdAsync(request.Id);
}
