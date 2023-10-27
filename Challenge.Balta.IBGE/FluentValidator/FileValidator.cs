using FluentValidation;

namespace Challenge.Balta.IBGE.FluentValidator
{
    public class ExcelFileValidator : AbstractValidator<IFormFile>
    {
        public ExcelFileValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("File size is larger than allowed (5 MB max)");

            RuleFor(x => x.ContentType).NotNull().Must(x =>
                x.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") || 
                x.Equals("application/vnd.ms-excel") 
            ).WithMessage("File type is not allowed for Excel");

            RuleFor(file => file.FileName).NotNull().Must(fileName =>
                fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) || 
                fileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) 
            ).WithMessage("Invalid file extension. Only .xlsx and .xls are allowed for Excel files.");
        }
    }

}
