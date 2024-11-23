using Microsoft.AspNetCore.Identity;
using Moq;
using System.Net;
using UserManagementSystem.Application.Commands.LoginUser;
using UserManagementSystem.Application.Services;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;


namespace UserManagementSystem.Application.UnitTest.Commands
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtTokenServiceMock = new Mock<IJwtTokenService>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object,
                _jwtTokenServiceMock.Object,
                _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsValidationException()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsValidationException()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "wrongpassword" };
            var user = new User { Email = command.Email, PasswordHash = "hashedpassword" };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyHashedPassword(user, user.PasswordHash, command.Password))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Invalid credentials.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidUser_ReturnsToken()
        {
            // Arrange
            var command = new LoginUserCommand { Email = "test@example.com", Password = "password" };
            var user = new User { Email = command.Email, PasswordHash = "hashedpassword" };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyHashedPassword(user, user.PasswordHash, command.Password))
                .Returns(PasswordVerificationResult.Success);
            var token = "generated_jwt_token";
            _jwtTokenServiceMock.Setup(service => service.GenerateToken(user)).Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.Equal(token, result.Data);
        }
    }
}
