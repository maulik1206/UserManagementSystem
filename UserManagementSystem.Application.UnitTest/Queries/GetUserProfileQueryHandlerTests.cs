using Moq;
using System.Net;
using UserManagementSystem.Application.Queries.GetUserProfile;
using UserManagementSystem.Domain.Entities;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.UnitTest.Queries
{
    public class GetUserProfileQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserProfileQueryHandler _handler;

        public GetUserProfileQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserProfileQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsValidationException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserProfileQuery { UserId = userId };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async Task Handle_ValidUser_ReturnsUserProfileDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserProfileQuery { UserId = userId };
            var user = new User { Id = userId, Username = "testuser", Email = "test@example.com" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Status);
            Assert.NotNull(result.Data);
            Assert.Equal(userId, result.Data.Id);
            Assert.Equal(user.Username, result.Data.Username);
            Assert.Equal(user.Email, result.Data.Email);
        }
    }
}
