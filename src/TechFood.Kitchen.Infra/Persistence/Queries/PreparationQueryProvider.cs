using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechFood.Application.Preparations.Dto;
using TechFood.Domain.Enums;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;
using TechFood.Kitchen.Application.Services.Interfaces;
using TechFood.Kitchen.Infra.Persistence.Contexts;

namespace TechFood.Kitchen.Infra.Persistence.Queries;

public class PreparationQueryProvider(
    IBackofficeService backofficeService,
    KitchenContext kitchenContext) : IPreparationQueryProvider
{

    public async Task<PreparationDto?> GetByIdAsync(Guid id)
    {
        return await kitchenContext.Preparations
            .AsNoTracking()
            .Where(order => order.Id == id)
            .Select(preparation => new PreparationDto(
                preparation.Id,
                preparation.OrderId,
                preparation.CreatedAt,
                preparation.StartedAt,
                preparation.ReadyAt,
                preparation.Status
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<List<DailyPreparationDto>> GetDailyPreparationsAsync()
    {
        var status = new PreparationStatusType[]
        {
            PreparationStatusType.Pending,
            PreparationStatusType.Started
        };

        var products = await backofficeService.GetProductsAsync();

        return kitchenContext.Preparations.Select(data => new DailyPreparationDto(
            data.Id,
            data.OrderId,
            data.CreatedAt,
            data.StartedAt,
            data.ReadyAt,
            data.Status))
            .ToList();
    }

    public async Task<List<TrackingPreparationDto>> GetTrackingItemsAsync()
    {
        var preparationStatus = new PreparationStatusType[]
        {
            PreparationStatusType.Pending,
            PreparationStatusType.Started,
            PreparationStatusType.Ready
        };

        return kitchenContext.Preparations.Select(data => new TrackingPreparationDto(
            data.Id,
            data.OrderId,
            data.Status
            )).ToList();
    }
}
