using MediatR;
using System.Net;
using UserManagementSystem.Application.Dtos;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, BaseVM<UserProfileDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserProfileQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseVM<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                throw new FluentValidation.ValidationException("User not found.");
            }

            return new BaseVM<UserProfileDto>()
            {
                Status = HttpStatusCode.OK,
                Data = new UserProfileDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                }
            };
        }
    }
}
