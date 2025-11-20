using System;
using MediatR;
using TechFood.Application.Preparations.Dto;

namespace TechFood.Kitchen.Application.Queries.GetPreparation;

public record GetPreparationQuery(Guid Id) : IRequest<PreparationDto?>;
