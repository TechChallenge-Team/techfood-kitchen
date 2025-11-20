// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using TechFood.Kitchen.Application.Services.Interfaces;
// using TechFood.Kitchen.Infra.Persistence.Contexts;
// using TechFood.Kitchen.Application.Commands.CreateOrder;
// using TechFood.Shared.Domain.UoW;
// using TechFood.Shared.Infra.Persistence.UoW;
//
// namespace TechFood.Kitchen.Integration.Tests.Fixtures;
//
// public class IntegrationTestFixture : IDisposable
// {
//     public IServiceProvider ServiceProvider { get; }
//
//     public OrderContext DbContext { get; }
//
//     public IntegrationTestFixture()
//     {
//         var services = new ServiceCollection();
//
//         // Configure in-memory database
//         services.AddDbContext<OrderContext>(options =>
//             options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));
//
//         // Configure Unit of Work
//         services.TryAddScoped<IUnitOfWorkTransaction, UnitOfWorkTransaction>();
//         services.TryAddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<OrderContext>());
//
//         // Register application services
//         services.AddMediatR(typeof(CreateOrderCommand).Assembly);
//
//         // Register domain repositories
//         services.AddScoped<Domain.Repositories.IOrderRepository, Kitchen.Infra.Persistence.Repositories.OrderRepository>();
//
//         // Mock external services
//         var backofficeServiceMock = new Mock<IBackofficeService>();
//         services.AddScoped(_ => backofficeServiceMock.Object);
//
//         var orderNumberServiceMock = new Mock<IOrderNumberService>();
//         orderNumberServiceMock.Setup(x => x.GetAsync()).ReturnsAsync(1);
//         services.AddScoped(_ => orderNumberServiceMock.Object);
//
//         ServiceProvider = services.BuildServiceProvider();
//         DbContext = ServiceProvider.GetRequiredService<OrderContext>();
//     }
//
//     public void Dispose()
//     {
//         DbContext?.Database.EnsureDeleted();
//         DbContext?.Dispose();
//
//         if (ServiceProvider is IDisposable disposable)
//         {
//             disposable.Dispose();
//         }
//
//         GC.SuppressFinalize(this);
//     }
// }
