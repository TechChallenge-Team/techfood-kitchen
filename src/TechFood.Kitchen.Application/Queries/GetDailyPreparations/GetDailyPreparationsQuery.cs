using System.Collections.Generic;
using MediatR;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Queries.GetDailyPreparations;

public record GetDailyPreparationsQuery : IRequest<List<DailyPreparationDto>>;
