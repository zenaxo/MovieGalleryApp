CREATE PROCEDURE GetMoviesFilteredByGenre @Genre varchar(MAX)
AS
BEGIN
SELECT MovieID, Title, Genre, MovieImage, ReleaseDate
FROM Movies
WHERE Genre = @Genre
END;