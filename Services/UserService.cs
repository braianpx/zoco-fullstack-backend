using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Repositories;

namespace Zoco.Api.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Obtener todos los usuarios
        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                RoleId = u.RoleId,
                RoleName = u.Role?.Name,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        // Obtener usuario por Id
        public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                CreatedAt = user.CreatedAt
            };
        }

        // Crear usuario
        public async Task<(bool Success, string? Error, UserResponseDTO? User)> CreateUserAsync(UserCreateDTO dto)
        {
            // Validar email duplicado
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
                return (false, "El email ya está en uso", null);

            // Crear entidad
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = 2 //Role user siempre 
            };

            await _userRepository.AddAsync(user);

            var response = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt
            };

            return (true, null, response);
        }

        // Actualizar usuario
        public async Task<(bool Success, string? Error)> UpdateUserAsync(int id, UserUpdateDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return (false, "Usuario no encontrado");

            //Validar rol existente
            if (dto.RoleId == null)
                return (false, "RoleId es obligatorio");

            if (!await _userRepository.RoleExistsAsync(dto.RoleId.Value))
                return (false, "RoleId no válido, el rol no existe");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.RoleId = dto.RoleId.Value;

            await _userRepository.UpdateAsync(user);
            return (true, null);
        }

        // Eliminar usuario
        public async Task<(bool Success, string? Error)> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return (false, "Usuario no encontrado");

            await _userRepository.DeleteAsync(user);
            return (true, null);
        }
    }
}
