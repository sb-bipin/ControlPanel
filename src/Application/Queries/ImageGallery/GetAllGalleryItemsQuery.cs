using MediatR;
using Application.DTOs;

namespace Application.Queries;

public record GetAllGalleryItemsQuery : IRequest<List<GalleryItemDto>>;