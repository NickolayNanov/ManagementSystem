namespace ManagementSystem.Models
{
    public class CreateUserResp
    {
        public string UserId { get; set; }

        public List<string> Errors { get; set; }
    }
}
