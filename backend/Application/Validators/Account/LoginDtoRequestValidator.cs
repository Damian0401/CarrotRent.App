using Application.Dtos.Account;
using FluentValidation;

namespace Application.Validators.Account;

public class LoginDtoRequestValidator : AbstractValidator<LoginDtoRequest>
{
    public LoginDtoRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}