CREATE PROCEDURE GetAllMovies
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        MovieID,
        Title,
        Genre,
        MovieImage,
        ReleaseDate,
        MovieDescription
    FROM Movies;
END;