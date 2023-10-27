using Challenge.Balta.IBGE.Model;
using FluentValidation;

namespace Challenge.Balta.IBGE.FluentValidator
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("The Email field is required.")
                .EmailAddress().WithMessage("The Email is not valid.");

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("The Password field is required.")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[\W_]).{8,}$")
                .WithMessage("The password must contain at least 8 characters, including at least one letter, one number and one special character.");
        }
    }
}
