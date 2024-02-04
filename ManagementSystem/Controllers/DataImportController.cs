using ManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DataImportController : ControllerBase
    {
        private ILogger<OrganizationController> _logger;
        private ICSVImportService importService;

        public DataImportController(ILogger<OrganizationController> logger, ICSVImportService importService)
        {
            _logger = logger;
            this.importService = importService;
        }

        [HttpPost(Name = nameof(UploadOrganizations))]
        public async Task<IActionResult> UploadOrganizations(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var rowsAffected = await importService.ImportOrganizationsAsync(file);

            return Ok($"{rowsAffected} got imported");
        }

        [HttpPost(Name = nameof(UploadEmployees))]
        public async Task<IActionResult> UploadEmployees(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var rowsAffected = await importService.ImportEmployeesAsync(file);

            return Ok($"{rowsAffected} got imported");
        }
    }
}
