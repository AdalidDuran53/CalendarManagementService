using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class VwUserCalendarEvent
{
    public Guid UserId { get; set; }

    public int CalendarId { get; set; }

    public string CalendarName { get; set; } = null!;

    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public string EventDescription { get; set; } = null!;

    public DateTime DateEvent { get; set; }

    public DateTime EndDateEvent { get; set; }

    public int? EventImageId { get; set; }

    public byte[]? ImgEvent { get; set; }
}
