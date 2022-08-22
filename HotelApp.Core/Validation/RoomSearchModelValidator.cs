using FluentValidation;
using HotelApp.Core.Models;

namespace HotelApp.Core.Validation
{
    /// <summary>
    /// FluentValidation for RoomSearchModel <see cref="RoomSearchModel"/>
    /// </summary>
    public class RoomSearchModelValidator : AbstractValidator<RoomSearchModel>
    {
        public RoomSearchModelValidator()
        {
            RuleFor(x => x.DateBegin)
                .GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.DateEnd)
                .GreaterThan(DateTime.UtcNow);
        }
    }
}
