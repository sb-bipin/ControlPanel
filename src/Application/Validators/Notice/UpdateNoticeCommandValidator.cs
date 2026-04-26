using FluentValidation;
using Application.Commands;

namespace Application.Validators;

public class UpdateNoticeCommandValidator : AbstractValidator<UpdateNoticeCommand>
{
    public UpdateNoticeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(10);
        RuleFor(x => x.Date).NotEmpty();
    }
}