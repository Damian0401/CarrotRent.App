using Application.Dtos.Account;
using Application.Dtos.Department;
using Application.Dtos.Vehicle;
using AutoMapper;
using Domain.Models;

namespace Application.Core;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        MapsForAddress();
        MapsForUser();
        MapsForVehicle();
        MapsForDepartment();
        MapsForRent();
        MapsForUser();
        MapsForModels();
        MapsForBrand();
        MapsForFuel();
    }
    
    private void MapsForVehicle()
    {
        CreateMap<Vehicle, GetVehicleByIdDtoResponse>()
            .ForMember(x => x.Price, s => 
                s.MapFrom(p => p.Price.PricePerDay))
            .ForMember(x => x.Department, s =>
                s.MapFrom(d => d.Department.Name))
            .ForMember(x => x.Status, s =>
                s.MapFrom(vs => vs.VehicleStatus.Name))
            .ForMember(x => x.Fuel, s =>
                s.MapFrom(f => f.Fuel.Type))
            .ForMember(x => x.Model, s =>
                s.MapFrom(m => m.Model.Name))
            .ForMember(x => x.Brand, s =>
                s.MapFrom(b => b.Model.Brand.Name));
        CreateMap<Vehicle, VehicleForGetFilteredVehiclesDtoResponse>()
            .ForMember(x => x.Brand, s => s.MapFrom(b => b.Model.Brand.Name))
            .ForMember(x => x.Model, s =>
                s.MapFrom(m => m.Model.Name));
        CreateMap<CreateVehicleDtoRequest, Vehicle>()
            .ForMember(x => x.Price, s => s.Ignore());
        CreateMap<UpdateVehicleDtoRequest, Vehicle>()
            .ForMember(x => x.Price, s => s.MapFrom(v => 
                new Price{PricePerDay = v.Price}));
        CreateMap<Vehicle, VehicleForGetDepartmentByIdDtoResponse>()
            .ForMember(x => x.Model, s =>
                s.MapFrom(v => v.Model.Name))
            .ForMember(x => x.Brand, s =>
                s.MapFrom(v => v.Model.Brand.Name));
    }

    private void MapsForUser()
    {
        CreateMap<RegisterDtoRequest, User>();
        CreateMap<RegisterDtoRequest, UserData>();
        CreateMap<User, RegisterDtoResponse>()
            .ForMember(x => x.Role, s =>
                s.MapFrom(u => u.Role.Name));
        CreateMap<User, LoginDtoResponse>()
            .ForMember(x => x.Role, s =>
                s.MapFrom(u => u.Role.Name))
            .ForMember(x => x.DepartmentIds, s =>
                s.MapFrom(u => u.DepartmentId == null 
                    ? u.OwnedDepartments.Select(x => x.Id) 
                    : u.OwnedDepartments.Select(x => x.Id)
                        .Append(u.DepartmentId.Value)));
    }

    private void MapsForDepartment()
    {
        CreateMap<Department, DepartmentForGetVehicleFilterDataDtoResponse>();
        CreateMap<Department, DepartmentForGetAllDepartmentsDtoResponse>()
            .ForMember(x => x.Address, s => 
                s.MapFrom(d => $"{d.Address.City}, {d.Address.Street}, " +
                    $"{d.Address.HouseNumber}/{d.Address.ApartmentNumber}"))
            .ForMember(x => x.XPosition, s =>
                s.MapFrom(d => d.Localization.XPosition))
            .ForMember(x => x.YPosition, s =>
                s.MapFrom(d => d.Localization.YPosition));
        CreateMap<Department, GetDepartmentByIdDtoResponse>()            
            .ForMember(x => x.Address, s => 
                s.MapFrom(d => $"{d.Address.City}, {d.Address.Street}, " +
                    $"{d.Address.HouseNumber}/{d.Address.ApartmentNumber}"))
            .ForMember(x => x.Manager, s =>
                s.MapFrom(d => $"{d.Manager.UserData.FirstName} {d.Manager.UserData.LastName}"))
            .ForMember(x => x.Vehicles, s =>
                s.MapFrom(d => d.Vehicles));
    }

    private void MapsForRent()
    {

    }

    private void MapsForAddress()
    {
        CreateMap<RegisterDtoRequest, Address>();
    }

    private void MapsForFuel()
    {
        CreateMap<Fuel, FuelForGetVehicleFilterDataDtoResponse>();
    }

    private void MapsForBrand()
    {
        CreateMap<Brand, BrandForGetVehicleFilterDataDtoResponse>()
            .ForMember(x => x.Models, s =>
                s.MapFrom(m => m.Models));
    }

    private void MapsForModels()
    {
        CreateMap<Model, ModelForGetVehicleFilterDataDtoResponse>();
    }
}