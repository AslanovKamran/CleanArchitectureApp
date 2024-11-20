GO
CREATE PROC GetEvents
AS
BEGIN
   SET NOCOUNT ON;

   -- Get Events and Difficulty Level
   SELECT 
       Events.Id,
       Events.Name,
       Events.Description,
       Events.StartsAt,
       Events.EndsAt,
       Events.MaxParticipantsCount,
       Events.CurrentParticipantsCount,
       Events.DifficultyId,
       DifficultyLevels.Name AS DifficultyLevelName,
       DifficultyLevels.Description AS DifficultyLevelDescription,
       Events.Price,
       Events.Location,
       Events.CreatedAt,
       Events.ModifiedAt
   FROM Events
   LEFT JOIN DifficultyLevels ON DifficultyLevels.Id = Events.DifficultyId
   ORDER BY Events.Id; 

   -- Get Categories for each Event
   SELECT 
       EventsCategories.EventId AS EventId,
       Categories.Id AS CategoryId,
       Categories.Name AS CategoryName,
       Categories.Description AS CategoryDescription
   FROM EventsCategories
   LEFT JOIN Categories ON Categories.Id = EventsCategories.CategoryId
   ORDER BY EventsCategories.EventId;
   
END


GO
CREATE PROC GetEventById @Id  INT
AS
BEGIN
   SET NOCOUNT ON;

   -- Get Event and Difficulty Level
   SELECT 
       Events.Id,
       Events.Name,
       Events.Description,
       Events.StartsAt,
       Events.EndsAt,
       Events.MaxParticipantsCount,
       Events.CurrentParticipantsCount,
       Events.DifficultyId,
       DifficultyLevels.Name AS DifficultyLevelName,
       DifficultyLevels.Description AS DifficultyLevelDescription,
       Events.Price,
       Events.Location,
       Events.CreatedAt,
       Events.ModifiedAt
   FROM Events
   LEFT JOIN DifficultyLevels ON DifficultyLevels.Id = Events.DifficultyId
   WHERE Events.Id = @Id
   ORDER BY Events.Id;

   -- Get Categories for the Event
   SELECT 
       EventsCategories.EventId AS EventId,
       Categories.Id AS CategoryId,
       Categories.Name AS CategoryName,
       Categories.Description AS CategoryDescription
   FROM EventsCategories
   LEFT JOIN Categories ON Categories.Id = EventsCategories.CategoryId
   WHERE EventsCategories.EventId = @Id
   ORDER BY EventsCategories.EventId;
   
END


GO
CREATE PROC AddEvent
    @Name NVARCHAR(255),
    @Description NVARCHAR(255),
    @StartsAt DATETIME,
    @EndsAt DATETIME,
    @MaxParticipantsCount INT,
    @CurrentParticipantsCount INT,
    @DifficultyId INT,
    @Price DECIMAL(10,2),
    @Location NVARCHAR(255),
    @CategoryIds NVARCHAR(MAX), -- Comma-separated list of category Ids
    @EventId INT OUTPUT -- Output parameter to return the EventId
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM DifficultyLevels WHERE Id = @DifficultyId)
        BEGIN
            RAISERROR('The selected difficulty level does not exist. Please choose a valid difficulty level.', 16, 1);
            RETURN;
        END

        -- Insert Event
        INSERT INTO Events (Name, Description, StartsAt, EndsAt, MaxParticipantsCount, 
                            CurrentParticipantsCount, DifficultyId, Price, Location, CreatedAt, ModifiedAt)
        VALUES (@Name, @Description, @StartsAt, @EndsAt, @MaxParticipantsCount, 
                @CurrentParticipantsCount, @DifficultyId, @Price, @Location, GETDATE(), GETDATE());

        -- Get the Id of the newly inserted Event
        SET @EventId = SCOPE_IDENTITY();

        -- Insert Categories for the Event
        DECLARE @CategoryId INT;

        -- Declare a cursor to iterate through the categories
        DECLARE CategoryCursor CURSOR FOR
            SELECT value 
            FROM STRING_SPLIT(@CategoryIds, ','); -- Split the CategoryIds and use the result

        OPEN CategoryCursor;

        -- Loop through the cursor
        FETCH NEXT FROM CategoryCursor INTO @CategoryId;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId)
            BEGIN
                RAISERROR('One or more selected categories do not exist. Please choose valid categories.', 16, 1);
                CLOSE CategoryCursor;
                DEALLOCATE CategoryCursor;
                RETURN;
            END

            -- Insert category for the Event
            INSERT INTO EventsCategories (EventId, CategoryId) 
            VALUES (@EventId, @CategoryId);

            FETCH NEXT FROM CategoryCursor INTO @CategoryId;
        END

        -- Clean up cursor
        CLOSE CategoryCursor;
        DEALLOCATE CategoryCursor;

        -- Commit the transaction if everything is successful
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
       -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION;

        -- Ensure cursor cleanup if it was already opened
        IF CURSOR_STATUS('global', 'CategoryCursor') >= -1
        BEGIN
            CLOSE CategoryCursor;
            DEALLOCATE CategoryCursor;
        END

        -- Rethrow the error message from the catch block
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR('An error occurred while adding the event: %s', 16, 1, @ErrorMessage);
    END CATCH
END


GO
CREATE PROCEDURE DeleteEvent @Id  INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;

        -- Check if the event exists before attempting to delete
        IF NOT EXISTS (SELECT 1 FROM Events WHERE Id = @Id)
        BEGIN
            RAISERROR('The event with the given ID does not exist. Please check the event ID and try again.', 16, 1);
            RETURN;
        END

        -- Delete related categories from EventsCategories table first (if any)
        DELETE FROM EventsCategories WHERE EventId = @Id;

        -- Delete the event from the Events table
        DELETE FROM Events WHERE Id = @Id;

        -- Commit the transaction
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback transaction in case of an error
        ROLLBACK TRANSACTION;

        -- Rethrow the error message
        RAISERROR('The event with the given ID does not exist. Please check the event ID and try again.', 16, 1);
    END CATCH
END


GO
CREATE PROC UpdateEvent
    @Id INT,
    @Name NVARCHAR(255),
    @Description NVARCHAR(255),
    @StartsAt DATETIME,
    @EndsAt DATETIME,
    @MaxParticipantsCount INT,
    @CurrentParticipantsCount INT,
    @DifficultyId INT,
    @Price DECIMAL(10,2),
    @Location NVARCHAR(255),
    @CategoryIds NVARCHAR(MAX) -- Comma-separated list of category Ids
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;

        -- Validate Difficulty Level
        IF NOT EXISTS (SELECT 1 FROM DifficultyLevels WHERE Id = @DifficultyId)
        BEGIN
            RAISERROR('The selected difficulty level does not exist. Please choose a valid difficulty level.', 16, 1);
            RETURN;
        END

        -- Update Event
        UPDATE Events
        SET 
            Name = @Name,
            Description = @Description,
            StartsAt = @StartsAt,
            EndsAt = @EndsAt,
            MaxParticipantsCount = @MaxParticipantsCount,
            CurrentParticipantsCount = @CurrentParticipantsCount,
            DifficultyId = @DifficultyId,
            Price = @Price,
            Location = @Location,
            ModifiedAt = GETDATE()
        WHERE Id = @Id;

        -- Check if the Event exists
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Event with the given ID does not exist. Please provide a valid EventId.', 16, 1);
            RETURN;
        END

        -- Delete all existing categories for this event
        DELETE FROM EventsCategories WHERE EventId = @Id;

        -- Insert New Categories for the Event
        DECLARE @CategoryId INT;
        DECLARE @CategoryCursor CURSOR;

        -- Declare the cursor to iterate through the categories
        SET @CategoryCursor = CURSOR FOR
            SELECT value 
            FROM STRING_SPLIT(@CategoryIds, ','); -- Split the CategoryIds and use the result

        OPEN @CategoryCursor;

        -- Loop through the cursor
        FETCH NEXT FROM @CategoryCursor INTO @CategoryId;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Check if Category exists
            IF NOT EXISTS (SELECT 1 FROM Categories WHERE Id = @CategoryId)
            BEGIN
                RAISERROR('One or more selected categories do not exist. Please choose valid categories.', 16, 1);
                CLOSE @CategoryCursor;
                DEALLOCATE @CategoryCursor;
                RETURN;
            END

            -- Insert the new category for the Event
            INSERT INTO EventsCategories (EventId, CategoryId) 
            VALUES (@Id, @CategoryId);

            FETCH NEXT FROM @CategoryCursor INTO @CategoryId;
        END

        -- Clean up cursor
        CLOSE @CategoryCursor;
        DEALLOCATE @CategoryCursor;

        -- Commit the transaction if everything is successful
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR('An error occurred while updating the event: %s', 16, 1, @ErrorMessage);
    END CATCH
END
