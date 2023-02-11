using Application.Dtos.Rent;
using FluentValidation;

namespace Application.Validators.Rent;

public class CreateRentDtoRequestValidator : AbstractValidator<CreateRentDtoRequest>
{
    public CreateRentDtoRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .Must((dto, endDate) => endDate > dto.StartDate);

        RuleFor(x => x.VehicleId)
            .NotEmpty();
    }
}