using System;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using TechFood.Application.Preparations.Dto;

namespace TechFood.Kitchen.Application.Queries.GetPreparation;

[ExcludeFromCodeCoverage]
public record GetPreparationQuery(Guid Id) : IRequest<PreparationDto?>;
