using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.Application.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<BaseVM>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
