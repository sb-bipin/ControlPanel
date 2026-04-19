using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using Application.Queries;
using MediatR;

namespace Application.Handlers;

public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, NewsDto?>
{
    private readonly IRepository<News> _newsRepo;

    public GetNewsByIdQueryHandler(IRepository<News> newsRepo)
    {
        _newsRepo = newsRepo;
    }

    public async Task<NewsDto?> Handle(GetNewsByIdQuery request, CancellationToken ct)
    {
        var news = await _newsRepo.GetByIdAsync(request.Id, ct);
        return news == null ? null : new NewsDto
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