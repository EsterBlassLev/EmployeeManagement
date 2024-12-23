
using EmployeeManager.Core.DTOs;

namespace EmployeeManager.Core.Interfaces
{
    public interface IManagerService
    {
        Task<(bool success, string message, string token)> RegisterManager(RegisterManagerDto dto);
        Task<(bool success, string message, string token)> Login(LoginDto dto);
    }
}