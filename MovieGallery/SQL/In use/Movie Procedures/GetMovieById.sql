CREATE PROCEDURE GetMovieById
    @movieId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve movie details
    SELECT
        MovieID,
        Title,
        Genre,
        MovieImage,
        ReleaseDate,
        MovieDescription
    FROM Movies
    WHERE MovieID = @movieId;
END;