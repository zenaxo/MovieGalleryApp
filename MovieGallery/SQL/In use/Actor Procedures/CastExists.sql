CREATE PROCEDURE CastExists
    @actorId INT,
    @movieId INT,
    @result INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the movie producer already exists
    IF EXISTS (SELECT 1 FROM Casts WHERE ActorID = @actorId AND MovieID = @movieId)
    BEGIN
        SET @result = 1; -- Movie producer exists
    END
    ELSE
    BEGIN
        SET @result = 0; -- Movie producer does not exist
    END
END