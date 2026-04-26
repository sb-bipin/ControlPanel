using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using MediatR;

namespace Application.Handlers;

public class DeleteNoticeCommandHandler : IRequestHandler<DeleteNoticeCommand>
{
    private readonly IRepository<Notice> _noticeRepo;
    private readonly IFileStorage _fileStorage;

    public DeleteNoticeCommandHandler(IRepository<Notice> noticeRepo, IFileStorage fileStorage)
    {
        _noticeRepo = noticeRepo;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteNoticeCommand request, CancellationToken ct)
    {
        var notice = await _noticeRepo.GetByIdAsync(request.Id, ct);
        if (notice == null)
            throw new Exception($"Notice with id {request.Id} not found");

        if (!string.IsNullOrEmpty(notice.ImageUrl))
            await _fileStorage.DeleteFileAsync(notice.ImageUrl, ct);

        _noticeRepo.Delete(notice);
        await _noticeRepo.SaveChangesAsync(ct);
    }
}