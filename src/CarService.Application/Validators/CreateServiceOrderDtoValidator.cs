using CarService.Application.DTOs;
using FluentValidation;

namespace CarService.Application.Validators;

public class CreateServiceOrderDtoValidator : AbstractValidator<CreateServiceOrderDto>
{
    public CreateServiceOrderDtoValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.MechanicId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.LaborCost).GreaterThanOrEqualTo(0);
        RuleForEach(x => x.Parts).ChildRules(p =>
        {
            p.RuleFor(x => x.PartId).NotEmpty();
            p.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}
