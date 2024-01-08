using FluentValidation;
using Schema.Request;

namespace Business.Validator;

public class ConvertCurrencyValidator : AbstractValidator<ConvertCurrencyRequest>
{
    public ConvertCurrencyValidator()
    {
        RuleFor(x => x.From)
            .NotEmpty()
            .WithMessage("From Currency is required")
            .MaximumLength(3)
            .WithMessage("From Currency must be 3 characters")
            .Must(receivingCurrency => ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == receivingCurrency))
            .WithMessage("Please enter a valid currency code");
            
        RuleFor(x => x.To)
            .NotEmpty()
            .WithMessage("To Currency is required")
            .MaximumLength(3)
            .WithMessage("To Currency must be 3 characters")
            .Must(receivingCurrency => ISO._4217.CurrencyCodesResolver.Codes.Any(c => c.Code == receivingCurrency))
            .WithMessage("Please enter a valid currency code");


        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required")
            .GreaterThan(0)
            .WithMessage("Amount must be equal or greater than 0");
    }
}