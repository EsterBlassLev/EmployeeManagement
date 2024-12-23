using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Core.DTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).*$",
            ErrorMessage = "Password must contain at least one uppercase letter and one number")]
        public string Password { get; set; }

        [Required]
        [MinLength(2)]
        public string FullName { get; set; }
    }
}
