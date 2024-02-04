using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models.Entities
{
    public class Organization : IDeletable
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        public string Country { get; set; }

        public string Description { get; set; }

        public int Founded { get; set; }

        public string Industry { get; set; }
        
        public int NumberOfEmployees { get; set; }

        public virtual List<Employee> Employees { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string DeletedBy { get; set; }
    }
}
