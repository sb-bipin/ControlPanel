using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public class UpdateNoticeCommand : IRequest<NoticeDto>
{
    public Guid Id { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; } 
    public DateTime Date { get; set; }
    public bool Important { get; set; }
    public IFormFile? ImageFile { get; set; }
}