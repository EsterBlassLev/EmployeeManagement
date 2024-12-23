using EmployeeManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Core.Interfaces
{
    public interface IManagerRepository : IRepository<Manager>
    {
        Task<IEnumerable<Manager>> GetByEmailAsync(string email);
    }
}
