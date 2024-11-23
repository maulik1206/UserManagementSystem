using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using UserManagementSystem.Application.Dtos;
using UserManagementSystem.Application.Services;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, BaseVM<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginUserCommandHandler(IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
             IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<BaseVM<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                throw new FluentValidation.ValidationException("User not found.");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                throw new FluentValidation.ValidationException("Invalid credentials.");
            }

            return new BaseVM<string>()
            {
                Status = HttpStatusCode.OK,
                Data = _jwtTokenService.GenerateToken(user)
            };
        }
    }
}
