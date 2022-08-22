using FluentValidation;
using HotelApp.Core.Models;

namespace HotelApp.Core.Validation
{
    /// <summary>
    /// FluentValidation for LoginModel<see cref="LoginModel"/>
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
