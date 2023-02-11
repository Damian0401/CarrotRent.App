using Application.Dtos.Account;
using FluentValidation;

namespace Application.Validators.Account;

public class RegisterDtoRequestValidator : AbstractValidator<RegisterDtoRequest>
{
    public RegisterDtoRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();
            
        RuleFor(x => x.Password)
            .NotEmpty();
            
        RuleFor(x => x.FirstName)
            .NotEmpty();
            
        RuleFor(x => x.LastName)
            .NotEmpty();
            
        RuleFor(x => x.Pesel)
            .NotEmpty();
            
        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
            
        RuleFor(x => x.PostCode)
            .NotEmpty();
            
        RuleFor(x => x.City)
            .NotEmpty();
            
        RuleFor(x => x.Street)
            .NotEmpty();
            
        RuleFor(x => x.HouseNumber)
            .NotEmpty();
            
        RuleFor(x => x.ApartmentNumber)
            .NotEmpty();
    }
}