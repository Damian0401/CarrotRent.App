using Application.Dtos.Vehicle;
using FluentValidation;

namespace Application.Validators.Vehicle;

public class UpdateVehicleDtoRequestValidator : AbstractValidator<UpdateVehicleDtoRequest>
{
    public UpdateVehicleDtoRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty();
            
        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.YearOfProduction)
            .NotEmpty()
            .GreaterThanOrEqualTo(1950);

        RuleFor(x => x.Seats)
            .NotEmpty()
            .GreaterThan(0);
    }
}