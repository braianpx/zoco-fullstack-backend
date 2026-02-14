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

        public async Task<List<SessionLog>> GetAllAsync()
        {
            return await _context.SessionLogs
                .Include(sl => sl.User)
                .ToListAsync();
        }

        public async Task<SessionLog?> GetByIdAsync(int id)
        {
            return await _context.SessionLogs
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SessionLog?> GetActiveSessionByUserIdAsync(int userId)
        {
            return await _context.SessionLogs
                .Where(s => s.UserId == userId && s.EndDate == null)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(SessionLog sessionLog)
        {
            _context.SessionLogs.Add(sessionLog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SessionLog sessionLog)
        {
            _context.SessionLogs.Update(sessionLog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EndSessionAsync(SessionLog session)
        {
            if (session == null || session.EndDate != null)
                return false;

            session.EndDate = DateTime.UtcNow;
            _context.Update(session);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(SessionLog sessionLog)
        {
            _context.SessionLogs.Remove(sessionLog);
            await _context.SaveChangesAsync();
        }
    }
}
