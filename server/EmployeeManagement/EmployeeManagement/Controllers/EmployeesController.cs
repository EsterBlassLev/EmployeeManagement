using EmployeeManager.Core.DTOs;
using EmployeeManager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.Application.Controllers
{
    /// <summary>
    /// Web API controller for managing employees.
    /// Requires authorization for all actions.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="employeeService">The employee service.</param>
        /// <param name="logger">The logger.</param>
        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all employees managed by the current user (manager).
        /// </summary>
        /// <returns>An IActionResult containing a list of employees or an error message.</returns>
        [HttpGet("GetEmployeesByManagerId")]
        public async Task<IActionResult> GetEmployeesByManagerId()
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var employees = await _employeeService.GetEmployeesByManagerId(managerId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employees");
                return StatusCode(500, "An error occurred while retrieving employees");
            }
        }

        /// <summary>
        /// Searches for employees by name, filtered by the current user's managed employees.
        /// </summary>
        /// <param name="name">The name (or part of the name) to search for.</param>
        /// <returns>An IActionResult containing a list of matching employees or an error message.</returns>
        [HttpGet("SearchEmployees")]
        public async Task<IActionResult> SearchEmployees([FromQuery] string name)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var employees = await _employeeService.SearchEmployees(name, managerId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching employees");
                return StatusCode(500, "An error occurred while searching employees");
            }
        }

        /// <summary>
        /// Creates a new employee under the current user (manager).
        /// </summary>
        /// <param name="dto">The data transfer object containing employee creation details.</param>
        /// <returns>An IActionResult containing the created employee or an error message.</returns>
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var employee = await _employeeService.CreateEmployee(dto, managerId);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return StatusCode(500, "An error occurred while creating employee");
            }
        }

        /// <summary>
        /// Gets an employee by ID, filtered by the current user's managed employees.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>An IActionResult containing the employee data or an error message.</returns>
        [HttpGet("GetEmployee/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var employee = await _employeeService.GetEmployee(id, managerId);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee");
                return StatusCode(500, "An error occurred while retrieving employee");
            }
        }

        /// <summary>
        /// Updates an employee by ID, filtered by the current user's managed employees.
        /// </summary>
        /// <param name="id" name="employee">The ID of the employee to retrieve and the employee details to update</param>
        /// <returns>An IActionResult with no content or an error message.</returns>
        [HttpPut("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var success = await _employeeService.UpdateEmployee(id, dto, managerId);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(500, "An error occurred while updating employee");
            }
        }

        /// <summary>
        /// Deletes an employee by ID, filtered by the current user's managed employees.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>An IActionResult with no content data or an error message.</returns>
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                var success = await _employeeService.DeleteEmployee(id, managerId);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return StatusCode(500, "An error occurred while deleting employee");
            }
        }

        /// <summary>
        /// Gets an ID by current user's managed employees.
        /// </summary>
        /// <returns>the manager ID data or an error message.</returns>
        private int GetCurrentManagerId()
        {
            //var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            var claim = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException("User ID claim not found");

            return int.Parse(claim.Value);
        }
    }
}

