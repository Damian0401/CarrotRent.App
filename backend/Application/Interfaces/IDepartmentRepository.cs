using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IDepartmentRepository
    {
        List<Department> GetAllDepartments();
        Department? GetDepartmentById(Guid id);
    }
}