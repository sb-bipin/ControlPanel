using Domain.Common;
using Domain.Interfaces;

namespace Domain.Entities;

public class GalleryItem : CommonEntities
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImagePath { get; private set; } 
  
    private GalleryItem() { }

    public GalleryItem(string title, string description, string imagePath)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        ImagePath = imagePath;
    }

    public void UpdateDetails(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public void UpdateImage(string newImagePath)
    {
        ImagePath = newImagePath;
    }

}