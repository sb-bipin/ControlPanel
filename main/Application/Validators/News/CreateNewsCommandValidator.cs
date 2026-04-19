using FluentValidation;
using Application.Commands;

namespace Application.Validators;

public class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(10).MaximumLength(200);
        RuleFor(x => x.NewsURL).NotEmpty().MinimumLength(20).MaximumLength(1000);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PublishDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
    }
}