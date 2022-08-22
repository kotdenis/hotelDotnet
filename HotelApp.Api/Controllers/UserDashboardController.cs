using HotelApp.Core.Dto;
using HotelApp.Core.Models;
using HotelApp.Core.Services.Interfaces;
using HotelApp.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelApp.Api.Controllers
{
    /// <summary>
    /// Authorized controller that implements endpoints bound with methods of UserDashboardService <see cref="Core.Services.Implementation.UserDashboardService"/>
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserDashboardController : ControllerBase
    {
        private readonly IUserDashboardService _dashboardService;
        private readonly UserManager<User> _userManager;

        
        public UserDashboardController(IUserDashboardService dashboardService, UserManager<User> userManager)
        {
            _dashboardService = dashboardService;
            _userManager = userManager;
        }

        [HttpGet("vacant")]
        public async Task<ActionResult<List<RoomReadDto>>> GetVacantRooms(CancellationToken ct = default)
        {
            var rooms = await _dashboardService.GetVacantRoomsAsync(ct);
            return Ok(rooms);
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<RoomReadDto>>> SearchRoom([FromBody] RoomSearchModel roomSearchModel, CancellationToken ct = default)
        {
            var rooms = await _dashboardService.SearchRoomToBookAsync(roomSearchModel, ct);
            return Ok(rooms);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRoom([FromBody] BookModel bookModel, CancellationToken ct = default)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name);
            var result = await _dashboardService.BookRoomAsync(bookModel, user, ct);
            if(result)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
