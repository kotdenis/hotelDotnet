using HotelApp.Core.Dto;
using HotelApp.Core.Models;
using HotelApp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Api.Controllers
{
    /// <summary>
    /// Authorized controller that implements endpoints bound with methods of AdminService <see cref="Core.Services.Implementation.AdminService"/>
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GuestReadDto>>> GetAllGuests(CancellationToken ct = default)
        {
            var guests = await _adminService.GetAllGuestsAsync(ct);
            return Ok(guests);
        }

        [HttpPost]
        public async Task<ActionResult<List<GuestReadDto>>> SearchGuests([FromBody] GuestSearchModel searchModel, CancellationToken ct = default)
        {
            if (searchModel == null)
                return BadRequest();
            else
            {
                var guests = await _adminService.SearchGuestAsync(searchModel, ct);
                return Ok(guests);
            }

        }
    }
}
