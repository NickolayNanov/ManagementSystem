namespace ManagementSystem.Models
{
    public interface IDeletable
    {
        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }

        public string DeletedBy { get; set; }
    }
}
