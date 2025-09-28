using FluentValidation;
using PDFBackend.DTOs;

namespace PDFBackend.Validators
{
    public class TextEditValidator : AbstractValidator<TextEditDto>
    {
        public TextEditValidator()
        {
            RuleFor(x => x.NewText).NotEmpty().MaximumLength(500);
            RuleFor(x => x.X).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10000);
            RuleFor(x => x.Y).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10000);
        }
    }
}
