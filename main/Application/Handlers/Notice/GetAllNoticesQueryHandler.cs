using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using Application.Queries;
using MediatR;

namespace Application.Handlers;

public class GetAllNoticesQueryHandler : IRequestHandler<GetAllNoticesQuery, List<NoticeDto>>
{
    private readonly IRepository<Notice> _noticeRepo;

    public GetAllNoticesQueryHandler(IRepository<Notice> noticeRepo)
    {
        _noticeRepo = noticeRepo;
    }

    public async Task<List<NoticeDto>> Handle(GetAllNoticesQuery request, CancellationToken ct)
    {
        var notices = await _noticeRepo.GetAllAsync(ct);
        var ordered = notices.OrderByDescending(n => n.Important)
                            .ThenByDescending(n => n.Date)
                            .ToList();
        return ordered.Select(n => new NoticeDto
        {
            Id = n.Id,
            Title = n.Title,
            Description = n.Description,
            Date = n.Date,
            Important = n.Important,
            ImageUrl = n.ImageUrl
        }).ToList();

    }
}