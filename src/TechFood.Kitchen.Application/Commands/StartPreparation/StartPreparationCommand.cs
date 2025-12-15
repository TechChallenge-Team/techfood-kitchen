using System;
using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace TechFood.Kitchen.Application.Commands.StartPreparation;

[ExcludeFromCodeCoverage]
public record StartPreparationCommand(Guid Id) : IRequest<Unit>;
