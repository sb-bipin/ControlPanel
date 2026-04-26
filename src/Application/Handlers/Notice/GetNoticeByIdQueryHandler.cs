using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using Application.Queries;
using MediatR;

namespace Application.Handlers;

public class GetNoticeByIdQueryHandler : IRequestHandler<GetNoticeByIdQuery, NoticeDto?>
{
    private readonly IRepository<Notice> _noticeRepo;

    public GetNoticeByIdQueryHandler(IRepository<Notice> noticeRepo)
    {
        _noticeRepo = noticeRepo;
    }

    public async Task<NoticeDto?> Handle(GetNoticeByIdQuery request, CancellationToken ct)
    {
        var notice = await _noticeRepo.GetByIdAsync(request.Id, ct);
        return notice == null ? null : new NoticeDto
        {
            Id = notice.Id,
            Title = notice.Title,
            Description = notice.Description,
            Date = notice.Date,
            Important = notice.Important,
            ImageUrl = notice.ImageUrl
        };

    }
}