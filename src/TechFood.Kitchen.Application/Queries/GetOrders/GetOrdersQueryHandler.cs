using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderQueryProvider queries) : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        => queries.GetOrdersAsync();
}
