using System.Collections.Generic;
using MediatR;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetReadyOrders;

public class GetReadyOrdersQuery : IRequest<List<OrderDto>>;
