CREATE PROCEDURE ActorExists
    @firstName VARCHAR(255),
    @lastName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActorCount INT;

    SELECT @ActorCount = COUNT(*)
    FROM Actors
    WHERE FirstName = @firstName AND LastName = @lastName;

    SELECT @ActorCount AS ActorCount;
END;