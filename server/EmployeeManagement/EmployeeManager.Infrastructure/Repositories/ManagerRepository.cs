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
    /// Repository class responsible for CRUD operations on Manager data.
    /// Implements the <see cref="IManagerRepository"/> interface.
    /// </summary>
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerRepository"/> class.
        /// </summary>
        /// <param name="context">The ApplicationDbContext instance used for interacting with the database.</param>
        public ManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a manager by email asynchronously.
        /// </summary>
        /// <param name="email">The email address of the manager to retrieve.</param>
        /// <returns>The manager object with the specified email, or null if not found.</returns>
        public async Task<IEnumerable<Manager>> GetByEmailAsync(string email)
        {
            return await _context.Managers
                .Where(e => e.Email.Equals(email)) // Case-sensitive comparison for email addresses
                .ToListAsync();
        }

        /// <summary>
        /// Gets a manager by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the manager to retrieve.</param>
        /// <returns>The manager object with the specified ID, or null if not found.</returns>
        public async Task<Manager> GetByIdAsync(int id)
        {
            return await _context.Managers
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Gets all managers asynchronously.
        /// </summary>
        /// <returns>An IEnumerable collection of all manager objects.</returns>
        public async Task<IEnumerable<Manager>> GetAllAsync()
        {
            return await _context.Managers
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new manager asynchronously.
        /// </summary>
        /// <param name="manager">The manager object to be added.</param>
        /// <returns>The added manager object.</returns>
        public async Task<Manager> AddAsync(Manager manager)
        {
            await _context.Managers.AddAsync(manager);
            await _context.SaveChangesAsync();
            return manager;
        }

        /// <summary>
        /// Updates an existing manager asynchronously.
        /// </summary>
        /// <param name="manager">The manager object with updated properties.</param>
        /// <returns>Task</returns>
        public async Task UpdateAsync(Manager manager)
        {
            _context.Entry(manager).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a manager asynchronously.
        /// </summary>
        /// <param name="manager">The manager object to be deleted.</param>
        /// <returns>Task</returns>
        public async Task DeleteAsync(Manager manager)
        {
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
        }
    }
}