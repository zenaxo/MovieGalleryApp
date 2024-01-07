CREATE PROCEDURE GetMoviesByGenre
    @genre NVARCHAR(255)
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
    FROM 
		Movies
	WHERE 
        Genre = @genre COLLATE SQL_Latin1_General_CP1_CI_AS;
END;