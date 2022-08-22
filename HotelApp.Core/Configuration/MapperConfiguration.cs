using AutoMapper;
using HotelApp.Core.Dto;
using HotelApp.Core.Models;
using HotelApp.Data.Entities;

namespace HotelApp.Core.Configuration
{
    /// <summary>
    /// AutoMapper configuration
    /// </summary>
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<Room, RoomReadDto>();
            CreateMap<Guest, GuestReadDto>();
            //CreateMap<BookModel, Guest>();

        }
    }
}
