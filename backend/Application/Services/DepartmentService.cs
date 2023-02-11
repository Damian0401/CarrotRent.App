using Application.Dtos.Department;
using Application.Interfaces;
using AutoMapper;

namespace Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }
        public GetAllDepartmentsDtoResponse GetAllDepartments()
        {
            var departments = _departmentRepository.GetAllDepartments();

            var mappedDepartments = _mapper.Map<List<DepartmentForGetAllDepartmentsDtoResponse>>(departments);
            var response = new GetAllDepartmentsDtoResponse
            {
                Departments = mappedDepartments
            };

            return response;
        }

        public GetDepartmentByIdDtoResponse? GetDepartmentById(Guid id)
        {
            var department = _departmentRepository
                .GetDepartmentById(id);

            if (department is null)
                return null;

            var response = _mapper.Map<GetDepartmentByIdDtoResponse>(department);

            return response;
        }
    }
}