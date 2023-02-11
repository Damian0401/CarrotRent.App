using Application.Dtos.Vehicle;
using FluentValidation;

namespace Application.Validators.Vehicle;

public class CreateVehicleDtoRequestValidator : AbstractValidator<CreateVehicleDtoRequest>
{
    public CreateVehicleDtoRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty();
            
        RuleFor(x => x.Registration)
            .NotEmpty();

        RuleFor(x => x.Vin)
            .NotEmpty();

        RuleFor(x => x.Seats)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.YearOfProduction)
            .NotEmpty()
            .GreaterThanOrEqualTo(1950);

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.ModelId)
            .NotEmpty();

        RuleFor(x => x.DepartmentId)
            .NotEmpty();

        RuleFor(x => x.FuelId)
            .NotEmpty();
    }
}