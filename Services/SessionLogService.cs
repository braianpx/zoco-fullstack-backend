using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Repositories;

namespace Zoco.Api.Services
{
    public class SessionLogService
    {
        private readonly SessionLogRepository _repository;

        public SessionLogService(SessionLogRepository repository)
        {
            _repository = repository;
        }

        // Obtener todos los logs
        public async Task<List<SessionLogResponseDTO>> GetAllLogsAsync()
        {
            var logs = await _repository.GetAllAsync();
            return logs.Select(l => new SessionLogResponseDTO
            {
                Id = l.Id,
                UserId = l.UserId,
                UserName = $"{l.User?.FirstName} {l.User?.LastName}",
                StartDate = l.StartDate.AddHours(-3),
                EndDate = l.EndDate?.AddHours(-3)
            }).ToList();
        }

        // Obtener un Log específico por Id
        public async Task<SessionLogResponseDTO?> GetLogByIdAsync(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return null;

            return new SessionLogResponseDTO
            {
                Id = log.Id,
                UserId = log.UserId,
                UserName = $"{log.User?.FirstName} {log.User?.LastName}",
                StartDate = log.StartDate.AddHours(-3),
                EndDate = log.EndDate?.AddHours(-3)
            };
        }

        // Crear log (login)
        public async Task<SessionLogResponseDTO> CreateLogAsync(int userId)
        {
            var log = new SessionLog
            {
                UserId = userId,
                StartDate = DateTime.UtcNow
            };

            await _repository.AddAsync(log);

            return new SessionLogResponseDTO
            {
                Id = log.Id,
                UserId = log.UserId,
                StartDate = log.StartDate
            };
        }

        // Obtener sesión por ID (delegar al repository)
        public async Task<SessionLog?> GetSessionByIdAsync(int sessionId)
        {
            return await _repository.GetByIdAsync(sessionId);
        }

        // Obtener sesión activa de usuario
        public async Task<SessionLog?> GetActiveSessionByUserIdAsync(int userId)
        {
            return await _repository.GetActiveSessionByUserIdAsync(userId);
        }

        // Finalizar sesión
        public async Task<bool> EndLogAsync(int sessionId)
        {
            var session = await _repository.GetByIdAsync(sessionId);

            if (session == null) return false; // no existe, ya no se pasa null al repository

            return await _repository.EndSessionAsync(session);
        }

        // Eliminar log
        public async Task<bool> DeleteLogAsync(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return false;

            await _repository.DeleteAsync(log);
            return true;
        }
    }
}
