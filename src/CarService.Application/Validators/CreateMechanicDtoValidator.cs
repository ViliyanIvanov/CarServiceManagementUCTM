using CarService.Application.DTOs;
using FluentValidation;

namespace CarService.Application.Validators;

public class CreateMechanicDtoValidator : AbstractValidator<CreateMechanicDto>
{
    public CreateMechanicDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Specialization).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
    }
}
