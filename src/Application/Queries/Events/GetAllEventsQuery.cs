using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetAllEventsQuery : IRequest<List<EventDto>>;