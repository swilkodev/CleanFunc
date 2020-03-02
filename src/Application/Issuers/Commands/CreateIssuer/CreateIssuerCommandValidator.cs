using FluentValidation;

namespace CleanFunc.Application.Issuers.Commands.CreateIssuer
{
    public class CreateIssuerCommandValidator : AbstractValidator<CreateIssuerCommand>
    {
        public CreateIssuerCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Issuer Name is required.");
        }
    }
}