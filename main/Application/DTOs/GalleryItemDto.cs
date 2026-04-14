namespace Application.DTOs;

public class GalleryItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }  // full URL to access image
}