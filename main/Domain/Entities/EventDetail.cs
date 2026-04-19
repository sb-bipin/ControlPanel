using Domain.Common;
using Domain.Interfaces;

namespace Domain.Entities;

public class EventDetail : CommonEntities,IBaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public string Time { get; private set; } 
    public string Location { get; private set; }
    public string? ImageUrl { get; private set; }
    public string Organizer { get; private set; }

    public EventDetail(string title, string description, DateTime date, string time,
                 string location, string organizer, string? imageUrl = null)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Date = date;
        Time = time;
        Location = location;
        Organizer = organizer;
        ImageUrl = imageUrl;
    }

    public void Update(string title, string description, DateTime date, string time,
                       string location, string organizer, string? imageUrl)
    {
        Title = title;
        Description = description;
        Date = date;
        Time = time;
        Location = location;
        Organizer = organizer;
        ImageUrl = imageUrl;
    }
}