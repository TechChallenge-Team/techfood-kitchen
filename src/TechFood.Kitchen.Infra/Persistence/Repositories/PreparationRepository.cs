using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Kitchen.Infra.Persistence.Contexts;

namespace TechFood.Kitchen.Infra.Persistence.Repositories;

public class PreparationRepository(KitchenContext kitchenContext) : IPreparationRepository
{
    private readonly DbSet<Preparation> _preparations = kitchenContext.Preparations;

    public async Task<Guid> AddAsync(Preparation preparation)
    {
        var entry = await _preparations.AddAsync(preparation);

        return entry.Entity.Id;
    }

    public Task<Preparation?> GetByIdAsync(Guid id)
    {
        var preparation = _preparations.FirstOrDefaultAsync(p => p.Id == id);

        return preparation;
    }

    public Task<Preparation?> GetByOrderIdAsync(Guid orderId)
    {
        var preparation = _preparations.FirstOrDefaultAsync(p => p.OrderId == orderId);

        return preparation;
    }
}
