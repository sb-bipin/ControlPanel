using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, NewsDto>
{
    private readonly IRepository<News> _newsRepo;
    private readonly IFileStorage _fileStorage;
    public UpdateNewsCommandHandler(IRepository<News> newsRepo, IFileStorage fileStorage)
    {
        _newsRepo = newsRepo;
        _fileStorage = fileStorage;
    }

    public async Task<NewsDto> Handle(UpdateNewsCommand request, CancellationToken ct)
    {
        var news = await _newsRepo.GetByIdAsync(request.Id, ct);
        if (news == null)
            throw new Exception($"News with id {request.Id} not found");

        if (request.ImageUrl != null)
        {
            if (!string.IsNullOrEmpty(news.ImageUrl) && !news.ImageUrl.Contains("default"))
            {
                await _fileStorage.DeleteFileAsync(news.ImageUrl, ct);
            }
            using var stream = request.ImageUrl.OpenReadStream();
            news.ImageUrl = await _fileStorage.SaveFileAsync(stream, request.ImageUrl.FileName, ct);
        }

        news.Title = request.Title;
        news.Description = request.Description;
        news.NewsURL = request.NewsURL;
        news.Author = request.Author;
        news.PublishDate = request.PublishDate;

        _newsRepo.Update(news);
        await _newsRepo.SaveChangesAsync(ct);

        return new NewsDto
        {
            Id = news.Id,
            Title = news.Title,
            Description = news.Description,
            NewsURL = news.NewsURL,
            Author = news.Author,
            PublishDate = news.PublishDate,
            ImageUrl = news.ImageUrl
        };
    }
}