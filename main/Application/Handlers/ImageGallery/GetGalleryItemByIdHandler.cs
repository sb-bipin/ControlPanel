using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class GetGalleryItemByIdHandler : IRequestHandler<GetGalleryItemByIdQuery, GalleryItemDto>
{
    private readonly IRepository<GalleryItem> _repository;

    public GetGalleryItemByIdHandler(IRepository<GalleryItem> repository)
    {
        _repository = repository;
    }

    public async Task<GalleryItemDto> Handle(GetGalleryItemByIdQuery request, CancellationToken ct)
    {
        var item = await _repository.GetByIdAsync(request.Id, ct);
        if (item == null)
            throw new FileNotFoundException($"Gallery item with id {request.Id} not found.");

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