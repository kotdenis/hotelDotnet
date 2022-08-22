using HotelApp.Core.Options;
using HotelApp.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelApp.Core.Services.Implementation
{
    /// <summary>
    /// Sets and checks parameters of JWT tokens
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Generates JWT tokens
        /// </summary>
        /// <param name="claims">List of claims</param>
        /// <param name="lifetime">Token's lifetime</param>
        /// <returns>Token</returns>
        public Task<string> GenerateTokenAsync(IEnumerable<Claim> claims, TimeSpan lifetime)
        {
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                notBefore: now,
                claims: claims,
                expires: now.Add(lifetime),
                signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(_jwtOptions.Key), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Task.FromResult(encodedJwt);
        }

        /// <summary>
        /// Gets principals from token
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>ClaimsPrincipal</returns>
        /// <exception cref="SecurityTokenException"></exception>
        public Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwtOptions.Audience,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtOptions.GetSymmetricSecurityKey(_jwtOptions.Key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Fialed token validation.");
            return Task.FromResult(principal);
        }
    }
}
