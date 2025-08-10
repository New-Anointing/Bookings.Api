using Bookings.Api.Extensions;
using Bookings.Modules.Events.Api;
using Bookings.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEventsModules(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    app.UseSwaggerUI(swaggerOptions=> swaggerOptions.SwaggerEndpoint("/openapi/v1.json", "Bookings_v1"));
    app.MapGet("/", () => "Bookings API is running!");
}

app.UseHttpsRedirection();

EventsModules.MapEndpoints(app);
await app.RunAsync();
