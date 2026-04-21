using FluentValidation;
using Application.Commands;

namespace Application.Validators;

public class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(10).MaximumLength(1000);
        RuleFor(x => x.NewsURL).NotEmpty().MinimumLength(5).MaximumLength(100);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PublishDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);

    }
}