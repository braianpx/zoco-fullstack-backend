using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoco.Api.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [MaxLength(100, ErrorMessage = "El email no puede superar los 100 caracteres")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones
        // Role FK
        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        public ICollection<Study> Studies { get; set; } = new List<Study>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<SessionLog> SessionLogs { get; set; } = new List<SessionLog>();
    }
}
