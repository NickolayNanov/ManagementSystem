using ManagementSystem.Models;

namespace ManagementSystem.Services
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> AddToRoleAsync(string userId, string role);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<CreateUserResp> CreateUserAsync(string email, string password);

        Task<bool> DeleteUserAsync(string userId);

        Task<LoginUserResp> Login(string email, string password);

        Task<IEnumerable<string>> GetUserRoles(string userId);
    }
}
