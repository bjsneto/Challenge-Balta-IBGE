using Challenge.Balta.IBGE.Model;
using FluentValidation;

namespace Challenge.Balta.IBGE.FluentValidator
{
    public class IbgeModelValidator : AbstractValidator<IbgeModel>
    {
        public IbgeModelValidator()
        {
            RuleFor(model => model.Id)
                .NotEmpty().WithMessage("The code field is required.")
                .Length(7).WithMessage("The code field must be 7 characters long.");

            RuleFor(model => model.State)
                .NotEmpty().WithMessage("The state field is required.")
                .MaximumLength(2).WithMessage("The status field must have a maximum of 2 characters.");

            RuleFor(model => model.City)
                .NotEmpty().WithMessage("The city field is required.")
                .MaximumLength(80).WithMessage("The city field must have a maximum of 80 characters.");
        }
    }
}
