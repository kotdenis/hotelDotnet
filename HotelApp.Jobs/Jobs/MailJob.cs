using Hangfire;
using HotelApp.Core.Services.Interfaces;
using HotelApp.Data.Entities;
using HotelApp.Data.Repositories.Interfaces;

namespace HotelApp.Jobs.Jobs
{
    public class MailJob
    {
        private readonly IMailService _mailService;
        private readonly IGuestRepository _guestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        public MailJob(IMailService mailService,
            IGuestRepository guestRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _mailService = mailService;
            _guestRepository = guestRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task SendMailAsync(IJobCancellationToken token)
        {
            var date = DateTime.UtcNow.Date;
            var books = await _bookRepository.GetAllAsync((x) => x.DateBegin!.Value == date.AddDays(1));
            if (books.Any())
            {
                var guests = new List<Guest>();
                foreach (var book in books)
                {
                    var guest = await _guestRepository.FindAsync((x) => x.Id == book.GuestId, default);
                    if (guest != null)
                        guests.Add(guest);
                }

                foreach (var item in guests)
                {
                    var user = await _userRepository.FindAsync((x) => x.Id == item.UserId, default);
                    await _mailService.SendEmailAsync(new Core.Models.MailRequest
                    {
                        Body = "You have a booking for tomorrow",
                        Subject = "Hotel",
                        ToEmail = user.Email
                    }, default);
                }
            }
        }
    }
}
