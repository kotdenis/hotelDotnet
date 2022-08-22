using HotelApp.Core.Dto;
using HotelApp.Core.Models;

namespace HotelApp.Core.Services.Interfaces
{
    /// <summary>
    /// The contract for AdminService <see cref="Implementation.AdminService"/>
    /// </summary>
    public interface IAdminService
    {
        Task<List<GuestReadDto>> GetAllGuestsAsync(CancellationToken ct);
        Task<List<GuestReadDto>> SearchGuestAsync(GuestSearchModel searchModel, CancellationToken ct);
    }
}
