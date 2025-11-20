using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetDailyPreparations;

public class GetDailyPreparationsQueryHandler(IPreparationQueryProvider queries) : IRequestHandler<GetDailyPreparationsQuery, List<DailyPreparationDto>>
{
    public Task<List<DailyPreparationDto>> Handle(GetDailyPreparationsQuery request, CancellationToken cancellationToken)
        => queries.GetDailyPreparationsAsync();
}
