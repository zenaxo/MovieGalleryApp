CREATE PROCEDURE SearchMoviesByTitle
    @title VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MovieID, Title
    FROM Movies
    WHERE Title LIKE '%' + @title + '%';
END;

