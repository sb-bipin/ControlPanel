using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using Application.DTOs;
using MediatR;

namespace Application.Handlers;

public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, NewsDto>
{
    private readonly IRepository<News> _newsRepo;
    private readonly IFileStorage _fileStorage;
    public CreateNewsCommandHandler(IRepository<News> newsRepo, IFileStorage fileStorage)
    {
        _newsRepo = newsRepo;
        _fileStorage = fileStorage;
    }

    public async Task<NewsDto> Handle(CreateNewsCommand request, CancellationToken ct)
    {
        string? imageUrl = null;
        if (request.ImageUrl != null)
        {
            using var stream = request.ImageUrl.OpenReadStream();
            imageUrl = await _fileStorage.SaveFileAsync(stream, request.ImageUrl.FileName, ct);
        }

        var news = new News
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            NewsURL = request.NewsURL,
            Author = request.Author,
            PublishDate = request.PublishDate,
            ImageUrl = imageUrl
        };

        await _newsRepo.AddAsync(news, ct);
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