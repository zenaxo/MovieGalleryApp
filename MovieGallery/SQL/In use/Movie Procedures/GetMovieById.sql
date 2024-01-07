CREATE PROCEDURE GetMovieById
    @movieId INT
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
		MovieID = @movieId;
END;