using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers;

public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, List<NewsDto>>
{
    private readonly IRepository<News> _newsRepo;

    public GetAllNewsQueryHandler(IRepository<News> newsRepo)
    {
        _newsRepo = newsRepo;
    }

    public async Task<List<NewsDto>> Handle(GetAllNewsQuery request, CancellationToken ct)
    {
        var news = await _newsRepo.GetAllAsync(ct);
        var ordered = news.OrderByDescending(n => n.PublishDate).ToList();
        return ordered.Select(n => new NewsDto
        {
            Id = n.Id,
            Title = n.Title,
            Description = n.Description,
            NewsURL = n.NewsURL,
            Author = n.Author,
            PublishDate = n.PublishDate,
            ImageUrl = n.ImageUrl
        }).ToList();
    }
}