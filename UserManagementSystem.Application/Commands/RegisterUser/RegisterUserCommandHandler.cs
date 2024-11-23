using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using UserManagementSystem.Application.Dtos;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseVM<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<BaseVM<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.Email);
            if (user != null)
            {
                throw new FluentValidation.ValidationException("Email already been register.");
            }

            var hashedPassword = _passwordHasher.HashPassword(new User(), request.Password);
            user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddAsync(user);

            return new BaseVM<Guid>()
            {
                Status = HttpStatusCode.OK,
                Data = user.Id
            };
        }
    }
}
