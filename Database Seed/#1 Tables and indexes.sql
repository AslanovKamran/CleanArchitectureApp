GO
CREATE DATABASE WildPathDataBase

GO
CREATE TABLE DifficultyLevels 
(
[Id] INT IDENTITY PRIMARY KEY, 
[Name] NVARCHAR (255) UNIQUE NOT NULL,
[Description] NVARCHAR (255) NULL,
[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
[ModifiedAt] DATETIME NULL
)

GO
CREATE TABLE Categories (
[Id] INT IDENTITY PRIMARY KEY, 
[Name] NVARCHAR (255) UNIQUE NOT NULL,
[Description] NVARCHAR (255) NULL,
[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
[ModifiedAt] DATETIME NULL
)

GO
CREATE TABLE Events (
[Id] INT IDENTITY PRIMARY KEY, 
[Name] NVARCHAR(255) NOT NULL,
[Description] NVARCHAR (255) NULL,
[StartsAt] DATETIME NOT NULL,
[EndsAt] DATETIME NOT NULL,
[MaxParticipantsCount] INT NOT NULL,
[CurrentParticipantsCount] INT NOT NULL, 
[DifficultyId] INT FOREIGN KEY REFERENCES DifficultyLevels(Id) NOT NULL,
[Price] DECIMAL (10,2) NOT NULL,
[Location] NVARCHAR(255) NOT NULL,
[CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
[ModifiedAt] DATETIME NULL
)

--Many-to-many relationship 
GO
CREATE TABLE EventsCategories
(
[EventId] INT FOREIGN KEY  REFERENCES Events(Id),
[CategoryId] INT FOREIGN KEY  REFERENCES Categories(Id),

--Composite key that will preserve uniqueness of events and their difficulty levels
CONSTRAINT PK_EventsCategories PRIMARY KEY (EventId, CategoryId)
)

--Non-clustered indexes to boost the performance
GO
CREATE NONCLUSTERED INDEX IX_Events_DifficultyId ON Events (DifficultyId);
CREATE NONCLUSTERED INDEX IX_EventsCategories_EventId_CategoryId ON EventsCategories (EventId, CategoryId);
CREATE NONCLUSTERED INDEX IX_Categories_Name ON Categories (Name);
CREATE NONCLUSTERED INDEX IX_DifficultyLevels_Name ON DifficultyLevels (Name);