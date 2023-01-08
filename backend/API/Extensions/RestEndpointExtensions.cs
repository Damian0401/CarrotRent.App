using Application.Constants;
using Application.Dtos.Account;
using Application.Dtos.Vehicle;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class RestEndpointExtensions
{
    public static WebApplication MapRestEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapPost("/seed", 
        (ISeedRepository seedRepository) =>
        {
            seedRepository.SeedRoles();
            seedRepository.SeedFuels();
            seedRepository.SeedVehicleStatuses();
            seedRepository.SeedRentStatuses();
            seedRepository.SeedModelsAndBrands();
            seedRepository.SeedDepartments();

            return Results.Ok();
        });

        app.MapGet("/api/v1/vehicle/filters", 
        (IVehicleService service) =>
        {
            var data = service.GetVehicleFilterData();

            return Results.Ok(data);
        });

        app.MapPost("/api/v1/vehicle", 
        [Authorize(Roles=Roles.Manager)]
        ([FromBody] CreateVehicleDtoRequest dto, IVehicleService service) =>
        {
            var isSuccess = service.CreateVehicle(dto);

            return isSuccess
            ? Results.NoContent()
            : Results.BadRequest();
        });

        app.MapGet("/api/v1/vehicle", 
        (GetFilteredVehiclesDtoRequest dto, IVehicleService service) =>
        {
            var response = service.GetFilteredVehicles(dto);

            return Results.Ok(response);
        });

        app.MapGet("/api/v1/vehicle/{id:guid}", 
        ([FromRoute] Guid id, IVehicleService service) =>
        {
            var response = service.GetVehicleById(id);

            if (response is null)
                return Results.NotFound();

            return Results.Ok(response);
        });

        app.MapPut("/api/v1/vehicle/{id}", 
        [Authorize(Roles=Roles.Manager + "," + Roles.Employee)] 
        ([FromRoute] Guid id, UpdateVehicleDtoRequest dto, IVehicleService vehicleService) =>
        {
            var isUpdated = vehicleService.UpdateVehicle(id, dto);

            if (!isUpdated)
                return Results.NotFound();

            return Results.Ok();
        });

        app.MapDelete("/api/v1/vehicle/{id}", 
        [Authorize(Roles=Roles.Manager)] 
        ([FromRoute] Guid id, IVehicleService service) =>
        {
            var isDeleted = service.DeleteVehicle(id);

            if (!isDeleted)
                return Results.BadRequest();

            return Results.Ok();
        });

        app.MapGet("/api/v1/department", 
        (IDepartmentService service) =>
        {
            var response = service.GetAllDepartments();

            return Results.Ok(response);
        });

        app.MapGet("/api/v1/department/{id:guid}", 
        (Guid id, IDepartmentService service) =>
        {
            var response = service.GetDepartmentById(id);

            if (response is null)
                return Results.NotFound();

            return Results.Ok(response);
        });

        app.MapPost("/api/v1/account/login", 
        (LoginDtoRequest dto, IAccountService service) =>
        {
            var response = service.Login(dto);

            if (response is null)
                return Results.Unauthorized();

            return Results.Ok(response);
        });

        app.MapPost("/api/v1/account/register", 
        (RegisterDtoRequest dto, IAccountService service) =>
        {
            var response = service.Register(dto);

            if (response is null)
                return Results.BadRequest();

            return Results.Ok(response);
        });

        return app;
    }
}