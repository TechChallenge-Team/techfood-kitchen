using System.Collections.Generic;
using MediatR;
using TechFood.Application.Preparations.Dto;

namespace TechFood.Kitchen.Application.Queries.GetTrackingPreparations;

public record GetTrackingPreparationsQuery : IRequest<List<TrackingPreparationDto>>;
