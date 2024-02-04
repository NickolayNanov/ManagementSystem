using ManagementSystem.Data;
using ManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManagementSystem.Services
{
    public class IdentityService : IIdentityService
    {
        private const string USER_ROLE_NAME = "User";
        private const string ADMIN_ROLE_NAME = "Admin";
        private readonly AppDbContext context;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> signInManager;
        private readonly IUserClaimsPrincipalFactory<DbUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration configuration;
        private readonly RoleManager<DbRole> roleManager;

        public IdentityService(
            AppDbContext context,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IUserClaimsPrincipalFactory<DbUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IConfiguration configuration,
            RoleManager<DbRole> roleManager)
        {
            this.context = context;
            _userManager = userManager;
            this.signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<LoginUserResp> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, password, true, false);

            if (!signInResult.Succeeded)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(type: "Username", user.UserName),
                new Claim(type: "Id", user.Id)
            };

            foreach (var userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginUserResp { Token = jwt, Expiration = token.ValidTo };
        }

        public async Task<CreateUserResp> CreateUserAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return null;
            }

            var newUser = new DbUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
            };

            await _userManager.CreateAsync(newUser, password);

            await AddToRoleAsync(newUser.Id, USER_ROLE_NAME);

            return new CreateUserResp { UserId = newUser.Id };
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(DbUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> AddToRoleAsync(string userId, string role)
        {
            var result = await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), role);
            return result.Succeeded;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string userId)
        {
            return await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId));
        }
    }
}
