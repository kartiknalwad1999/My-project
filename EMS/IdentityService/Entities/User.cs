using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserId")]
        public Guid UserId { get; set; } = Guid.NewGuid();   // GUID primary key

        [Required]
        [Column("Username")]
        public string Username { get; set; }

        [Required]
        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // auto-increment from 50001
        [Column("EmployeeId")]
        public int EmployeeId { get; set; }                   // five-digit int identity

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("RoleId")]
        public Guid RoleId { get; set; }

        // Navigation property (optional, if you want EF to handle relationships)
        public Role Role { get; set; }
    }
}
