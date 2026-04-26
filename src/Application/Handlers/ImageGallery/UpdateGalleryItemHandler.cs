using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Handlers;

public class UpdateGalleryItemHandler : IRequestHandler<UpdateGalleryItemCommand, GalleryItemDto>
{
    private readonly IRepository<GalleryItem> _repository;
    private readonly IFileStorage _fileStorage;

    public UpdateGalleryItemHandler(IRepository<GalleryItem> repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<GalleryItemDto> Handle(UpdateGalleryItemCommand request, CancellationToken ct)
    {
        var item = await _repository.GetByIdAsync(request.Id, ct);
        if (item == null)
            throw new FileNotFoundException($"Gallery item with id {request.Id} not found.");

        item.UpdateDetails(request.Title, request.Description);

        if (request.ImageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                throw new ValidationException("Invalid image format.");
            if (request.ImageFile.Length > 5 * 1024 * 1024)
                throw new ValidationException("File size cannot exceed 5MB.");

            await _fileStorage.DeleteFileAsync(item.ImagePath, ct);

            using var stream = request.ImageFile.OpenReadStream();
            var newPath = await _fileStorage.SaveFileAsync(stream, request.ImageFile.FileName, ct);
            item.UpdateImage(newPath);
        }

        _repository.Update(item);
        await _repository.SaveChangesAsync(ct);

        return new GalleryItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            ImageUrl = item.ImagePath,
            CreatedAt = item.CreatedOn,
            UpdatedAt = item.UpdatedOn
        };
    }
}