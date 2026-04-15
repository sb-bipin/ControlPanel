namespace Application.DTOs;

public class GalleryItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }
}