using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class GetNoticeByIdQuery : IRequest<NoticeDto?>
{
    public Guid Id { get; set; }
}