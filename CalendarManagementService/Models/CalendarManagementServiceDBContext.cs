using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagementService.Models;

public partial class CalendarManagementServiceDbContext : DbContext
{
    public CalendarManagementServiceDbContext()
    {
    }

    public CalendarManagementServiceDbContext(DbContextOptions<CalendarManagementServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<CalendarEvent> CalendarEvents { get; set; }

    public virtual DbSet<EventImage> EventImages { get; set; }

    public virtual DbSet<OperationLog> OperationLogs { get; set; }

    public virtual DbSet<RequestJointCalendar> RequestJointCalendars { get; set; }

    public virtual DbSet<RequestJointCalendarStatus> RequestJointCalendarStatuses { get; set; }

    public virtual DbSet<SessionLog> SessionLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCalendar> UserCalendars { get; set; }

    public virtual DbSet<VwUserCalendarEvent> VwUserCalendarEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC273E70129A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CalendarName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
        });

        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3214EC27E33BD412");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CalendarId).HasColumnName("CalendarID");
            entity.Property(e => e.DateEvent).HasColumnType("datetime");
            entity.Property(e => e.EndDateEvent).HasColumnType("datetime");
            entity.Property(e => e.EventDescription).HasMaxLength(100);
            entity.Property(e => e.EventName).HasMaxLength(50);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");

            entity.HasOne(d => d.Calendar).WithMany(p => p.CalendarEvents)
                .HasForeignKey(d => d.CalendarId)
                .HasConstraintName("FK__CalendarE__isDel__36B12243");
        });

        modelBuilder.Entity<EventImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventIma__3214EC27BB88ABBD");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ImgEvent).HasColumnType("image");

            entity.HasOne(d => d.Event).WithMany(p => p.EventImages)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__EventImag__Event__398D8EEE");
        });

        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC6416D8E239");

            entity.ToTable("OperationLog");

            entity.Property(e => e.OperationId).HasColumnName("OperationID");
            entity.Property(e => e.OperationDate).HasColumnType("datetime");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");

            entity.HasOne(d => d.Session).WithMany(p => p.OperationLogs)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__Operation__Respo__2B3F6F97");
        });

        modelBuilder.Entity<RequestJointCalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestJ__3214EC27338CB326");

            entity.ToTable("RequestJointCalendar");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CalendarId).HasColumnName("CalendarID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.Calendar).WithMany(p => p.RequestJointCalendars)
                .HasForeignKey(d => d.CalendarId)
                .HasConstraintName("FK__RequestJo__Calen__3E52440B");

            entity.HasOne(d => d.RequestingUserNavigation).WithMany(p => p.RequestJointCalendarRequestingUserNavigations)
                .HasForeignKey(d => d.RequestingUser)
                .HasConstraintName("FK__RequestJo__Reque__3F466844");

            entity.HasOne(d => d.Status).WithMany(p => p.RequestJointCalendars)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__RequestJo__Statu__412EB0B6");

            entity.HasOne(d => d.UserRequestedNavigation).WithMany(p => p.RequestJointCalendarUserRequestedNavigations)
                .HasForeignKey(d => d.UserRequested)
                .HasConstraintName("FK__RequestJo__UserR__403A8C7D");
        });

        modelBuilder.Entity<RequestJointCalendarStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestJ__3214EC27D6345220");

            entity.ToTable("RequestJointCalendarStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusDescription).HasMaxLength(50);
        });

        modelBuilder.Entity<SessionLog>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__SessionL__C9F4927065A968EB");

            entity.ToTable("SessionLog");

            entity.Property(e => e.SessionId)
                .ValueGeneratedNever()
                .HasColumnName("SessionID");
            entity.Property(e => e.EndSession).HasColumnType("datetime");
            entity.Property(e => e.InitSession).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SessionLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SessionLo__EndSe__286302EC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACD9036EE2");

            entity.HasIndex(e => e.UserEmail, "UQ__Users__08638DF8EC4A8AC7").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UserEmail).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserCalendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserCale__3214EC2725821772");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CalendarId).HasColumnName("CalendarID");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Calendar).WithMany(p => p.UserCalendars)
                .HasForeignKey(d => d.CalendarId)
                .HasConstraintName("FK__UserCalen__isDel__31EC6D26");

            entity.HasOne(d => d.User).WithMany(p => p.UserCalendars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserCalen__UserI__32E0915F");
        });

        modelBuilder.Entity<VwUserCalendarEvent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserCalendarEvents");

            entity.Property(e => e.CalendarId).HasColumnName("CalendarID");
            entity.Property(e => e.CalendarName).HasMaxLength(100);
            entity.Property(e => e.DateEvent).HasColumnType("datetime");
            entity.Property(e => e.EndDateEvent).HasColumnType("datetime");
            entity.Property(e => e.EventDescription).HasMaxLength(100);
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.EventImageId).HasColumnName("EventImageID");
            entity.Property(e => e.EventName).HasMaxLength(50);
            entity.Property(e => e.ImgEvent).HasColumnType("image");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
