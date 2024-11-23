using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid userId);
        Task<User> GetByEmail(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> IsDuplicate(string email, Guid id);
    }
}
