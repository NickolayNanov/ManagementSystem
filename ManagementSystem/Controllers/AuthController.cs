using ManagementSystem.Models;
using ManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public AuthController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost(Name = nameof(Register))]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto) => Ok(await identityService.CreateUserAsync(dto.Email, dto.Password));

        [HttpPost(Name = nameof(Login))]
        public async Task<ActionResult<LoginUserResp>> Login([FromBody] LoginCredsDto dto) => await identityService.Login(dto.Email, dto.Password);

        [HttpPost(Name = nameof(AddToRole))]
        public async Task<ActionResult<LoginUserResp>> AddToRole([FromBody] AddToRoleDto dto) => Ok(await identityService.AddToRoleAsync(dto.UserId, dto.Role));

        [HttpGet(Name = nameof(GetUserRoles))]
        public async Task<ActionResult<LoginUserResp>> GetUserRoles(string userId) => Ok(await identityService.GetUserRoles(userId));
    }
}
