using Application.Constants;
using Application.Dtos.Account;
using Application.Dtos.Rent;
using Application.Dtos.Vehicle;
using Application.Interfaces;
using FluentValidation;
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
        [Authorize(Roles = Roles.Manager)]
        ([FromBody] CreateVehicleDtoRequest dto, [FromServices] IVehicleService service,
            [FromServices] IValidator<CreateVehicleDtoRequest> validator) =>
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return Results.BadRequest(new { Errors = errors });
            }

            var isSuccess = service.CreateVehicle(dto);

            return isSuccess
                ? Results.NoContent()
                : Results.BadRequest();
        });

        app.MapGet("/api/v1/vehicle",
        (GetFilteredVehiclesDtoRequest dto, [FromServices] IVehicleService service,
            [FromServices] IValidator<GetFilteredVehiclesDtoRequest> validator) =>
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return Results.BadRequest(new { Errors = errors });
            }

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
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromRoute] Guid id, UpdateVehicleDtoRequest dto, [FromServices] IVehicleService vehicleService,
            [FromServices] IValidator<UpdateVehicleDtoRequest> validator) =>
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return Results.BadRequest(new { Errors = errors });
            }

            var isUpdated = vehicleService.UpdateVehicle(id, dto);

            if (!isUpdated)
                return Results.NotFound();

            return Results.Ok();
        });

        app.MapDelete("/api/v1/vehicle/{id}",
        [Authorize(Roles = Roles.Manager)]
        ([FromRoute] Guid id, [FromServices] IVehicleService service) =>
        {
            var isDeleted = service.DeleteVehicle(id);

            if (!isDeleted)
                return Results.BadRequest();

            return Results.Ok();
        });

        app.MapGet("/api/v1/department",
        ([FromServices] IDepartmentService service) =>
        {
            var response = service.GetAllDepartments();

            return Results.Ok(response);
        });

        app.MapGet("/api/v1/department/{id:guid}",
        ([FromRoute] Guid id, [FromServices] IDepartmentService service) =>
        {
            var response = service.GetDepartmentById(id);

            if (response is null)
                return Results.NotFound();

            return Results.Ok(response);
        });

        app.MapGet("/api/v1/department/{id:guid}/rents",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromRoute] Guid id, [FromServices] IRentService service) =>
        {
            var response = service.GetDepartmentRents(id);

            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest();
        });

        app.MapGet("/api/v1/department/{id:guid}/rents/archived",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromRoute] Guid id, [FromServices] IRentService service) =>
        {
            var response = service.GetDepartmentArchivedRents(id);

            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest();
        });

        app.MapPost("/api/v1/account/login",
        ([FromBody] LoginDtoRequest dto, [FromServices] IAccountService service,
            [FromServices] IValidator<LoginDtoRequest> validator) =>
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return Results.BadRequest(new { Errors = errors });
            }

            var response = service.Login(dto);

            if (response is null)
                return Results.Unauthorized();

            return Results.Ok(response);
        });

        app.MapPost("/api/v1/account/register",
        ([FromBody] RegisterDtoRequest dto, [FromServices] IAccountService service,
            [FromServices] IValidator<RegisterDtoRequest> validator) =>
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return Results.BadRequest(new { Errors = errors });
            }

            var response = service.Register(dto);

            if (response is null)
                return Results.BadRequest();

            return Results.Ok(response);
        });

        app.MapPost("/api/v1/account/verify/{id:guid}",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromRoute] Guid id, [FromServices] IAccountService service) =>
        {
            var response = service.VerifyUser(id);

            return response
                ? Results.NoContent()
                : Results.BadRequest();
        });

        app.MapGet("/api/v1/account/unverified",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromServices] IAccountService service) =>
        {
            var response = service.GetUnverifiedUsers();

            return Results.Ok(response);
        });

        app.MapGet("/api/v1/rent/{id:guid}",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee + "," + Roles.Client)]
        ([FromRoute] Guid id, [FromServices] IRentService service) =>
        {
            var response = service.GetRentById(id);

            return response is not null
                ? Results.Ok(response)
                : Results.NotFound();
        });

        app.MapPost("/api/v1/rent/{id:guid}/cancel",
        [Authorize(Roles = Roles.Client)]
        ([FromRoute] Guid id, [FromServices] IRentService service) =>
        {
            var response = service.CancelRent(id);

            return response
                ? Results.Ok()
                : Results.BadRequest();
        });

        app.MapPost("/api/v1/rent/{id:guid}/issue",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromRoute] Guid id, [FromServices] IRentService service) =>
        {
            var response = service.IssueRent(id);

            return response
                ? Results.Ok()
                : Results.BadRequest();
        });

        app.MapPost("/api/v1/rent/{id:guid}/receive",
        [Authorize(Roles = Roles.Manager + "," + Roles.Employee)]
        ([FromRoute] Guid id, [FromServices] IRentService service) =>
        {
            var response = service.ReceiveRent(id);

            return response
                ? Results.Ok(response)
                : Results.BadRequest();
        });

        app.MapGet("/api/v1/rent/my",
        [Authorize(Roles = Roles.Client)]
        ([FromServices] IRentService service) =>
        {
            var response = service.GetMyRents();

            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest();
        });

        app.MapGet("/api/v1/rent/my/archived",
        [Authorize(Roles = Roles.Client)]
        ([FromServices] IRentService service) =>
        {
            var response = service.GetMyArchivedRents();

            return response is not null
                ? Results.Ok(response)
                : Results.BadRequest();
        });


        app.MapPost("/api/v1/rent",
        [Authorize(Roles = Roles.Client)]
        ([FromBody] CreateRentDtoRequest dto, [FromServices] IRentService service,
            [FromServices] IValidator<CreateRentDtoRequest> validator) =>
        {
            var validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return Results.BadRequest(new { Errors = errors });
            }


            var response = service.CreateRent(dto);

            return response
                ? Results.NoContent()
                : Results.BadRequest();
        });

        return app;
    }
}