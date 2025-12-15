using System;
using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace TechFood.Kitchen.Application.Commands.CompletePreparation;

[ExcludeFromCodeCoverage]
public record CompletePreparationCommand(Guid Id) : IRequest<Unit>;
