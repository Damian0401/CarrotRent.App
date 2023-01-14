using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserData>()
            .HasMany(x => x.Users)
            .WithOne(x => x.UserData)
            .HasForeignKey(x => x.UserDataId);
        builder.Entity<Address>()
            .HasOne(x => x.UserData)
            .WithOne(x => x.Address)
            .HasForeignKey<UserData>(x => x.AddressId);
        builder.Entity<Address>()
            .HasOne(x => x.Department)
            .WithOne(x => x.Address)
            .HasForeignKey<Department>(x => x.AddressId);
        builder.Entity<User>()
            .HasOne(x => x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .IsRequired(false);
        builder.Entity<Department>()
            .HasOne(x => x.Manager)
            .WithMany(x => x.OwnedDepartments)
            .HasForeignKey(x => x.ManagerId)
            .IsRequired(false);
        builder.Entity<User>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId);
        builder.Entity<Vehicle>()
            .HasOne(x => x.VehicleStatus)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.VehicleStatusId);
        builder.Entity<Vehicle>()
            .HasOne(x => x.Price)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.PriceId);
        builder.Entity<Vehicle>()
          .HasOne(x => x.Fuel)
          .WithMany(x => x.Vehicles)
          .HasForeignKey(x => x.FuelId);
        builder.Entity<Vehicle>()
            .HasOne(x => x.VehicleStatus)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.VehicleStatusId);
        builder.Entity<Model>()
            .HasOne(x => x.Brand)
            .WithMany(x => x.Models)
            .HasForeignKey(x => x.BrandId);
        builder.Entity<Vehicle>()
            .HasOne(x => x.Department)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.DepartmentId);
        builder.Entity<Rent>()
            .HasOne(x => x.Client)
            .WithMany(x => x.OwnRentings)
            .HasForeignKey(x => x.ClientId)
            .IsRequired(false);
        builder.Entity<Rent>()
            .HasOne(x => x.Renter)
            .WithMany(x => x.IssuedRentings)
            .HasForeignKey(x => x.RenterId)
            .IsRequired(false);
        builder.Entity<Rent>()
            .HasOne(x => x.Receiver)
            .WithMany(x => x.RecievedRentings)
            .HasForeignKey(x => x.ReceiverId)
            .IsRequired(false);
        builder.Entity<Rent>()
            .HasOne(x => x.RentStatus)
            .WithMany(x => x.Rentings)
            .HasForeignKey(x => x.RentStatusId);
        builder.Entity<Rent>()
            .HasOne(x => x.Vehicle)
            .WithMany(x => x.Rentings)
            .HasForeignKey(x => x.VehicleId);
        builder.Entity<Localization>()
            .HasOne(x => x.Department)
            .WithOne(x => x.Localization)
            .HasForeignKey<Department>(x => x.LocalizationId);

        builder.Entity<UserData>()
            .HasIndex(x => x.PhoneNumber)
            .IsUnique();
        builder.Entity<UserData>()
            .HasIndex(x => x.Pesel)
            .IsUnique();
        builder.Entity<Vehicle>()
            .HasIndex(x => x.Registration)
            .IsUnique();
        builder.Entity<Vehicle>()
           .HasIndex(x => x.Vin)
           .IsUnique();
        builder.Entity<User>()
            .HasIndex(x => x.Login)
            .IsUnique();

        builder.Entity<Brand>()
            .Property(x => x.Name)
            .HasMaxLength(255);
        builder.Entity<Model>()
            .Property(x => x.Name)
            .HasMaxLength(255);
        builder.Entity<Vehicle>()
            .Property(x => x.ImageUrl)
            .HasMaxLength(255);
        builder.Entity<Vehicle>()
            .Property(x => x.Description)
            .HasMaxLength(255);
        builder.Entity<Vehicle>()
            .Property(x => x.Registration)
            .HasMaxLength(255);
        builder.Entity<Vehicle>()
            .Property(x => x.Vin)
            .HasMaxLength(255);
        builder.Entity<Fuel>()
            .Property(x => x.Type)
            .HasMaxLength(255);
        builder.Entity<VehicleStatus>()
            .Property(x => x.Name)
            .HasMaxLength(255);
        builder.Entity<Address>()
            .Property(x => x.PostCode)
            .HasMaxLength(255);
        builder.Entity<Address>()
            .Property(x => x.City)
            .HasMaxLength(255);
        builder.Entity<Address>()
            .Property(x => x.Street)
            .HasMaxLength(255);
        builder.Entity<Address>()
            .Property(x => x.HouseNumber)
            .HasMaxLength(255);
        builder.Entity<Address>()
            .Property(x => x.ApartmentNumber)
            .HasMaxLength(255);
        builder.Entity<UserData>()
            .Property(x => x.FirstName)
            .HasMaxLength(255);
        builder.Entity<UserData>()
            .Property(x => x.LastName)
            .HasMaxLength(255);
        builder.Entity<UserData>()
            .Property(x => x.PhoneNumber)
            .HasMaxLength(255);
        builder.Entity<UserData>()
            .Property(x => x.Email)
            .HasMaxLength(255);
        builder.Entity<Department>()
            .Property(x => x.Name)
            .HasMaxLength(255);
        builder.Entity<User>()
            .Property(x => x.Login)
            .HasMaxLength(255);
        builder.Entity<User>()
            .Property(x => x.PasswordHash)
            .HasMaxLength(255);
        builder.Entity<RentStatus>()
            .Property(x => x.Name)
            .HasMaxLength(255);
        builder.Entity<Role>()
            .Property(x => x.Name)
            .HasMaxLength(255);
    }

    public DbSet<Address> Addresses { get; set; } = default!;
    public DbSet<Brand> Brands { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<Fuel> Fuels { get; set; } = default!;
    public DbSet<Localization> Localizations { get; set; } = default!;
    public DbSet<Model> Models { get; set; } = default!;
    public DbSet<Price> Prices { get; set; } = default!;
    public DbSet<Rent> Rents { get; set; } = default!;
    public DbSet<RentStatus> RentStatuses { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserData> UserData { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
    public DbSet<VehicleStatus> VehicleStatuses { get; set; } = default!;
}