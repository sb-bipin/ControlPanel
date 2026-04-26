namespace Application.DTOs;

public class NoticeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string? ImageUrl { get; set; }
    public bool Important { get; set; }
}