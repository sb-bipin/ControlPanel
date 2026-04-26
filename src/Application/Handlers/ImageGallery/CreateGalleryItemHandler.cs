using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Handlers;

public class CreateGalleryItemHandler : IRequestHandler<CreateGalleryItemCommand, GalleryItemDto>
{
    private readonly IRepository<GalleryItem> _repository;
    private readonly IFileStorage _fileStorage;

    public CreateGalleryItemHandler(IRepository<GalleryItem> repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<GalleryItemDto> Handle(CreateGalleryItemCommand request, CancellationToken ct)
    {
        if (request.ImageFile == null || request.ImageFile.Length == 0)
            throw new ValidationException("Image file is required.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var ext = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
            throw new ValidationException("Invalid image format. Allowed: jpg, jpeg, png, gif, webp.");

        if (request.ImageFile.Length > 5 * 1024 * 1024) // 5MB
            throw new ValidationException("File size cannot exceed 5MB.");

        using var stream = request.ImageFile.OpenReadStream();
        var savedUrlPath = await _fileStorage.SaveFileAsync(stream, request.ImageFile.FileName, ct);

        var galleryItem = new GalleryItem(request.Title, request.Description, savedUrlPath);

        await _repository.AddAsync(galleryItem, ct);
        await _repository.SaveChangesAsync(ct);

        return new GalleryItemDto
        {
            Id = galleryItem.Id,
            Title = galleryItem.Title,
            Description = galleryItem.Description,
            ImageUrl = savedUrlPath 
        };
    }
}