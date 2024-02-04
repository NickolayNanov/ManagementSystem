using CsvHelper.Configuration;
using CsvHelper;
using ManagementSystem.Models.Entities;
using ManagementSystem.Models;
using System.Globalization;
using ManagementSystem.Data;

namespace ManagementSystem.Services
{
    public class CSVImportService : ICSVImportService
    {
        private readonly AppDbContext dbContext;

        public CSVImportService(AppDbContext appDbContext)
        {
            this.dbContext = appDbContext;
        }

        public async Task<int> ImportEmployeesAsync(IFormFile file)
        {
            var organizations = new List<EmployeeCSVImportDto>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            }))
            {
                organizations = csv.GetRecords<EmployeeCSVImportDto>().ToList();
            }

            var entities = organizations.Select(dto => new Employee
            {
                Id = dto.EmployeeId,
                Department = dto.Department,
                HireDate = dto.HireDate,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                OrganizationId = dto.OrganizationId,
                Position = dto.Position,
                Salary = dto.Salary,
                Status = dto.Status
            });

            await dbContext.Employees.AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> ImportOrganizationsAsync(IFormFile file)
        {
            var organizations = new List<OrganizationCSVImportDto>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            }))
            {
                organizations = csv.GetRecords<OrganizationCSVImportDto>().ToList();
            }

            var entities = organizations.Select(dto => new Organization
            {
                Name = dto.Name,
                Website = dto.Website,
                Country = dto.Country,
                Description = dto.Description,
                Founded = dto.Founded,
                Id = dto.OrganizationId,
                NumberOfEmployees = dto.NumberOfEmployees,
                Industry = dto.Industry
            });

            await dbContext.Organizations.AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();
        }
    }
}
