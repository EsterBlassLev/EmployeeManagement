using EmployeeManager.Core.Interfaces;
using EmployeeManager.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Infrastructure.Services
{
    /// <summary>
    /// Service class responsible for authentication tasks.
    /// Implements the <see cref="IAuthService"/> interface.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance used to access JWT settings.</param>
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) for a manager.
        /// </summary>
        /// <param name="manager">The manager object for whom the JWT token needs to be generated.</param>
        /// <returns>The generated JWT token as a string.</returns>
        public async Task<string> GenerateJwtToken(Manager manager)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, manager.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, manager.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Hashes a plain text password using the BCrypt algorithm.
        /// </summary>
        /// <param name="password">The plain text password to be hashed.</param>
        /// <returns>The hashed password string.</returns>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifies a plain text password against a hashed password using the BCrypt algorithm.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="passwordHash">The hashed password to compare against.</param>
        /// <returns>True if the password matches the hash, false otherwise.</returns>
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}