CREATE PROCEDURE DeleteMovieProducer
    @producerId INT,
    @movieId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the MovieProducer exists
    IF NOT EXISTS (SELECT 1 FROM MovieProducers WHERE ProducerId = @producerId AND MovieId = @movieId)
    BEGIN
        -- Movie Producer not found, return an error code or message
        RETURN -1;
    END

    -- Delete the MovieProducer association
    DELETE FROM MovieProducers WHERE ProducerId = @producerId AND MovieId = @movieId;

    -- Optionally, perform additional actions or validations here

    -- Return the number of rows affected (1 if successful, 0 if not found)
    RETURN @@ROWCOUNT;
END