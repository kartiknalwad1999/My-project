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

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("RoleId")]
        public Guid RoleId { get; set; }

        [Column("EmployeeId")]
        public Guid? EmployeeId { get; set; }   // optional FK     
    }
}
