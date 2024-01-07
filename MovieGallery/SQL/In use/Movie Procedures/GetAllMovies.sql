CREATE PROCEDURE GetAllMovies
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        MovieID,
        Title,
        Genre,
        MovieImage,
		MovieBackgroundImage,
        ReleaseDate,
        MovieDescription
    FROM Movies;
END;