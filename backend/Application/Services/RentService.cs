using Application.Constants;
using Application.Dtos.Rent;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;

namespace Application.Services;

public class RentService : IRentService
{
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;
    private readonly IRentRepository _rentRepository;
    public RentService(IMapper mapper, IUserAccessor userAccessor, IRentRepository rentRepository)
    {
        _rentRepository = rentRepository;
        _userAccessor = userAccessor;
        _mapper = mapper;
    }
    public bool CancelRent(Guid rentId)
    {
        var rent = _rentRepository.GetRentById(rentId);

        if (rent is null)
            return false;

        var rentDepartmentId = _rentRepository.GetRentDepartmentId(rent.Id);

        if (rentDepartmentId is null)
            return false;

        var user = _userAccessor.GetCurrentlyLoggedUser();

        var isUserRentOwner = IsUserRentOwner(rent, user);
        var isUserManagerOrEmployee = IsUserManagerOrEmployee(rentDepartmentId.Value, user);

        if (!isUserRentOwner && !isUserManagerOrEmployee)
            return false;

        var reservedStatusId = _rentRepository
            .GetRentStatusIdByName(RentStatuses.Reserved);

        if (!rent.RentStatusId.Equals(reservedStatusId))
            return false;

        var archivedStatusId = _rentRepository
            .GetRentStatusIdByName(RentStatuses.Archived);

        if (archivedStatusId is null)
            return false;

        rent.RentStatusId = archivedStatusId.Value;

        var isUpdated = _rentRepository.UpdateRent(rent);

        return isUpdated;
    }

    public bool CreateRent(CreateRentDtoRequest dto)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null || !user.Role.Name.Equals(Roles.Client))
            return false;

        var rentStatusId = _rentRepository.GetRentStatusIdByName(RentStatuses.Reserved);

        if (rentStatusId is null)
            return false;

        var mappedRent = _mapper.Map<Rent>(dto);
        mappedRent.ClientId = user.Id;
        mappedRent.RentStatusId = rentStatusId.Value;

        var isCreated = _rentRepository.CreateRent(mappedRent);

        return isCreated;
    }

    public GetDepartmentArchivedRentsDtoResponse? GetDepartmentArchivedRents(Guid departmentId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();
        var isUserManagerOrEmployee = IsUserManagerOrEmployee(departmentId, user);

        if (!isUserManagerOrEmployee)
            return null;

        var archivedRents = _rentRepository.GetDepartmentArchivedRents(departmentId);

        var mappedRents = _mapper
            .Map<List<RentForGetDepartmentArchivedRentsDtoResponse>>(archivedRents);

        var response = new GetDepartmentArchivedRentsDtoResponse
        {
            Rents = mappedRents
        };

        return response;
    }

    public GetDepartmentRentsDtoResponse? GetDepartmentRents(Guid departmentId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();
        var isUserManagerOrEmployee = IsUserManagerOrEmployee(departmentId, user);

        if (!isUserManagerOrEmployee)
            return null;

        var archivedRents = _rentRepository.GetDepartmentRents(departmentId);

        var mappedRents = _mapper
            .Map<List<RentForGetDepartmentRentsDtoResponse>>(archivedRents);

        var response = new GetDepartmentRentsDtoResponse
        {
            Rents = mappedRents
        };

        return response;
    }

    public GetMyArchivedRentsDtoResponse? GetMyArchivedRents()
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null || !user.Role.Name.Equals(Roles.Client))
            return null;

        var archivedRents = _rentRepository.GetUserArchivedRents(user.Id);

        var mappedRents = _mapper
            .Map<List<RentForGetMyArchivedRentsDtoResponse>>(archivedRents);

        var response = new GetMyArchivedRentsDtoResponse
        {
            Rents = mappedRents
        };

        return response;
    }

    public GetMyRentsDtoResponse? GetMyRents()
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null || !user.Role.Name.Equals(Roles.Client))
            return null;

        var archivedRents = _rentRepository.GetUserRents(user.Id);

        var mappedRents = _mapper
            .Map<List<RentForGetMyRentsDtoResponse>>(archivedRents);

        var response = new GetMyRentsDtoResponse
        {
            Rents = mappedRents
        };

        return response;
    }

    public GetRentByIdDtoResponse? GetRentById(Guid rentId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();

        if (user is null)
            return null;

        var rent = _rentRepository.GetRentById(rentId);

        if (rent is null)
            return null;

        if (UserIsNotClient(user.Id, rent.ClientId) &&
            UserIsNotReceiver(user.Id, rent.ReceiverId) &&
            UserIsNotRenter(user.Id, rent.RenterId))
            return null;

        var response = _mapper.Map<GetRentByIdDtoResponse>(rent);

        return response;
    }

    public bool IssueRent(Guid rentId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();
        var rent = _rentRepository.GetRentById(rentId);

        var userCanManageRent = UserCanManageRent(user, rent);

        if (!userCanManageRent)
            return false;

        var activeStatusId = _rentRepository.GetRentStatusIdByName(RentStatuses.Active);

        if (activeStatusId is null)
            return false;

        rent!.RenterId = user!.Id;
        rent.RentStatusId = activeStatusId.Value;

        var isUpdated = _rentRepository.UpdateRent(rent);

        return isUpdated;
    }

    public ReceiveRentDtoResponse? ReceiveRent(Guid rentId)
    {
        var user = _userAccessor.GetCurrentlyLoggedUser();
        var rent = _rentRepository.GetRentById(rentId);

        var userCanManageRent = UserCanManageRent(user, rent);

        if (!userCanManageRent)
            return null;

        var archivedStatusId = _rentRepository.GetRentStatusIdByName(RentStatuses.Archived);

        var reservedDays = (rent!.EndDate - rent.StartDate).Days;
        var exceededDays = (DateTime.Now - rent.EndDate).Days;

        if (archivedStatusId is null)
            return null;

        rent.RentStatusId = archivedStatusId.Value;
        rent.EndDate = DateTime.Now;
        rent.ReceiverId = user!.Id;

        var pricePerDay = _rentRepository.GetVehiclePrice(rent.VehicleId);

        if (pricePerDay is null)
            return null;

        var isUpdated = _rentRepository.UpdateRent(rent);

        if (!isUpdated)
            return null;

        var price = pricePerDay.Value * (reservedDays + 2 * exceededDays);

        var response = new ReceiveRentDtoResponse
        {
            TotalPrice = price,
        };

        return response;
    }

    private bool IsUserManagerOrEmployee(Guid departmentId, User? user)
    {
        if (user is null)
            return false;

        var department = _rentRepository.GetDepartmentById(departmentId);

        if (department is null)
            return false;

        if (!department.ManagerId.Equals(user.Id) &&
            !department.Employees.Any(x => x.Id.Equals(user.Id)))
            return false;

        return true;
    }

    private bool UserIsNotRenter(Guid userId, Guid? renterId)
    {
        if (renterId is null)
            return false;

        var isUserRenter = !renterId.Equals(userId);

        return isUserRenter;
    }

    private bool UserIsNotReceiver(Guid userId, Guid? receiverId)
    {
        if (receiverId is null)
            return false;

        var isUserReceiver = !receiverId.Equals(userId);

        return isUserReceiver;
    }

    private bool UserIsNotClient(Guid userId, Guid clientId)
    {
        var isUserClient = !userId.Equals(clientId);

        return isUserClient;
    }

    private bool UserCanManageRent(User? user, Rent? rent)
    {
        if (rent is null)
            return false;

        var reservedStatusId = _rentRepository
            .GetRentStatusIdByName(RentStatuses.Reserved);

        if (!rent.RentStatusId.Equals(reservedStatusId))
            return false;

        var departmentId = _rentRepository.GetRentDepartmentId(rent.Id);

        if (departmentId is null)
            return false;

        var isUserManagerOrEmployee = IsUserManagerOrEmployee(departmentId.Value, user);

        if (!isUserManagerOrEmployee)
            return false;

        return true;
    }

    private bool IsUserRentOwner(Rent rent, User? user)
    {
        if (user is null)
            return false;

        if (user.Role.Name.Equals(Roles.Client) && !rent.ClientId.Equals(user.Id))
            return false;

        return true;
    }
}