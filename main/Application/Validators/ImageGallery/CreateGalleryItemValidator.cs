using FluentValidation;
using Application.Commands;

namespace Application.Validators;

public class CreateGalleryItemValidator : AbstractValidator<CreateGalleryItemCommand>
{
    public CreateGalleryItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(80).WithMessage("Title max 80 characters");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description max 300 characters");

        RuleFor(x => x.ImageFile)
            .NotNull().WithMessage("Image file is required");
    }
}