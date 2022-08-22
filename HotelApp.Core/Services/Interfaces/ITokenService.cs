using System.Security.Claims;

namespace HotelApp.Core.Services.Interfaces
{
    /// <summary>
    /// The contract for TokenService<see cref="Implementation.TokenService"/>
    /// </summary>
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(IEnumerable<Claim> claims, TimeSpan lifetime);
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
