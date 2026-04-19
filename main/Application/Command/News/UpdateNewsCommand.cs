// Application/Commands/UpdateNewsCommand.cs
using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public class UpdateNewsCommand : IRequest<NewsDto>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NewsURL { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public IFormFile? ImageUrl { get; set; }  
}