using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class EventImage
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public byte[] ImgEvent { get; set; } = null!;

    public virtual CalendarEvent Event { get; set; } = null!;
}
