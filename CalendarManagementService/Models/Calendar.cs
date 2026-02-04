using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class Calendar
{
    public int Id { get; set; }

    public string? CalendarName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

    public virtual ICollection<RequestJointCalendar> RequestJointCalendars { get; set; } = new List<RequestJointCalendar>();

    public virtual ICollection<UserCalendar> UserCalendars { get; set; } = new List<UserCalendar>();
}
