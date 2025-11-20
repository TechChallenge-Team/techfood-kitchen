using System;
using System.Threading.Tasks;

namespace TechFood.Kitchen.Domain.Repositories;

public interface IOrderRepository
{
    Task<Guid> AddAsync(Entities.Order order);

    Task<Entities.Order?> GetByIdAsync(Guid id);
}
