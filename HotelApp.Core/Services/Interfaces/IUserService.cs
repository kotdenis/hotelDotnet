using HotelApp.Core.Models;

namespace HotelApp.Core.Services.Interfaces
{
    /// <summary>
    /// The contract for UserService<see cref="Implementation.UserService"/>
    /// </summary>
    public interface IUserService
    {
        Task<bool> CreateUserAsync(RegisterModel registerModel, CancellationToken ct);
        Task<TokenModel> SignInAsync(LoginModel loginModel, CancellationToken ct);
        Task<TokenModel> RefreshAsync(string token, CancellationToken ct);
        Task LogoutAsync(string accessToken, CancellationToken ct);
    }
}
