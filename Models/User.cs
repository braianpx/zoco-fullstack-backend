using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Zoco.Api.Models
{
    [Index(nameof(Email), IsUnique = true)]

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } // Puede ser Admin o User

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Relaciones
    }
}
