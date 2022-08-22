using AutoMapper;
using FluentValidation;
using HotelApp.Core.Dto;
using HotelApp.Core.Models;
using HotelApp.Core.Services.Interfaces;
using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Implementation;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Core.Services.Implementation
{
    /// <summary>
    /// Implements methods to manage booking
    /// </summary>
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly IValidator<RoomSearchModel> _searchValidator;
        private readonly IMailService _mailService;

        public UserDashboardService(IRoomRepository roomRepository,
            IGuestRepository guestRepository,
            IBookRepository bookRepository,
            ICacheManager cacheManager,
            UnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<RoomSearchModel> validator,
            IMailService mailService)
        {
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _bookRepository = bookRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _searchValidator = validator;
            _mailService = mailService;
        }

        /// <summary>
        /// Gets vacant rooms
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>List</returns>
        public async Task<List<RoomReadDto>> GetVacantRoomsAsync(CancellationToken ct)
        {
            var vacantRooms = await _roomRepository.GetAllAsync((r) => r.IsVacant == true, ct: ct);
            var dtos = _mapper.Map<List<RoomReadDto>>(vacantRooms);
            return dtos;
        }

        /// <summary>
        /// Books room
        /// </summary>
        /// <param name="bookModel"><see cref="BookModel"/></param>
        /// <param name="user">Current user</param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> BookRoomAsync(BookModel bookModel, User user, CancellationToken ct)
        {
            if (bookModel == null)
                throw new ArgumentNullException(nameof(BookRoomAsync));
            using var transaction = await _unitOfWork.EnsureTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                var guest = new Guest
                {
                    FirstName = bookModel.FirstName,
                    LastName = bookModel.LastName,
                    UserId = user.Id
                };
                await _guestRepository.CreateAsync(guest, ct);
                await _unitOfWork.SaveAsync(ct);
                var room = await _roomRepository.FindAsync((x) => x.Number == bookModel.RoomNumber, ct);
                if (bookModel.BookForDate.Date == DateTime.UtcNow.Date)
                {
                    room.IsVacant = false;
                    await _roomRepository.UpdateAsync(room, ct);
                    await _unitOfWork.SaveAsync(ct);
                }

                var dateBegin = bookModel.BookForDate.Date.ToUniversalTime();
                var dateEnd = bookModel.DateOut?.Date.ToUniversalTime();
                var book = new Book
                {
                    DateBegin = dateBegin,
                    DateEnd = dateEnd,
                    RoomId = room.Id,
                    GuestId = guest.Id,
                };
                await _bookRepository.CreateAsync(book, ct);
                await _unitOfWork.SaveAsync(ct);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
            await _mailService.SendEmailAsync(new MailRequest
            {
                Body = "You have booked room!",
                Subject = "Hotel",
                ToEmail = user.Email
            }, ct);
            return true;
        }

        /// <summary>
        /// Searches vacant rooms to book 
        /// </summary>
        /// <param name="searchModel"><see cref="RoomSearchModel"/></param>
        /// <param name="ct">CancellationToken</param>
        /// <returns>List of RoomSearchModel</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<RoomReadDto>> SearchRoomToBookAsync(RoomSearchModel searchModel, CancellationToken ct)
        {
            var result = await _searchValidator.ValidateAsync(searchModel, ct);
            if (!result.IsValid)
                throw new ArgumentException("Your dates are not correct");
            if (searchModel == null)
                throw new ArgumentNullException(nameof(SearchRoomToBookAsync));

            var rooms = new List<Room>();
            var books = await _bookRepository.GetAllAsync(ct: ct);
            if (books.Count == 0)
                rooms = await _roomRepository.GetAllAsync((x) => x.Capacity == searchModel.Capacity, ct: ct);
            else
            {
                var ids = new List<Guid>();
                books = await _bookRepository.GetAllAsync((x) => x.DateEnd > DateTime.UtcNow, ct: ct);
                if(books.Count != 0)
                {

                    for (int i = 0; i < books.Count; i++)
                    {
                        var date = books[i].DateBegin!.Value.Date;
                        while(date <= books[i].DateEnd!.Value.Date)
                        {
                            if(searchModel.DateBegin == date)
                                ids.Add(books[i].RoomId);
                            date = date.AddDays(1);
                        }
                    }
                }
                rooms = await _roomRepository.GetAllAsync((x) => x.Capacity == searchModel.Capacity, ct: ct);
                rooms = rooms.Where(x => !ids.Contains(x.Id)).ToList();
            }

            var dtos = _mapper.Map<List<RoomReadDto>>(rooms);
            return dtos;
        }
    }
}
