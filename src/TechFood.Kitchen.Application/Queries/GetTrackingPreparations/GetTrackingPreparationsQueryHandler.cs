using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Application.Preparations.Dto;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;

namespace TechFood.Kitchen.Application.Queries.GetTrackingPreparations;

public class GetTrackingPreparationsQueryHandler(IPreparationQueryProvider queries) : IRequestHandler<GetTrackingPreparationsQuery, List<TrackingPreparationDto>>
{
    public Task<List<TrackingPreparationDto>> Handle(GetTrackingPreparationsQuery request, CancellationToken cancellationToken)
        => queries.GetTrackingItemsAsync();
}
