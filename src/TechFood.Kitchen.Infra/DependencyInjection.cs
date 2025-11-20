using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechFood.Kitchen.Application.Queries;
using TechFood.Kitchen.Application.Queries.GetDailyPreparations;
using TechFood.Kitchen.Application.Services.Interfaces;
using TechFood.Kitchen.Domain.Repositories;
using TechFood.Kitchen.Infra.Persistence.Contexts;
using TechFood.Kitchen.Infra.Persistence.Queries;
using TechFood.Kitchen.Infra.Persistence.Repositories;
using TechFood.Kitchen.Infra.Services;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.Kitchen.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddSharedInfra<KitchenContext>(new InfraOptions
        {
            DbContext = (serviceProvider, dbOptions) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                dbOptions.UseSqlServer(config.GetConnectionString("DataBaseConection"));
            },
            ApplicationAssembly = typeof(Kitchen.Application.DependencyInjection).Assembly
        });

        //Data
        services.AddScoped<IPreparationRepository, PreparationRepository>();

        //Queries
        services.AddScoped<IPreparationQueryProvider, PreparationQueryProvider>();

        //Services
        services.AddTechFoodClient<IBackofficeService, BackofficeService>("Backoffice");

        return services;
    }
}
