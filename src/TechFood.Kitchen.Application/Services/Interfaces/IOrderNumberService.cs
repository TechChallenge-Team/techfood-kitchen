using System.Threading.Tasks;

namespace TechFood.Kitchen.Application.Services.Interfaces;

public interface IOrderNumberService
{
    Task<int> GetAsync();
}
