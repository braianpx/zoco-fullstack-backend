using Microsoft.EntityFrameworkCore;
using Zoco.Api.Data;
using Zoco.Api.Models;

namespace Zoco.Api.Repositories
{
    public class StudyRepository
    {
        private readonly AppDbContext _context;

        public StudyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Study>> GetAllAsync()
        {
            return await _context.Studies
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<List<Study>> GetByUserIdAsync(int userId)
        {
            return await _context.Studies
                .Where(s => s.UserId == userId)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<Study?> GetByIdAsync(int id)
        {
            return await _context.Studies
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Study study)
        {
            _context.Studies.Add(study);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Study study)
        {
            _context.Studies.Update(study);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Study study)
        {
            _context.Studies.Remove(study);
            await _context.SaveChangesAsync();
        }
    }

}
