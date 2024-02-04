namespace ManagementSystem.Models
{
    public class LoginUserResp
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
