using Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class LocalFileStorage : IFileStorage
{
    private readonly string _storageRoot;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalFileStorage(IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
    {
        _storageRoot = configuration["ImageStorage:RootPath"]
            ?? throw new InvalidOperationException("ImageStorage:RootPath is not configured");

        _httpContextAccessor = httpContextAccessor;
        if (!Directory.Exists(_storageRoot))
            Directory.CreateDirectory(_storageRoot);
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var fullPath = Path.Combine(_storageRoot, uniqueName);

        using var file = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await fileStream.CopyToAsync(file, ct);

        return $"/images/{uniqueName}";
    }

    public Task DeleteFileAsync(string imageUrlPath, CancellationToken ct = default)
    {
        var fileName = Path.GetFileName(imageUrlPath);
        var fullPath = Path.Combine(_storageRoot, fileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}