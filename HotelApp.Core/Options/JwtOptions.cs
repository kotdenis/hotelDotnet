using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelApp.Core.Options
{
    /// <summary>
    /// JWT settings
    /// </summary>
    public class JwtOptions
    {
        public const string Jwt = "JWT";
        public string Key { get; set; } = string.Empty;
        public string AccessLifetime { get; set; } = string.Empty;
        public string RefreshLifetime { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        public SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
