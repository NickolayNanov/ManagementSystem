using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models.Entities
{
    public class Employee : IDeletable
    {
        [Key]
        public int Id { get; set; }

        public string OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string DeletedBy { get; set; }
    }
}
