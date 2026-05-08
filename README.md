# CarServiceManagementUCTM

Car service management system. ASP.NET Core 8 Web API with PostgreSQL.

## Project structure

    src/
      CarService.Domain          - entities
      CarService.Application     - DTOs, MediatR handlers, validators, mappings, interfaces
      CarService.Infrastructure  - EF Core, AppDbContext, repositories, unit of work
      CarService.API             - controllers, middleware, JWT, Swagger
    tests/
      CarService.Application.Tests - xUnit tests for handlers

## Setup

The connection string is in `src/CarService.API/appsettings.json` under `ConnectionStrings:DefaultConnection`. Replace the `<paste-password-here>` placeholder with the database password (provided separately).

Apply EF Core migrations:

    dotnet ef database update --project src/CarService.Infrastructure --startup-project src/CarService.API

## Run

    dotnet run --project src/CarService.API

Swagger is available at `https://localhost:<port>/swagger` in development.

## Tests

    dotnet test

## Endpoints

Vehicles:

    GET    /api/vehicles
    GET    /api/vehicles/{id}
    POST   /api/vehicles
    PUT    /api/vehicles/{id}
    DELETE /api/vehicles/{id}    (requires JWT)

Customers:

    GET    /api/customers
    GET    /api/customers/{id}
    POST   /api/customers

Mechanics:

    GET    /api/mechanics
    GET    /api/mechanics/{id}
    POST   /api/mechanics

Parts:

    GET    /api/parts
    GET    /api/parts/{id}
    POST   /api/parts

Service orders:

    GET    /api/serviceorders/{id}
    POST   /api/serviceorders

Auth:

    POST   /api/auth/login

## Authentication

Login with:

    { "username": "admin", "password": "Admin123!" }

The response contains a JWT. Send it on protected endpoints:

    Authorization: Bearer <token>

JWT settings (issuer, audience, key, expiration) are in `appsettings.json` under `Jwt`.

## Demo flow

To exercise the full stack from an empty database:

1. `POST /api/auth/login` to get a JWT, then click Authorize in Swagger.
2. `POST /api/customers` and keep the returned id.
3. `POST /api/mechanics` and keep the returned id.
4. `POST /api/parts` (twice for two different parts) and keep the ids.
5. `POST /api/vehicles` using the customer id from step 2.
6. `POST /api/serviceorders` using the vehicle, mechanic and part ids. This runs in a transaction, decreases part stock and computes total cost.
7. `GET /api/serviceorders/{id}` to read the saved order back.
8. `DELETE /api/vehicles/{id}` exercises the protected endpoint.

## Notes

- `CreatedAt` is stamped automatically by `AppDbContext.SaveChangesAsync` for new entities.
- `CreateServiceOrderCommand` runs in a database transaction, decreases part stock, and computes total cost as labor cost plus the sum of part quantity times unit price. It throws a business exception if stock is insufficient.
- Global exception middleware maps `NotFoundException` to 404, `ValidationException` and `BusinessException` to 400, others to 500.
