using HotelApp.Core.Dto;
using HotelApp.Core.Models;
using HotelApp.Data.Entities;

namespace HotelApp.Core.Services.Interfaces
{
    /// <summary>
    /// The contract for UserDashboardService<see cref="Implementation.UserDashboardService"/>
    /// </summary>
    public interface IUserDashboardService
    {
        Task<List<RoomReadDto>> GetVacantRoomsAsync(CancellationToken ct);
        Task<bool> BookRoomAsync(BookModel bookModel, User user, CancellationToken ct);
        Task<List<RoomReadDto>> SearchRoomToBookAsync(RoomSearchModel searchModel, CancellationToken ct);
    }
}
