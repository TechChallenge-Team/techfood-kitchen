using System;
using TechFood.Domain.Enums;

namespace TechFood.Kitchen.Application.Dto;

public record DailyPreparationDto(
    Guid Id,
    Guid OrderId,
    DateTime CreatedAt,
    DateTime? StartedAt,
    DateTime? ReadyAt,
    PreparationStatusType Status);
