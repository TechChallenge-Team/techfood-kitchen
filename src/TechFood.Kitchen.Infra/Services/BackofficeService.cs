using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using TechFood.Kitchen.Application.Dto;
using TechFood.Kitchen.Application.Services.Interfaces;

namespace TechFood.Kitchen.Infra.Services;

public class BackofficeService : IBackofficeService
{
    private readonly HttpClient _httpClient;

    public BackofficeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("/v1/products", cancellationToken);

        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>(cancellationToken);

        return products ?? [];
    }
}
