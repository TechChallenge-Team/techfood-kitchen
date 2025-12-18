using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetDailyPreparations;

[ExcludeFromCodeCoverage]
public record GetDailyPreparationsQuery : IRequest<List<DailyPreparationDto>>;
