using TechFood.Kitchen.Application;
using TechFood.Kitchen.Infra;
using TechFood.Kitchen.Infra.Persistence.Contexts;
using TechFood.Shared.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddPresentation(builder.Configuration, new PresentationOptions
    {
        AddSwagger = true,
        AddJwtAuthentication = true,
        SwaggerTitle = "TechFood Kitchen API V1",
        SwaggerDescription = "TechFood Kitchen API V1"
    });

    builder.Services.AddApplication();

    builder.Services.AddInfra();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("kitchen.read", policy => policy.RequireClaim("scope", "kitchen.read"))
        .AddPolicy("kitchen.write", policy => policy.RequireClaim("scope", "kitchen.write"));
}

var app = builder.Build();
{
    app.RunMigration<KitchenContext>();

    app.UsePathBase("/kitchen");

    app.UseForwardedHeaders();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger(options =>
        {
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
        });
    }

    app.UseSwaggerUI();

    app.UseInfra();

    app.UseHealthChecks("/health");

    app.UseRouting();

    app.UseCors();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
