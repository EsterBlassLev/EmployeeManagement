using EmployeeManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(Manager manager);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
