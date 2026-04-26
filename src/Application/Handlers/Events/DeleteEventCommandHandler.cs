using Application.Commands;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Handlers;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IRepository<EventDetail> _repository;
    private readonly IFileStorage _fileStorage;

    public DeleteEventCommandHandler(IRepository<EventDetail> repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken ct)
    {
        var eventEntity = await _repository.GetByIdAsync(request.Id, ct);
        if (eventEntity == null)
            throw new ValidationException("Event not found.");

        if (!string.IsNullOrEmpty(eventEntity.ImageUrl))
            await _fileStorage.DeleteFileAsync(eventEntity.ImageUrl, ct);

        _repository.Delete(eventEntity);
        await _repository.SaveChangesAsync(ct);
    }
}