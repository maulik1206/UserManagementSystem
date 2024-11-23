using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.Application.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<BaseVM<UserProfileDto>>
    {
        public Guid UserId { get; set; }
    }
}
