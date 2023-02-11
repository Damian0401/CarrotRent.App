using Application.Dtos.Vehicle;
using FluentValidation;

namespace Application.Validators.Vehicle;

public class GetFilteredVehiclesDtoRequestValidator : AbstractValidator<GetFilteredVehiclesDtoRequest>
{
    public GetFilteredVehiclesDtoRequestValidator()
    {
        RuleFor(x => x.Seats)
            .GreaterThan(0);
    }
}