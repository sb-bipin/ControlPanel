using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class CreateNoticeCommandHandler : IRequestHandler<CreateNoticeCommand, NoticeDto>
{
    private readonly IRepository<Notice> _noticeRepo;
    private readonly IFileStorage _fileStorage;

    public CreateNoticeCommandHandler(IRepository<Notice> noticeRepo, IFileStorage fileStorage)
    {
        _noticeRepo = noticeRepo;
        _fileStorage = fileStorage;
    }

    public async Task<NoticeDto> Handle(CreateNoticeCommand request, CancellationToken ct)
    {
        string? imageUrl = null;
        if (request.ImageFile != null)
        {
            using var stream = request.ImageFile.OpenReadStream();
            imageUrl = await _fileStorage.SaveFileAsync(stream, request.ImageFile.FileName, ct);
        }

        var notice = new Notice
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Date = request.Date,
            Important = request.Important,
            ImageUrl = imageUrl,
        };

        await _noticeRepo.AddAsync(notice, ct);
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