using System.ComponentModel.DataAnnotations;

namespace Zoco.Api.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; } = string.Empty;

        // Relaciones 
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
