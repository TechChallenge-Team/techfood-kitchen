using System;
using MediatR;

namespace TechFood.Kitchen.Application.Commands.CompletePreparation;

public record CompletePreparationCommand(Guid Id) : IRequest<Unit>;
