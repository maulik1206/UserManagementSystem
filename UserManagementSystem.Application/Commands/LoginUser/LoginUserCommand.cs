using MediatR;
using System.ComponentModel.DataAnnotations;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<BaseVM<string>>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
