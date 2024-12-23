using EmployeeManager.Core.Constants;
using EmployeeManager.Core.DTOs;
using EmployeeManager.Core.Exceptions;
using EmployeeManager.Core.Interfaces;
using EmployeeManager.Core.Models;
using EmployeeManager.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeRepository _employeeRepository;
        private IManagerRepository _managerRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEmployeeRepository Employees =>
            _employeeRepository ??= new EmployeeRepository(_context);

        public IManagerRepository Managers =>
            _managerRepository ??= new ManagerRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
