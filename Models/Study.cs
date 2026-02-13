using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoco.Api.Models
{
    public class Study
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La institución es obligatoria.")]
        [StringLength(100, ErrorMessage = "La institución no puede superar los 100 caracteres.")]
        public string Institution { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título o grado es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
        public string Degree { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        // Relaciones
        // User FK
        [Required(ErrorMessage = "El UserId es obligatorio.")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
