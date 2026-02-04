using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class UserCalendar
{
    public int Id { get; set; }

    public int CalendarId { get; set; }

    public Guid UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Calendar Calendar { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
