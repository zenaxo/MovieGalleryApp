CREATE PROCEDURE GetActorsForMovie
    @movieId INT
AS
BEGIN
    SELECT
        a.ActorId,
        a.FirstName,
        a.LastName
    FROM
        Actors AS a
    INNER JOIN
        Casts AS C ON A.ActorID = C.ActorID
    WHERE
        C.MovieID = @movieId;
END;