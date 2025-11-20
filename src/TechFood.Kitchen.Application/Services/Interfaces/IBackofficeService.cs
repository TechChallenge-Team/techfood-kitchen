using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechFood.Kitchen.Application.Dto;

namespace TechFood.Kitchen.Application.Services.Interfaces;

public interface IBackofficeService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default);
}
