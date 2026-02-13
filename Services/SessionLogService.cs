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
                StartDate = l.StartDate,
                EndDate = l.EndDate
            }).ToList();
        }

        // Obtener un Log especifico por Id
        public async Task<SessionLogResponseDTO?> GetLogByIdAsync(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return null;

            return new SessionLogResponseDTO
            {
                Id = log.Id,
                UserId = log.UserId,
                UserName = $"{log.User?.FirstName} {log.User?.LastName}",
                StartDate = log.StartDate,
                EndDate = log.EndDate
            };
        }

        // Crear log (se usa en login)
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

        // Actualizar log (se usa en logout)
        public async Task<bool> EndLogAsync(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return false;

            log.EndDate = DateTime.UtcNow;
            await _repository.UpdateAsync(log);
            return true;
        }

        //Eliminar un log
        public async Task<bool> DeleteLogAsync(int id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return false;

            await _repository.DeleteAsync(log);
            return true;
        }
    }
}
