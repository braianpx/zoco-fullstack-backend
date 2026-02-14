using System.ComponentModel.DataAnnotations;

namespace Zoco.Api.Models.DTOs
{
    public class StudyCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Institution { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Degree { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class StudyUpdateDTO : StudyCreateDTO
    {
    }
    public class StudyResponseDTO
    {
        public int Id { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }

}
