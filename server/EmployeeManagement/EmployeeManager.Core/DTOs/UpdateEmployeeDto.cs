using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Core.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).*$",
            ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string? Password { get; set; }

        [Required]
        [MinLength(2)]
        public string FullName { get; set; }
    }
}
