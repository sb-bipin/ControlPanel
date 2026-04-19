using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<EventDto>>
{
    private readonly IRepository<EventDetail> _repository;
    public GetAllEventsQueryHandler(IRepository<EventDetail> repository) => _repository = repository;

    public async Task<List<EventDto>> Handle(GetAllEventsQuery request, CancellationToken ct)
    {
        var events = await _repository.GetAllAsync(ct);
        return events.Select(e => new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Date = e.Date,
            Time = e.Time,
            Location = e.Location,
            Organizer = e.Organizer,
            ImageUrl = e.ImageUrl
        }).ToList();
    }
}
