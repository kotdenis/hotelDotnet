using HotelApp.Core.Models;
using HotelApp.Core.Services.Implementation;
using HotelApp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Api.Controllers
{
    /// <summary>
    /// Controller that implements endpoints bound with methods of UserService <see cref="Core.Services.Implementation.UserService"/>
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel registerModel, CancellationToken ct = default)
        {
            var created = await _userService.CreateUserAsync(registerModel, ct);
            if (created)
                return Ok();
            return BadRequest();
        }

        [HttpPost("signin")]
        public async Task<ActionResult<TokenModel>> SignIn([FromBody] LoginModel loginModel, CancellationToken ct = default)
        {
            var tokenModel = await _userService.SignInAsync(loginModel, ct);
            if(tokenModel == null)
                return BadRequest();
            return Ok(tokenModel);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] string accessToken, CancellationToken ct = default)
        {
            await _userService.LogoutAsync(accessToken, ct);
            return Ok();
        }

        [HttpPost("token-refresh")]
        public async Task<ActionResult<TokenModel>> RefreshToken([FromQuery] string token, CancellationToken ct = default)
        {
            var tokenModel = await _userService.RefreshAsync(token, ct);
            if (tokenModel == null)
                return BadRequest();
            return Ok(tokenModel);
        }
    }
}
