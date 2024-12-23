using EmployeeManager.Core.Interfaces;
using EmployeeManager.Core.Models;
using EmployeeManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeManager.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class responsible for CRUD operations on Employee data.
    /// Implements the <see cref="IEmployeeRepository"/> interface.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="context">The ApplicationDbContext instance used for interacting with the database.</param>
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets an employee by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee object with the specified ID, or null if not found.</returns>
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Manager) // Eager loading of the Manager navigation property
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Gets all employees asynchronously.
        /// </summary>
        /// <returns>An IEnumerable collection of all employee objects.</returns>
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Manager) // Eager loading of the Manager navigation property
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new employee asynchronously.
        /// </summary>
        /// <param name="employee">The employee object to be added.</param>
        /// <returns>The added employee object.</returns>
        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        /// <summary>
        /// Updates an existing employee asynchronously.
        /// </summary>
        /// <param name="employee">The employee object with updated properties.</param>
        /// <returns>Task</returns>
        public async Task UpdateAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an employee asynchronously.
        /// </summary>
        /// <param name="employee">The employee object to be deleted.</param>
        /// <returns>Task</returns>
        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all employees for a specific manager asynchronously.
        /// </summary>
        /// <param name="managerId">The ID of the manager whose employees need to be retrieved.</param>
        /// <returns>An IEnumerable collection of employee objects under the specified manager.</returns>
        public async Task<IEnumerable<Employee>> GetByManagerIdAsync(int managerId)
        {
            return await _context.Employees
                .Include(e => e.Manager) // Eager loading of the Manager navigation property
                .Where(e => e.ManagerId == managerId)
                .ToListAsync();
        }

        /// <summary>
        /// Searches for employees by name asynchronously.
        /// </summary>
        /// <param name="name">The name (or part of the name) to search for.</param>
        /// <returns>An IEnumerable collection of employee objects matching the search criteria.</returns>
        public async Task<IEnumerable<Employee>> SearchByNameAsync(string name)
        {
            return await _context.Employees
                .Include(e => e.Manager) // Eager loading of the Manager navigation property
                .Where(e => e.FullName.Contains(name))
                .ToListAsync();
        }

        /// <summary>
        /// Gets an employee by email asynchronously.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>An IEnumerable collection of employee objects matching the search criteria.</returns>
        public async Task<IEnumerable<Employee>> GetByEmailAsync(string email)
        {
            return await _context.Employees
                     .Include(e => e.Manager)
                     .Where(e => e.Email == email)
                     .ToListAsync();
        }
    }

}
