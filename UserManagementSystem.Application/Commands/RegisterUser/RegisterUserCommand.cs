using MediatR;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.Application.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<BaseVM<Guid>>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
