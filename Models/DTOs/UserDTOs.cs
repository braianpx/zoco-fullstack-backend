using System;
using System.ComponentModel.DataAnnotations;

namespace Zoco.Api.Models.DTOs
{
    // DTO para crear un usuario
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;

    }

    // DTO para actualizar un usuario
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        // La contraseña es opcional al actualizar
        public string? Password { get; set; }

        [Required(ErrorMessage = "El roleId es obligatorio")]
        public string? RoleName { get; set; } = string.Empty;
    }

    // DTO para respuesta al cliente
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreatedAt { get; set; }

    }

    public class UserDetailResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? RoleName { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<StudyResponseDTO>? Studies { get; set; }
        public List<AddressResponseDTO>? Addresses { get; set; }
        public List<SessionLogResponseDTO>? SessionLogs { get; set; }
    }

}
