using Challenge.Balta.IBGE.Model;
using FluentValidation;

namespace Challenge.Balta.IBGE.FluentValidator
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("The Email field is required.")
                .EmailAddress().WithMessage("The Email is not valid.");

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("The Password field is required.");
        }
    }
}
