using System;

namespace TechFood.Kitchen.Application.Dto;

public record ProductDto(
    Guid Id,
    string Name,
    string ImageUrl,
    decimal Price
);
