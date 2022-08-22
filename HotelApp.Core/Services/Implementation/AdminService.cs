using AutoMapper;
using HotelApp.Core.Constants;
using HotelApp.Core.Dto;
using HotelApp.Core.Models;
using HotelApp.Core.Services.Interfaces;
using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Core.Services.Implementation
{
    /// <summary>
    /// Implements methods bound with administration
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly IUserService _userService;
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;
        public AdminService(UserManager<User> userManager, 
            ICacheManager cacheManager,
            IUserService userService, 
            IGuestRepository guestRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
            _userService = userService;
            _guestRepository = guestRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all guests
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Mapped list of guests</returns>
        public async Task<List<GuestReadDto>> GetAllGuestsAsync(CancellationToken ct)
        {
            var guests = await _cacheManager.GetAsync<Guest>(CacheConstant.GuestCache);
            if(guests.Count == 0)
            {
                guests = await _guestRepository.GetAllAsync(ct: ct);
                await _cacheManager.SetAsync(CacheConstant.GuestCache, guests, 30, 15);
            }
            var dtos = _mapper.Map<List<GuestReadDto>>(guests);
            return dtos;
        }

        /// <summary>
        /// Searches guests by parameters
        /// </summary>
        /// <param name="searchModel"><see cref="GuestSearchModel"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Mapped list of guests</returns>
        public async Task<List<GuestReadDto>> SearchGuestAsync(GuestSearchModel searchModel, CancellationToken ct)
        {
            var guests = await _guestRepository.GetAllAsync((x) => x.FirstName == searchModel.FirstName && x.LastName == searchModel.LastName, ct:ct);
            if (!guests.Any())
                return new List<GuestReadDto>();
            var dtos = _mapper.Map<List<GuestReadDto>>(guests);
            return dtos;
        }
    }
}
