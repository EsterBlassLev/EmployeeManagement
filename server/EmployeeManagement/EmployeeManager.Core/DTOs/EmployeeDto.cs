namespace EmployeeManager.Core.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ManagerId { get; set; }
    }
}
