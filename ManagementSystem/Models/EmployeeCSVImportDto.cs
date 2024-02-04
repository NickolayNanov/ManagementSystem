namespace ManagementSystem.Models
{
    public class EmployeeCSVImportDto
    {
        public int EmployeeId { get; set; }
        public string OrganizationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
    }
}
