using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class RequestJointCalendar
{
    public int Id { get; set; }

    public int? CalendarId { get; set; }

    public Guid? RequestingUser { get; set; }

    public Guid? UserRequested { get; set; }

    public int? StatusId { get; set; }

    public virtual Calendar? Calendar { get; set; }

    public virtual User? RequestingUserNavigation { get; set; }

    public virtual RequestJointCalendarStatus? Status { get; set; }

    public virtual User? UserRequestedNavigation { get; set; }
}
