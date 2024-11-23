using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagementSystem.Application.Commands.UpdateUser;
using UserManagementSystem.Application.Queries.GetUserProfile;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/me")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            var query = new GetUserProfileQuery { UserId = Guid.Parse(userIdClaim) };
            return Ok(await _mediator.Send(query));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            command.Id = Guid.Parse(userIdClaim);
            return Ok(await _mediator.Send(command));
        }
    }
}
