using Application.Constants;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Infrastructure.Repositories;

public class SeedRepository : ISeedRepository
{
    private readonly DataContext _context;
    public SeedRepository(DataContext context)
    {
        _context = context;
    }

    public void SeedDepartments()
    {
        if (_context.Departments.Count() > 0)
            return;

        var availableVehicleStatus = _context
            .VehicleStatuses
            .FirstOrDefault(x => x.Name.Equals(VehicleStatuses.Available));

        if (availableVehicleStatus is null)
            throw new InvalidDataException();

        var managerRole = _context
            .Roles
            .FirstOrDefault(x => x.Name.Equals(Roles.Manager));

        var fuels = _context
            .Fuels
            .ToList();

        var models = _context
            .Models
            .ToList();

        var prices = new List<Price>
        {
            new Price
            {
                Id = Guid.NewGuid(),
                PricePerDay = 50.1
            },
            new Price
            {
                Id = Guid.NewGuid(),
                PricePerDay = 100.2
            },
            new Price
            {
                Id = Guid.NewGuid(),
                PricePerDay = 150.3
            },
        };

        var imageUrl = @"https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.pngitem.com%2Fpimgs%2Fm%2F135-1359965_transparent-carrots-png-vegetable-carrot-png-download.png&f=1&nofb=1&ipt=d95e94cc1dc182d7d2f2c327ace0bf6d0fed497ed581fd97a994774904245647&ipo=images";

        if (managerRole is null)
            throw new InvalidDataException();

        var hasher = new PasswordHasher<User>();

        var random = new Random();

        var managerAddress = new Address
        {
            ApartmentNumber = "123",
            City = "Wroclaw",
            HouseNumber = "456",
            PostCode = "22-333",
            Street = "Nicewarner",
        };

        var managerUserData = new UserData
        {
            Address = managerAddress,
            Email = "manager@gmail.com",
            FirstName = "John",
            LastName = "Smith",
            Pesel = "987654321",
            PhoneNumber = "+48123456789",
        };

        var firstManager = new User
        {
            Login = "firstAdmin",
            Role = managerRole,
            UserData = managerUserData,
        };
        firstManager.PasswordHash = hasher.HashPassword(firstManager, "admin123");
        var secondManager = new User
        {
            Login = "secondAdmin",
            Role = managerRole,
            UserData = managerUserData,
        };
        secondManager.PasswordHash = hasher.HashPassword(firstManager, "admin123");
        var thirdManager = new User
        {
            Login = "thirdAdmin",
            Role = managerRole,
            UserData = managerUserData,
        };
        thirdManager.PasswordHash = hasher.HashPassword(firstManager, "admin123");
        var departments = new List<Department>
        {
            new Department
            {
                Name = "Never ending story",
                Address = new Address
                {
                    ApartmentNumber = "123",
                    City = "Wroclaw",
                    HouseNumber = "3C",
                    PostCode = "11-123",
                    Street = "Hidden",
                },
                Localization = new Localization
                {
                    XPosition = 51.1078852,
                    YPosition = 17.0385376
                },
                Manager = firstManager,
                Vehicles = new List<Vehicle>
                {
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-123A",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    },
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-456A",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    },
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-789A",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    }
                }
            },
            new Department
            {
                Name = "Long live the king",
                Address = new Address
                {
                    ApartmentNumber = "456",
                    City = "Wroclaw",
                    HouseNumber = "7C",
                    PostCode = "11-123",
                    Street = "Stanton",
                },
                Localization = new Localization
                {
                    XPosition = 51.1151852,
                    YPosition = 17.0525376
                },
                Manager = secondManager,
                Vehicles = new List<Vehicle>
                {
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-123B",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    },
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-456B",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    },
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-789B",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    }
                }
            },
            new Department
            {
                Name = "Less is more",
                Address = new Address
                {
                    ApartmentNumber = "789",
                    City = "Wroclaw",
                    HouseNumber = "12C",
                    PostCode = "11-123",
                    Street = "Rosemoore",
                },
                Localization = new Localization
                {
                    XPosition = 51.1125852,
                    YPosition = 17.0275376
                },
                Manager = thirdManager,
                Vehicles = new List<Vehicle>
                {
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-123C",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    },
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-456C",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    },
                    new Vehicle
                    {
                        ImageUrl = imageUrl,
                        Description = "This is the vehicle",
                        Fuel = fuels[random.Next(fuels.Count())],
                        Model = models[random.Next(models.Count())],
                        Price = prices[random.Next(prices.Count())],
                        Registration = "DW-789C",
                        Vin = Guid.NewGuid().ToString(),
                        Seats = random.Next(4, 8),
                        YearOfProduction = DateTime.Now.AddYears(-random.Next(10)).Year,
                        VehicleStatus = availableVehicleStatus 
                    }
                }
            }
        };

        _context.Departments.AddRange(departments);
        _context.SaveChanges();
    }

    public void SeedFuels()
    {
        if (_context.Fuels.Count() > 0)
            return;

        var fuelsToSeed = new List<Fuel>
        {
            new Fuel
            {
                Type = Fuels.Petrol
            },
            new Fuel
            {
                Type = Fuels.Diesel
            },
            new Fuel
            {
                Type = Fuels.Gas
            },
            new Fuel
            {
                Type = Fuels.Hybrid
            },
        };

        _context.Fuels.AddRange(fuelsToSeed);
        _context.SaveChanges();
    }

    public void SeedModelsAndBrands()
    {
        if (_context.Brands.Count() > 0)
            return;

        var brands = new List<Brand>
        {
            new Brand
            {
                Name = "BMW",
                Models = new List<Model>
                {
                    new Model
                    {
                        Name = "E23"
                    },
                    new Model
                    {
                        Name = "E39"
                    },
                    new Model
                    {
                        Name = "Z8"
                    },
                    new Model
                    {
                        Name = "X1"
                    },
                    new Model
                    {
                        Name = "M3"
                    },
                }
            },
            new Brand
            {
                Name = "Audi",
                Models = new List<Model>
                {
                    new Model
                    {
                        Name = "Q5"
                    },
                    new Model
                    {
                        Name = "A3"
                    },
                    new Model
                    {
                        Name = "A6"
                    },
                    new Model
                    {
                        Name = "R8"
                    },
                    new Model
                    {
                        Name = "RS6"
                    },
                }
            },
            new Brand
            {
                Name = "Skoda",
                Models = new List<Model>
                {
                    new Model
                    {
                        Name = "Felicia"
                    },
                    new Model
                    {
                        Name = "Octavia"
                    },
                    new Model
                    {
                        Name = "Citigo"
                    },
                    new Model
                    {
                        Name = "Yeti"
                    },
                    new Model
                    {
                        Name = "Fabia"
                    },
                }
            }
        };

        _context.Brands.AddRange(brands);
        _context.SaveChanges();
    }

    public void SeedRentStatuses()
    {
        if (_context.RentStatuses.Count() > 0)
            return;

        var rentStatusesToSeed = new List<RentStatus>
        {
            new RentStatus
            {
                Name = RentStatuses.Reserved
            },
            new RentStatus
            {
                Name = RentStatuses.Active
            },
            new RentStatus
            {
                Name = RentStatuses.Archived
            },
        };

        _context.RentStatuses.AddRange(rentStatusesToSeed);
        _context.SaveChanges();
    }

    public void SeedRoles()
    {
        if (_context.Roles.Count() > 0)
            return;

        var rolesToSeed = new List<Role>
        {
            new Role
            {
                Name = Roles.Manager
            },
            new Role
            {
                Name = Roles.Employee
            },
            new Role
            {
                Name = Roles.Client
            },
            new Role
            {
                Name = Roles.Unverified
            },
        };

        _context.Roles.AddRange(rolesToSeed);
        _context.SaveChanges();
    }

    public void SeedVehicleStatuses()
    {
        if (_context.VehicleStatuses.Count() > 0)
            return;

        var vehicleStatusesToSeed = new List<VehicleStatus>
        {
            new VehicleStatus
            {
                Name = VehicleStatuses.Available
            },
            new VehicleStatus
            {
                Name = VehicleStatuses.Unavailable
            },
        };

        _context.VehicleStatuses.AddRange(vehicleStatusesToSeed);
        _context.SaveChanges();
    }
}