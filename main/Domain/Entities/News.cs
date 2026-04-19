using Domain.Common;
using Domain.Interfaces;

namespace Domain.Entities;

public class News : CommonEntities,IBaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; } 
    public string NewsURL { get; set; } 
    public string Author { get; set; } 
    public DateTime PublishDate { get; set; }
    public string? ImageUrl { get; set; } 
}