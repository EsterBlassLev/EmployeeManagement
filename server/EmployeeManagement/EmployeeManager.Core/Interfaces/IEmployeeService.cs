using EmployeeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> CreateEmployee(CreateEmployeeDto dto, int managerId);
        Task<EmployeeDto> GetEmployee(int id, int managerId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByManagerId(int managerId);
        Task<IEnumerable<EmployeeDto>> SearchEmployees(string name, int managerId);
        Task<bool> UpdateEmployee(int id, UpdateEmployeeDto dto, int managerId);
        Task<bool> DeleteEmployee(int id, int managerId);
    }
}
