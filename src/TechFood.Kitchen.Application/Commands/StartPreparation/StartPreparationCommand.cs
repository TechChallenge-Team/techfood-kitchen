using System;
using MediatR;

namespace TechFood.Kitchen.Application.Commands.StartPreparation;

public record StartPreparationCommand(Guid Id) : IRequest<Unit>;
