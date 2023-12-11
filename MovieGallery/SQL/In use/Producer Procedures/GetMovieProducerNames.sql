CREATE PROCEDURE GetMovieProducerNames
    @movieId INT
AS
BEGIN
    SELECT
        P.FirstName,
        P.LastName
    FROM
        Producers AS P
    INNER JOIN
        MovieProducers AS MP ON P.ProducerID = MP.ProducerID
    WHERE
        MP.MovieID = @movieId;
END;