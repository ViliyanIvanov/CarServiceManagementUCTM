using CarService.Application.DTOs;
using FluentValidation;

namespace CarService.Application.Validators;

public class CreatePartDtoValidator : AbstractValidator<CreatePartDto>
{
    public CreatePartDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PartNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
    }
}
