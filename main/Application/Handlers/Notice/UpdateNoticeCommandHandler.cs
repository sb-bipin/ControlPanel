using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class UpdateNoticeCommandHandler : IRequestHandler<UpdateNoticeCommand, NoticeDto>
{
    private readonly IRepository<Notice> _noticeRepo;
    private readonly IFileStorage _fileStorage;

    public UpdateNoticeCommandHandler(IRepository<Notice> noticeRepo, IFileStorage fileStorage)
    {
        _noticeRepo = noticeRepo;
        _fileStorage = fileStorage;
    }

    public async Task<NoticeDto> Handle(UpdateNoticeCommand request, CancellationToken ct)
    {
        var notice = await _noticeRepo.GetByIdAsync(request.Id, ct);
        if (notice == null)
            throw new Exception($"Notice with id {request.Id} not found");

        if (request.ImageFile != null)
        {
            if (!string.IsNullOrEmpty(notice.ImageUrl))
                await _fileStorage.DeleteFileAsync(notice.ImageUrl, ct);
            using var stream = request.ImageFile.OpenReadStream();
            notice.ImageUrl = await _fileStorage.SaveFileAsync(stream, request.ImageFile.FileName, ct);
        }

        notice.Title = request.Title;
        notice.Description = request.Description;
        notice.Date = request.Date;
        notice.Important = request.Important;

        _noticeRepo.Update(notice);
        await _noticeRepo.SaveChangesAsync(ct);

        return new NoticeDto
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