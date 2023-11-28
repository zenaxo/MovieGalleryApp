CREATE PROCEDURE DeleteMovie
    @movieId INT
AS
BEGIN
    DELETE FROM Movies
    WHERE MovieID = @movieId;
END;