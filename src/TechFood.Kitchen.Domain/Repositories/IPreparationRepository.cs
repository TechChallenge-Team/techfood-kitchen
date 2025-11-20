using System;
using System.Threading.Tasks;
using TechFood.Kitchen.Domain.Entities;

namespace TechFood.Kitchen.Domain.Repositories;

public interface IPreparationRepository
{
    Task<Guid> AddAsync(Preparation preparation);

    Task<Preparation?> GetByIdAsync(Guid id);

    Task<Preparation?> GetByOrderIdAsync(Guid orderId);
}
