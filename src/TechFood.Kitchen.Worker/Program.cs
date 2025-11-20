using TechFood.Kitchen.Application;
using TechFood.Kitchen.Infra;

var builder = Host.CreateApplicationBuilder(args);
{
    builder.Services.AddWorker();
    builder.Services.AddApplication();
    builder.Services.AddInfra();
}

var app = builder.Build();
{
    app.Run();
}
