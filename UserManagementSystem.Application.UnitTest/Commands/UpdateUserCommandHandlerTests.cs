using Moq;
using System.Net;
using UserManagementSystem.Application.Commands.UpdateUser;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.UnitTest.Commands
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new UpdateUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsValidationException()
        {
            // Arrange
            var command = new UpdateUserCommand { Id = Guid.NewGuid(), Email = "test@example.com", Username = "testuser" };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task Handle_EmailAlreadyInUse_ThrowsValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand { Id = userId, Email = "test@example.com", Username = "testuser" };
            var existingUser = new User { Id = userId, Email = "other@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.IsDuplicate(command.Email, command.Id)).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Email already in use.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidUpdate_ReturnsOkStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new UpdateUserCommand { Id = userId, Email = "test@example.com", Username = "updateduser" };
            var existingUser = new User { Id = userId, Email = "other@example.com", Username = "olduser" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.IsDuplicate(command.Email, command.Id)).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Status);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(existingUser), Times.Once);
            Assert.Equal(command.Email, existingUser.Email);
            Assert.Equal(command.Username, existingUser.Username);
        }
    }
}
