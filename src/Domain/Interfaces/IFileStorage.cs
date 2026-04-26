namespace Domain.Interfaces;

public interface IFileStorage
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken ct = default);
    Task DeleteFileAsync(string imageUrlPath, CancellationToken ct = default);
}