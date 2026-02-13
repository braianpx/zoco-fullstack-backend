using System.ComponentModel.DataAnnotations;

namespace Zoco.Api.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(40, ErrorMessage = "El nombre del rol no puede superar los 40 caracteres.")]
        public string Name { get; set; } = string.Empty;

        // Relaciones
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

