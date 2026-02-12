using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoco.Api.Models
{
    public class Study
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Institution { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //Relaciones
        //User FK
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
