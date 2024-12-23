using AutoMapper;
using EmployeeManagement.Application.Extensions;
using EmployeeManager.Core.Constants;
using EmployeeManager.Core.DTOs;
using EmployeeManager.Core.Exceptions;
using EmployeeManager.Core.Interfaces;
using EmployeeManager.Core.Models;
using EmployeeManager.Infrastructure.Data;
using System.Data;

namespace EmployeeManagement.Application.Services
{
    /// <summary>
    /// Service class for managing employees.
    /// Implements the <see cref="IEmployeeService"/> interface.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IDbConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for data access.</param>
        /// <param name="authService">The authentication service.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="logger">The logger for recording service activity.</param>
        /// <param name="connection">The database connection (optional).</param>
        public EmployeeService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IMapper mapper,
            ILogger<EmployeeService> logger,
            IDbConnection connection = null)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
            _connection = connection;
        }

        /// <summary>
        /// Creates a new employee under the specified manager.
        /// </summary>
        /// <param name="dto">The data transfer object containing employee creation details.</param>
        /// <param name="managerId">The ID of the manager to whom the employee belongs.</param>
        /// <returns>An EmployeeDto representing the created employee.</returns>
        /// <exception cref="BadRequestException">Thrown if the email already exists.</exception>
        /// <exception cref="NotFoundException">Thrown if the manager is not found.</exception>
        public async Task<EmployeeDto> CreateEmployee(CreateEmployeeDto dto, int managerId)
        {
            _logger.LogInformation("Creating new employee for manager {ManagerId}", managerId);
            ValidationExtensions.ValidateEmployee(dto);

            // Check email uniqueness
            var existingEmployee = await _unitOfWork.Employees.GetByEmailAsync(dto.Email);
            if (existingEmployee?.Any() == true)
            {
                throw new BadRequestException(ErrorMessages.EmailAlreadyExists);
            }

            // Check manager existence
            var manager = await _unitOfWork.Managers.GetByIdAsync(managerId);
            if (manager == null)
            {
                throw new NotFoundException(ErrorMessages.ManagerNotFound);
            }

            var employee = new Employee
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = _authService.HashPassword(dto.Password),
                ManagerId = managerId,
                Guid = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} created successfully", employee.Id);

            return _mapper.Map<EmployeeDto>(employee);
        }

        /// <summary>
        /// Deletes an employee.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <param name="managerId">The ID of the manager who is trying to delete the employee.</param>
        /// <returns>True if the employee is deleted successfully, false otherwise.</returns>
        /// <exception cref="NotFoundException">Thrown if the employee is not found.</exception>
        /// <exception cref="UnauthorizedException">Thrown if the manager is not authorized to delete the employee.</exception>
        public async Task<bool> DeleteEmployee(int id, int managerId)
        {
            _logger.LogInformation("Deleting employee {EmployeeId}", id);

            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException(ErrorMessages.EmployeeNotFound);
            }

            if (employee.ManagerId != managerId)
            {
                throw new UnauthorizedException(ErrorMessages.UnauthorizedAccess);
            }

            await _unitOfWork.Employees.DeleteAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} deleted successfully", id);

            return true;
        }

        public async Task<EmployeeDto> GetEmployee(int id, int managerId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException(ErrorMessages.EmployeeNotFound);
            }

            if (employee.ManagerId != managerId)
            {
                throw new UnauthorizedException(ErrorMessages.UnauthorizedAccess);
            }

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByManagerId(int managerId)
        {
            var employees = await _unitOfWork.Employees.GetByManagerIdAsync(managerId);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployees(string name, int managerId)
        {
            var employees = await _unitOfWork.Employees.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<EmployeeDto>>(
                employees.Where(e => e.ManagerId == managerId));
        }

        public async Task<bool> UpdateEmployee(int id, UpdateEmployeeDto dto, int managerId)
        {
            _logger.LogInformation("Updating employee {EmployeeId}", id);

            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException(ErrorMessages.EmployeeNotFound);
            }

            if (employee.ManagerId != managerId)
            {
                throw new UnauthorizedException(ErrorMessages.UnauthorizedAccess);
            }

            // Check email uniqueness if changed
            if (employee.Email != dto.Email)
            {
                var existingEmployee = await _unitOfWork.Employees.GetByEmailAsync(dto.Email);
                if (existingEmployee != null && existingEmployee.Count()> 1)
                {
                    throw new BadRequestException(ErrorMessages.EmailAlreadyExists);
                }
            }

            employee.Email = dto.Email;
            employee.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                employee.PasswordHash = _authService.HashPassword(dto.Password);
            }

            await _unitOfWork.Employees.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} updated successfully", id);

            return true;
        }
    }

}
