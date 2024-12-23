using EmployeeManager.Core.DTOs;
using EmployeeManager.Core.Interfaces;
using EmployeeManager.Core.Models;

namespace EmployeeManagement.Application.Services
{
    /// <summary>
    /// Service class for managing managers (registration and login).
    /// Implements the <see cref="IManagerService"/> interface.
    /// </summary>
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerService"/> class.
        /// </summary>
        /// <param name="managerRepository">The manager repository.</param>
        /// <param name="authService">The authentication service.</param>
        public ManagerService(IManagerRepository managerRepository, IAuthService authService)
        {
            _managerRepository = managerRepository;
            _authService = authService;
        }

        /// <summary>
        /// Registers a new manager.
        /// </summary>
        /// <param name="dto">The data transfer object containing manager registration details.</param>
        /// <returns>A tuple containing success flag, message, and token on success, or error message on failure.</returns>
        public async Task<(bool success, string message, string token)> RegisterManager(RegisterManagerDto dto)
        {
            var existingManager = await _managerRepository.GetByEmailAsync(dto.Email);
            if (existingManager?.Any() == true)
            {
                return (false, "Email already exists", null);
            }

            var manager = new Manager
            {
                Email = dto.Email,
                PasswordHash = _authService.HashPassword(dto.Password),
                FullName = dto.FullName
            };

            await _managerRepository.AddAsync(manager);
            var token = await _authService.GenerateJwtToken(manager);

            return (true, "Registration successful", token);
        }

        /// <summary>
        /// Logs in a manager.
        /// </summary>
        /// <param name="dto">The data transfer object containing manager login credentials.</param>
        /// <returns>A tuple containing success flag, message, and token on success, or error message on failure.</returns>
        public async Task<(bool success, string message, string token)> Login(LoginDto dto)
        {
            var manager = await _managerRepository.GetByEmailAsync(dto.Email);
            var validManager = manager?.FirstOrDefault(m => m != null && _authService.VerifyPassword(dto.Password, m.PasswordHash));
            if (validManager == null)
            {
                return (false, "Invalid credentials", null);
            }

            var tokenTask = _authService.GenerateJwtToken(validManager);
            var token = (await tokenTask).ToString();

            return (true, "Login successful", token);
        }
    }
}