CREATE PROCEDURE GetProducersForMovie
    @movieId INT
AS
BEGIN
    SELECT
        P.ProducerId,
        P.FirstName,
        P.LastName
    FROM
        Producers AS P
    INNER JOIN
        MovieProducers AS MP ON P.ProducerID = MP.ProducerID
    WHERE
        MP.MovieID = @movieId;
END;