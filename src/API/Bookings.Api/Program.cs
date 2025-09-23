using System.Reflection;
using Bookings.Api.Extensions;
using Bookings.Api.Middleware;
using Bookings.Common.Application;
using Bookings.Common.Infrastructure;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Events.Infrastructure;
using Bookings.Modules.Ticketing.Infrastructure;
using Bookings.Modules.Users.Infrastucture;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


string dataBaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisCacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Host.UseSerilog((context, LoggerConfiguration) => LoggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

Assembly[] Assemblies = [Bookings.Modules.Events.Application.AssemblyRefrence.Assembly,
                        Bookings.Modules.Users.Application.AssemblyRefrence.Assembly,
                        Bookings.Modules.Ticketing.Application.AssemblyRefrence.Assembly];

builder.Services.AddApplication(Assemblies);

builder.Services.AddInfrastructure(dataBaseConnectionString, [TicketingModule.ConfigureConsumers], redisCacheConnectionString);

builder.Configuration.AddModuleConfiguration(["events", "users"]);

builder.Services.AddHealthChecks()
    .AddNpgSql(dataBaseConnectionString)
    .AddRedis(redisCacheConnectionString)
    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!), HttpMethod.Get, "Keycloak");

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    app.UseSwaggerUI(swaggerOptions => swaggerOptions.SwaggerEndpoint("/openapi/v1.json", "Bookings_v1"));
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

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();
