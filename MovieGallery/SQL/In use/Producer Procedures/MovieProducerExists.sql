CREATE PROCEDURE MovieProducerExists
    @producerId INT,
    @movieId INT,
    @result INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the movie producer already exists
    IF EXISTS (SELECT 1 FROM MovieProducers WHERE ProducerID = @producerId AND MovieID = @movieId)
    BEGIN
        SET @result = 1; -- Movie producer exists
    END
    ELSE
    BEGIN
        SET @result = 0; -- Movie producer does not exist
    END
END