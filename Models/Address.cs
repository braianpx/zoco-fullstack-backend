using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoco.Api.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La calle es obligatoria.")]
        [MaxLength(150, ErrorMessage = "La calle no puede superar los 150 caracteres.")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [MaxLength(100, ErrorMessage = "La ciudad no puede superar los 100 caracteres.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "El país es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El país no puede superar los 100 caracteres.")]
        public string Country { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "El código postal no puede superar los 20 caracteres.")]
        public string? PostalCode { get; set; }

        // Relaciones
        // User FK
        [Required(ErrorMessage = "El UserId es obligatorio.")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
