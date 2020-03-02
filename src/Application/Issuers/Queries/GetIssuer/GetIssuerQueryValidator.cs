using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace CleanFunc.Application.Issuers.Queries.GetIssuer
{
    public class GetIssuerQueryValidator : AbstractValidator<GetIssuerQuery>
    {
        public GetIssuerQueryValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Issuer Id is required.");
        }
    }
}