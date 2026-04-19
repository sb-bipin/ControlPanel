using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetEventByIdQuery(Guid Id) : IRequest<EventDto?>;