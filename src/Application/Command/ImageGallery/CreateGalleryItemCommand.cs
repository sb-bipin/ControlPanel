using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public class CreateGalleryItemCommand : IRequest<GalleryItemDto>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile ImageFile { get; set; }
}