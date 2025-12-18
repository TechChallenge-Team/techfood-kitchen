using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using TechFood.Application.Preparations.Dto;

namespace TechFood.Kitchen.Application.Queries.GetTrackingPreparations;

[ExcludeFromCodeCoverage]
public record GetTrackingPreparationsQuery : IRequest<List<TrackingPreparationDto>>;
