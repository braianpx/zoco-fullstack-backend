using System.ComponentModel.DataAnnotations;

namespace Zoco.Api.Models.DTOs
{
    //DTO para crear una session
    public class SessionLogCreateDTO
    {
        [Required(ErrorMessage = "UserId es obligatorio")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "StartDate es obligatorio")]
        public DateTime StartDate { get; set; }
    }
    //DTO para editar una session
    public class SessionLogUpdateDTO 
    {
        [Required(ErrorMessage = "EndDate es obligatorio")]
        public int EndDate { get; set; }

    }
    // DTO para respusta
    public class SessionLogResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
