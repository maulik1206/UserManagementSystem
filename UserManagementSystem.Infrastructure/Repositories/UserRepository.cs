using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;
using UserManagementSystem.Infrastructure.Data;

namespace UserManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task AddAsync(User user)
        {
            user.Email = user.Email.ToLower();
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            user.Email = user.Email.ToLower();
            user.UpdatedAt = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDuplicate(string email, Guid id)
        {
            var res = await _context.Users.Where(a => a.Email.ToLower() == email.ToLower() && a.Id != id).ToListAsync();
            return res.Count > 0;
        }
    }
}
