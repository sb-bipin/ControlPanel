using Domain.Common;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;
public class Notice : CommonEntities, IBaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string ImageUrl { get; set; }
    public bool Important { get; set; } = false;
}

