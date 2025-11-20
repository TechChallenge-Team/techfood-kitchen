using System;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Application.Preparations.Dto;

public record TrackingPreparationDto(Guid Id, Guid OrderId, int Number, PreparationStatusType Status);
