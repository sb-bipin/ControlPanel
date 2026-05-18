using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Handlers;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IRepository<EventDetail> _repository;
    private readonly IFileStorage _fileStorage;

    public CreateEventCommandHandler(IRepository<EventDetail> repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken ct)
    {
        string? imageUrl = null;
        if (request.ImageFile != null)
        {
            ValidateImage(request.ImageFile);
            using var stream = request.ImageFile.OpenReadStream();
            imageUrl = await _fileStorage.SaveFileAsync(stream, request.ImageFile.FileName, ct);
        }

        var utcDate = request.Date.Kind == DateTimeKind.Unspecified
        ? DateTime.SpecifyKind(request.Date, DateTimeKind.Utc)
        : request.Date.ToUniversalTime();

        var eventEntity = new EventDetail(
            request.Title, request.Description, utcDate, request.Time,
            request.Location, request.Organizer, imageUrl);

        await _repository.AddAsync(eventEntity, ct);
        await _repository.SaveChangesAsync(ct);

        return MapToDto(eventEntity);
    }

    private void ValidateImage(IFormFile file)
    {
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
            throw new ValidationException("Invalid image format.");
        if (file.Length > 5 * 1024 * 1024)
            throw new ValidationException("Max 5MB.");
    }

    private EventDto MapToDto(EventDetail e) => new()
    {
        Id = e.Id,
        Title = e.Title,
        Description = e.Description,
        Date = e.Date,
        Time = e.Time,
        Location = e.Location,
        Organizer = e.Organizer,
        ImageUrl = e.ImageUrl
    };
}