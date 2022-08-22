using FluentValidation;
using HotelApp.Core.Models;
using HotelApp.Core.Services.Interfaces;
using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Implementation;
using HotelApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace HotelApp.Core.Services.Implementation
{
    /// <summary>
    /// Implements operations which bound to user (create, login etc)
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IValidator<RegisterModel> _registerValidator;
        private readonly IValidator<LoginModel> _loginValidator;
        private readonly ITokenService _tokenService;
        private readonly IRefreshRepository _refreshRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly UnitOfWork _unitOfWork;
        public UserService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IValidator<RegisterModel> registerValidator,
            IValidator<LoginModel> loginValidator,
            ITokenService tokenService,
            IRefreshRepository refreshRepository,
            IGuestRepository guestRepository,
            UnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _refreshRepository = refreshRepository;
            _guestRepository = guestRepository;
        }

        /// <summary>
        /// Creates new user with given role
        /// </summary>
        /// <param name="registerModel"><see cref="RegisterModel"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<bool> CreateUserAsync(RegisterModel registerModel, CancellationToken ct)
        {
            var validation = await _registerValidator.ValidateAsync(registerModel, ct);
            if (!validation.IsValid)
                throw new ArgumentException("Your datas are not valid");
            var userByEmail = await _userManager.FindByEmailAsync(registerModel.Email);
            var userByName = await _userManager.FindByNameAsync(registerModel.UserName);
            if (userByEmail != null && userByName != null)
                throw new NullReferenceException($"This email or name are already in use.");
            else
            {
                var user = new User
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email
                };
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(registerModel.Role))
                        registerModel.Role = "User";
                    var role = await _roleManager.FindByNameAsync(registerModel.Role);
                    if (role == null)
                        await _roleManager.CreateAsync(new Role { Name = registerModel.Role });
                    await _userManager.AddToRoleAsync(user, registerModel.Role);
                    await _guestRepository.CreateAsync(new Guest
                    {
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        UserId = user.Id
                    }, ct);
                    await _unitOfWork.SaveAsync(ct);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Logins user
        /// </summary>
        /// <param name="loginModel"><see cref="LoginModel"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>TokenModel</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<TokenModel> SignInAsync(LoginModel loginModel, CancellationToken ct)
        {
            var validation = await _loginValidator.ValidateAsync(loginModel, ct);
            if (!validation.IsValid)
                throw new ArgumentException("Your datas are not valid");
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var accessToken = await _tokenService.GenerateTokenAsync(await GetClaimsAsync(user, ct).ToListAsync(), TimeSpan.FromMinutes(10));
                var refreshToken = await _tokenService.GenerateTokenAsync(await GetRefreshClaims(user, ct).ToListAsync(), TimeSpan.FromDays(10));
                await SaveRefreshTokenAsync(refreshToken, user, ct);
                await _signInManager.SignInAsync(user, false);
                return new TokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserName = user.UserName
                };
            }
            else
                throw new InvalidOperationException("Wrong data in login model");
        }

        /// <summary>
        /// Refreshes token
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>TokenModel</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TokenModel> RefreshAsync(string token, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(RefreshAsync));
            var user = await GetUserFromPrincipalAsync(token, ct);
            var accessToken = await _tokenService.GenerateTokenAsync(await GetClaimsAsync(user, ct).ToListAsync(), TimeSpan.FromMinutes(10));
            var refreshToken = await _tokenService.GenerateTokenAsync(await GetRefreshClaims(user, ct).ToListAsync(), TimeSpan.FromDays(10));
            await SaveRefreshTokenAsync(refreshToken, user, ct);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserName = user.UserName
            };
        }

        /// <summary>
        /// Logouts user
        /// </summary>
        /// <param name="accessToken">JWT token</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns></returns>
        public async Task LogoutAsync(string accessToken, CancellationToken ct)
        {
            var user = await GetUserFromPrincipalAsync(accessToken, ct);
            var refreshToken = await _refreshRepository.FindAsync((x) => x.UserId == user.Id, ct);
            await _refreshRepository.DeleteAsync(refreshToken, ct);
            await _signInManager.SignOutAsync();
            await _unitOfWork.SaveAsync(ct);
        }

        private async Task<User> GetUserFromPrincipalAsync(string token, CancellationToken ct)
        {
            var principal = await _tokenService.GetPrincipalFromExpiredTokenAsync(token);
            if (principal == null)
                throw new NullReferenceException(nameof(RefreshAsync) + $" {nameof(principal)}");

            var userName = principal.FindFirstValue(ClaimTypes.Name);
            if (userName == null)
                throw new NullReferenceException(nameof(RefreshAsync) + $" {nameof(userName)}");
            return await _userManager.FindByNameAsync(userName);
        }

        private async IAsyncEnumerable<Claim> GetClaimsAsync(User user, [EnumeratorCancellation] CancellationToken ct)
        {
            var roles = await _userManager.GetRolesAsync(user);
            yield return new Claim(JwtRegisteredClaimNames.Sid, Guid.NewGuid().ToString());
            yield return new Claim(JwtRegisteredClaimNames.Email, user.Email);
            yield return new Claim(JwtRegisteredClaimNames.Name, user.UserName);
            yield return new Claim(JwtRegisteredClaimNames.FamilyName, roles.FirstOrDefault()!);
            await Task.CompletedTask;
        }

        private async IAsyncEnumerable<Claim> GetRefreshClaims(User user, [EnumeratorCancellation] CancellationToken ct)
        {
            yield return new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString());
            yield return new Claim(JwtRegisteredClaimNames.Email, user.Email);
            yield return new Claim(JwtRegisteredClaimNames.Name, user.UserName);
            await Task.CompletedTask;
        }

        private async Task SaveRefreshTokenAsync(string refreshToken, User user, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException(nameof(SaveRefreshTokenAsync));
            
            var tokenToRefresh = new RefreshToken
            {
                Expire = DateTime.UtcNow.AddDays(10),
                Token = refreshToken,
                UserId = user.Id
            };
            var tokenRefresh = await _refreshRepository.FindAsync((x) => x.UserId == user.Id, ct);
            if (tokenRefresh == null)
                await _refreshRepository.CreateAsync(tokenToRefresh, ct);
            else
            {
                await _refreshRepository.UpdateAsync(tokenRefresh, ct);
            }
            await _unitOfWork.SaveAsync(ct);
        }
    }
}
