-- Feature: CMS-1
-- Author: Adalid
-- Purpose: Create a new database
CREATE DATABASE CalendarManagementServiceDB
GO

USE WebApiProjectBaseDB;
-- Define the Users table with necessary fields for user management.
CREATE TABLE CalendarManagementServiceDB.dbo.Users (
    UserID UNIQUEIDENTIFIER PRIMARY KEY,		   -- Unique identifier for each user
    UserEmail NVARCHAR(100) UNIQUE NOT NULL,       -- Email must be unique across all users
    UserName NVARCHAR(100) NOT NULL,               -- User's name (not necessarily unique)
    PasswordHash NVARCHAR(MAX) NOT NULL,		   -- Hashed password (required field)
    PasswordSalt NVARCHAR(MAX) NOT NULL,		   -- Salt used in hashing the password (required field)
    isDeleted BIT DEFAULT 0						   -- Indicates if the user has been logically deleted (default value: false)
);

-- Define the SessionLog table to track sessions initiated by users.
CREATE TABLE CalendarManagementServiceDB.dbo.SessionLog (
    SessionID UNIQUEIDENTIFIER PRIMARY KEY,      -- Unique identifier for each session
    UserID UNIQUEIDENTIFIER,                     -- Foreign key referencing Users table
    InitSession DATETIME NOT NULL,               -- Timestamp when the session was initiated (required field)
    EndSession DATETIME                          -- Timestamp when the session ended (can be null if still active)
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Define the OperationLog table to log operations performed by users
CREATE TABLE CalendarManagementServiceDB.dbo.OperationLog (
    OperationID INT IDENTITY(1,1) PRIMARY KEY,   -- Auto-incrementing primary key
    SessionID UNIQUEIDENTIFIER,                  -- Foreign key referencing SessionLog table
    OperationDate DATETIME,                      -- Timestamp when the operation was performed
    Request NVARCHAR(MAX),                       -- The request made (e.g., JSON payload)
    Response NVARCHAR(MAX)                       -- The response received from the system (e.g., JSON payload)
	FOREIGN KEY (SessionID) REFERENCES SessionLog(SessionID)
);

-- Define the Calendars table to store calendar-related data.
CREATE TABLE CalendarManagementServiceDB.dbo.Calendars (
    ID INT IDENTITY(1,1) PRIMARY KEY,           -- Auto-incrementing primary key
    CalendarName NVARCHAR(100) NOT NULL,        -- Name of the calendar (required field)
    isDeleted BIT DEFAULT 0                     -- Indicates if the calendar has been logically deleted (default value: false)
);

-- Define the UserCalendars table to manage user-calendar relationships.
CREATE TABLE CalendarManagementServiceDB.dbo.UserCalendars (
    ID INT IDENTITY(1,1) PRIMARY KEY,           -- Auto-incrementing primary key
    CalendarID INT NOT NULL,                    -- Foreign key referencing Calendars table (required field)
    UserID UNIQUEIDENTIFIER NOT NULL,           -- Foreign key referencing Users table (required field)
    isDeleted BIT DEFAULT 0                     -- Indicates if the user-calendar relationship has been logically deleted (default value: false)
    FOREIGN KEY (CalendarID) REFERENCES Calendars(ID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Define the CalendarEvents table to store events related to calendars.
CREATE TABLE CalendarManagementServiceDB.dbo.CalendarEvents (
    ID INT IDENTITY(1,1) PRIMARY KEY,           -- Auto-incrementing primary key
    CalendarID INT NOT NULL,                    -- Foreign key referencing Calendars table (required field)
    EventName NVARCHAR(50) NOT NULL,            -- Name of the event (required field)
    EventDescription NVARCHAR(100) NOT NULL,    -- Description of the event (required field)
    DateEvent DATETIME NOT NULL,                -- Start date and time of the event (required field)
    EndDateEvent DATETIME NOT NULL,             -- End date and time of the event (required field)
    isDeleted BIT DEFAULT 0                     -- Indicates if the calendar event has been logically deleted (default value: false)
    FOREIGN KEY (CalendarID) REFERENCES Calendars(ID)
);

-- Define the EventImages table to store images related to events.
CREATE TABLE CalendarManagementServiceDB.dbo.EventImages (
    ID INT IDENTITY(1,1) PRIMARY KEY,           -- Auto-incrementing primary key
    EventID INT NOT NULL,                       -- Foreign key referencing CalendarEvents table (required field)
    ImgEvent IMAGE NOT NULL                     -- Binary data for the event image (required field)
    FOREIGN KEY (EventID) REFERENCES CalendarEvents(ID)
);

-- Define a table to manage status of joint calendar requests.
CREATE TABLE CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (
    ID INT IDENTITY(1,1) PRIMARY KEY,           -- Auto-incrementing primary key
    StatusDescription NVARCHAR(50) NOT NULL     -- Description of the request status (required field)
);

-- Insert predefined statuses into RequestJointCalendarStatus table.
INSERT INTO CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (StatusDescription)
VALUES ('Pending'), ('Accepted'), ('Declined'), ('Deleted');

-- Define a table to manage joint calendar requests between users and calendars.
CREATE TABLE CalendarManagementServiceDB.dbo.RequestJointCalendar (
ID INT IDENTITY(1,1) PRIMARY KEY,                        -- Auto-incrementing primary key
    CalendarID INT NOT NULL,                             -- Foreign key referencing Calendars table (required field)
    RequestingUser UNIQUEIDENTIFIER NOT NULL,            -- User who requested to join the calendar (foreign key)
    UserRequested UNIQUEIDENTIFIER NOT NULL,             -- The user whose calendar was requested for joint access (foreign key)
    StatusID INT NOT NULL,                               -- Foreign key referencing RequestJointCalendarStatus table
    FOREIGN KEY (CalendarID) REFERENCES Calendars(ID),
    FOREIGN KEY (RequestingUser) REFERENCES Users(UserID),
    FOREIGN KEY (UserRequested) REFERENCES Users(UserID),
    FOREIGN KEY (StatusID) REFERENCES RequestJointCalendarStatus(ID)
);


USE CalendarManagementServiceDB;
GO

-- Create a view to provide a consolidated view of user-calendar events.
CREATE VIEW dbo.vw_UserCalendarEvents
AS
SELECT 
    uc.UserID,                                  -- User ID who owns the calendar
    c.ID AS CalendarID,                         -- Calendar ID
    c.CalendarName,                             -- Name of the calendar
    ce.ID AS EventID,                           -- Event ID within a calendar
    ce.EventName,                               -- Name of the event
    ce.EventDescription,                        -- Description of the event
    ce.DateEvent,                               -- Start date and time of the event
    ce.EndDateEvent,                            -- End date and time of the event (can be null if single-event or ongoing)
    ei.ID AS EventImageID,                      -- ID for the image associated with an event
    ei.ImgEvent                                 -- Binary data for the image
FROM dbo.UserCalendars uc
INNER JOIN dbo.Calendars c 
    ON uc.CalendarID = c.ID						-- Joining calendars table on CalendarID
INNER JOIN dbo.CalendarEvents ce 
    ON c.ID = ce.CalendarID						-- Joining calendar events table on CalendarID
LEFT JOIN dbo.EventImages ei 
    ON ce.ID = ei.EventID						-- Left join to get event images if available
WHERE uc.isDeleted = 0
  AND c.isDeleted = 0
  AND ce.isDeleted = 0;
GO

