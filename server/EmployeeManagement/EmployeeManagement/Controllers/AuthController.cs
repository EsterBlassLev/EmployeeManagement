using EmployeeManager.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Core.Interfaces;

namespace EmployeeManagement.Application.Controllers
{
    /// <summary>
    /// Web API controller for authentication (manager registration and login).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="managerService">The manager service.</param>
        /// <param name="logger">The logger.</param>
        public AuthController(IManagerService managerService, ILogger<AuthController> logger)
        {
            _managerService = managerService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new manager.
        /// </summary>
        /// <param name="dto">The data transfer object containing manager registration details.</param>
        /// <returns>An IActionResult containing a token on success or a BadRequest message on failure.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterManagerDto dto)
        {
            try
            {
                var result = await _managerService.RegisterManager(dto);
                if (!result.success)
                {
                    return BadRequest(result.message);
                }

                return Ok(new { token = result.token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, "An error occurred during registration");
            }
        }

        /// <summary>
        /// Logs in a manager.
        /// </summary>
        /// <param name="dto">The data transfer object containing manager login credentials.</param>
        /// <returns>An IActionResult containing a token on success or a BadRequest message on failure.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var result = await _managerService.Login(dto);
                if (!result.success)
                {
                    return BadRequest(result.message);
                }

                return Ok(new { token = result.token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "An error occurred during login");
            }
        }
    }
}