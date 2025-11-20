using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Application.Queries;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries;

namespace TechFood.Kitchen.Application.Queries.GetReadyOrders;

public class GetReadyOrdersQueryHandler(IOrderQueryProvider queries) : IRequestHandler<GetReadyOrdersQuery, List<OrderDto>>
{
    public Task<List<OrderDto>> Handle(GetReadyOrdersQuery request, CancellationToken cancellationToken)
        => queries.GetReadyOrdersAsync();
}
