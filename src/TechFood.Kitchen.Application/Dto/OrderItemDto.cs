using System;

namespace TechFood.Kitchen.Application.Dto;

public record OrderItemDto(Guid Id, string Name, string? ImageUrl, decimal Price, int Quantity);
