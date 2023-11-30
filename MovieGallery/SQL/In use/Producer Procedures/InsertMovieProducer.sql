CREATE PROCEDURE InsertMovieProducer
    @producerId INT,
    @movieId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO MovieProducers (ProducerID, MovieID)
        VALUES (@producerId, @movieId);
END