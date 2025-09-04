using System.Reflection;
using Bookings.Api.Extensions;
using Bookings.Api.Middleware;
using Bookings.Common.Application;
using Bookings.Common.Infrastructure;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Events.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

Assembly[] Assemblies = [Bookings.Modules.Events.Application.AssemblyRefrence.Assembly];

string dataBaseConnectionString = builder.Configuration.GetConnectionString("EventsDatabase")!;
string redisCacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Host.UseSerilog((context, LoggerConfiguration) => LoggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.Services.AddApplication(Assemblies);

builder.Services.AddInfrastructure(dataBaseConnectionString, redisCacheConnectionString);

builder.Configuration.AddModuleConfiguration(["events"]);

builder.Services.AddHealthChecks()
    .AddNpgSql(dataBaseConnectionString)
    .AddRedis(redisCacheConnectionString);

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

app.MapEndpoints();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseExceptionHandler();

await app.RunAsync();
