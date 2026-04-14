namespace Domain.Entities;

public class GalleryItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImagePath { get; private set; } 
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private GalleryItem() { }

    public GalleryItem(string title, string description, string imagePath)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        ImagePath = imagePath;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}