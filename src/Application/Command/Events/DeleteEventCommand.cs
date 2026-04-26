using MediatR;

namespace Application.Commands;

public record DeleteEventCommand(Guid Id) : IRequest;