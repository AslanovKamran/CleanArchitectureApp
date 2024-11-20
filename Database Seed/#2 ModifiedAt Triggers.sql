GO
CREATE TRIGGER trg_UpdateDifficultyLevelModifiedAt
ON DifficultyLevels
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE DifficultyLevels
    SET ModifiedAt = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END

GO
CREATE TRIGGER trg_UpdateCategoryModifiedAt
ON Categories
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Categories
    SET ModifiedAt = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END

GO
CREATE TRIGGER trg_UpdateEventModifiedAt
ON Events
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Events
    SET ModifiedAt = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END