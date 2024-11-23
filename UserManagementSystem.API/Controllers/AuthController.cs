using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserManagementSystem.API.Middleware;
using UserManagementSystem.Application.Commands.LoginUser;
using UserManagementSystem.Application.Commands.RegisterUser;
using UserManagementSystem.Application.Dtos;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _mediator.Send(command));
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                LogoutUserMiddleware.AddToBlacklist(token); // Add the token to the blacklist
            }

            return Ok(new BaseVM(HttpStatusCode.OK));
        }
    }
}
