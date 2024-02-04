namespace ManagementSystem.Services
{
    public interface ICSVImportService
    {
        Task<int> ImportOrganizationsAsync(IFormFile file);

        Task<int> ImportEmployeesAsync(IFormFile file);
    }
}
