using MediatR;
using System.Net;
using UserManagementSystem.Application.Dtos;
using UserManagementSystem.Domain.Repositories;

namespace UserManagementSystem.Application.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseVM>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseVM> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                throw new FluentValidation.ValidationException("User not found.");
            }

            var isExist = await _userRepository.IsDuplicate(request.Email, request.Id);
            if (isExist)
            {
                throw new FluentValidation.ValidationException("Email already in use.");
            }

            user.Username = request.Username;
            user.Email = request.Email;

            await _userRepository.UpdateAsync(user);

            return new BaseVM(HttpStatusCode.OK);
        }
    }
}
