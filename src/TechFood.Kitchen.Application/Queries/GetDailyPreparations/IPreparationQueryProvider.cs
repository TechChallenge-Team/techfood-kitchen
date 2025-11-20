using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFood.Application.Preparations.Dto;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetDailyPreparations;

public interface IPreparationQueryProvider
{
    Task<PreparationDto?> GetByIdAsync(Guid id);

    Task<List<DailyPreparationDto>> GetDailyPreparationsAsync();

    Task<List<TrackingPreparationDto>> GetTrackingItemsAsync();
}
