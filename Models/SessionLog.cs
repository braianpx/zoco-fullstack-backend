using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoco.Api.Models
{
    public class SessionLog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de inicio (StartDate) es obligatoria")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La fecha de fin (EndDate) debe ser proporcionada si se cierra la sesión")]
        public DateTime? EndDate { get; set; }

        // Relación
        [Required(ErrorMessage = "El UserId es obligatorio")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}

