using ManagementSystem.Data;
using ManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrganizationController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public OrganizationController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrganization(Organization organization)
        {
            await dbContext.Organizations.AddAsync(organization);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return Ok($"Created {rowsAffected} item successfuly");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrganization(Organization organization)
        {
            dbContext.Organizations.Update(organization);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return Ok($"Updated {rowsAffected} item successfuly");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrganization(Organization organization)
        {
            dbContext.Organizations.Remove(organization);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return Ok($"Deleted {rowsAffected} item successfuly");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganization(string id)
        {
            var organization = await dbContext.Organizations.FindAsync(id);

            if (organization is null || organization.IsDeleted)
            {
                return NotFound($"Organization with id: {id} does not exist");
            }

            return Ok(organization);
        }

        [HttpGet]
        public async Task<IActionResult> ListOrganizations()
        {
            return Ok(await dbContext.Organizations.Where(x => !x.IsDeleted).ToListAsync());
        }
    }
}
