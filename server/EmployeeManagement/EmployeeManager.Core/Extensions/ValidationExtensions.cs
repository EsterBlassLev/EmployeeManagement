using EmployeeManager.Core.DTOs;
using EmployeeManager.Core.Exceptions;

namespace EmployeeManagement.Application.Extensions
{
    /// <summary>
    /// Provides static extension methods for data validation.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validates a <see cref="CreateEmployeeDto"/> object to ensure it meets the required criteria for creating a new employee.
        /// Throws an exception if any validation rule fails.
        /// </summary>
        /// <param name="dto">The <see cref="CreateEmployeeDto"/> object to validate.</param>
        /// <exception cref="BadRequestException">Thrown if any validation rule fails.</exception>
        public static void ValidateEmployee(this CreateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new BadRequestException("Email is required");
            }

            if (!IsValidEmail(dto.Email))
            {
                throw new BadRequestException("Invalid email format");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Password is required");
            }

            if (dto.Password.Length < 8)
            {
                throw new BadRequestException("Password must be at least 8 characters");
            }

            if (!dto.Password.Any(char.IsUpper))
            {
                throw new BadRequestException("Password must contain at least one uppercase letter");
            }

            if (!dto.Password.Any(char.IsDigit))
            {
                throw new BadRequestException("Password must contain at least one number");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new BadRequestException("Full name is required");
            }

            if (dto.FullName.Length < 2)
            {
                throw new BadRequestException("Full name must be at least 2 characters");
            }
        }

        /// <summary>
        /// Checks if the provided email string is in a valid email format.
        /// </summary>
        /// <param name="email">The email string to validate.</param>
        /// <returns>True if the email is valid, false otherwise.</returns>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}