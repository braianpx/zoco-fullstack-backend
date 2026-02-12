using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoco.Api.Models
{
    public class SessionLog
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //Relacion
        // User FK
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User {get; set;}
    }
}
