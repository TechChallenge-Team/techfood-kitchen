using System;
using System.Collections.Generic;
using TechFood.Shared.Domain.Enums;

namespace TechFood.Kitchen.Application.Dto;

public record OrderDto(
    Guid Id,
    int Number,
    decimal Amount,
    DateTime CreatedAt,
    Guid? CustomerId,
    OrderStatusType Status,
    List<OrderItemDto> Items);
