using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers;

public class GetAllGalleryItemsHandler : IRequestHandler<GetAllGalleryItemsQuery, List<GalleryItemDto>>
{
    private readonly IRepository<GalleryItem> _repository;

    public GetAllGalleryItemsHandler(IRepository<GalleryItem> repository)
    {
        _repository = repository;
    }

    public async Task<List<GalleryItemDto>> Handle(GetAllGalleryItemsQuery request, CancellationToken ct)
    {
        var items = await _repository.GetAllAsync(ct);
        return items.OrderByDescending(x => x.CreatedOn).Select(x => new GalleryItemDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            ImageUrl = x.ImagePath 
        }).ToList();
    }
}