CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    IsInactive BIT NOT NULL,
    LastLogin DATETIME NOT NULL
);
go
CREATE TABLE AuditLogs (
    Id INT PRIMARY KEY IDENTITY,
    EntityName NVARCHAR(100) NOT NULL,
    EntityId INT NOT NULL,
    ChangeType NVARCHAR(50) NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Description NVARCHAR(MAX)
);
go
-- Optional trigger for delete auditing
CREATE TRIGGER trg_User_Delete
ON Users
AFTER DELETE
AS
BEGIN
    INSERT INTO AuditLogs (EntityName, EntityId, ChangeType, ChangedAt, Description)
    SELECT 'User', Id, 'Deleted', GETUTCDATE(), CONCAT('User ', Name, ' was deleted') FROM deleted;
END;