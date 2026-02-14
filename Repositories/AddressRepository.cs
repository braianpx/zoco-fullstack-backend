using Microsoft.EntityFrameworkCore;
using Zoco.Api.Data;
using Zoco.Api.Models;

namespace Zoco.Api.Repositories
{
    public class AddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAllAsync()
        {
            return await _context.Addresses
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Address address)
        {
            _context.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Address address)
        {
            _context.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Address address)
        {
            _context.Remove(address);
            await _context.SaveChangesAsync();
        }
    }

}
