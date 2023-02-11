using Application.Core;
using Application.Interfaces;
using Application.Services;
using Application.Validators.Account;
using FluentValidation;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        services.AddValidatorsFromAssemblyContaining<LoginDtoRequestValidator>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IRentService, RentService>();
        services.AddScoped<IVehicleService, VehicleService>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IRentRepository, RentRepository>();
        services.AddScoped<ISeedRepository, SeedRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        services.AddHttpContextAccessor();

        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}