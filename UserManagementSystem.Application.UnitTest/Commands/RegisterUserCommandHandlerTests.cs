using Microsoft.AspNetCore.Identity;
using Moq;
using System.Net;
using UserManagementSystem.Application.Commands.RegisterUser;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.UnitTest.Commands
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _handler = new RegisterUserCommandHandler(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Handle_EmailAlreadyRegistered_ThrowsValidationException()
        {
            // Arrange
            var command = new RegisterUserCommand { Email = "test@example.com", Password = "password", Username = "testuser" };
            var existingUser = new User { Email = command.Email };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Email already been register.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidUser_ReturnsUserId()
        {
            // Arrange
            var command = new RegisterUserCommand { Email = "test@example.com", Password = "password", Username = "testuser" };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(command.Email)).ReturnsAsync((User)null);
            var hashedPassword = "hashedPassword123";
            _passwordHasherMock.Setup(hasher => hasher.HashPassword(It.IsAny<User>(), command.Password)).Returns(hashedPassword);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.NotEqual(Guid.Empty, result.Data);
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }
    }
}
