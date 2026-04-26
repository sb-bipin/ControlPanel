using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public class UpdateEventCommand : IRequest<EventDto>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Organizer { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
}