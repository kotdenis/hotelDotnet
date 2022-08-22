using FluentValidation;
using HotelApp.Core.Models;

namespace HotelApp.Core.Validation
{
    /// <summary>
    /// FluentValidation for RegisterModel<see cref="RegisterModel"/>
    /// </summary>
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserName)
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty();
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
