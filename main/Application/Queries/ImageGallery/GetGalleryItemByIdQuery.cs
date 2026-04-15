using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetGalleryItemByIdQuery(Guid Id) : IRequest<GalleryItemDto>;
