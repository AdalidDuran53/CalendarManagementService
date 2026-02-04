using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class RequestJointCalendarStatus
{
    public int Id { get; set; }

    public string StatusDescription { get; set; } = null!;

    public virtual ICollection<RequestJointCalendar> RequestJointCalendars { get; set; } = new List<RequestJointCalendar>();
}
