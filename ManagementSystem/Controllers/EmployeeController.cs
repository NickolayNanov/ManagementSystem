using ManagementSystem.Data;
using ManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public EmployeeController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            await dbContext.Employees.AddAsync(employee);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return Ok($"Imported {rowsAffected} item successfuly");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            dbContext.Employees.Update(employee);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return Ok($"Updated {rowsAffected} item successfuly");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(Employee employee)
        {
            dbContext.Employees.Remove(employee);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return Ok($"Deleted {rowsAffected} item successfuly");
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);

            if (employee is null || employee.IsDeleted)
            {
                return NotFound($"Employee with id: {id} does not exist");
            }

            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> ListEmployees()
        {
            return Ok(await dbContext.Employees.Where(x => !x.IsDeleted).ToListAsync());
        }
    }
}
