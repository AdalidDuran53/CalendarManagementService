using System;
using System.Collections.Generic;

namespace CalendarManagementService.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserEmail { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<RequestJointCalendar> RequestJointCalendarRequestingUserNavigations { get; set; } = new List<RequestJointCalendar>();

    public virtual ICollection<RequestJointCalendar> RequestJointCalendarUserRequestedNavigations { get; set; } = new List<RequestJointCalendar>();

    public virtual ICollection<SessionLog> SessionLogs { get; set; } = new List<SessionLog>();

    public virtual ICollection<UserCalendar> UserCalendars { get; set; } = new List<UserCalendar>();
}
