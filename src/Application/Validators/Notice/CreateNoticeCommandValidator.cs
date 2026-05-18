using FluentValidation;
using Application.Commands;

namespace Application.Validators;

public class CreateNoticeCommandValidator : AbstractValidator<CreateNoticeCommand>
{
    public CreateNoticeCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(10);
        RuleFor(x => x.Date).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddMonths(3)); 
    }
}