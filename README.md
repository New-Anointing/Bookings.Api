# Bookings.Api — Project Workflow & Developer Guide

This repository contains a modular booking platform implemented in .NET 9 (C# 13). This README documents the project layout, runtime request workflow, error/result handling patterns, and common developer commands to get started.

## Quick facts
- .NET Target: `net9.0` (C# 13)
- Startup project: `src/API/Bookings.Api/Bookings.Api.csproj`
- Main modules: Users, Ticketing, Events
- DB: PostgreSQL (Npgsql + EF Core)
- Auth: Keycloak (configured under `Users:KeyCloak`)
- Observability: Serilog + Seq, HealthChecks, OpenAPI/Swagger

## Project structure (folders)
- `README.md`
- `Directory.build.props`
- `docker-compose.dcproj` (if present)
- `src/`
  - `API/`
    - `Bookings.Api/`
      - `Program.cs`
      - `Dockerfile`
      - other API host files
  - `Common/`
    - `Bookings.Common.Domain/` — domain primitives (`Result`, `Error`, etc.)
    - `Bookings.Common.Application/` — cross-cutting app abstractions (Messaging, Data, Caching)
    - `Bookings.Common.Infrastructure/` — infra wiring (AuthN/Z, MassTransit, Redis, EF interceptors)
    - `Bookings.Common.Presentation/` — API helpers (`ApiResults`, `ResultExtensions`, endpoint discovery)
  - `Modules/`
    - `Events/`
      - `Bookings.Modules.Events.Domain/`
      - `Bookings.Modules.Events.Application/`
      - `Bookings.Modules.Events.Infrastructure/`
      - `Bookings.Modules.Events.Presentation/`
    - `Ticketing/`
      - `Bookings.Modules.Ticketing.Domain/`
      - `Bookings.Modules.Ticketing.Application/`
      - `Bookings.Modules.Ticketing.Infrastructure/`
      - `Bookings.Modules.Ticketing.Presentation/`
      - `Bookings.Modules.Ticketing.IntegrationEvents/`
    - `Users/`
      - `Bookings.Modules.Users.Domain/`
      - `Bookings.Modules.Users.Application/`
      - `Bookings.Modules.Users.Infrastructure/`
      - `Bookings.Modules.Users.Presentation/`
      - `Bookings.Modules.Users.IntegrationEvents/`
      - `Bookings.Modules.Users.PublicApi/`

## High-level request workflow
1. HTTP request arrives and is routed to a Presentation-layer endpoint (minimal API or controller).
2. Presentation calls Application-layer services/handlers to perform business logic.
3. Application returns `Result` or `Result<T>` indicating success or failure (with an `Error`).
4. Presentation maps the `Result` to an HTTP response — typically using `ResultExtensions.Match(...)`.
   - Success ? 2xx response (with payload for `Result<T>`).
   - Failure ? appropriate 4xx/5xx response based on `Error.Type` (NotFound, Validation, Conflict, etc.).
5. Infrastructure provides DB, messaging, external clients, interceptors (domain events), and logging.

AuthN/Z & permissions flow
- Authentication is configured in `Bookings.Common.Infrastructure` (JWT bearer).
- A custom claims transformation (`CustomClaimsTransformation`) loads user permissions via `IPermissionService` and adds `permission` claims.
- Endpoints use `.RequireAuthorization(<permission>)` to enforce permission-based access.

Domain events & messaging
- EF Core interceptor `PublishDomainEventsInterceptor` publishes domain events raised in aggregates.
- `MassTransit` is configured (in-memory bus by default) via `AddInfrastructure(..., moduleConfigureConsumers, ...)` and module consumer registrations (e.g., `TicketingModule.ConfigureConsumers`).

Data access
- EF Core + Npgsql for writes/aggregates; Dapper used for some query handlers (e.g., orders read model).
- PostgreSQL schema per module (e.g., `ticketing`, `events`, `users`).

Caching & health
- Redis used via `Microsoft.Extensions.Caching.StackExchangeRedis`; falls back to memory cache if Redis unavailable.
- Health checks include PostgreSQL, Redis, and Keycloak URL.

OpenAPI & logging
- OpenAPI/Swagger UI enabled in Development; Serilog for structured logging and Seq sink configured.

## Result / Error model (overview)
- `Result`: non-generic success/failure container.
  - Factories: `Result.Success()`, `Result.Failure(Error)`.
- `Result<T>`: carries a `Value` when success.
  - Factories: `Result<T>.Success(value)`, `Result<T>.Failure(error)`.
- `Error`: standardized error with `Code`, `Description`, `Type` (e.g., NotFound, Conflict, Failure).

Purpose: return explicit domain outcomes from application services without using exceptions for expected flows. Presentation maps these into HTTP responses.

## `ResultExtensions.Match(...)`
Location: `src/Common/Bookings.Common.Presentation/ApiResults/ResultExtensions.cs`

Behavior:
- `Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Result, TOut> onFailure)`
  - Calls `onSuccess()` when `result.IsSuccess` is true; otherwise calls `onFailure(result)`.
- `Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> onSuccess, Func<Result<TIn>, TOut> onFailure)`
  - Calls `onSuccess(result.Value)` when success; otherwise calls `onFailure(result)`.

This helper keeps endpoint code concise and consistent when translating domain results to HTTP responses.

## API Endpoints & Components (by module)
This section summarizes the main HTTP endpoints, the presentation classes that map them, and important supporting components a new developer should know.

Events
- `GET /events` — `GetEvents` endpoint: returns `IReadOnlyCollection<EventResponse>`. Requires `Permissions.GetEvents`.
- `GET /events/{id:guid}` — `GetEvent` endpoint: returns `EventResponse`. Requires `Permissions.GetEvents`.
- `POST /events` — `CreateEvent` endpoint: creates an event. Request DTO: `Title`, `CategoryId`, `Description`, `Location`, `StartsAtUtc`, `EndsAtUtc`. Requires `Permissions.ModifyEvents`.
- `PUT /events/{id}/publish` — `PublishEvent` endpoint: publishes an event. Returns NoContent on success. Requires `Permissions.ModifyEvents`.
- `PUT /events/{id}/reschedule` — `RescheduledEvent` endpoint: reschedules an event (request includes `StartsAtUtc`, `EndsAtUtc`). Requires `Permissions.ModifyEvents`.
- `DELETE /events/{id}/cancel` — `CancelEvent` endpoint: cancels an event. Returns NoContent. Requires `Permissions.ModifyEvents`.
- `GET /events/search` — `SearchEvents` endpoint: supports optional query params `categoryId`, `startDate`, `endDate`, `page`, `pageSize`. Returns `SearchEventsResponse`. Requires `Permissions.SearchEvents`.

Categories
- `GET categories/{id}` — `GetCategory` endpoint: returns `CategoryResponse`. Requires `Permissions.GetCategories`.
- `POST categories` — `CreateCategory` endpoint: creates a category. Requires `Permissions.ModifyCategories`.
- `PUT categories/{id}` — `UpdateCategory` endpoint: updates a category name. Requires `Permissions.ModifyCategories`.

Ticket Types
- `POST ticket-types` — `CreateTicketType` endpoint: creates a ticket type for an event. Request DTO: `EventId`, `Name`, `Price`, `Currency`, `Quantity`. Requires `Permissions.ModifyTicketTypes`.
- `GET ticket-types` — `GetTicketTypes` endpoint: query by `eventId` and returns list of `TicketTypeResponse`. Requires `Permissions.GetTicketTypes`.

Ticketing
- `GET tickets/{id}` — `GetTicket` endpoint: returns `TicketResponse`. Requires `Permissions.GetTickets`.
- `GET tickets/code/{code}` — `GetTicketByCode` endpoint: returns ticket by code. Requires `Permissions.GetTickets`.
- `POST cart/add` — `AddToCart` endpoint: adds item to customer's cart. Request DTO: `CustomerId`, `TicketTypeId`, `Quantity`. Requires `Permissions.AddToCart`.
- `GET orders/{id}` — `GetOrder` endpoint: returns `OrderResponse` (includes `OrderItems`). Requires `Permissions.GetOrders`.

Users
- Presentation and public API surfaces exist under `src/Modules/Users`. Common permissions live in `Bookings.Modules.Users.Presentation/Permissions.cs`.

Core patterns & helpers
- Presentation endpoints implement `IEndpoint` (`src/Common/Bookings.Common.Presentation/Endpoints/IEndpoint.cs`).
- `EndpointExtensions.AddEndPoints(...)` registers all `IEndpoint` implementations found in provided assemblies; `MapEndpoints()` calls `MapEndpoint` on each instance during startup.
- Application messaging uses MediatR patterns with `IQuery<TResponse>` / `ICommand` and handlers implementing `IQueryHandler` / `IRequestHandler`.
- `Result` / `Result<T>` (see `src/Common/Bookings.Common.Domain/Result.cs`) is the standard way application handlers return success/failure.
- `ApiResults.Problem(...)` maps failed `Result`s to an `IResult` (HTTP Problem responses) with proper status codes and problem details (`src/Common/Bookings.Common.Presentation/ApiResults/ApiResults.cs`).
- `ResultExtensions.Match(...)` provides a concise mapping from domain `Result` to HTTP responses (`src/Common/Bookings.Common.Presentation/ApiResults/ResultExtensions.cs`).
- Permissions are enforced via `RequireAuthorization(...)` on endpoints; permission codes are stored in module `Permissions` classes and a domain `Permission` class.

Examples & notable implementations
- `GetOrderQueryHandler` (Ticketing) uses `IDbConnectionFactory` + Dapper multi-mapping to fetch an order and its items, assembles `OrderResponse` and returns a `Result<OrderResponse>` or `OrderErrors.NotFound`.
- Many endpoints use MediatR `ISender` to dispatch queries/commands and then map results using `Match(Results.Ok, ApiResults.Problem)` or similar.

How to add a new endpoint
1. Add application layer command/query and handler (return `Result`/`Result<T>`).
2. Add presentation `IEndpoint` implementation with `MapEndpoint` wiring `app.MapGet/Post/Put/Delete(...)`.
3. Register the application assembly in `Program.cs` via `AddApplication(...)` if new.
4. Implement permissions and register any infrastructure (DbContext, clients) in the relevant Module class.

Program startup wiring (high level)
- `Program.cs` sets up Serilog, exception handling, OpenAPI, health checks, module registration (`AddEventsModule`, `AddUsersModule`, `AddTicketingModule`), and calls `app.MapEndpoints()` to wire all `IEndpoint` implementations.

Important files & locations
- `src/API/Bookings.Api/Program.cs` — application startup and module registration.
- `src/Common/Bookings.Common.Presentation/Endpoints/IEndpoint.cs` — endpoint contract for presentation layer.
- `src/Common/Bookings.Common.Presentation/Endpoints/EndpointExtensions.cs` — discovery & registration of endpoints.
- `src/Common/Bookings.Common.Domain/Result.cs` — Result and Result<T> types.
- `src/Common/Bookings.Common.Presentation/ApiResults/ApiResults.cs` — maps domain errors to HTTP Problem responses.
- `src/Common/Bookings.Common.Presentation/ApiResults/ResultExtensions.cs` — Match extension methods used by endpoints.
- `src/Modules/*/Presentation` — minimal API endpoint implementations.
- `src/Modules/*/Application` — application commands/queries and handlers.
- `src/Modules/*/Infrastructure` — DbContexts, repositories, message consumers, external clients.

Known issues & TODOs
- Consider adding integration tests for endpoint-to-handler wiring and Result mapping.

## Recent changes
- Fixed route typo in `CreateCategory` endpoint: `"catrgories"` ? `"categories"` (`src/Modules/Events/.../CreateCategory.cs`).
- Fixed claim transformation bug where claim used incorrect `PermissionResponse` property (`USerId` ? `UserId`) (`src/Common/Bookings.Common.Infrastructure/Authorization/CustomClaimsTransformation.cs`).
- Build validated successfully after fixes.

## Common commands
Build
- `dotnet build src/API/Bookings.Api/Bookings.Api.csproj`

Run locally
- `dotnet run --project src/API/Bookings.Api/Bookings.Api.csproj`

EF Core migrations (example for Users module)
- Add migration:
  - `dotnet ef migrations add <Name> --project src/Modules/Users/Bookings.Modules.Users.Infrastructure --startup-project src/API/Bookings.Api/Bookings.Api.csproj`
- Apply migrations:
  - `dotnet ef database update --project src/Modules/Users/Bookings.Modules.Users.Infrastructure --startup-project src/API/Bookings.Api/Bookings.Api.csproj`

Docker
- From repo root: `docker compose up --build -d` (requires Docker running)

## Configuration keys to know
- `ConnectionStrings:Database` — PostgreSQL connection string
- `Users:KeyCloak` — Keycloak admin client configuration
- API project has UserSecrets enabled (see `UserSecretsId` in `src/API/Bookings.Api/Bookings.Api.csproj`) — use secrets or env vars for sensitive values

---

If you want, I can also generate a short QuickStart section with step-by-step local setup (Postgres + Keycloak using Docker Compose) and example requests for each endpoint.If you want, I can also generate a short QuickStart section with step-by-step local setup (Postgres + Keycloak using Docker Compose) and example requests for each endpoint.