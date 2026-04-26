using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using MediatR;

namespace Application.Handlers;

public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand>
{
    private readonly IRepository<News> _newsRepo;
    private readonly IFileStorage _fileStorage;

    public DeleteNewsCommandHandler(IRepository<News> newsRepo, IFileStorage fileStorage)
    {
        _newsRepo = newsRepo;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteNewsCommand request, CancellationToken ct)
    {
        var news = await _newsRepo.GetByIdAsync(request.Id, ct);
        if (news == null)
            throw new Exception($"News with id {request.Id} not found");

        if (!string.IsNullOrEmpty(news.ImageUrl) && !news.ImageUrl.Contains("default"))
        {
            await _fileStorage.DeleteFileAsync(news.ImageUrl, ct);
        }

        _newsRepo.Delete(news);
        await _newsRepo.SaveChangesAsync(ct);
    }
}