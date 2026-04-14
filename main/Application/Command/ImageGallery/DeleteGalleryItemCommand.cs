using MediatR;

namespace Application.Commands;

public record DeleteGalleryItemCommand(Guid Id) : IRequest<Unit>;