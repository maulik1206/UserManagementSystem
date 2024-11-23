using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
