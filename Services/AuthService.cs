using BCrypt.Net;
using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Repositories;
using Zoco.Api.Services.Interfaces;

namespace Zoco.Api.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly SessionLogRepository _sessionLogRepository;
        private readonly IJwtService _jwtService;

        public AuthService(
            UserRepository userRepository,
            SessionLogRepository sessionLogRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _sessionLogRepository = sessionLogRepository;
            _jwtService = jwtService;
        }

        //Iniciar Session
        public async Task<(bool Success, string Message, AuthResponseDTO? Data)>
            LoginAsync(LoginDTO dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return (false, "Credenciales inválidas.", null);

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return (false, "Credenciales inválidas.", null);

            // Terminar SessionLog Previa
            var activeSession =
                await _sessionLogRepository
                .GetActiveSessionByUserIdAsync(user.Id);

            if (activeSession != null)
            {
                activeSession.EndDate = DateTime.UtcNow;
                await _sessionLogRepository.UpdateAsync(activeSession);
            }

            // Crear SessionLog
            var session = new SessionLog
            {
                UserId = user.Id,
                StartDate = DateTime.UtcNow
            };

            await _sessionLogRepository.AddAsync(session);

            // Generar token
            var (token, expiration) =
                _jwtService.GenerateToken(user, user.Role!.Name);

            var userModify = new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };

            return (true, "Login exitoso.", new AuthResponseDTO
            {
                Token = token,
                Expiration = expiration,
                Role = user.Role.Name,
                User  = userModify,
            });

        }

        // Cerrar Session
        public async Task<(bool Success, string Message)>
            LogoutAsync(int userId)
        {
            var activeSession =
                await _sessionLogRepository
                .GetActiveSessionByUserIdAsync(userId);

            if (activeSession == null)
                return (false, "No hay sesión activa.");

            activeSession.EndDate = DateTime.UtcNow;

            await _sessionLogRepository.UpdateAsync(activeSession);

            return (true, "Logout exitoso.");
        }
    }
}
