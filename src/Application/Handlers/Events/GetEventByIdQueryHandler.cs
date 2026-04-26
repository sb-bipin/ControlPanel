
using Application.DTOs;
using Application.Queries;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto?>
{
    private readonly IRepository<EventDetail> _repository;
    public GetEventByIdQueryHandler(IRepository<EventDetail> repository) => _repository = repository;

    public async Task<EventDto?> Handle(GetEventByIdQuery request, CancellationToken ct)
    {
        var e = await _repository.GetByIdAsync(request.Id, ct);
        if (e == null) return null;
        return new EventDto
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Date = e.Date,
            Time = e.Time,
            Location = e.Location,
            Organizer = e.Organizer,
            ImageUrl = e.ImageUrl
        };
    }
}