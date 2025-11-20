using System.Collections.Generic;
using MediatR;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetOrders;

public record GetOrdersQuery : IRequest<List<OrderDto>>;
