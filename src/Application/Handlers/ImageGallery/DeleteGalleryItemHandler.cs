using Application.Commands;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class DeleteGalleryItemHandler : IRequestHandler<DeleteGalleryItemCommand,Unit>
{
    private readonly IRepository<GalleryItem> _repository;
    private readonly IFileStorage _fileStorage;

    public DeleteGalleryItemHandler(IRepository<GalleryItem> repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<Unit> Handle(DeleteGalleryItemCommand request, CancellationToken ct)
    {
        var item = await _repository.GetByIdAsync(request.Id, ct);
        if (item == null)
            throw new FileNotFoundException($"Gallery item with id {request.Id} not found.");

        await _fileStorage.DeleteFileAsync(item.ImagePath, ct);

        _repository.Delete(item);
        await _repository.SaveChangesAsync(ct);

        return Unit.Value;
    }
}