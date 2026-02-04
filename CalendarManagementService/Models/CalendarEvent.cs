using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class CalendarEvent
{
    public int Id { get; set; }

    public int CalendarId { get; set; }

    public string EventName { get; set; } = null!;

    public string EventDescription { get; set; } = null!;

    public DateTime DateEvent { get; set; }

    public DateTime EndDateEvent { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Calendar Calendar { get; set; } = null!;

    public virtual ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();
}
