using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries;

public interface IOrderQueryProvider
{
    Task<List<OrderDto>> GetOrdersAsync();

    Task<List<OrderDto>> GetReadyOrdersAsync();
}
