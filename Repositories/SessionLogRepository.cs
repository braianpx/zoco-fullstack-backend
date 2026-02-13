using Microsoft.EntityFrameworkCore;
using Zoco.Api.Data;
using Zoco.Api.Models;

namespace Zoco.Api.Repositories
{
    public class SessionLogRepository
    {
        private readonly AppDbContext _context;

        public SessionLogRepository(AppDbContext context) 
        {
            _context = context;
        }

        // Obtener todos los sessionlogs
        public async Task<List<SessionLog>> GetAllAsync()
        {
            return await _context.SessionLogs
                .Include(sl => sl.User)
                .ToListAsync();
        }

        // Obtener sessionlog por Id
        public async Task<SessionLog?> GetByIdAsync(int id)
        {
            return await _context.SessionLogs
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Craer SessionLog
        public async Task AddAsync(SessionLog sessionLog)
        {
            _context.SessionLogs.Add(sessionLog);
            await _context.SaveChangesAsync();
        }

        // Actualizar sessionlog
        public async Task UpdateAsync(SessionLog sessionLog)
        {
            _context.SessionLogs.Update(sessionLog);
            await _context.SaveChangesAsync();
        }

        // Eliminar sessionLog
        public async Task DeleteAsync(SessionLog sessionLog)
        {
            _context.SessionLogs.Remove(sessionLog);
            await _context.SaveChangesAsync();
        }

    }
}
