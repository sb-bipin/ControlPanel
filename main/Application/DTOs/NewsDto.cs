namespace Application.DTOs;

public class NewsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; } 
    public string NewsURL { get; set; } 
    public string Author { get; set; }
    public DateTime PublishDate { get; set; }
    public string? ImageUrl { get; set; } 
}