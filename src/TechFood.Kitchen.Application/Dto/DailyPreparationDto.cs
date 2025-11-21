using System;
using System.Collections.Generic;
using TechFood.Application.Preparations.Dto;
using TechFood.Domain.Enums;

namespace TechFood.Kitchen.Application.Dto;

public record DailyPreparationDto(
    Guid Id,
    Guid OrderId,
    int Number,
    DateTime CreatedAt,
    DateTime? StartedAt,
    DateTime? ReadyAt,
    PreparationStatusType Status,
    List<DailyPreparationItemDto> Items);
