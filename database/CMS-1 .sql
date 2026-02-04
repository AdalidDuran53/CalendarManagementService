-- Feature: CMS-1
-- Author: Adalid
-- Purpose: Create a new database
CREATE DATABASE CalendarManagementServiceDB
GO

USE WebApiProjectBaseDB;
CREATE TABLE CalendarManagementServiceDB.dbo.Users (
    UserID UNIQUEIDENTIFIER PRIMARY KEY, -- UserID, must be UNIQUEIDENTIFIER
	UserEmail NVARCHAR(100) UNIQUE,
    UserName NVARCHAR(100),         -- User name, must be UNIQUE
    PasswordHash NVARCHAR(MAX) NOT NULL,          -- PasswordHash, required
    PasswordSalst NVARCHAR(MAX) NOT NULL,          -- PasswordHash, required
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
);

CREATE TABLE CalendarManagementServiceDB.dbo.SessionLog (
    SessionID UNIQUEIDENTIFIER PRIMARY KEY, 
    UserID UNIQUEIDENTIFIER,         -- Foreign key
    InitSession DATETIME NOT NULL,          
    EndSession DATETIME 
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE CalendarManagementServiceDB.dbo.OperationLog (
    OperationID INT IDENTITY(1,1) PRIMARY KEY, 
    SessionID UNIQUEIDENTIFIER,      -- Foreign key 
    OperationDate DATETIME,         
    Request NVARCHAR(MAX),          
    Response NVARCHAR(MAX)
    FOREIGN KEY (SessionID) REFERENCES SessionLog(SessionID)
);

CREATE TABLE CalendarManagementServiceDB.dbo.Calendars (
    ID INT IDENTITY(1,1) PRIMARY KEY, 
	CalendarName NVARCHAR(100),
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
);

CREATE TABLE CalendarManagementServiceDB.dbo.UserCalendars (
    ID INT IDENTITY(1,1) PRIMARY KEY, 
	CalendarID INT,
	UserID UNIQUEIDENTIFIER,
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
    FOREIGN KEY (CalendarID) REFERENCES Calendars(ID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE CalendarManagementServiceDB.dbo.CalendarEvents (
    ID INT IDENTITY(1,1) PRIMARY KEY, 
	CalendarID INT,
	EventName NVARCHAR(50),
	EventDescription NVARCHAR(100),
	DateEvent DATETIME,
	EndDateEvent DATETIME,
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
    FOREIGN KEY (CalendarID) REFERENCES Calendars(ID)
);

CREATE TABLE CalendarManagementServiceDB.dbo.EventImages (
    ID INT IDENTITY(1,1) PRIMARY KEY, 
	EventID INT,
	ImgEvent IMAGE,
    FOREIGN KEY (EventID) REFERENCES CalendarEvents(ID)
);


CREATE TABLE CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (
    ID INT IDENTITY(1,1) PRIMARY KEY, 
	StatusDescription NVARCHAR(50)
);

INSERT INTO CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (StatusDescription) VALUES ('Pending');
INSERT INTO CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (StatusDescription) VALUES ('Accepted');
INSERT INTO CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (StatusDescription) VALUES ('Declined');
INSERT INTO CalendarManagementServiceDB.dbo.RequestJointCalendarStatus (StatusDescription) VALUES ('Deleted');

CREATE TABLE CalendarManagementServiceDB.dbo.RequestJointCalendar (
    ID INT IDENTITY(1,1) PRIMARY KEY, 
	CalendarID INT,
	RequestingUser UNIQUEIDENTIFIER,
	UserRequested UNIQUEIDENTIFIER,
    StatusID INT,
    FOREIGN KEY (CalendarID) REFERENCES Calendars(ID),
    FOREIGN KEY (RequestingUser) REFERENCES  Users(UserID),
    FOREIGN KEY (UserRequested) REFERENCES  Users(UserID),
    FOREIGN KEY (StatusID) REFERENCES  RequestJointCalendarStatus(ID)
);


USE CalendarManagementServiceDB;
GO

CREATE VIEW dbo.vw_UserCalendarEvents
AS
SELECT 
    uc.UserID,
    c.ID AS CalendarID,
    c.CalendarName,
    ce.ID AS EventID,
    ce.EventName,
    ce.EventDescription,
    ce.DateEvent,
    ce.EndDateEvent,
    ei.ID AS EventImageID,
    ei.ImgEvent
FROM dbo.UserCalendars uc
INNER JOIN dbo.Calendars c 
    ON uc.CalendarID = c.ID
INNER JOIN dbo.CalendarEvents ce 
    ON c.ID = ce.CalendarID
LEFT JOIN dbo.EventImages ei 
    ON ce.ID = ei.EventID
WHERE uc.isDeleted = 0
  AND c.isDeleted = 0
  AND ce.isDeleted = 0;
GO

